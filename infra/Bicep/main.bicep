// --------------------------------------------------------------------------------
// Main Bicep file that creates all of the Azure Resources for one environment
// --------------------------------------------------------------------------------
// To deploy this Bicep manually:
// 	 az login
//   az account set --subscription <subscriptionId>
//
//   Test azd deploy:
//     az deployment group create -n main-deploy-20221115T150000Z --resource-group rg_durable_azd  --template-file 'main.bicep' --parameters appName=lll-dur-azd environmentCode=azd keyVaultOwnerUserId=xxxxxxxx-xxxx-xxxx
//   Test AzDO Pipeline deploy:
//     az deployment group create -n main-deploy-20221115T150000Z --resource-group rg_durable_azdo --template-file 'main.bicep' --parameters appName=lll-dur-dev environmentCode=dev keyVaultOwnerUserId=xxxxxxxx-xxxx-xxxx
// --------------------------------------------------------------------------------
param appName string = ''
@allowed(['azd','gha','azdo','dev','demo','qa','stg','ct','prod'])
param environmentCode string = 'dev'
param location string = resourceGroup().location
param keyVaultOwnerUserId string = ''

// optional parameters
@allowed(['Standard_LRS','Standard_GRS','Standard_RAGRS'])
param storageSku string = 'Standard_LRS'
param functionAppSku string = 'Y1'
param functionAppSkuFamily string = 'Y'
param functionAppSkuTier string = 'Dynamic'
param twilioAccountSid string = ''
param twilioAuthToken string = ''
param twilioPhoneNumber string = ''

param runDateTime string = utcNow()

// --------------------------------------------------------------------------------
var deploymentSuffix = '-${runDateTime}'
var commonTags = {         
  LastDeployed: runDateTime
  Application: appName
  Environment: environmentCode
}

// --------------------------------------------------------------------------------
module resourceNames 'resourcenames.bicep' = {
  name: 'resourcenames${deploymentSuffix}'
  params: {
    appName: appName
    environmentCode: environmentCode
    functionStorageNameSuffix: 'func'
    dataStorageNameSuffix: 'data'
  }
}

// --------------------------------------------------------------------------------
module logAnalyticsWorkspaceModule 'loganalyticsworkspace.bicep' = {
  name: 'logAnalytics${deploymentSuffix}'
  params: {
    logAnalyticsWorkspaceName: resourceNames.outputs.logAnalyticsWorkspaceName
    location: location
    commonTags: commonTags
  }
}

// --------------------------------------------------------------------------------
module functionStorageModule 'storageaccount.bicep' = {
  name: 'functionstorage${deploymentSuffix}'
  params: {
    storageSku: storageSku
    storageAccountName: resourceNames.outputs.functionStorageName
    location: location
    commonTags: commonTags
  }
}

module functionModule 'functionapp.bicep' = {
  name: 'function${deploymentSuffix}'
  dependsOn: [ functionStorageModule ]
  params: {
    functionAppName: resourceNames.outputs.functionAppName
    functionAppServicePlanName: resourceNames.outputs.functionAppServicePlanName
    functionInsightsName: resourceNames.outputs.functionInsightsName

    appInsightsLocation: location
    location: location
    commonTags: commonTags

    functionKind: 'functionapp,linux'
    functionAppSku: functionAppSku
    functionAppSkuFamily: functionAppSkuFamily
    functionAppSkuTier: functionAppSkuTier
    functionStorageAccountName: functionStorageModule.outputs.name
    workspaceId: logAnalyticsWorkspaceModule.outputs.id
  }
}

module dataStorageModule 'storageaccount.bicep' = {
  name: 'datastorage${deploymentSuffix}'
  params: {
    storageAccountName: resourceNames.outputs.dataStorageName
    location: location
    commonTags: commonTags
    storageSku: storageSku
  }
}
module keyVaultModule 'keyvault.bicep' = {
  name: 'keyvault${deploymentSuffix}'
  dependsOn: [ functionModule ]
  params: {
    keyVaultName: resourceNames.outputs.keyVaultName
    location: location
    commonTags: commonTags
    adminUserObjectIds: [ keyVaultOwnerUserId ]
    applicationUserObjectIds: [ functionModule.outputs.principalId ]
    workspaceId: logAnalyticsWorkspaceModule.outputs.id
  }
}

