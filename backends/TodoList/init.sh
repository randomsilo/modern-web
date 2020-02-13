
#/bin/bash
PROJECT=TodoList
# brashcli - generated project initialization script

## File System
mkdir -p $PROJECT.Api
mkdir -p $PROJECT.Domain
mkdir -p $PROJECT.Infrastructure
mkdir -p $PROJECT.Infrastructure.Test/TestOutput

## Solution
dotnet new sln

## Project
cd $PROJECT.Api
dotnet new webapi
cd ..

cd $PROJECT.Domain
dotnet new classlib
cd ..

cd $PROJECT.Infrastructure
dotnet new classlib
cd ..

cd $PROJECT.Infrastructure.Test
dotnet new xunit
cd ..

## Projects to Solution
dotnet sln $PROJECT.sln add $PROJECT.Api/$PROJECT.Api.csproj
dotnet sln $PROJECT.sln add $PROJECT.Domain/$PROJECT.Domain.csproj
dotnet sln $PROJECT.sln add $PROJECT.Infrastructure/$PROJECT.Infrastructure.csproj
dotnet sln $PROJECT.sln add $PROJECT.Infrastructure.Test/$PROJECT.Infrastructure.Test.csproj

## References

### API
dotnet add $PROJECT.Api/$PROJECT.Api.csproj reference $PROJECT.Domain/$PROJECT.Domain.csproj
dotnet add $PROJECT.Api/$PROJECT.Api.csproj reference $PROJECT.Infrastructure/$PROJECT.Infrastructure.csproj

### Domain

### Infrastructure
dotnet add $PROJECT.Infrastructure/$PROJECT.Infrastructure.csproj reference $PROJECT.Domain/$PROJECT.Domain.csproj

### Infrastructure Test
dotnet add $PROJECT.Infrastructure.Test/$PROJECT.Infrastructure.Test.csproj reference $PROJECT.Domain/$PROJECT.Domain.csproj
dotnet add $PROJECT.Infrastructure.Test/$PROJECT.Infrastructure.Test.csproj reference $PROJECT.Infrastructure/$PROJECT.Infrastructure.csproj

## Packages

cd $PROJECT.Domain
dotnet add package Dapper
dotnet add package Serilog
dotnet add package Brash
cd ..

cd $PROJECT.Infrastructure
dotnet add package System.Data.SQLite
dotnet add package Dapper
dotnet add package Serilog
dotnet add package Brash
cd ..

cd $PROJECT.Infrastructure.Test
dotnet add package Bogus
dotnet add package Serilog
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit.runner.visualstudio
cd ..

cd $PROJECT.Api
dotnet add package Autofac
dotnet add package Autofac.Extensions.DependencyInjection
dotnet add package Dapper
dotnet add package Serilog
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
cd ..

