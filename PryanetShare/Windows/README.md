## Windows
You can choose to build PryanetShare from source or to run the Windows installer.


### Installing build requirements

Install version 4.0 of the [.NET Framework](http://www.microsoft.com/download/en/details.aspx?id=17851) if you haven't already.

Install [msysGit](http://code.google.com/p/msysgit/downloads/) and copy the contents of the install folder
(`C:\Program Files (x86)\Git` by default) to `C:\path\to\PryanetShare\sources\bin\msysgit\` (in the PryanetShare source directory).

Open a command prompt and execute the following:

```
cd C:\path\to\PryanetShare\sources
cd PryanetShare\Windows
build
```

`C:\path\to\PryanetShare\sources\bin` should now contain `PryanetShare.exe`, which you can run.


### Creating a Windows installer

To create an installer package, install [WiX 3.6](http://wix.codeplex.com/), restart Windows and run:

```
build installer
```

This will create `PryanetShare.msi` in the same directory.


### Resetting PryanetShare settings

Remove `My Documents\PryanetShare` and `AppData\Roaming\pryanetshare` (`AppData` is hidden by default).


### Uninstalling

You can uninstall PryanetShare through the Windows Control Panel.

