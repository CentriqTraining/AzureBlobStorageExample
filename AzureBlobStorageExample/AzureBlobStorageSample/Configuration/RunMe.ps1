# Promp for user login
az login 

# Create a resource group called CentriqAzureDemo
#   We'll use this only for our demo so we can easily clean up
az group create -n CentriqAzureDemo -l eastus

#  Create the AzureDemo Storage Account
az storage account create --kind Storage -n centriqstoragedemo -g CentriqAzureDemo -l eastus

# Query the Keys for this new Redis CAche and place it in a json file under our
#  configuration directory to be used when we run 
az storage account show-connection-String --n centriqstoragedemo > Config.json

Write-Host "==============================================================="
Write-Host "Ready to use your Storage Account..."
Write-Host "You can now build your project and run"