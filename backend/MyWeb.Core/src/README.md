# Prerequisite - Install Entity Framework

## install .Net5.0 sdk

can download at https://dotnet.microsoft.com/download

## install Entity Framework tool and Format tool

dotnet tool install --global dotnet-ef
<br>
dotnet tool install --global dotnet-format

# Some dotnet commands

First, change directory to backend dir

## Build

dotnet build

## Run

dotnet run

## Migrate / Update database table

dotnet ef migrations add SOMECOMMITMESSAGE<br />
dotnet ef database update
