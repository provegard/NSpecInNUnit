param (
  [Parameter(Mandatory=$true)]
  [string]$apikey
)

$items = (get-childitem  .\NSpecInNUnit\bin\Release -recurse *.nupkg)
if ($items.Count -ne 1) {
  throw "Expected exactly 1 nupkg file"
}

$pkg = $items[0].FullName

write-host "Will push $pkg using API key $apikey"
$result = (read-host "Type 'ok' to continue")
if ($result -ne "ok") {
  throw "Aborted"
}

dotnet nuget push $pkg --api-key $apiKey --source https://www.nuget.org/api/v2/package

