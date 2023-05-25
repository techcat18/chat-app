param location string = resourceGroup().location
param apiAppServiceName string = 'chatapiappservice'
param blazorAppServiceName string = 'chatblazorappservice'
param appServicePlanId string

resource apiAppService 'Microsoft.Web/sites@2021-01-15' = {
  name: apiAppServiceName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlanId
  }
}

resource blazorAppService 'Microsoft.Web/sites@2021-01-15' = {
  name: blazorAppServiceName
  location: location
  properties: {
    serverFarmId: appServicePlanId
  }
}

output apiAppServicePrincipalId string = apiAppService.identity.principalId
