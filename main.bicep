param location string
param storageSkuName string = 'Standard_LRS'
param storageAccountName string = 'chatappstorageacc18'
param sqlServerName string = 'chatappsqlserver'
param sqlServerDatabaseName string = 'chatappsqlserverdatabase'
param apiAppServiceName string
param blazorAppServiceName string
@secure()
param dbLogin string
@secure()
param dbPassword string

module storageAccount './bicep-modules/storageaccount.bicep' = {
  name: 'storageAccountDeploy'
  params: {
    location: location
    storageSkuName: storageSkuName
    storageAccountName: storageAccountName
  }
}

module sqlServerDatabase './bicep-modules/sqldatabase.bicep' = {
  name: 'sqlDatabaseDeploy'
  params: {
    location: location
    dbLogin: dbLogin
    dbPassword: dbPassword
    sqlServerName: sqlServerName
    sqlServerDatabaseName: sqlServerDatabaseName
  }
}

module appServicePlan './bicep-modules/appserviceplan.bicep' = {
  name: 'appServicePlanDeploy'
  params: {
    location: location
  }
}

module appService './bicep-modules/appservice.bicep' = {
  name: 'appServiceDeploy'
  params: {
    location: location
    appServicePlanId: appServicePlan.outputs.appServicePlanId
    apiAppServiceName: apiAppServiceName
    blazorAppServiceName: blazorAppServiceName
  }
  dependsOn: [
    appServicePlan
  ]
}

module keyVault './bicep-modules/keyvault.bicep' = {
  name: 'KeyVaultDeploy'
  params: {
    location: location
    apiAppServicePrincipalId: appService.outputs.apiAppServicePrincipalId
    sqlServerDatabaseConnection: sqlServerDatabase.outputs.sqlServerDatabaseConnection
    storageAccessKey: storageAccount.outputs.storageAccessKey
    storageConnectionString: storageAccount.outputs.storageConnectionString
  }
}
