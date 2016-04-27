$initialVersion = '"version": "0.0.1"'
$version = '"version": "{0}"' -f $args[0]
$project_json = '..\src\MsgPack.Light\project.json'

Write-Host "Setting $project_json version to $version..."

$content = (Get-Content $project_json) 
$content = $content -replace $initialVersion ,$version

$content | Out-File $project_json
