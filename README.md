# Aurelia Global Tools (agt)
> Get up and running with Aurelia and .NET Core quickly and easily via the CLI

Intended as a conversation starting point. (Abandoned for reasons to be outlined later.)

![](https://aurelia.io/media/aurelia-logo.png)

## Installation

Windows:

Clone and build in Visual Studio 2017

Install from solution root:

```dotnet tool install --global --add-source ./CLIC/nupkg clic```

Uninstall from solution root:

```dotnet tool uninstall --global clic```

Use:

Currently only works with a single project type: (Using Visual Studio 2017 (installationVersion: 15.9.28307.344))

File > New Project > Visual C# > .NET Core > ASP.NET Core Web Application > Change Authentication (Individual User Accounts)

Start global tool: ```agt```

```add -t {template number} -p {path to .sln} -ib```(add template number to specified path, install dependencies and build Aurelia app.)

or, use -h if already at that location in console:

```add -t {template number} -h -ib```
(add template number here, install dependencies and build Aurelia app.)

OS X & Linux:

(Not tested)

```...```