# Stop any running backend processes
Stop-Process -Name FMG_Backend,dotnet,iisexpress -ErrorAction SilentlyContinue -Force

# Remove old build outputs
Remove-Item -Recurse -Force .\bin, .\obj -ErrorAction SilentlyContinue

# Restore dependencies and rebuild
dotnet restore
dotnet build
