param location string
param storageSkuName string = 'Standard_LRS'
param storageAccountName string

targetScope = 'resourceGroup'

resource storageaccount 'Microsoft.Storage/storageAccounts@2021-02-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: storageSkuName
  }
}

output storageEndpoint object = storageaccount.properties.primaryEndpoints
