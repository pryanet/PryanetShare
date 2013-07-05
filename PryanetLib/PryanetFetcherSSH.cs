using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace PryanetLib {

    public abstract class PryanetFetcherSSH : PryanetFetcherBase {

        public PryanetFetcherSSH (PryanetFetcherInfo info) : base (info)
        {
        }


        public override bool Fetch ()
        {
            if (RemoteUrl.Host.EndsWith(".onion")) {
                // Tor has special domain names called ".onion addresses".  They can only be
                // resolved by using a proxy via tor. While the rest of the openssh suite
                // fully supports proxying, ssh-keyscan does not, so we can't use it for .onion
                PryanetLogger.LogInfo ("Auth", "using tor .onion address skipping ssh-keyscan");
            } else if (!RemoteUrl.Scheme.StartsWith ("http")) {
                string host_key = FetchHostKey ();
                
                if (string.IsNullOrEmpty (RemoteUrl.Host) || host_key == null) {
                    PryanetLogger.LogInfo ("Auth", "Could not fetch host key");
                    this.errors.Add ("error: Could not fetch host key");
                    
                    return false;
                }
                
                bool warn = true;
                if (RequiredFingerprint != null) {
                    string host_fingerprint;

                    try {
                        host_fingerprint = DeriveFingerprint (host_key);
                    
                    } catch (InvalidOperationException e) {
                        // "Unapproved cryptographic algorithms" won't work when FIPS is enabled on Windows.
                        // Software like Cisco AnyConnect can demand this feature is on, so we show an error
                        PryanetLogger.LogInfo ("Auth", "Unable to derive fingerprint: ", e);
                        this.errors.Add ("error: Can't check fingerprint due to FIPS being enabled");
                        
                        return false;
                    }


                    if (host_fingerprint == null || !RequiredFingerprint.Equals (host_fingerprint)) {
                        PryanetLogger.LogInfo ("Auth", "Fingerprint doesn't match");
                        this.errors.Add ("error: Host fingerprint doesn't match");
                        
                        return false;
                    }
                    
                    warn = false;
                    PryanetLogger.LogInfo ("Auth", "Fingerprint matches");
                    
                } else {
                    PryanetLogger.LogInfo ("Auth", "Skipping fingerprint check");
                }
                
                AcceptHostKey (host_key, warn);
            }
            
            return true;
        }


        private string FetchHostKey ()
        {
            PryanetLogger.LogInfo ("Auth", "Fetching host key for " + RemoteUrl.Host);
            
            Process process = new Process ();
            process.StartInfo.FileName               = "ssh-keyscan";
            process.StartInfo.WorkingDirectory       = PryanetConfig.DefaultConfig.TmpPath;
            process.StartInfo.UseShellExecute        = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow         = true;
            process.EnableRaisingEvents              = true;
            
            string [] key_types = {"rsa", "dsa", "ecdsa"};
            
            foreach (string key_type in key_types) {
                if (RemoteUrl.Port < 1)
                    process.StartInfo.Arguments = "-t " + key_type + " -p 22 " + RemoteUrl.Host;
                else
                    process.StartInfo.Arguments = "-t " + key_type + " -p " + RemoteUrl.Port + " " + RemoteUrl.Host;
                
                PryanetLogger.LogInfo ("Cmd", process.StartInfo.FileName + " " + process.StartInfo.Arguments);
                
                process.Start ();
                string host_key = process.StandardOutput.ReadToEnd ().Trim ();
                process.WaitForExit ();
                
                if (process.ExitCode == 0 && !string.IsNullOrWhiteSpace (host_key))
                    return host_key;
            }
            
            return null;
        }
        
        
        private string DeriveFingerprint (string public_key)
        {
            try {
                MD5 md5            = new MD5CryptoServiceProvider ();
                string key         = public_key.Split (" ".ToCharArray ()) [2];
                byte [] b64_bytes  = Convert.FromBase64String (key);
                byte [] md5_bytes  = md5.ComputeHash (b64_bytes);
                string fingerprint = BitConverter.ToString (md5_bytes);
                
                return fingerprint.ToLower ().Replace ("-", ":");
                
            } catch (Exception e) {
                PryanetLogger.LogInfo ("Fetcher", "Failed creating fingerprint: " + e.Message + " " + e.StackTrace);
                return null;
            }
        }
        
        
        private void AcceptHostKey (string host_key, bool warn)
        {
            string ssh_config_path       = Path.Combine (PryanetConfig.DefaultConfig.HomePath, ".ssh");
            string known_hosts_file_path = Path.Combine (ssh_config_path, "known_hosts");
            
            if (!File.Exists (known_hosts_file_path)) {
                if (!Directory.Exists (ssh_config_path))
                    Directory.CreateDirectory (ssh_config_path);
                
                File.Create (known_hosts_file_path).Close ();
            }
            
            string host                 = RemoteUrl.Host;
            string known_hosts          = File.ReadAllText (known_hosts_file_path);
            string [] known_hosts_lines = File.ReadAllLines (known_hosts_file_path);
            
            foreach (string line in known_hosts_lines) {
                if (line.StartsWith (host + " "))
                    return;
            }
            
            if (known_hosts.EndsWith ("\n"))
                File.AppendAllText (known_hosts_file_path, host_key + "\n");
            else
                File.AppendAllText (known_hosts_file_path, "\n" + host_key + "\n");
            
            PryanetLogger.LogInfo ("Auth", "Accepted host key for " + host);
            
            if (warn)
                this.warnings.Add ("The following host key has been accepted:\n" + DeriveFingerprint (host_key));
        }
    }
}
