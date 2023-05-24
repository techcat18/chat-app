param location string = resourceGroup().location
param sqlServerName string = 'chatappsqlserver'
param sqlServerDatabaseName string = 'chatappsqlserverdatabase'
@secure()
param dbLogin string
@secure()
param dbPassword string

targetScope = 'resourceGroup'

resource sqlServer 'Microsoft.Sql/servers@2014-04-01' ={
  name: sqlServerName
  location: location
  properties:{
    administratorLogin: dbLogin
    administratorLoginPassword: dbPassword
  }
}

resource sqlServerDatabase 'Microsoft.Sql/servers/databases@2021-11-01' = {
  parent: sqlServer
  name: sqlServerDatabaseName
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
  }
}
