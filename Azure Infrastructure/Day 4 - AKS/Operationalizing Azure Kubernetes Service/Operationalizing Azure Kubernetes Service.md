# Demo guide <!-- omit in toc -->

- [Prerequisites](#prerequisites)
  - [Cluster for SP Reset](#cluster-for-sp-reset)
  - [Cluster for AAD](#cluster-for-aad)
- [Demo: Managing cluster identity lifecycle](#demo-managing-cluster-identity-lifecycle)
- [Demo: Creating Azure AD-enabled cluster and assigning permissions](#demo-creating-azure-ad-enabled-cluster-and-assigning-permissions)
  - [AAD RBAC](#aad-rbac)
  - [Access the cluster with AAD](#access-the-cluster-with-aad)
- [Demo: Cluster autoscaling in multiple node pools](#demo-cluster-autoscaling-in-multiple-node-pools)

## Prerequisites

Pre-stage your environment with the following resource groups and resources.

### Cluster for SP Reset

```sh
az group create -l eastus -n clusterResetRG
SP_NAME="aksClusterReset_sp"
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

az aks create \
    --resource-group clusterResetRG \
    --name aksCluster \
    --service-principal "${SP_ID}" \
    --client-secret "${CLIENT_SECRET}"
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
