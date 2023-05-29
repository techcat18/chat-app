param location string = resourceGroup().location
param signalRServiceName string 

resource signalRService 'Microsoft.SignalRService/signalR@2023-02-01' = {
  name: signalRServiceName
  location: location
  sku:{
    name: 'Free_F1'
    tier: 'Free'
  }
  kind: 'SignalR'
  properties: {
    cors:{
      allowedOrigins:[
        '*'
      ]
    }
    disableAadAuth: false
    disableLocalAuth: false
    features: [
      {
        flag: 'ServiceMode'
        value: 'Serverless'
        properties: {}
      }
      {
        flag: 'EnableConnectivityLogs'
        value: 'True'
        properties: {}
      }
      {
        flag: 'EnableMessagingLogs'
        value: 'True'
        properties: {}
      }
      {
        flag: 'EnableLiveTrace'
        value: 'True'
        properties: {}
      }
    ]
    networkACLs: {
      defaultAction: 'Deny'
      privateEndpoints: [
      ]
      publicNetwork: {
        allow: [
           'ServerConnection'
           'ClientConnection'
           'RESTAPI'
           'Trace' 
        ]
      }
    }
    publicNetworkAccess: 'Enabled'
    tls: {
      clientCertEnabled: false
    }
    upstream: {
      templates: [
      ]
    }
  }
}

output connectionString string = signalRService.listKeys().primaryConnectionString
