Nuget package release workflow

Set api key

0. Create a folder for nuget D:/Nuget
1. Go to D:/Nuget/Debug and run D:/Nuget/Debug/MgDEBUG.bat batch script to build debug packages and symbols
2. Go to D:/Nuget/Release and run D:/Nuget/Release/MgRELEASE.bat batch script to build release packages and symbols

3. Push release packages to nuget.org
4. Push release, debug packages + symbols to symbolsource.org
5. Push release, debug packages + symbols to myget.org

Push
nuget push Release\Magnesium.4.0.0.nupkg <api-key> -Source https://www.nuget.org/api/v2/package
nuget.exe push Release\Magnesium.4.0.0.symbols.nupkg <api-key> -Source https://nuget.gw.symbolsource.org/Public/NuGet

nuget push Release\Magnesium.Vulkan.4.0.1.nupkg <api-key> -Source https://www.nuget.org/api/v2/package

nuget push Release\Magnesium.OpenGL.4.0.0.nupkg <api-key> -Source https://www.nuget.org/api/v2/package

nuget push Magnesium.Metal.4.0.1.nupkg <api-key> -Source https://www.nuget.org/api/v2/package