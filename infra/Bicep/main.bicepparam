// ----------------------------------------------------------------------------------------------------
// Bicep Parameter File
// ----------------------------------------------------------------------------------------------------

using './main.bicep'

param appName = '#{appName}#'
param environmentCode = '#{environmentNameLower}#'
param location = '#{location}#'
param storageSku = '#{storageSku}#'
param functionAppSku = '#{functionAppSku}#'
param functionAppSkuFamily = '#{functionAppSkuFamily}#'
param functionAppSkuTier = '#{functionAppSkuTier}#'
param keyVaultOwnerUserId = '#{keyVaultOwnerUserId}#'
param twilioAccountSid = '#{twilioAccountSid}#'
param twilioAuthToken = '#{twilioAuthToken}#'
param twilioPhoneNumber = '#{twilioPhoneNumber}#'
param runDateTime = '#{runDateTime}#'
