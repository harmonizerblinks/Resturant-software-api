@echo off

dotnet clean
dotnet build /p:DebugType=Full
dotnet minicover instrument --workdir ../ --assemblies Resturant.Test/**/bin/**/*.dll --sources Resturant/**/*.cs --exclude-sources Resturant/Migrations/**/*.cs --exclude-sources Resturant/*.cs --exclude-sources Resturant\DbContext\AppDbContext.cs

dotnet minicover reset --workdir ../

dotnet test --no-build
dotnet minicover uninstrument --workdir ../
dotnet minicover report --workdir ../ --threshold 60