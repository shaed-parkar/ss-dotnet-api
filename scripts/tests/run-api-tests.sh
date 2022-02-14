#!/bin/sh

# Script is for docker compose tests where the script is at the root level
dotnet test tests/Api.Tests/Api.Tests.csproj --no-build -c Release -r ../TestResults --logger "trx;LogFileName=Api-Tests-TestResults.trx" \
"/p:CollectCoverage=true" \
"/p:CoverletOutput=/Coverage/" \
"/p:MergeWith=/Coverage/coverage.json" \
"/p:CoverletOutputFormat=\"json,cobertura\"" 