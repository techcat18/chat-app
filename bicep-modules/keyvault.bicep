param location string = resourceGroup().location
param keyVaultName string = 'chatappkeyvaulttttt'
param apiAppServicePrincipalId string
@secure()
param sqlServerDatabaseConnection string
@secure()
param storageAccessKey string
@secure()
param storageConnectionString string

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
            'list'
          ]
        }
      }
      {
        tenantId: subscription().tenantId
        objectId: 'c95fc188-feb0-42fb-86ac-27763f04107a'
        permissions: {
          secrets: [
            'get'
            'list'
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
    value: storageAccessKey
  }
}

resource blobStorageConnectionString 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  parent: keyVault
  name: 'Azure--Blob--ConnectionString'
  properties: {
    value: storageConnectionString
  }
}

output keyVaultUri string = keyVault.properties.vaultUri
