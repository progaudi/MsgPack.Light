$initialVersion = '"version": "0.0.1"'
$version = '"version": "{0}"' -f $args[1]
$project_json = $args[0]

Write-Host "Setting $project_json version to $version..."

$content = (Get-Content $project_json) 
$content = $content -replace $initialVersion ,$version

$content | Out-File $project_json
