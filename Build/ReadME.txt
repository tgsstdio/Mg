Nuget package release workflow

Powershell scripts for 

1. Workflow.ps1
Updating assembly version of nuget packages and their assemblies according to build/nugets.json
2. Uploads.ps1
Uploading nuget packages according to build/nugets.json

CMD > Powershell -ExecutionPolicy Bypass -File <script>

Manually push to nuget server

D:\Nuget\nuget.exe push .\Magnesium.X.X.X-beta.nupkg <api-key> -Source https://www.nuget.org/api/v2/package