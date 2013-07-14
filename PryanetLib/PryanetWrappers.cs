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
        public string Revision;
        public DateTime Timestamp;
        public DateTime FirstTimestamp;
        public Uri RemoteUrl;

        public List<PryanetChange> Changes = new List<PryanetChange> ();

        public string ToMessage ()
        {
            string message = "added: {0}";
            
            switch (Changes [0].Type) {
            case PryanetChangeType.Edited:  message = "edited: {0}"; break;
            case PryanetChangeType.Deleted: message = "deleted: {0}"; break;
            case PryanetChangeType.Moved:   message = "moved: {0}"; break;
            }

            if (Changes.Count > 0)
                return string.Format (message, Changes [0].Path);
            else
                return "did something magical";
        }
    }


    public class PryanetChange {

        public PryanetChangeType Type;
        public DateTime Timestamp;
        public bool IsFolder = false;
        
        public string Path;
        public string MovedToPath;
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


    public class PryanetAnnouncement {

        public readonly string FolderIdentifier;
        public readonly string Message;


        public PryanetAnnouncement (string folder_identifier, string message)
        {
            FolderIdentifier = folder_identifier;
            Message          = message;
        }
    }
}
