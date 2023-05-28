param location string
param storageSkuName string = 'Standard_LRS'
param storageAccountName string
param sqlServerName string
param sqlServerDatabaseName string
param apiAppServiceName string
param blazorAppServiceName string
param keyVaultName string
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
    keyVaultName: keyVaultName
  }
  dependsOn: [
    appServicePlan
  ]
}

module keyVault './bicep-modules/keyvault.bicep' = {
  name: 'KeyVaultDeploy'
  params: {
    location: location
    keyVaultName: keyVaultName
    apiAppServicePrincipalId: appService.outputs.apiAppServicePrincipalId
    sqlServerDatabaseConnection: 'Server=tcp:${sqlServerName}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=${sqlServerDatabaseName};Persist Security Info=False;User ID=${dbLogin};Password=${dbPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
    storageAccessKey: storageAccount.outputs.storageAccessKey
    storageConnectionString: storageAccount.outputs.storageConnectionString
    frontUrlString: 'http://${blazorAppServiceName}.azurewebsites.net/'
    jwtSettingsKeyString: 'ChatApp1230912048901283'
    jwtSettingsAudienceString: 'BlazorApp'
    jwtSettingsIssuerString: 'ChatAppAPI'
  }
}
