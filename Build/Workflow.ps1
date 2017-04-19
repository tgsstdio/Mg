# Code from http://www.luisrocha.net/2009/11/setting-assembly-version-with-windows.html

function Update-Nuspec([String] $localDir, [String] $packageName, [String] $version)
{
    $folder = Join-Path $localDir $package.name
    #echo $folder

    if (Test-Path $folder)
    {
        #echo $db
        #READ .nuspec file
        
        $nuspecFile = Join-Path $folder "package.nuspec"
        foreach($f in 
            (Join-Path $folder ($packageName + ".csproj.nuspec")),
            (Join-Path $folder ($packageName + ".nuspec"))
            )
        {            
            if (Test-Path $f)
            {
                $nuspecFile = $f;
                break;
            }
        }

        #echo $nuspecFile;

        $nuspec = New-Object System.Xml.XmlDocument;
        $nuspec.PreserveWhitespace = $true;
        $nuspec.Load($nuspecFile);
 
        $hasChanged = $nuspec.package.metadata.version -ne $version;

        if ($hasChanged)
        {
            #UPDATE NUGET VERSION of .nuspec file
            $nuspec.package.metadata.version = $version
            $nuspec.Save($nuspecFile);
        }

        return $hasChanged;
    }
    else
    {
        return $false;
    }
}

function Update-AssemblyInfo([String] $localDir, [String] $packageName, [String] $version)
{
    $folder = Join-Path $localDir $package.name
		
	foreach ($fs in Get-ChildItem $folder -Recurse -Filter "AssemblyInfo.cs")
    {
        Write-Host "Updating  '$($fs.FullName)' -> $Version"
    
        $assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
        $fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
        $assemblyVersion = 'AssemblyVersion("' + $version + '")';
        $fileVersion = 'AssemblyFileVersion("' + $version + '")';
        
        (Get-Content $fs.FullName) | ForEach-Object  { 
           % {$_ -replace $assemblyVersionPattern, $assemblyVersion }
        } | Out-File $fs.FullName -encoding UTF8 -force
    }
}

# syncs nuget version and assemblyinfo of all assemblies in json file
function Sync-NugetAssemblies([String] $packages)
{
    $db = Get-Content -Raw $packages | ConvertFrom-Json

    # should scan Magnesium root folder
    $localDir = (Get-Item -Path "..\" -Verbose).FullName;

    foreach($package in $db.projects)
    {
        $needsUpload = Update-Nuspec $localDir $package.name $package.nuget
		
		if ($needsUpload)
		{
			Write-Host "Updating  '$($package.name)' -> $package.nuget"
			
			Update-AssemblyInfo $localDir $package.name $package.version
		}
    }
}

Sync-NugetAssemblies("nugets.json");

#echo $output

# Ensure we haven't run this by accident.
#  $yes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes", "Uploads the packages."
#  $no = New-Object System.Management.Automation.Host.ChoiceDescription "&No", "Does not upload the packages."
  #$options = [System.Management.Automation.Host.ChoiceDescription[]]($no, $yes)
 
  #$result = $host.ui.PromptForChoice("Upload packages", "Do you want to upload the NuGet packages to the NuGet server?", $options, 0)

