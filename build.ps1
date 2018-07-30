# Step into build-template directory and invoke the build script.

Set-Location .\build-template
Invoke-Expression "& `".\build.ps1`" -Verbosity Diagnostic"