//   PryanetShare, a collaboration and sharing tool.
//   Copyright (C) 2010  Hylke Bons <hylkebons@gmail.com>
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program. If not, see <http://www.gnu.org/licenses/>.


using System;
using System.IO;
using System.Collections.Generic;

namespace PryanetLib {

    public enum PryanetChangeType {
        Added,
        Edited,
        Deleted,
        Moved
    }


    public class PryanetChangeSet {

        public PryanetUser User = new PryanetUser ("Unknown", "Unknown");

        public PryanetFolder Folder;
        public Uri RemoteUrl;

        public string Revision;
        public DateTime Timestamp;
        public DateTime FirstTimestamp;
        public List<PryanetChange> Changes = new List<PryanetChange> ();
    }


    public class PryanetChange {

        public PryanetChangeType Type;
        public string Path;
        public string MovedToPath;
        public DateTime Timestamp;
    }


    public class PryanetFolder {

        public string Name;
        public Uri RemoteAddress;

        public string FullPath {
            get {
                string custom_path = PryanetConfig.DefaultConfig.GetFolderOptionalAttribute (Name, "path");

                if (custom_path != null)
                    return Path.Combine (custom_path, Name);
                else
                    return Path.Combine (PryanetConfig.DefaultConfig.FoldersPath, Name);
            }
        }


        public PryanetFolder (string name)
        {
            Name = name;
        }
    }
}
