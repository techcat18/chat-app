param location string
param storageSkuName string = 'Standard_LRS'

module saModule './bicep-modules/storageaccount.bicep' = {
  name: 'storageAccountDeploy'
  params: {
    location: location
    storageSkuName: storageSkuName
  }
}

output storageEndpoint object = saModule.outputs.storageEndpoint
