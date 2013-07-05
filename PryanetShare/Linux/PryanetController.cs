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
using System.Diagnostics;
using System.IO;

using Gtk;
using Mono.Unix.Native;
using PryanetLib;

namespace PryanetShare {

    public class PryanetController : PryanetControllerBase {

        public PryanetController () : base ()
        {
        }


        public override string PluginsPath {
            get {
                return new string [] { Defines.INSTALL_DIR, "plugins" }.Combine ();
            }
        }


        // Creates a .desktop entry in autostart folder to
        // start PryanetShare automatically at login
        public override void CreateStartupItem ()
        {
            string autostart_path = Path.Combine (
                Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "autostart");

            string desktopfile_path = Path.Combine (autostart_path, "pryanetshare.desktop");

            if (!Directory.Exists (autostart_path))
                Directory.CreateDirectory (autostart_path);

            if (!File.Exists (desktopfile_path)) {
                try {
                    File.WriteAllText (desktopfile_path,
                        "[Desktop Entry]\n" +
                        "Type=Application\n" +
                        "Name=PryanetShare\n" +
                        "Exec=pryanetshare start\n" +
                        "Icon=folder-pryanetshare\n" +
                        "Terminal=false\n" +
                        "X-GNOME-Autostart-enabled=true\n" +
                        "Categories=Network");

                    PryanetLogger.LogInfo ("Controller", "Added PryanetShare to login items");

                } catch (Exception e) {
                    PryanetLogger.LogInfo ("Controller", "Failed adding PryanetShare to login items: " + e.Message);
                }
            }
        }
        
        
        public override void InstallProtocolHandler ()
        {
            // pryanetshare-invite-opener.desktop launches the handler on newer
            // systems (like GNOME 3) that implement the last freedesktop.org specs.
            // For GNOME 2 however we need to tell gconf about the protocol manually

            try {
                // Add the handler to gconf...
                Process process = new Process ();
                process.StartInfo.FileName  = "gconftool-2";
                process.StartInfo.Arguments =
                    "-s /desktop/gnome/url-handlers/pryanetshare/command 'pryanetshare open %s' --type String";

                process.Start ();
                process.WaitForExit ();

                // ...and enable it
                process.StartInfo.Arguments = "-s /desktop/gnome/url-handlers/pryanetshare/enabled --type Boolean true";

                process.Start ();
                process.WaitForExit ();

            } catch {
                // Pity...
            }
        }


        // Adds the PryanetShare folder to the user's
        // list of bookmarked places
        public override void AddToBookmarks ()
        {
            string bookmarks_file_path   = Path.Combine (PryanetConfig.DefaultConfig.HomePath, ".gtk-bookmarks");
            string pryanetshare_bookmark = "file://" + FoldersPath + " PryanetShare";

            if (File.Exists (bookmarks_file_path)) {
                string bookmarks = File.ReadAllText (bookmarks_file_path);

                if (!bookmarks.Contains (pryanetshare_bookmark))
                    File.AppendAllText (bookmarks_file_path, "file://" + FoldersPath + " PryanetShare");

            } else {
                File.WriteAllText (bookmarks_file_path, "file://" + FoldersPath + " PryanetShare");
            }
        }


        // Creates the PryanetShare folder in the user's home folder
        public override bool CreatePryanetShareFolder ()
        {
			bool folder_created = false;
			
            if (!Directory.Exists (PryanetConfig.DefaultConfig.FoldersPath)) {
            	Directory.CreateDirectory (PryanetConfig.DefaultConfig.FoldersPath);
                Syscall.chmod (PryanetConfig.DefaultConfig.FoldersPath, (FilePermissions) 448); // 448 -> 700

				PryanetLogger.LogInfo ("Controller", "Created '" + PryanetConfig.DefaultConfig.FoldersPath + "'");
				folder_created = true;
			}

            string gvfs_command_path = new string [] { Path.VolumeSeparatorChar.ToString (),
            	"usr", "bin", "gvfs-set-attribute" }.Combine ();
			
            // Add a special icon to the PryanetShare folder
            if (File.Exists (gvfs_command_path)) {
                Process process = new Process ();
                process.StartInfo.FileName = "gvfs-set-attribute";

                // Give the PryanetShare folder an icon name, so that it scales
                process.StartInfo.Arguments = PryanetConfig.DefaultConfig.FoldersPath +
                    " metadata::custom-icon-name 'pryanetshare'";

                process.Start ();
                process.WaitForExit ();
            }

            return folder_created;
        }
        

        public override string EventLogHTML {
            get {
                string html_path = new string [] { Defines.INSTALL_DIR, "html", "event-log.html" }.Combine ();
                string jquery_file_path = new string [] { Defines.INSTALL_DIR, "html", "jquery.js" }.Combine ();

                string html   = File.ReadAllText (html_path);
                string jquery = File.ReadAllText (jquery_file_path);

                return html.Replace ("<!-- $jquery -->", jquery);
            }
        }

        
        public override string DayEntryHTML {
            get {
                string path = new string [] { Defines.INSTALL_DIR, "html", "day-entry.html" }.Combine ();
                return File.ReadAllText (path);
            }
        }

        
        public override string EventEntryHTML {
            get {
                string path = new string [] { Defines.INSTALL_DIR, "html", "event-entry.html" }.Combine ();
                return File.ReadAllText (path);
            }
        }

            
        public override void OpenFolder (string path)
        {
            OpenFile (path);
        }


        public override void OpenFile (string path)
        {
            Process process             = new Process ();
            process.StartInfo.FileName  = "xdg-open";
            process.StartInfo.Arguments = "\"" + path + "\"";
            process.Start ();
        }


        public override void CopyToClipboard (string text)
        {
            Clipboard clipboard = Clipboard.Get (Gdk.Atom.Intern ("CLIPBOARD", false));
            clipboard.Text      = text;
        }


		public override void OpenWebsite (string url)
		{
			OpenFile (url);
		}
    }
}
