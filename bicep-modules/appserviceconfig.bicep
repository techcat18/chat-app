param location string
param configStoreName string = 'appserviceconfigstore'


resource configStore 'Microsoft.AppConfiguration/configurationStores@2023-03-01' = {
  name: configStoreName
  sku: {
    name: 'standard'
  }
}

output configStoreUri string = configStore.properties.endpoint
