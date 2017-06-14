Write-Host (Get-Item -Path ".\" -Verbose).FullName

# Perform a submodule init
git submodule init

# Step into build-template directory and invoke the build script.

Set-Location .\build-template
Invoke-Expression -Command .\build.ps1