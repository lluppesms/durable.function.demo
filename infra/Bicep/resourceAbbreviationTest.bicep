output appServicePlanSuffix string = 'appsvc'
output webSiteSuffix string = 'web'
output sqlAbbreviation string = 'sql'
output storageAccountSuffix string = 'store'
output appInsightsSuffix string = 'insights'
output logWorkspaceSuffix string = 'logworkspace'
output bastionAbbreviation string = 'bastion'
output privateEndpointAbbreviation string = 'pe'
output publicIpAbbreviation string = 'pip'
output vnetAbbreviation string = 'vnet'
output networkSecurityGroupAbbreviation string = 'nsg'
output keyVaultAbbreviation string = 'vault'

// --------------------------------------------------------------------------------------------------------------------------------------------
// this version works too if you want to group these variables...
// // var app = {
// //     appServicePlanSuffix: 'appsvc'
// //     webSiteSuffix: 'web'
// // }
// // var data = {      
// //     sqlAbbreviation: 'sql'
// //     storageAccountSuffix: 'store'
// // }
// // var monitor = {      
// //     appInsightsSuffix: 'insights'
// //     logWorkspaceSuffix: 'logworkspace'
// // }
// // var network = {      
// //     bastionAbbreviation: 'bastion'
// //     privateEndpointAbbreviation: 'pe'
// //     publicIpAbbreviation: 'pip'
// //     vnetAbbreviation: 'vnet'
// //     networkSecurityGroupAbbreviation: 'nsg'
// // }
// // var security = {      
// //     keyVaultAbbreviation: 'vault'
// // }
// // output app object = app
// // output data object = data
// // output monitor object = monitor
// // output network object = network
// // output security object = security


// --------------------------------------------------------------------------------------------------------------------------------------------
// trying to figure out how to create an ARRAY of objects that I can loop through, but I can't get this to compile and work...
// // var environmentSettings = {
// //     test: {
// //       instanceSize: 'Small'
// //       instanceCount: 1
// //     }
// //     prod: {
// //       instanceSize: 'Large'
// //       instanceCount: 4
// //     }
// //   }

// // var objectExample = {
// //     app: {
// //         appServicePlanSuffix: 'appsvc'
// //         webSiteSuffix: 'web'
// //     }

// //     data: {
// //         sqlAbbreviation: 'sql'
// //         storageAccountSuffix: 'store'
// //     }
// //     monitor: {
// //         appInsightsSuffix: 'insights'
// //         logWorkspaceSuffix: 'logworkspace'
// //     }
// //     network: {
// //         bastionAbbreviation: 'bastion'
// //         privateEndpointAbbreviation: 'pe'
// //         publicIpAbbreviation: 'pip'
// //         vnetAbbreviation: 'vnet'
// //         networkSecurityGroupAbbreviation: 'nsg'
// //     }
// //     security: {
// //         keyVaultAbbreviation: 'vault'
// //     }
// // }
