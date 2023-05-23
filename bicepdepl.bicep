param rg_name string
param location string

module rg 'bicep-modules/resourcegroup.bicep' = {
  name: 'rg'
  scope: subscription()
  params: {
    resourceGroupName: rg_name
    location: location
  }
}
