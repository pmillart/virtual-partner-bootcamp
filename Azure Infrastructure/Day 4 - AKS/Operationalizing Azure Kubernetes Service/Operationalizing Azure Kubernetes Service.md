# Demo guide <!-- omit in toc -->

- [Prerequisites](#prerequisites)
- [Demo: Managing cluster identity lifecycle](#demo-managing-cluster-identity-lifecycle)

## Prerequisites

Pre-stage your environment with the following resource groups and resources.

```sh
az group create -l eastus -n clusterRG
SP_NAME="aksCluster_sp"
CLIENT_SECRET_VALID=""
while [ -z $ CLIENT_SECRET_VALID ]; do
  echo "Creating new SP and secret..."
  CLIENT_SECRET=$(az ad sp create-for-rbac --skip-assignment -n $SP_NAME -o json | jq -r .password)
  if [[ $CLIENT_SECRET == *"'"* ]]; then
    echo "Found invalid character. Recreating..."
    CLIENT_SECRET_VALID =""
  else
    echo "Appears valid..."
    CLIENT_SECRET_VALID ="true"
  fi
done
echo "CLIENT_SECRET ready: ${CLIENT_SECRET}"

SP_ID=$(az ad sp show --id "http://$SP_NAME" -o json | jq -r .appId)
echo "SP_ID: ${SP_ID}"

az aks create \
    --resource-group clusterRG \
    --name aksCluster \
    --service-principal "${SP_ID}" \
    --client-secret "${CLIENT_SECRET}"
```

## Demo: Managing cluster identity lifecycle

1. Get the current SP assigned to the cluster:

    ```sh
    SP_ID=$(az aks show --resource-group clusterRG --name aksCluster --query servicePrincipalProfile.clientId -o tsv)
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
        --resource-group clusterRG \
        --name aksCluster \
        --reset-service-principal \
        --service-principal $SP_ID \
        --client-secret $SP_SECRET
    ```
