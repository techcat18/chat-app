param location string = resourceGroup().location
param sqlServerName string = 'chatappsqlserver'
param sqlServerDatabaseName string = 'chatappsqlserverdatabase'
param initialCatalog string = 'chatdb'
@secure()
param dbLogin string
@secure()
param dbPassword string

targetScope = 'resourceGroup'

resource sqlServer 'Microsoft.Sql/servers@2021-11-01' ={
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

output sqlServerDatabaseConnection string = 'Server=tcp:${sqlServerDatabase.name}${environment().suffixes.sqlServerHostname},1433;Initial Catalog=db${initialCatalog};Persist Security Info=False;User ID=username;Password=password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
