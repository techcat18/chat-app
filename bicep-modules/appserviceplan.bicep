param location string = resourceGroup().location
param appServicePlanName string = 'chatappserviceplan'

resource appServicePlan 'Microsoft.Web/serverfarms@2020-12-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'F1'
    capacity: 1
  }
}

output appServicePlanId string = appServicePlan.id
