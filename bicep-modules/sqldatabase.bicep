param location string = resourceGroup().location
param sqlServerName string
param sqlServerDatabaseName string

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

resource sqlFirewallRuleForMyIP 'Microsoft.Sql/servers/firewallRules@2021-02-01-preview' = {
  parent: sqlServer
  name: 'AllowMyIp'
  properties: {
    startIpAddress: '159.224.53.92'
    endIpAddress: '159.224.53.92'
  }
}

resource sqlFirewallRuleForAzureResources 'Microsoft.Sql/servers/firewallRules@2021-02-01-preview' = {
  parent: sqlServer
  name: 'AllowAllAzureResources'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0' 
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
