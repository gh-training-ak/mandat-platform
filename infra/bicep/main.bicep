targetScope = 'resourceGroup'

@description('Environment short name, used in every resource name.')
@allowed([ 'dev', 'test', 'prod' ])
param environment string

@description('Azure region for all resources.')
param location string = resourceGroup().location

@description('Container image tag deployed to the app.')
param imageTag string

var namePrefix = 'mandat-${environment}'
var tags = {
  application: 'mandat'
  environment: environment
  managedBy: 'bicep'
}

module logAnalytics 'modules/log-analytics.bicep' = {
  name: 'law-${environment}'
  params: {
    name: '${namePrefix}-law'
    location: location
    tags: tags
  }
}

module sql 'modules/sql.bicep' = {
  name: 'sql-${environment}'
  params: {
    serverName: '${namePrefix}-sql'
    databaseName: 'mandat'
    location: location
    tags: tags
    skuName: environment == 'prod' ? 'S3' : 'Basic'
  }
}

module containerApp 'modules/container-app.bicep' = {
  name: 'app-${environment}'
  params: {
    name: '${namePrefix}-api'
    location: location
    tags: tags
    imageTag: imageTag
    logAnalyticsId: logAnalytics.outputs.workspaceId
    minReplicas: environment == 'prod' ? 3 : 1
    maxReplicas: environment == 'prod' ? 20 : 3
  }
}

output apiFqdn string = containerApp.outputs.fqdn
output sqlServerName string = sql.outputs.serverName
