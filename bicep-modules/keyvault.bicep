param location string = resourceGroup().location
param keyVaultName string = 'chatappkeyvaulttttt'
param apiAppServicePrincipalId string
param frontUrlString string
param jwtSettingsKeyString string
param jwtSettingsAudienceString string
param jwtSettingsIssuerString string
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

resource frontUrl 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  parent: keyVault
  name: 'FrontUrl'
  properties: {
    value: frontUrlString
  }
}

resource jwtSettingsKey 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  parent: keyVault
  name: 'JwtSettings--Key'
  properties: {
    value: jwtSettingsKeyString
  }
}

resource jwtSettingsAudience 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  parent: keyVault
  name: 'JwtSettings--Audience'
  properties: {
    value: jwtSettingsAudienceString
  }
}

resource jwtSettingsIssuer 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  parent: keyVault
  name: 'JwtSettings--Issuer'
  properties: {
    value: jwtSettingsIssuerString
  }
}

output keyVaultUri string = keyVault.properties.vaultUri
