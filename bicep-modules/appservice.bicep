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
    siteConfig: {
      appSettings: [
        {
          name: 'AzureKeyVaultUrl'
          value: ''
        }
      ]
    }
  }
}

resource appSettingsFile 'Microsoft.Web/sites/config@2021-02-01' = {
  parent: apiAppService
  name: 'appsettings'
  properties: {
    appSettings: [
      {
        name: 'Setting1'
        value: 'Value1'
      }
      {
        name: 'Setting2'
        value: 'Value2'
      }
      {
        name: 'Setting3'
        value: 'Value3'
      }
    ]
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
