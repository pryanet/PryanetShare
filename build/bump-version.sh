#!/bin/bash

if [ "$1" = "" ]; then
  echo "No version number specified. Usage: ./bump-version.sh VERSION_NUMBER"
else
  sed -i.bak "s/ Version='[^']*'/ Version='$1'/" ../PryanetShare/Windows/PryanetShare.wxs
  sed -i.bak "s/assembly:AssemblyVersion *(\"[^\"]*\")/assembly:AssemblyVersion (\"$1\")/" ../PryanetLib/Defines.cs                 
  sed -i.bak "s/m4_define(.pryanetshare_version[^)]*)/m4_define([pryanetshare_version], [$1])/" ../configure.ac
  cat ../PryanetShare/Mac/Info.plist | eval "sed -e '/<key>CFBundleShortVersionString<\/key>/{N;s#<string>.*<\/string>#<string>$1<\/string>#;}'" > ../PryanetShare/Mac/Info.plist.tmp
  cat ../PryanetShare/Mac/Info.plist.tmp | eval "sed -e '/<key>CFBundleVersion<\/key>/{N;s#<string>.*<\/string>#<string>$1<\/string>#;}'" > ../PryanetShare/Mac/Info.plist
  rm ../PryanetShare/Mac/Info.plist.tmp
  rm ../PryanetShare/Windows/PryanetShare.wxs.bak
  rm ../PryanetLib/Defines.cs.bak
  rm ../configure.ac.bak
fi

