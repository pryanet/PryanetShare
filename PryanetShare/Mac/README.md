## Building on Mac

You can choose to build PryanetShare from source or to download the PryanetShare bundle.


### Installing build requirements

Install [Xcode](https://developer.apple.com/xcode/), the [Mono Framework](http://www.mono-project.com/) 
and [MonoDevelop](http://monodevelop.com/).

Start MonoDevelop and install the MonoMac add-in (it's in the menus: <tt>MonoDevelop</tt> > <tt>Add-in Manager</tt>).


You may need to adjust some environment variables to let the build environment tools find mono:
   
```bash
$ export PATH=/Library/Frameworks/Mono.framework/Versions/Current/bin:$PATH
$ export PKG_CONFIG=/Library/Frameworks/Mono.framework/Versions/Current/bin/pkg-config
$ export PKG_CONFIG_PATH=/Library/Frameworks/Mono.framework/Versions/Current/lib/pkgconfig
```

Install <tt>git</tt>, <tt>automake</tt>, <tt>libtool</tt>, <tt>pkgconfig</tt> and <tt>intltool</tt> using <tt>MacPorts</tt>:

```bash
$ sudo port install git-core automake intltool pkgconfig libtool
```

Get a [Git](http://code.google.com/p/git-osx-installer/) install, and place both the `bin` and `libexec` directories in `PryanetShare/Mac/git`.
The exact commands depend on where you installed/have Git. Assuming it's in `/usr/local`:

```bash
$ mkdir PryanetShare/Mac/git
$ cp -R /usr/local/git/bin PryanetShare/Mac/git
$ cp -R /usr/local/git/libexec PryanetShare/Mac/git
```

Start the first part of the build:

```bash
$ ./autogen.sh
```

Now that you have compiled the libraries, open `PryanetShare/Mac/PryanetShare.sln` in
MonoDevelop and start the build (Build > Build All).

If you get `Are you missing a using directive or an assembly reference?` errors related to MacOS objects, then run:

```
git clone https://github.com/mono/monomac
git clone https://github.com/mono/maccore
cd monomac
make
```

It should generate `MonoMac.dll`. Copy it over any `MonoMac.dll` you might have on your system, then restart Monodevelop, and the project should now build fine.

### Creating a Mac bundle

To create the <tt>PryanetShare.app</tt> select <tt>Build</tt> from the menu bar 
and click <tt>"Build PryanetShare"</tt>.

You'll find a PryanetShare.app in PryanetShare/Mac/bin. Now we need to copy some files over:

```
cp PryanetShare/Mac/config PryanetShare.app/Contents/MonoBundle/config
cp /Library/Frameworks/Mono.framework/Versions/Current/lib/libintl.dylib PryanetShare.app/Contents/Resources
```

To play nice with GateKeeper, open `PryanetShare.app/Contents/Info.plist` and remove the `CFBundleResourceSpecification` property.

**Note:** Adjust `PryanetShare.app/Contents/...` to where you saved the bundle.

Now you have a working bundle that you can run by double-clicking.


### Resetting PryanetShare settings

```
rm -Rf ~/PryanetShare
rm -Rf ~/.config/pryanetshare
```


### Uninstalling

Simply remove the PryanetShare bundle.

