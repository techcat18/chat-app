param location string
param storageSkuName string = 'Standard_LRS'
param storageAccountName string = 'chatappstorageacc18'
param sqlServerName string = 'chatappsqlserver'
param sqlServerDatabaseName string = 'chatappsqlserverdatabase'
param initialCatalog string = 'chatappsqlserverdatabase'
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
    sqlServerDatabaseConnection: 'Server=tcp:${sqlServerName}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=db${initialCatalog};Persist Security Info=False;User ID=${dbLogin};Password=${dbPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
    storageAccessKey: storageAccount.outputs.storageAccessKey
    storageConnectionString: storageAccount.outputs.storageConnectionString
  }
}

module ApiAppSettings './bicep-modules/appserviceconfig.bicep' = {
  name: '${apiAppServiceName}-appsettings'
  params: {
    webAppName: apiAppServiceName
    currentAppSettings: list(resourceId('Microsoft.Web/sites/config', apiAppServiceName, 'appsettings'), '2022-03-01').properties
    appSettings: {
      AzureKeyVaultUrl: keyVault.outputs.keyVaultUri
      FrontUrl: 'http://${blazorAppServiceName}.azurewebsites.net/'
      JwtSettings__Key: 'ChatApp123091204890128308120'
      JwtSettings__Audience: 'BlazorApp'
      JwtSettings__Issuer: 'ChatAppAPI'
      ConnectionStrings__SQLConnection: 'Server=tcp:${sqlServerName}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=db${initialCatalog};Persist Security Info=False;User ID=${dbLogin};Password=${dbPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
      Azure__Blob__ConnectionString: storageAccount.outputs.storageConnectionString
      Azure__Blob__AccessKey: storageAccount.outputs.storageAccessKey
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
