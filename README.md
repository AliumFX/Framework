# Framework

The core framework that allows the building of an AliumFX-based application. `Alium.Core` is used by apps themselves (targeting `netcoreappx.y` or `netxyz`. `Alium.Core.Abstractions` provides shared types that can be consumed by libraries (targeting `netstandardx.y`) without strongly coupling implementations to those libraries.

|Branch|AppVeyor|Azure Pipelines|
|-|-|-|
|`develop`|[![Build status](https://ci.appveyor.com/api/projects/status/8s9rqyb21ipalssv?svg=true)](https://ci.appveyor.com/project/AliumFX/framework/branch/develop)|[![Build Status](https://dev.azure.com/me0128/Alium%20FX/_apis/build/status/AliumFX.Framework?branchName=develop)](https://dev.azure.com/me0128/Alium%20FX/_build/latest?definitionId=1&branchName=develop)|
|`master`|[![Build status](https://ci.appveyor.com/api/projects/status/8s9rqyb21ipalssv/branch/master?svg=true)](https://ci.appveyor.com/project/AliumFX/framework/branch/master)|[![Build Status](https://dev.azure.com/me0128/Alium%20FX/_apis/build/status/AliumFX.Framework?branchName=master)](https://dev.azure.com/me0128/Alium%20FX/_build/latest?definitionId=1&branchName=master)|

[![Build history](https://buildstats.info/appveyor/chart/aliumfx/framework?includeBuildsFromPullRequest=true)](https://buildstats.info/appveyor/chart/aliumfx/framework?includeBuildsFromPullRequest=true)

The Alium Framework provides a baseline set of services for defining a module, extensible application.

## Package status

### Alium.Core
Core implementations, the foundations for an AliumFX-based application.<br />
[![Alium.Core](https://buildstats.info/myget/aliumfx/alium.core)](https://buildstats.info/myget/aliumfx/alium.core)

### Alium.Core.Abstractions
Core abstractions (interfaces and shared types) that allow you to hook into the core module system.<br />
[![Alium.Core.Abstractions](https://buildstats.info/myget/aliumfx/alium.core)](https://buildstats.info/myget/aliumfx/alium.core.abstractions)
