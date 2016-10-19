#!/usr/bin/env bash

#exit if any command fails
set -e

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then  
  rm -R $artifactsFolder
fi

dotnet restore

dotnet build Exrin/Exrin.Abstraction
dotnet build Exrin/Exrin.Common
dotnet build Exrin/Exrin.Insights
dotnet build Exrin/Exrin.Framework

revision=${TRAVIS_JOB_ID:=1}  
revision=$(printf "%04d" $revision) 