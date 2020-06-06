# tretton37
My attempt to a solution for the C# code assignment

Dev Environment
-

__.NET Core SDK:__

    Version:   3.1.300
    Commit:    2475c1295
 
__Runtime Environment:__
    
    OS Name:      ubuntu
    OS Version:   20.04
    OS Platform:  Linux
    RID:         linux-x64
    Base Path:   /usr/share/dotnet/sdk/3.1.300/

Additional Test Environment
-
__.NET Core SDK:__

    Version:   3.1.300
    Commit:    b2475c1295

__Runtime Environment:__

     OS Name:     Windows
     OS Version:  10.0.18362
     OS Platform: Windows
     RID:         win10-x64
     Base Path:   C:\Program Files\dotnet\sdk\3.1.300\

Usage
-

__Run__

To ease argument parsing this project uses "System.CommandLine.DragonFruit". From SiteSaver.ConsoleRunner directory run the following:

    dotnet run -- --help
to list available commands or

    dotnet run 
to run with defaults (domain=https://tretton37.com, directory=[workDir/tretton37com])

__Test__

In directory SiteSaver.Tests

    dotnet test
    


Scope and Limitations
-
-   No editing of html documents to work offline.
-   No validation that downloaded data actually match what was requested.
-   No resources outside the specified domain is downloaded.
-   If a child directory is specified as input but contains link to parent the parent will still be downloaded. 