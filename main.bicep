param location string
param storageSkuName string = 'Standard_LRS'
param storageAccountName string = 'chatappstorageacc18'

module storageAccountModule './bicep-modules/storageaccount.bicep' = {
  name: 'storageAccountDeploy'
  params: {
    location: location
    storageSkuName: storageSkuName
    storageAccountName: storageAccountName
  }
}

output storageEndpoint object = storageAccountModule.outputs.storageEndpoint
