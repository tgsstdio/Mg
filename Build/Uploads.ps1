function Upload-NugetAssemblies([String] $Packages, [String] $Configuration)
{
    $DefaultSourceURL = "https://www.nuget.org/api/v2/package"
    $SourceURL = Read-Host -Prompt "Input nuget source URL [$($DefaultSourceURL)]?"
    if ($SourceURL -eq "")
    {
        $SourceURL = $DefaultSourceURL
    }

    echo $SourceURL

    $ProjectRoot = (Get-Item -Path "..\" -Verbose).FullName;

    $ApiKey = Read-Host -Prompt 'Input nuget API key?'

    $Db = Get-Content -Raw $Packages | ConvertFrom-Json

    $NugetCmd = $Db.nugetCmd

    foreach($package in $db.projects)
    {

        #RUN NUGET
        $Folder = Join-Path $ProjectRoot $package.name
        #echo $folder

        if (Test-Path $Folder)
        {
            $OutputFile = Join-Path '.\' ($package.name + '.' + $package.nuget + ".nupkg");
            
            Write-Host -ForegroundColor Yellow -BackgroundColor DarkCyan "Build if missing $($package.name) -> $OutputFile"

            if (-Not (Test-Path $OutputFile))
            {
                foreach($NuspecFile in 
                    (Join-Path $Folder ($package.name + ".csproj")),
                    (Join-Path $Folder ($package.name + ".nuspec"))
                    )
                {            
                    Write-Host -ForegroundColor Magenta " ... Building $NuspecFile" 

                    if (Test-Path $NuspecFile)
                    {
                        &$NugetCmd pack -symbols $NuspecFile -Prop Configuration=Release
                        break;
                    }
                }
            }
        }
    }
}

Upload-NugetAssemblies("nugets.json");