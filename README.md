# whist
[![Build Status](https://dev.azure.com/jrgfogh/jrgfogh-whist/_apis/build/status/jrgfogh.whist?branchName=master)](https://dev.azure.com/jrgfogh/jrgfogh-whist/_build/latest?definitionId=1&branchName=master)
[![CodeQL](https://github.com/jrgfogh/whist/actions/workflows/codeql.yml/badge.svg)](https://github.com/jrgfogh/whist/actions/workflows/codeql.yml)
[![SonarCloud analysis](https://github.com/jrgfogh/whist/actions/workflows/sonarcloud.yml/badge.svg)](https://github.com/jrgfogh/whist/actions/workflows/sonarcloud.yml)

***NOTE: The game is not finished and not even playable at the moment.***

A whist game implemented with danish rules.

# Technologies Used

* C#
* JavaScript
* .NET Core
* SignalR
* NUnit
* Azure
  * Linux Web App
  * SignalR Service
* React

## Card Assets
The images used for the playing cards are from
https://github.com/digitaldesignlabs/responsive-playing-cards
and are licensed under the LGPL 3.0.

The original cards were created by Chris Aguilar. The Ace of Spades was by Byron Knoll.
The cards were optimized by Mike Hall with help from Warren Lockhart.

# Lessons learned
The main lesson learned for me is how poor the technology stack still is all around.

Microsoft's default React.js template contains a serious security flaw.
The client code contains syntax errors, which could easily have been detected by a lint tool.

The upgrade experience for npm packages is poor. (This is not specific to the .NET platform.)

The upgrade experience to .NET 5 is also poor. The NuGet UI in Visual Studio correctly tells me
that the platform version must be changed before the packages can be changed. However, there
does not seem to be a centralized place to make the change for all projects in the solution.
Each project file has to be opened and edited manually, since the NuGet UI can still not update
the packages after the platform has been changed using search&replace.

The Azure Devops had to be updated too, since it kept clearing the "Stack" settings for the Azure App Service.

---

Open source development licenses provided by JetBrains:

[![JetBrains Logo (Main) logo](https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.svg)](https://jb.gg/OpenSourceSupport)
