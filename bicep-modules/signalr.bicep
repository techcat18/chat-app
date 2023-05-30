param location string = resourceGroup().location
param signalRServiceName string 

resource signalRService 'Microsoft.SignalRService/signalR@2023-02-01' = {
  name: signalRServiceName
  location: location
  sku:{
    name: 'Free_F1'
    tier: 'Free'
  }
  identity: {
    type: 'SystemAssigned'
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
        value: 'Default'
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

//output connectionString string = 'Endpoint=https://${signalRService.name}.service.signalr.net;AuthType=azure.msi;Version=1.0;'
output connectionString string = signalRService.listKeys().primaryConnectionString
