@set DOTNET_CLI_TELEMETRY_OPTOUT=1

@pushd src\app\DediLib

dotnet restore src\DediLib.Core.sln

msbuild /p:Configuration=Release /p:OutputPath=%~dp0build || (pause && exit /b 1)

@popd
