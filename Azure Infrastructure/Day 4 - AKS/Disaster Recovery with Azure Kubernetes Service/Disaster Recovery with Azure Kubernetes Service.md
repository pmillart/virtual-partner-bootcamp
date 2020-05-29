# Demo guide <!-- omit in toc -->

- [Prerequisites](#prerequisites)
- [Demo: Verifying node distribution](#demo-verifying-node-distribution)
- [Demo: Backup and restore with Velero](#demo-backup-and-restore-with-velero)
- [Demo: Azure Container Registry geo-replication](#demo-azure-container-registry-geo-replication)

## Prerequisites

Pre-stage your environment with the following resource groups and resources.

```sh
RESOURCE_GROUP="clusterZonesRG"
REGION_NAME="eastus2"
REGION_NAME2="eastus"
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
echo "Creating cluster $AKS_RESOURCE in $REGION_NAME..."
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

echo "Adding node pool zonalpool to cluster $AKS_RESOURCE..."
az aks nodepool add \
    --cluster-name $AKS_RESOURCE \
    --name zonalpool \
    --resource-group $RESOURCE_GROUP \
    --mode User \
    --node-count 2 \
    --node-taints pool=zonalpool:NoSchedule \
    --zones 1 3

echo "Creating cluster ${AKS_RESOURCE}2 in $REGION_NAME2..."
az aks create \
    --resource-group $RESOURCE_GROUP \
    --name "${AKS_RESOURCE}2" \
    --location $REGION_NAME2 \
    --generate-ssh-keys \
    --vm-set-type VirtualMachineScaleSets \
    --load-balancer-sku standard \
    --node-count 1 \
    --service-principal "${SP_ID}" \
    --client-secret "${CLIENT_SECRET}"

echo "Creating Velero backup"
TENANT_ID=$(az account show --query "tenantId" | sed -r "s/\\\"//g")
SUBSCRIPTION_ID=$(az account show --query "id" | sed -r "s/\\\"//g")
SOURCE_AKS_RESOURCE_GROUP=$(az group list -o json | jq -r ".[] | select(.name | contains(\"MC_${RESOURCE_GROUP}_${AKS_RESOURCE}_${REGION_NAME}\")
) | .name")
TARGET_AKS_RESOURCE_GROUP=$(az group list -o json | jq -r ".[] | select(.name | contains(\"MC_${RESOURCE_GROUP}_${AKS_RESOURCE}2_${REGION_NAME2}\")
) | .name")
BACKUP_RESOURCE_GROUP=$RESOURCE_GROUP
BACKUP_STORAGE_ACCOUNT_NAME=velero$(uuidgen | cut -d '-' -f5 | tr '[A-Z]' '[a-z]')

# Create Azure Storage Account
echo "Creating storage account $BACKUP_STORAGE_ACCOUNT_NAME..."
az storage account create \
  --name $BACKUP_STORAGE_ACCOUNT_NAME \
  --resource-group $RESOURCE_GROUP \
  --sku Standard_GRS \
  --encryption-services blob \
  --https-only true \
  --kind BlobStorage \
  --access-tier Hot

echo "Created storage account $BACKUP_STORAGE_ACCOUNT_NAME. Sleep for 2 minutes while it finishes..."
sleep 120
  
 # Create Blob Container
 echo "Creating container..."
 az storage container create \
   --name velero \
   --public-access off \
   --account-name $BACKUP_STORAGE_ACCOUNT_NAME

# Create a Service Principal for RBAC
VELERO_CLIENT_SECRET=`az ad sp create-for-rbac \
  --name "${AKS_RESOURCE}Velero" \
  --role "Contributor" \
  --query 'password' \
  -o tsv \
  --scopes /subscriptions/$SUBSCRIPTION_ID/resourceGroups/$BACKUP_RESOURCE_GROUP /subscriptions/$SUBSCRIPTION_ID/resourceGroups/$SOURCE_AKS_RESOURCE_GROUP /subscriptions/$SUBSCRIPTION_ID/resourceGroups/$TARGET_AKS_RESOURCE_GROUP`
echo "VELERO_CLIENT_SECRET: $VELERO_CLIENT_SECRET"

VELERO_CLIENT_ID=`az ad sp list --display-name "${AKS_RESOURCE}Velero" --query '[0].appId' -o tsv`
echo "VELERO_CLIENT_ID: $VELERO_CLIENT_ID"

echo "Download and extract Velero client..."
mkdir temp
cd temp

wget https://github.com/vmware-tanzu/velero/releases/download/v1.4.0/velero-v1.4.0-linux-amd64.tar.gz

tar -xvf velero-v1.4.0-linux-amd64.tar.gz

cd velero-v1.4.0-linux-amd64

echo "Creating credential files..."
cat << EOF > ./credentials-velero-source
AZURE_SUBSCRIPTION_ID=${SUBSCRIPTION_ID}
AZURE_TENANT_ID=${TENANT_ID}
AZURE_CLIENT_ID=${VELERO_CLIENT_ID}
AZURE_CLIENT_SECRET=${VELERO_CLIENT_SECRET}
AZURE_RESOURCE_GROUP=${SOURCE_AKS_RESOURCE_GROUP}
AZURE_CLOUD_NAME=AzurePublicCloud
EOF

cat << EOF > ./credentials-velero-target
AZURE_SUBSCRIPTION_ID=${SUBSCRIPTION_ID}
AZURE_TENANT_ID=${TENANT_ID}
AZURE_CLIENT_ID=${VELERO_CLIENT_ID}
AZURE_CLIENT_SECRET=${VELERO_CLIENT_SECRET}
AZURE_RESOURCE_GROUP=${TARGET_AKS_RESOURCE_GROUP}
AZURE_CLOUD_NAME=AzurePublicCloud
EOF

echo "Retrieve credentials for cluster $AKS_RESOURCE..."
az aks get-credentials --resource-group $RESOURCE_GROUP --name $AKS_RESOURCE --overwrite-existing

echo "Installing Velero to cluster $AKS_RESOURCE..."
./velero install \
  --provider azure \
  --plugins velero/velero-plugin-for-microsoft-azure:v1.0.0 \
  --bucket velero \
  --secret-file ./credentials-velero-source \
  --backup-location-config resourceGroup=$BACKUP_RESOURCE_GROUP,storageAccount=$BACKUP_STORAGE_ACCOUNT_NAME \
  --snapshot-location-config apiTimeout=5m,resourceGroup=$BACKUP_RESOURCE_GROUP \
  --wait

echo "Retrieve credentials for cluster ${AKS_RESOURCE}2..."
az aks get-credentials --resource-group $RESOURCE_GROUP --name "${AKS_RESOURCE}2" --overwrite-existing

echo "Installing Velero to cluster ${AKS_RESOURCE}2..."
./velero install \
  --provider azure \
  --plugins velero/velero-plugin-for-microsoft-azure:v1.0.0 \
  --bucket velero \
  --secret-file ./credentials-velero-target \
  --backup-location-config resourceGroup=$BACKUP_RESOURCE_GROUP,storageAccount=$BACKUP_STORAGE_ACCOUNT_NAME \
  --snapshot-location-config apiTimeout=5m,resourceGroup=$BACKUP_RESOURCE_GROUP \
  --wait
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

7. As you can see, my pods even deployed across my five available nodes in my default node pool. Let's increase the number of replicas:

    ```sh
    kubectl scale --replicas=8 deployments/nginx-deployment
    kubectl get pods -l app=nginx -o wide
    ```

8. Why didn't the pods cross to my zonalpool? Well, because I have tainted the pool members:

    ```sh
    kubectl get nodes -o json | jq -r ".items[].spec.taints"
    ```

## Demo: Backup and restore with Velero

Prior to this, I already installed Velero and configured it to have access to both of my deployed clusters.

Let's create some basic infrastructure, deploying new a namespace and, a persistent volume that we can service some HTML from through NGINX, and deploy NGINX.

Save the following and call it `nginx-stateful.yaml`:

```sh
apiVersion: v1
kind: Namespace
metadata:
  name: velero-test
  
---

apiVersion: v1
kind: PersistentVolumeClaim
metadata:
  name: nginx-pvc
  namespace: velero-test
spec:
  accessModes:
  - ReadWriteOnce
  storageClassName: default
  resources:
    requests:
      storage: 5Gi

---

apiVersion: v1
kind: Pod
metadata:
  namespace: velero-test
  name: nginx
  labels:
    app: nginx
    environment: velero-test
spec:
  containers:
  - image: nginx
    name: nginx
    resources:
      requests:
        cpu: 100m
        memory: 128Mi
      limits:
        cpu: 250m
        memory: 256Mi
    volumeMounts:
    - mountPath: "/usr/share/nginx/html"
      name: volume
    ports:
    - containerPort: 80
      protocol: TCP
  volumes:
    - name: volume
      persistentVolumeClaim:
        claimName: nginx-pvc  
  
---

kind: Service
apiVersion: v1
metadata:
  name:  nginx
  namespace: velero-test
spec:
  selector:
    app: nginx
    environment: velero-test
  type:  LoadBalancer
  ports:
  - port:  80
    targetPort:  80
```

Now apply it:

```sh
kubectl apply -f nginx-stateful.yaml
```

Now let's create a quick HTML file we can store in the persistent volume:

```sh
cat << EOF > ./index.html
<!DOCTYPE html>
<html>
<body style="background-color:white;">
<font size="24">Let's test persistent volume backup and restore!</font>
</body>
</html>
EOF
```

Now let's copy the file in:

```sh
kubectl cp index.html velero/nginx:/usr/share/nginx/html/
```

And then get our services to find the IP:

```sh
kubectl get services -n velero-test
```

Now we can create a backup:

```sh
./velero backup create velero-backup --include-namespaces velero-test
```

And we can check our backup state:

```sh
./velero backup describe velero-backup
```

If we browse to the resource group where our backup storage resides, we can see a snapshot of the persistent disk has been created.

Now let's connect to our 2nd cluster to restore the backup:

```sh
RESOURCE_GROUP="clusterZonesRG"
AKS_RESOURCE="aksCluster"
az aks get-credentials --resource-group $RESOURCE_GROUP --name "${AKS_RESOURCE}2" --overwrite-existing
```

And then let's restore:

```sh
./velero restore create --from-backup velero-backup
```

Let's make sure our pods came up:

```sh
kubectl get pods,deployments,services -n velero-test
```

And if we browse to the IP, we can see that our app has been restored. Or not... Why didn't our pod come up?

```sh
kubectl describe pv $(kubectl get pvc -o json -n velero-test | jq -r ".items[].spec.volumeName")
```

We can see our pod came up in Zone 2, but let's take a look at our nodes:

```sh
kubectl get nodes -o json | \
    jq -r '.items[].metadata.labels | {nodename:.agentpool,nodezone:.["failure-domain.beta.kubernetes.io/zone"]}'
```

As you can see, my node is in Zone 0, or other words my cluster that I'm restoring to is not using Availability Zones. You can certainly overcome this by creating a ConfigMap to map the original StorageClass to the new StorageClass if you need to.

## Demo: Azure Container Registry geo-replication

