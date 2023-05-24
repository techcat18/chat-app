param location string
param storageSkuName string = 'Standard_LRS'

targetScope = 'resourceGroup'

resource storageaccount 'Microsoft.Storage/storageAccounts@2021-02-01' = {
  name: 'chatapp-sa'
  location: location
  kind: 'StorageV2'
  sku: {
    name: storageSkuName
  }
}

output storageEndpoint object = storageaccount.properties.primaryEndpoints
