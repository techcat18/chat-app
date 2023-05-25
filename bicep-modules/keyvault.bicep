param location string = resourceGroup().location
param keyVaultName string = 'chatappkeyvault'
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

resource keyVaultSecret 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: keyVault
  name: 'ConnectionStrings--SQLConnection'
  properties: {
    value: sqlServerDatabaseConnection
  }
}
