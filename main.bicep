param location string
param storageSkuName string = 'Standard_LRS'
param storageAccountName string = 'chatappstorageacc18'
param sqlServerName string = 'chatappsqlserver18'
param sqlServerDatabaseName string = 'chatappsqlserverdatabase'
param initialCatalog string = 'chatappsqlserverdatabase'
param apiAppServiceName string
param blazorAppServiceName string
param keyVaultName string = 'appkeyvault18chat'
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
    sqlServerDatabaseConnection: 'Server=tcp:${sqlServerName}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=${initialCatalog};Persist Security Info=False;User ID=${dbLogin};Password=${dbPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
    storageAccessKey: storageAccount.outputs.storageAccessKey
    storageConnectionString: storageAccount.outputs.storageConnectionString
    frontUrlString: 'http://${blazorAppServiceName}.azurewebsites.net/'
    jwtSettingsKeyString: 'ChatApp123091204890128308120'
    jwtSettingsAudienceString: 'BlazorApp'
    jwtSettingsIssuerString: 'ChatAppAPI'
  }
}

module ApiAppSettings './bicep-modules/appserviceconfig.bicep' = {
  name: '${apiAppServiceName}-appsettings'
  params: {
    webAppName: apiAppServiceName
    currentAppSettings: list(resourceId('Microsoft.Web/sites/config', apiAppServiceName, 'appsettings'), '2022-03-01').properties
    appSettings: {
      AzureKeyVaultUrl: keyVault.outputs.keyVaultUri
    }
  }
  dependsOn: [
    keyVault
  ]
}


module BlazorAppSettings './bicep-modules/appserviceconfig.bicep' = {
  name: '${blazorAppServiceName}-appsettings'
  params: {
    webAppName: blazorAppServiceName
    currentAppSettings: list(resourceId('Microsoft.Web/sites/config', blazorAppServiceName, 'appsettings'), '2022-03-01').properties
    appSettings: {
      APIUrl: 'http://${apiAppServiceName}.azurewebsites.net/'
    }
  }
  dependsOn: [
    keyVault
  ]
}