module keyVaultSecretList 'keyvaultlistsecretnames.bicep' = {
  name: 'keyVault-Secret-List-Names${deploymentSuffix}'
  dependsOn: [ keyVaultModule ]
  params: {
    keyVaultName: keyVaultModule.outputs.name
    location: location
    userManagedIdentityId: keyVaultModule.outputs.userManagedIdentityId
  }
}

module keyVaultSecret1 'keyvaultsecret.bicep' = {
  name: 'keyVaultSecret1${deploymentSuffix}'
  dependsOn: [ keyVaultModule, functionModule ]
  params: {
    keyVaultName: keyVaultModule.outputs.name
    secretName: 'TwilioAccountSid'
    secretValue: twilioAccountSid
    existingSecretNames: keyVaultSecretList.outputs.secretNameList
  }
}

module keyVaultSecret2 'keyvaultsecret.bicep' = {
  name: 'keyVaultSecret2${deploymentSuffix}'
  dependsOn: [ keyVaultModule, functionModule ]
  params: {
    keyVaultName: keyVaultModule.outputs.name
    secretName: 'TwilioAuthToken'
    secretValue: twilioAuthToken
    existingSecretNames: keyVaultSecretList.outputs.secretNameList
  }
}

module keyVaultSecret3 'keyvaultsecret.bicep' = {
  name: 'keyVaultSecret3${deploymentSuffix}'
  dependsOn: [ keyVaultModule, functionModule ]
  params: {
    keyVaultName: keyVaultModule.outputs.name
    secretName: 'TwilioPhoneNumber'
    secretValue: twilioPhoneNumber
    existingSecretNames: keyVaultSecretList.outputs.secretNameList
  }
}
module keyVaultSecret4 'keyvaultsecretstorageconnection.bicep' = {
  name: 'keyVaultSecret4${deploymentSuffix}'
  dependsOn: [ keyVaultModule, dataStorageModule ]
  params: {
    keyVaultName: keyVaultModule.outputs.name
    secretName: 'DataStorageConnectionAppSetting'
    storageAccountName: dataStorageModule.outputs.name
    existingSecretNames: keyVaultSecretList.outputs.secretNameList
  }
}
module functionAppSettingsModule 'functionappsettings.bicep' = {
  name: 'functionAppSettings${deploymentSuffix}'
  params: {
    functionAppName: functionModule.outputs.name
    functionStorageAccountName: functionStorageModule.outputs.name
    functionInsightsKey: functionModule.outputs.insightsKey
    customAppSettings: {
      OpenApi__HideSwaggerUI: 'false'
      OpenApi__HideDocument: 'false'
      OpenApi__DocTitle: 'Durable Functions Demo APIs'
      OpenApi__DocDescription: 'This repo is an example of how to use Durable Azure Functions'
      TwilioAccountSid: '@Microsoft.KeyVault(VaultName=${keyVaultModule.outputs.name};SecretName=TwilioAccountSid)'
      TwilioAuthToken: '@Microsoft.KeyVault(VaultName=${keyVaultModule.outputs.name};SecretName=TwilioAuthToken)'
      TwilioPhoneNumber: '@Microsoft.KeyVault(VaultName=${keyVaultModule.outputs.name};SecretName=TwilioPhoneNumber)'
      DataStorageConnectionAppSetting: '@Microsoft.KeyVault(VaultName=${keyVaultModule.outputs.name};SecretName=DataStorageConnectionAppSetting)'
    }
  }
}

output SUBSCRIPTION_ID string = subscription().subscriptionId
output RESOURCE_GROUP_NAME string = resourceGroup().name
