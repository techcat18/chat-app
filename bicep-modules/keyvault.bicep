param location string = resourceGroup().location
param keyVaultName string = 'chatappkeyvault18'
param apiAppServicePrincipalId string
param sqlServerDatabaseConnection string

resource keyVault 'Microsoft.KeyVault/vaults@2023-02-01' = {
  name: keyVaultName
  location: location
  properties: {
    tenantId: subscription().tenantId
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: apiAppServicePrincipalId
        permissions: {
          secrets: [
            'get'
          ]
        }
      }
    ]
    sku: {
      name: 'standard'
      family: 'A'
    }
  }
}

resource sqlConnectionString 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: keyVault
  name: 'ConnectionStrings--SQLConnection'
  properties: {
    value: sqlServerDatabaseConnection
  }
}

resource blobStorageAccessKey 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  parent: keyVault
  name: 'Azure--Blob--AccessKey'
  properties: {
    value: 'value'
  }
}

resource blobStorageConnectionString 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  parent: keyVault
  name: 'Azure--Blob--ConnectionString'
  properties: {
    value: 'value'
  }
}
