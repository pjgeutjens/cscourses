$project = "cscourses"
$envs = @("dev", "uat", "prd")
$location = "westeurope"
$tableName = "courses"

$resourceGroups = $envs | % { "$project-$_-001" }

Connect-AzAccount -Subscription "Visual Studio Professional"

foreach ($rg in $resourceGroups) {
  New-AzResourceGroup -Name $rg -Location $location
}

$stDev = New-AzStorageAccount -ResourceGroupName $resourceGroups[0] -Name "st$($project)$($envs[0])" -Location $location -SkuName Standard_LRS -Kind StorageV2

$StorageAccountAccessKey = (Get-AzureRmStorageAccountKey -ResourceGroupName $resourceGroups[0] -Name "st$($project)$($envs[0])"

$stDev | Enable-AzStorageStaticWebsite -IndexDocument "index.html" -ErrorDocument404Path "404.html"
