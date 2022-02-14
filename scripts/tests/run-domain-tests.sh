#!/bin/sh

# Script is for docker compose tests where the script is at the root level
dotnet test tests/SS.Domain.Tests/SS.Domain.Tests.csproj --no-build -c Release -r ../TestResults --logger "trx;LogFileName=SS-Domain-Tests-TestResults.trx" \
"/p:CollectCoverage=true" \
"/p:CoverletOutput=/Coverage/" \
"/p:MergeWith=/Coverage/coverage.json" \
"/p:CoverletOutputFormat=\"json,cobertura\"" 