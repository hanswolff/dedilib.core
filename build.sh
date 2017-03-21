#!/bin/bash

export DOTNET_CLI_TELEMETRY_OPTOUT=1

echo Current directory: $(pwd)
pushd src/app/DediLib
dotnet restore
dotnet publish -c Release -o $(pwd)/../../build
popd
