#!/bin/sh

# Script is for docker compose tests where the script is at the root level
dotnet test tests/SS.DAL.Tests/SS.DAL.Tests.csproj --no-build -c Release -r ../TestResults --logger "trx;LogFileName=SS-DAL-Tests-TestResults.trx" \
"/p:CollectCoverage=true" \
"/p:CoverletOutput=/Coverage/" \
"/p:MergeWith=/Coverage/coverage.json" \
"/p:CoverletOutputFormat=\"json,cobertura\"" 