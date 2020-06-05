# tretton37
My attempt to a solution for the C# code assignment

Environment
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

Usage
-

To ease argument parsing this project uses "System.CommandLine.DragonFruit". From SiteSaver.ConsoleRunner directory run the following:

    dotnet run -- --help

Scope and Limitations
-
-   Not tested on Windows!
-   No editing of html documents to work offline.
-   No validation that downloaded data actually match what was requested.
-   No resources outside the specified domain is downloaded.
-   If a child directory is specified but contains link to parent the parent will still be downloaded. 
-   Very large sites risk OutOfMemoryException
