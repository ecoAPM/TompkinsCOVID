# Tompkins County COVID Numbers

[![Status](https://github.com/ecoAPM/TompkinsCOVID/actions/workflows/run.yml/badge.svg)](https://github.com/ecoAPM/TompkinsCOVID/actions/workflows/run.yml)
[![CI](https://github.com/ecoAPM/TompkinsCOVID/actions/workflows/CI.yml/badge.svg)](https://github.com/ecoAPM/TompkinsCOVID/actions/workflows/CI.yml)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_TompkinsCOVID&metric=coverage)](https://sonarcloud.io/dashboard?id=ecoAPM_TompkinsCOVID)

[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_TompkinsCOVID&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=ecoAPM_TompkinsCOVID)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_TompkinsCOVID&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=ecoAPM_TompkinsCOVID)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=ecoAPM_TompkinsCOVID&metric=security_rating)](https://sonarcloud.io/dashboard?id=ecoAPM_TompkinsCOVID)

A community service project to automatically tweet daily COVID positive cases and vaccination statistics in Tompkins County, NY.

Follow [@TompkinsNYCOVID on Twitter](https://twitter.com/TompkinsNYCOVID) for updates.

## Requirements

- .NET SDK 7
- Twitter API keys set to the following environment variable names:
  - `ConsumerKey`
  - `ConsumerSecret`
  - `AccessKey`
  - `AccessSecret`

## Build from source

```
dotnet restore
dotnet build
dotnet test
```