* Azure Admin
Access Elevated Privileged Operations with Azure Self-Hosted Pipelines

* Features
	* .Net Core Worker Serice
	* Run powershell web requests from Azure yml file
	* Run service as appropriate admin to run elevated commands
    * USE CASE : Remove Appx Package before installation for UI testing

* Quick Publish

```
cd $proj
dotnet restore
dotnet publish -o $Path
```

* Install Service

- In elevated terminal!

```
sc.exe create AzureAdmin binpath= $Path\AzureAdmin.exe
sc.exe start AzureAdmin
```

* Azure Yaml Task Example 

```
- task: PowerShell@2
  inputs:
    targetType: 'inline'
    script: |  
        $url = "http://localhost:5005/removeallappxpackages"
        $pipeline = Invoke-RestMethod -Uri $url -Method Get;
        Write-Host "Pipeline = $($pipeline | ConvertTo-Json -Depth 100)"
```

* Project Goal List 
    * Authentication