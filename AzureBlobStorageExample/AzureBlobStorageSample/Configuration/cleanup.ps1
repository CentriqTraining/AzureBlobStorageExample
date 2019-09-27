# Promp for user login
az login 

# clean up
az group delete -n CentriqAzureDemo 

Remove-Item config.json
Write-Host "==============================================================="
Write-Host "Ready to use your Storage Account..."
Write-Host "You can now build your project and run"