# Pocketable Popcorn.NET #

Pocketable Popcorn.NET helps you organise your favourite movies (or series) by allowing to explore and rate them. Once rated, you can manage them in your personalised watched movie list. Discover new movies by reading details of them, such as the plot, the genre, the director, the actors etc.

The movie data comes from the OMDb API.

The program was written in C# and uses Windows Forms as the user interface.

Inspired by usePopcorn by Jonas Schmedtmann from The Ultimate React Course.

### Getting to work in Windows CE 2.0 and newer ###

If your version of Windows CE is 3.0 or .NET 4.0 then you will have to download the proper CAB file from: https://www.hpcfactor.com/scl/1083/Microsoft_Corporation/.net_Compact_Framework/version_1.0_SP3. In case of newer versions, it is recommended to download the MSI installer.

For an older version, you will have to use an unofficial port done by dstefanov. Go to the appropriate link and download the package from there: 

* Microsoft .NET Compact Framework 1.0 SP3 for Windows CE 2.00/2.01: https://www.hpcfactor.com/scl/1040/dstefanov/Microsoft_.NET_Compact_Framework_1.0_SP3_for_Windows_CE_2.00/2.01

* Microsoft .NET Compact Framework 1.0 SP3 for Windows CE 2.11: https://www.hpcfactor.com/scl/1041/dstefanov/Microsoft_.NET_Compact_Framework_1.0_SP3_for_Windows_CE_2.11

NOTE: In order to launch Pocketable Popcorn.NET via dstefanov's port, change the extension of "Pocketable Popcorn.NET.exe" from .exe to .net. Its icon will disappear in the File Explorer but no panic - the application will still identify itself with it in runtime.

### Handling the program ###

After initialisation, a window with a search bar and two lists will be displayed.

The list on the left stores the results of the latest search while the other one is used for displaying movies watched by the user - once they rate a movie, it is treated as watched. Every change made to the watched movie list is automatically saved to the file "WatchedMovies.dat" in the program's root directory.

To read the watched movie statistics, click on the ellipsis button and then select "Movies you watched". You will see a dialog box with the details about: 

* the quantity of watched movies, 

* average IMDb rating (0-10), 

* average user rating (0-10), 

* average runtime (in mins).

### Recommended programming setup ###

Source OS: Windows 2000 or newer

Target OS: Windows supporting .NET Framework 1.1 (Windows NT 4.0 SP6a+/Windows 98+) or .NET Compact Framework 1.0 SP3 (Windows CE 2.0+)

IDE: Visual Studio .NET 2003 Enterprise Architect

Additional info: 

* Please replace the value of `EncryptedOmdbApiKey` in GlobalConstants.cs with your active API key.

* Please replace the value of `EncryptionKey` in GlobalConstants.cs with the XOR cipher key that you used to encrypt your API key.

### Program licence ###

Copyright (c) by PeCeT_full 2026. Pocketable Popcorn.NET is published under The MIT License. Please refer to the LICENSE.txt file of this repository for more information regarding it.

### Screenshots ###

<img width="640" height="240" alt="Pocketable Popcorn.NET on Windows CE 2.11" src="https://github.com/user-attachments/assets/d46167c7-c7a1-47d3-b4ee-fbde7ddd01dc" />

<img width="640" height="240" alt="The movie details window" src="https://github.com/user-attachments/assets/34ab213f-f823-4a44-b926-1f485ea8eeb7" />

<img width="640" height="480" alt="Pocketable Popcorn.NET on Windows CE .NET 4.1" src="https://github.com/user-attachments/assets/1d2d81bb-524b-4bbd-b2a8-7780153090c8" />

<img width="608" height="420" alt="Pocketable Popcorn.NET on Windows 98" src="https://github.com/user-attachments/assets/65a5cd10-756f-41ed-8008-047de952a9f2" />

<img width="754" height="513" alt="Pocketable Popcorn.NET on Windows 11" src="https://github.com/user-attachments/assets/e19c3e85-e924-47ab-bee6-4fb5ddc62b11" />

### Videos ###

[![Pocketable Popcorn.NET — Demonstration [Channel's 20th Anniversary Special]](https://img.youtube.com/vi/YtooI8R2SSk/0.jpg)](https://www.youtube.com/watch?v=YtooI8R2SSk)
