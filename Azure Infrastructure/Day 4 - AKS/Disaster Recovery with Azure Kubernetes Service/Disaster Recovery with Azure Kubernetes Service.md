# Demo guide <!-- omit in toc -->

- [Prerequisites](#prerequisites)
- [Demo: Verifying node distribution](#demo-verifying-node-distribution)
- [Demo: Backup and restore with Velero](#demo-backup-and-restore-with-velero)

## Prerequisites

Pre-stage your environment with the following resource groups and resources.

```sh
RESOURCE_GROUP="clusterZonesRG"
REGION_NAME="eastus2"
AKS_RESOURCE="aksCluster"

az group create \
    --name $RESOURCE_GROUP \
    --location $REGION_NAME

SP_NAME="${AKS_RESOURCE}Zones_sp"
CLIENT_SECRET_VALID=""
while [ -z $CLIENT_SECRET_VALID ]; do
  echo "Creating new SP and secret..."
  CLIENT_SECRET=$(az ad sp create-for-rbac --skip-assignment -n $SP_NAME -o json | jq -r .password)
  echo "CLIENT_SECRET: ${CLIENT_SECRET}"
  if [[ $CLIENT_SECRET == *"'"* ]]; then
    echo "Found invalid character. Recreating..."
    CLIENT_SECRET_VALID=""
  elif [[ $CLIENT_SECRET == *"\`"* ]]; then
    echo "Found invalid character. Recreating..."
    CLIENT_SECRET_VALID=""
  else
    echo "Appears valid..."
    CLIENT_SECRET_VALID="true"
  fi
done
echo "CLIENT_SECRET ready: ${CLIENT_SECRET}"

SP_ID=$(az ad sp show --id "http://$SP_NAME" -o json | jq -r .appId)
echo "SP_ID: ${SP_ID}"

# Create a cluster with zonal support
az aks create \
    --resource-group $RESOURCE_GROUP \
    --name $AKS_RESOURCE \
    --generate-ssh-keys \
    --vm-set-type VirtualMachineScaleSets \
    --load-balancer-sku standard \
    --service-principal "${SP_ID}" \
    --client-secret "${CLIENT_SECRET}" \
    --node-count 5 \
    --zones 1 2 3

az aks nodepool add \
    --cluster-name $AKS_RESOURCE \
    --name zonalpool \
    --resource-group $RESOURCE_GROUP \
    --mode User \
    --node-count 2 \
    --node-taints pool=zonalpool:NoSchedule \
    --zones 1 3
```

## Demo: Verifying node distribution

1. Let's retrieve the credentials for our cluster:

    ```sh
    RESOURCE_GROUP="clusterZonesRG"
    AKS_RESOURCE="aksCluster"
    az aks get-credentials --resource-group $RESOURCE_GROUP --name $AKS_RESOURCE --overwrite-existing
    ```

2. Next, let's take a look at our nodes using `kubectl describe nodes`:

    ```sh
    kubectl describe nodes | grep -e "Name:" -e "failure-domain.beta.kubernetes.io/zone"
    ```

    Note that we're filtering on `failure-domain.beta.kubernetes.io/zone`.

    Based on the output we can see 7 nodes distributed across the specified region and availability zones, such as eastus2-1 for the first availability zone and eastus2-2 for the second availability zone. Because I have nodes in both two pools we might need some additional filtering.

3. Let's use jq to help us out:

    ```sh
    kubectl get nodes -o json | \
        jq -r '.items[].metadata.labels | {nodename:.agentpool,nodezone:.["failure-domain.beta.kubernetes.io/zone"]} | select(.nodename == "zonalpool")'
    ```

4. Discovering where pods are running is a bit more involved. You can certainly describe the running pods and manually map them back to their node and zone:

    ```sh
    kubectl describe pod --all-namespaces | grep -e "^Name:" -e "^Node:"
    ```

5. We can again use jq to help us out to get a cleaner list:

    ```sh
    kubectl get pods --all-namespaces -o json | \
        jq -r '.items[] | {name:.metadata.name,node:.spec.nodeName} | select(.node | contains("zonal"))'
    ```

6. Now let's deploy a few pods and see how they spread out. Let's just use nginx. First, let's create a deployment:

    ```yaml
    apiVersion: apps/v1
    kind: Deployment
    metadata:
      name: nginx-deployment
      labels:
        app: nginx
    spec:
      replicas: 5
      selector:
        matchLabels:
          app: nginx
      template:
        metadata:
          labels:
            app: nginx
        spec:
          containers:
          - name: nginx
            image: nginx:1.14.2
            ports:
            - containerPort: 80
    ```

    ```sh
    kubectl apply -f nginx.yaml
    ```

    ```sh
    kubectl get pods -l app=nginx -o wide
    ```

## Demo: Backup and restore with Velero

```sh
TENANT_ID=...
SUBSCRIPTION_ID=...
SOURCE_AKS_RESOURCE_GROUP=MC_...
TARGET_AKS_RESOURCE_GROUP=MC_... # (optional, if you want to migrate)
BACKUP_RESOURCE_GROUP=backups
BACKUP_STORAGE_ACCOUNT_NAME=velero$(uuidgen | cut -d '-' -f5 | tr '[A-Z]' '[a-z]')

# Create Azure Storage Account
az storage account create \
  --name $BACKUP_STORAGE_ACCOUNT_NAME \
  --resource-group $RESOURCE_GROUP \
  --sku Standard_GRS \
  --encryption-services blob \
  --https-only true \
  --kind BlobStorage \
  --access-tier Hot
  
 # Create Blob Container
 az storage container create \
   --name velero \
   --public-access off \
   --account-name $BACKUP_STORAGE_ACCOUNT_NAME
```
