$initialVersion = '"version": "0.0.1"'
$version = '"version": "0.0.{0}"' -f $env:appveyor_build_version 
$project_json = $args[0]

Write-Host "Setting $project_json version to $version..."

$content = (Get-Content $project_json) 
$content = $content -replace $initialVersion ,$version

$content | Out-File $project_json
