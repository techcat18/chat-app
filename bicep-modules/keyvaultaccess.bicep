param apiPrincipalId string
param functionPrincipalId string

resource keyVaultAdministratorRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01'existing  = {
  scope: subscription()
  name: '00482a5a-887f-4fb3-b363-3b7fe8e74483'
}

resource apiKeyVaultRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(resourceGroup().id, apiPrincipalId, keyVaultAdministratorRoleDefinition.id)
  properties: {
    roleDefinitionId: keyVaultAdministratorRoleDefinition.id
    principalId: apiPrincipalId
    principalType: 'ServicePrincipal'
  }
}

resource functionkeyVaultRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
  name: guid(resourceGroup().id, functionPrincipalId, keyVaultAdministratorRoleDefinition.id)
  properties: {
    roleDefinitionId: keyVaultAdministratorRoleDefinition.id
    principalId: functionPrincipalId
    principalType: 'ServicePrincipal'
  }
}
