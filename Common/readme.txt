**** Read Me - English ****

Pocketable Popcorn.NET 1.0
Author: PeCeT_full
Website: http://www.pecetfull.pl
Copyright (c) by PeCeT_full 2026. Pocketable Popcorn.NET is published under The MIT License. For more information, please refer to Licence.txt included with the application.
Inspired by usePopcorn by Jonas Schmedtmann from The Ultimate React Course.

If there are any problems or doubts, please contact me.

-------------------
Program description
-------------------

Pocketable Popcorn.NET helps you organise your favourite movies (or series) by allowing to explore and rate them. Once rated, you can manage them in your personalised watched movie list. Discover new movies by reading details of them, such as the plot, the genre, the director, the actors etc.

The movie data comes from the OMDb API.

The program was written in C# and uses Windows Forms as the user interface.

Minimal hardware and system requirements: 

* For Windows 98 or newer: 233 MHz or faster processor; 16 MB of available RAM; 1 MB of free hard disk space available where the program exists; Microsoft .NET Framework 1.1 installed; active Internet connection.

* For Windows CE 2.0 or newer: 133 MHz or faster processor; 8 MB of available RAM; 1 MB of free hard disk space available where the program exists; Microsoft .NET Compact Framework 1.0 SP3 installed; active Internet connection.

-------------------------------------------
Getting to work in Windows CE 2.0 and newer
-------------------------------------------

If your version of Windows CE is 3.0 or .NET 4.0 then you will have to download the proper CAB file from: https://www.hpcfactor.com/scl/1083/Microsoft_Corporation/.net_Compact_Framework/version_1.0_SP3. In case of newer versions, it is recommended to download the MSI installer.

For an older version, you will have to use an unofficial port done by dstefanov. Go to the appropriate link and download the package from there: 

* Microsoft .NET Compact Framework 1.0 SP3 for Windows CE 2.00/2.01: https://www.hpcfactor.com/scl/1040/dstefanov/Microsoft_.NET_Compact_Framework_1.0_SP3_for_Windows_CE_2.00/2.01

* Microsoft .NET Compact Framework 1.0 SP3 for Windows CE 2.11: https://www.hpcfactor.com/scl/1041/dstefanov/Microsoft_.NET_Compact_Framework_1.0_SP3_for_Windows_CE_2.11

NOTE: In order to launch Pocketable Popcorn.NET via dstefanov's port, change the extension of "Pocketable Popcorn.NET.exe" from .exe to .net. Its icon will disappear in the File Explorer but no panic - the application will still identify itself with it in runtime.

--------------------
Handling the program
--------------------

After initialisation, a window with a search bar and two lists will be displayed.

The list on the left stores the results of the latest search while the other one is used for displaying movies watched by the user - once they rate a movie, it is treated as watched. Every change made to the watched movie list is automatically saved to the file "WatchedMovies.dat" in the program's root directory.

To read the watched movie statistics, click on the ellipsis button and then select "Movies you watched". You will see a dialog box with the details about: 

* the quantity of watched movies, 

* average IMDb rating (0-10), 

* average user rating (0-10), 

* average runtime (in mins).
