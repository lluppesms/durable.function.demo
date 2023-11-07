# Set up an Azure DevOps Variable Groups

## Note: The Azure DevOps pipelines needs a variable group named "DurableDemo"

To create these variable groups, customize and run this command in the Azure Cloud Shell:

``` bash
   az login

   az pipelines variable-group create 
     --organization=https://dev.azure.com/<yourAzDOOrg>/ 
     --project='<yourAzDOProject>' 
     --name DurableFunctions
     --variables 
         appName='<yourInitials>-durablea' 
         serviceConnectionName='<yourServiceConnection>' 
         azureSubscription='<yourAzureSubscriptionName>' 
         subscriptionId='<yourSubscriptionId>' 
         location='eastus' 
         storageSku='Standard_LRS' 
         functionAppSku='Y1' 
         functionAppSkuFamily='Y' 
         functionAppSkuTier='Dynamic' 
         resourceGroupPrefix='rg_durable_function'
         runSecurityDevOpScan='false'
         keyVaultOwnerUserId='owner1SID'
         twilioAccountSid='<twilioAccountSid>'
         twilioAuthToken='<twilioAuthToken>'
         twilioPhoneNumber='<twilioPhoneNumber>'
```
