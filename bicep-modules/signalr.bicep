param location string = resourceGroup().location
param signalRServiceName string 
param apiPrincipalId string

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

var signalRAppServerRoleId = '420fcaa2-552c-430f-98ca-3264be4806c7'

resource signalRServerRole 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: signalRAppServerRoleId
  scope: signalRService
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(signalRService.id, signalRServerRole.id, apiPrincipalId)
  properties: {
    roleDefinitionId: signalRServerRole.id
    principalId: apiPrincipalId
    principalType: 'ServicePrincipal'
  }
}

output connectionString string = 'Endpoint=https://${signalRService.name}.service.signalr.net;AuthType=aad;Version=1.0;'
