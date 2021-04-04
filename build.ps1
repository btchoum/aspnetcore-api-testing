dotnet restore .\src\ServiceDesk.sln

dotnet build .\src\ServiceDesk.sln --no-restore

dotnet test .\src\ServiceDesk.sln --no-build
