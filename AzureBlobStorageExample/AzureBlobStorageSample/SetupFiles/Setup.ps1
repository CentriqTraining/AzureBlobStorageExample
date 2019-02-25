Write-Host ("***************************************************************************")
Write-Host ("**  Azure Account Setup  **************************************************")
Write-Host ("***************************************************************************")
$acctDetails=Get-AzureRmContext
if ([string]::IsNullOrEmpty($acctDetails.Account)) {
    $null=Connect-AzureRmAccount 
}
$subscriptions=Get-AzureRmSubscription -- | Select -ExpandProperty Name
Write-Host "* Logged in"

if (@($subscriptions).Count > 1) {
    $title="Choose subscription"
    $info="Choose the subscription to use for this demo.  Resources will be created on your behalf"
    $options=[System.Management.Automation.Host.ChoiceDescription[]]$subscriptions
    $optionChosen=$host.UI.PromptForChoice($title, $info, $options, 0)
}
else {
  $optionChosen = $subscriptions
}

Select-AzureRmSubscription $optionChosen
Write-Host "* Subscription set - $($optionChosen)"

$demoResourceGroupName="centriqazuredemo"
$resourceGroupList=Get-AzureRmResourceGroup | Select -ExpandProperty ResourceGroupName
if ($demoResourceGroupName -in $resourceGroupList) {
    Write-Host "* Found $($demoResourceGroupName) in account"
} else {
    Write-Host "* Creating $($demoResourceGroupName)..." -NoNewline
    $null=New-AzureRmResourceGroup -Name $demoResourceGroupName -Location 'East US' -ErrorAction SilentlyContinue -ErrorVariable $rgfail
    if ($rgfail) {
        Write-Host "* ERROR: Creating $($demoResourceGroupName)"
    } else {
        Write-Host "Success" 
    }
}

$demoStorageAccountName="centriqazuredemostorage"
$storageAccountNames=Get-AzureRmStorageAccount | Select -ExpandProperty StorageAccountName
if ($demoStorageAccountName -in @($storageAccountNames)) {
    Write-Host "* Found $($demoStorageAccountName) in account"
} else {
    Write-Host "* Creating $($demoStorageAccountName)..." -NoNewline
    $null=New-AzureRmStorageAccount -Location 'East US' -Name $demoStorageAccountName -ResourceGroupName $demoResourceGroupName -SkuName Standard_LRS -ErrorVariable $safail
    if ($safail) {
        Write-Host "* Error: Creating $($demoStorageAccountName)"
    } else {
        Write-Host "Success"
    }
}

Write-Host "* Writing key file keys.config"

$acct = Get-AzureRmStorageAccount -Name $demoStorageAccountName -ResourceGroupName $demoResourceGroupName
$Finalkey = (Get-AzureRmStorageAccountKey -StorageAccountName $demoStorageAccountName -ResourceGroupName $demoResourceGroupName)[1]


Set-Content ../../SetupFiles/Keys.config "DefaultEndpointsProtocol=https;AccountName=$($acct.StorageAccountName);AccountKey=$($Finalkey.Value)" -Force