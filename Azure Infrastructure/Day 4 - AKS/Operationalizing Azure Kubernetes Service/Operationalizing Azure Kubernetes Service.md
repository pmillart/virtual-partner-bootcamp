# Demo guide <!-- omit in toc -->

- [Prerequisites](#prerequisites)
  - [Cluster for SP Reset](#cluster-for-sp-reset)
  - [Cluster for AAD](#cluster-for-aad)
- [Demo: Managing cluster identity lifecycle](#demo-managing-cluster-identity-lifecycle)
- [Demo: Creating Azure AD-enabled cluster and assigning permissions](#demo-creating-azure-ad-enabled-cluster-and-assigning-permissions)
  - [AAD RBAC](#aad-rbac)
  - [Access the cluster with AAD](#access-the-cluster-with-aad)
- [Demo: Cluster autoscaling in multiple node pools](#demo-cluster-autoscaling-in-multiple-node-pools)
  - [Adding a new node pool](#adding-a-new-node-pool)
  - [Enabling the cluster autoscaler](#enabling-the-cluster-autoscaler)
- [Demo: Exploring Azure Monitor for containers](#demo-exploring-azure-monitor-for-containers)

## Prerequisites

Pre-stage your environment with the following resource groups and resources.

### Cluster for SP Reset

```sh
REGION_NAME="eastus"
RESOURCE_GROUP="clusterResetRG"
AKS_RESOURCE="aksCluster"
az group create -l $REGION_NAME -n $RESOURCE_GROUP
SP_NAME="${AKS_RESOURCE}Reset_sp"
CLIENT_SECRET_VALID=""
while [ -z $CLIENT_SECRET_VALID ]; do
  echo "Creating new SP and secret..."
  CLIENT_SECRET=$(az ad sp create-for-rbac --skip-assignment -n $SP_NAME -o json | jq -r .password)
  if [[ $CLIENT_SECRET == *"'"* ]]; then
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

echo "Creating AKS cluster..."
az aks create \
    --resource-group $RESOURCE_GROUP \
    --name $AKS_RESOURCE \
    --service-principal "${SP_ID}" \
    --client-secret "${CLIENT_SECRET}"

echo "Creating Log Analytics workspace..."
az monitor log-analytics workspace create \
    --resource-group $RESOURCE_GROUP \
    --name "${AKS_RESOURCE}LA"

WORKSPACE_ID=$(az monitor log-analytics workspace show \
    --resource-group $RESOURCE_GROUP \
    --workspace-name "${AKS_RESOURCE}LA" \
    --query "id" \
    -o tsv)
echo "WORKSPACE_ID: $WORKSPACE_ID"

echo "Enabling monitoring for cluster..."
az aks enable-addons -a monitoring \
    --name $AKS_RESOURCE \
    --resource-group $RESOURCE_GROUP \
    --workspace-resource-id $WORKSPACE_ID
```

### Cluster for AAD

```sh
aksname="aksAADCluster"

serverAppId=$(az ad app create \
    --display-name "${aksname}Server" \
    --identifier-uris "https://${aksname}Server" \
    --query appId -o tsv)

az ad app update --id $serverAppId --set groupMembershipClaims=All

az ad sp create --id $serverAppId

serverAppSecret=$(az ad sp credential reset \
    --name $serverAppId \
    --credential-description "AKSPassword" \
    --query password -o tsv)

az ad app permission add \
    --id $serverAppId \
    --api 00000003-0000-0000-c000-000000000000 \
    --api-permissions e1fe6dd8-ba31-4d61-89e7-88639da4683d=Scope 06da0dbc-49e2-44d2-8312-53f166ab848a=Scope 7ab1d382-f21e-4acd-a863-ba3e13f7da61=Role

az ad app permission grant --id $serverAppId \
    --api 00000003-0000-0000-c000-000000000000
az ad app permission admin-consent --id $serverAppId

clientAppId=$(az ad app create \
    --display-name "${aksname}Client" \
    --native-app \
    --reply-urls "https://${aksname}Client" \
    --query appId -o tsv)

az ad sp create --id $clientAppId

oAuthPermissionId=$(az ad app show --id $serverAppId \
    --query "oauth2Permissions[0].id" -o tsv)

az ad app permission add --id $clientAppId --api $serverAppId \
    --api-permissions ${oAuthPermissionId}=Scope
az ad app permission grant --id $clientAppId --api $serverAppId

az group create --name clusterAADRG --location eastus

tenantId=$(az account show --query tenantId -o tsv)

az aks create \
    --resource-group clusterAADRG \
    --name $aksname \
    --generate-ssh-keys \
    --aad-server-app-id $serverAppId \
    --aad-server-app-secret $serverAppSecret \
    --aad-client-app-id $clientAppId \
    --aad-tenant-id $tenantId
```

## Demo: Managing cluster identity lifecycle

1. Get the current SP assigned to the cluster:

    ```sh
    SP_ID=$(az aks show --resource-group clusterResetRG --name aksCluster --query servicePrincipalProfile.clientId -o tsv)
    ```

2. Get the end date for the credential:

    ```sh
    echo "Get credential endDate..."
    SP_END_DATE=$(az ad sp credential list --id $SP_ID --query "[].endDate" -o tsv)
    echo "endDate: $SP_END_DATE"
    ```

3. Get the secret after a reset:

    ```sh
    SP_SECRET=$(az ad sp credential reset --name $SP_ID --query password -o tsv)
    ```

4. Update the cluster (this takes a few minutes)"

    ```sh
    az aks update-credentials \
        --resource-group clusterResetRG \
        --name aksCluster \
        --reset-service-principal \
        --service-principal $SP_ID \
        --client-secret $SP_SECRET
    ```

## Demo: Creating Azure AD-enabled cluster and assigning permissions

*Execute this demo locally. There are some commands which fail in Cloud Shell!*

### AAD RBAC

1. Get the admin creds:

    ```sh
    az aks get-credentials --resource-group clusterAADRG --name $aksname --admin
    ```

2. Before an Azure Active Directory account can be used with the AKS cluster, a role binding or cluster role binding needs to be created. Roles define the permissions to grant, and bindings apply them to desired users.

   ```sh
   CURRENT_USER_UPN=$(az ad signed-in-user show --query userPrincipalName -o tsv)
   CURRENT_USER_OBJECT_ID=$(az ad signed-in-user show --query objectId -o tsv)
   echo "Current user: $CURRENT_USER_UPN"
   echo "Current user object id: $CURRENT_USER_OBJECT_ID"
   ```

3. Create the a file called `azure-basic-ad-binding.yaml` and paste the following:

    ```yaml
    apiVersion: rbac.authorization.k8s.io/v1
    kind: ClusterRoleBinding
    metadata:
      name: contoso-cluster-admins
    roleRef:
      apiGroup: rbac.authorization.k8s.io
      kind: ClusterRole
      name: cluster-admin
    subjects:
    - apiGroup: rbac.authorization.k8s.io
      kind: User
      name: userPrincipalName_or_objectId
    ```

    Or download and show with:

    ```sh
    wget https://raw.githubusercontent.com/opsgility/virtual-partner-bootcamp/master/Azure%20Infrastructure/Day%204%20-%20AKS/Operationalizing%20Azure%20Kubernetes%20Service/azure-basic-ad-binding.yaml
    cat azure-basic-ad-binding.yaml
    ```

4. Apply the manifest:

    ```sh
    echo "Update userPrincipalName_or_objectId in azure-basic-ad-binding.yaml..."
    if [[ $CURRENT_USER_UPN == *"EXT"* ]]; then sed -i "s/userPrincipalName_or_objectId/${CURRENT_USER_OBJECT_ID}/g" azure-basic-ad-binding.yaml; else sed -i "s/userPrincipalName_or_objectId/${CURRENT_USER_UPN}/g" azure-basic-ad-binding.yaml; fi
    kubectl apply -f azure-basic-ad-binding.yaml
    ```

### Access the cluster with AAD

1. Get the credentials

    ```sh
    az aks get-credentials --resource-group clusterAADRG --name $aksname --overwrite-existing
    ```

2. Fire off kubectl so we get a login prompt:

    ```sh
    kubectl get nodes
    ```

    The authentication token received for kubectl is cached. You are only reprompted to sign in when the token has expired or the Kubernetes config file is re-created.

## Demo: Cluster autoscaling in multiple node pools

*Use the same cluster created in the first demo: [Cluster for SP Reset](#cluster-for-sp-reset)*

### Adding a new node pool

1. Let's take a look at the existing cluster and view its node pools:

    ```sh
    az aks nodepool list --resource-group clusterResetRG --cluster-name aksCluster
    ```

2. We can see we have a single system node pool at the moment. Let's extend this with a new user node pool.

    ```sh
    az aks nodepool add \
        --resource-group clusterResetRG \
        --cluster-name aksCluster \
        --name userpool \
        --node-count 3 \
        --node-taints pool=userpool:NoSchedule \
        --kubernetes-version $(az aks show --resource-group clusterResetRG --name aksCluster --query "kubernetesVersion" -o tsv)
    ```

    > **Note:** This command takes a few minutes to run. While it is running, navigate to the Azure Portal and the *aksCluster* resource. Show how the node pools are visible through the **Node pools** blade and also the node pool creation experience is availabe in the portal as well.

3. Once the node pool is created, let's go back and view our list again:

    ```sh
    az aks nodepool list --resource-group clusterResetRG --cluster-name aksCluster -o table
    ```

4. Let's connect to our cluster and take a look at the existing nodes:

    ```sh
    az aks get-credentials --resource-group clusterResetRG --name aksCluster --overwrite-existing
    kubectl get nodes
    ```

5. Now to schedule pods on our new nodes using taints and tolerations, let's make sure out taints where applied to the pool when we created it earlier. Note that taints can only be applied to a pool at the time it is created and cannot be added later, so some planning is required here.

    ```sh
    az aks nodepool show --resource-group clusterResetRG --cluster-name aksCluster --name userpool --query "nodeTaints"
    ```

6. With out taint verified, let's create a quick manifest, deploy some pods, and make sure they're scheduled on the right nodes. (Copy yaml and save as `userpool.yaml`):

    ```yaml
    apiVersion: v1
    kind: Pod
    metadata:
      name: userpoolpod
    spec:
      containers:
      - image: nginx:1.15.9
        name: mypod
        resources:
          requests:
            cpu: 100m
            memory: 128Mi
          limits:
            cpu: 1
            memory: 2G
      tolerations:
      - key: "pool"
        operator: "Equal"
        value: "userpool"
        effect: "NoSchedule"
    ```

7. Let's apply our manifest:

    ```sh
    kubectl apply -f userpool.yaml
    ```

8. With our pod deployed, let's verify it ended up on a node in the right pool:

    ```sh
    kubectl describe pod userpoolpod
    ```

### Enabling the cluster autoscaler

1. Ok, let's move on to enabling the cluster autoscaler. (Note this takes a few minutes)

    ```sh
    az aks nodepool update \
        --resource-group clusterResetRG \
        --cluster-name aksCluster \
        --name userpool \
        --enable-cluster-autoscaler \
        --min-count 1 \
        --max-count 3
    ```

2. Now we can see how even though we set the autoscaler for a single node pool, it did in fact effect the entire cluster:

    ```sh
    az aks show --resource-group clusterResetRG -n aksCluster --query "autoScalerProfile"
    ```

3. If we want to update the configuration, we use use az aks update, but before we can do that we need to install the `aks-preview` extension:

    ```sh
    # Install the aks-preview extension
    az extension add --name aks-preview

    # Update the extension to make sure you have the latest version installed
    az extension update --name aks-preview
    ```

4. With the extension installed, let's  update our autoscaler configuration:

    ```sh
    az aks update \
        --resource-group clusterResetRG \
        --name aksCluster \
        --cluster-autoscaler-profile scan-interval=30s
    ```

## Demo: Exploring Azure Monitor for containers

1. Earlier, I enabled monitoring on the cluster at the time I created it using `az aks enable-addons`:

    ```sh
    echo "Enabling monitoring for cluster..."
    az aks enable-addons -a monitoring \
        --name $AKS_RESOURCE \
        --resource-group $RESOURCE_GROUP \
        --workspace-resource-id $WORKSPACE_ID
    ```

kubectl get ds omsagent --namespace=kube-system
