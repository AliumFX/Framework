# Framework

The core framework that allows the building of an AliumFX-based application. `Alium.Core` is used by apps themselves (targeting `netcoreappx.y` or `netxyz`. `Alium.Core.Abstractions` provides shared types that can be consumed by libraries (targeting `netstandardx.y`) without strongly coupling implementations to those libraries.

[![Build status](https://ci.appveyor.com/api/projects/status/8s9rqyb21ipalssv?svg=true)](https://ci.appveyor.com/project/AliumFX/framework) [![Build status](https://dev.azure.com/aliumfx/AliumFx%20-%20Framework/_apis/build/status/AliumFx%20-%20Framework-CI)](https://dev.azure.com/aliumfx/AliumFx%20-%20Framework/_build/latest?definitionId=1)

[![Build history](https://buildstats.info/appveyor/chart/aliumfx/framework?includeBuildsFromPullRequest=true)](https://buildstats.info/appveyor/chart/aliumfx/framework?includeBuildsFromPullRequest=true)

The Alium Framework provides a baseline set of services for defining a module, extensible application.

## Package status

### Alium.Core
Core implementations, the foundations for an AliumFX-based application.<br />
[![Alium.Core](https://buildstats.info/myget/aliumfx/alium.core)](https://buildstats.info/myget/aliumfx/alium.core)

### Alium.Core.Abstractions
Core abstractions (interfaces and shared types) that allow you to hook into the core module system.<br />
[![Alium.Core.Abstractions](https://buildstats.info/myget/aliumfx/alium.core)](https://buildstats.info/myget/aliumfx/alium.core.abstractions)
