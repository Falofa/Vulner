using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Security.Principal;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Web;
using System.Management;
using System.Drawing;
using System.Globalization;

namespace Vulner
{
    class Commands
    {
        public Dictionary<string, Command> Get(Main m, TerminalController t)
        {
            Dictionary<string, Command> C = new Dictionary<string, Command>();

            /*
             * Standard colors:
             * Info: 8
             * Warning: E
             * Sucess: A
             * Error: C
             */
            bool __debug__ = false;
#if (DEBUG)
            __debug__ = true;
            #region Example Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    return null;
                },
            }.Save(C, new string[] { "example" }, __debug__);
            #endregion
            #region Todo Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    t.ColorWrite("$cTHINGS TO DO:\n");
                    foreach (KeyValuePair<string, Command> c in m.Cmds)
                    {
                        if (c.Key.Length == 0) { continue; }
                        if (c.Value.Debug) { continue; }
                        List<string> s = new List<string>();
                        if (Equals(c.Value.Help, null))
                        {
                            s.Add("Make help");
                        }
                        else
                        {
                            if (Equals(c.Value.Help.Description, null)) s.Add("Make help description");
                            if (Equals(c.Value.Help.Examples, null) && Equals(c.Value.Help.Usage, null)) s.Add("Make examples or usage");
                        }

                        if (s.Count > 0)
                        {
                            t.ColorWrite("$a{0}", c.Value.Name.ToUpper());
                            foreach (string str in s) { t.ColorWrite("$e {0}", str); }
                            t.WriteLine();
                        }
                    }
                    return null;
                },
            }.Save(C, new string[] { "todo" }, __debug__);
            #endregion
            #region Invs Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    char[] ch = Funcs.InvisibleChars();
                    List<char> c = new List<char>();
                    foreach( char cha in ch )
                    {
                        t.WriteLine("Use this one: A{0}B?", cha);
                        if (t.ReadLine().ToLower().StartsWith("y"))
                        {
                            c.Add(cha);
                        }
                    }
                    t.WriteLine(string.Join("|", c.Select(b => b.ToString()).ToArray()));
                    return null;
                },
            }.Save(C, new string[] { "invs" }, __debug__);
            #endregion
            __debug__ = false;
#endif
            #region Fake Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Populates a folder with fake files",
                    Usage = new string[] {
                        "{NAME} [Subfolder count]"
                    },
                    Examples = new string[] {
                        "{NAME}",
                        "{NAME} 4",
                    }
                },
                Main = (Argumenter a) =>
                {
                    int c = 0;
                    int.TryParse(a.Get(1), out c);
                    if (c <= 0) { c = 2; }

                    List<string> dirs = new List<string>() { "" };
                    List<string> fils = new List<string>();

                    string[] Frmt = new string[]
                    {
                        "txt", "pptx", "docx", "exe", "bat", "exe", "msi", "vbs", "bat",  "doc", "png", "jpg", "jpeg",
                        "gif", "bmp",  "dll",  "sys", "bat", "msc", "ico", "js",  "html", "php", "hta", "css", "mp3",
                        "wav", "mp4",  "avi",  "mkv", "svg"
                    };

                    Func<string, bool, int> f = null;
                    f = (str, last) =>
                    {
                        if (!last)
                        {
                            for (int i = 0; i < Funcs.Rnd(2, 5); i++)
                            {
                                string s = Funcs.RandomString(5, 15);
                                Directory.CreateDirectory(Path.Combine(str, s));
                                dirs.Add(Path.Combine(str, s));
                            }
                        }
                        for (int i = 0; i < Funcs.Rnd(4, 25); i++)
                        {
                            string s = Funcs.RandomString(5, 15);
                            File.WriteAllBytes(string.Format("{0}.{1}", Path.Combine(str, s), Frmt[Funcs.Rnd(0, Frmt.Length - 1)]), Funcs.RandomBytes(6000, 10000000));
                            fils.Add(Path.Combine(str, s));
                        }
                        return 0;
                    };

                    int cnt = 0;
                    int nc = 0;
                    for( int i = 0; i < c; i++ )
                    {
                        nc = dirs.Count;
                        List<string> d = dirs.Skip(cnt).ToList();
                        foreach (string s in d)
                        {
                            f.Invoke(s, i == c);
                        }
                        cnt += nc;
                    }

                    t.ColorWrite("$aCreated {0} directories and {1} files.", dirs.Count, fils.Count);

                    return null;
                },
            }.Save(C, new string[] { "fake" });
            #endregion
            #region Args Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Prints how the arguments got parsed",
                    Examples = new string[]
                    {
                        "{NAME} 'arg1' >"
                    }
                },
                Main = (Argumenter a) =>
                {
                    foreach (KeyValuePair<int, string[]> b in a.FormatStr)
                    {
                        t.ColorWrite("$a{0} - $f{1}", b.Value[0], b.Value[1]);
                    }
                    return null;
                },
            }.Save(C, new string[] { "args" });
            #endregion
            #region Echo Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Echoes back all arguments",
                    Examples = new string[]
                    {
                        "{NAME} '%temp%/test.exe'",
                    }
                },
                Main = (Argumenter a) =>
                {
                    foreach (string s in a.Parsed.Skip(1))
                    {
                        t.WriteLine(s);
                    }
                    return null;
                },
            }.Save(C, new string[] { "echo" });
            #endregion
            #region Nul Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Does absolutely nothing :v",
                    Examples = new string[]
                    {
                        "{NAME}"
                    }
                },
                Main = (Argumenter a) =>
                {
                    return null;
                },
            }.Save(C, new string[] { "nul" });
            #endregion
            #region WhoAmI Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Displays current identity information of Vulner",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {
                    WindowsIdentity c = WindowsIdentity.GetCurrent();
                    t.ColorWrite("$7User Name: $a{0}", Environment.UserName);
                    t.ColorWrite("$7Domain Name: $a{0}", Environment.UserDomainName);
                    t.ColorWrite("$7Name: $a{0}", c.Name);
                    t.ColorWrite("$7Groups:");
                    foreach (var v in c.Groups) {
                        t.ColorWrite("$8 {0}", v.Translate(typeof(NTAccount)).Value );
                    }
                    t.ColorWrite("\n$7Administrator: $a{0}", Funcs.IsAdmin());
                    return null;
                },
            }.Save(C, new string[] { "whoami" });
            #endregion
            #region Users Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Displays all users in this computer",
                    Usage = new string[] {"{NAME}"}
                },
                Main = (Argumenter a) =>
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_UserAccount");
                    t.ColorWrite("$eUsers in this computer:");
                    foreach (ManagementObject envVar in searcher.Get())
                    {
                       t.WriteLine(" {0}", envVar["Name"]);
                    }
                    return null;
                },
            }.Save(C, new string[] { "users" });
            #endregion
            #region Root Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Restarts Vulner as admin",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {
#if (DEBUG)
                    string Command = Environment.GetCommandLineArgs()[0].Replace(".vshost", "");
#else
                    string Command = Environment.GetCommandLineArgs()[0];
#endif
                    string arg = "";
                    foreach( string s in Environment.GetCommandLineArgs().Skip(1) )
                    {
                        arg += string.Format("\"{0}\" ", s.Replace("\"", "\\\""));
                    }
                    if (Funcs.CheckExploit())
                    {
                        Funcs.Exploit(string.Format("\"{0}\" \"root\" {1}", Command, arg));
                        Environment.Exit(0);
                        return null;
                    }
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = Command,
                            Arguments = string.Format( "runas {0}", arg ),
                            Verb = "runas",
                        });
                        Environment.Exit(0);
                        return null;
                    }
                    catch (Exception)
                    {
                        t.WriteLine("Failed to get admin rights");
                    }
                    return null;
                },
            }.Save(C, new string[] { "root" });
#endregion
            #region NetFix Command
                        new Command
                        {
                            Help = new CommandHelp
                            {
                                Description = "Flushes DNS cache and renews ipconfig",
                                Usage = new string[] { "{NAME}" }
                            },
                            Main = (Argumenter a) =>
                            {
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = "ipconfig",
                                    Arguments = "/flushdns",
                                    WindowStyle = ProcessWindowStyle.Hidden,
                                    CreateNoWindow = true,
                                }).WaitForExit();
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = "ipconfig",
                                    Arguments = "/renew",
                                    WindowStyle = ProcessWindowStyle.Hidden,
                                    CreateNoWindow = true,
                                });
                                t.ColorWrite("Ip config renewed and dns cache flushed.");
                                return null;
                            },
                        }.Save(C, new string[] { "netfix" });
            #endregion
            #region Clear Command
                        new Command
                        {
                            Help = new CommandHelp
                            {
                                Description = "Clears screen",
                                Examples = new string[] {"{NAME}"}
                            },
                            Main = (Argumenter a) =>
                            {
                                t.WriteLine(new string('\n', 50));
                                return null;
                            },
                        }.Save(C, new string[] { "clear", "cls" });
            #endregion
            #region DNS Command
                        new Command
                        {
                            Switches = new string[] { "r" },
                            Help = new CommandHelp
                            {
                                Description = "Performs a DNS lookup",
                                Examples = new string[] {
                                    "{NAME} google.com",
                                    "{NAME} google.com /r",
                                    "{NAME} 8.8.8.8 /r",
                                },
                                Usage = new string[] { "{NAME} [hostOrIp]" },
                                Switches = new Dictionary<string, string>() { { "r", "Reverse DNS lookup" } }
                            },
                            Main = (Argumenter a) =>
                            {
                                int DnsTime = ServicePointManager.DnsRefreshTimeout;
                                ServicePointManager.DnsRefreshTimeout = 0;

                                try
                                {
                                    IPAddress[] i = Dns.GetHostAddresses(a.Get(1));
                                    if (a.GetSw("r")) { t.ColorWrite("$e{0} {1}", "[ IP ADDRESS ]".PadRight(16), "[ HOSTNAME ]".PadRight(16)); }
                                    foreach (IPAddress ip in i)
                                    {
                                        if (a.GetSw("r"))
                                        {
                                            try
                                            {
                                                IPHostEntry iph = Dns.GetHostEntry(ip);
                                                t.WriteLine("{0} {1}", ip.ToString().PadRight(16), iph.HostName);
                                            }catch(Exception)
                                            {
                                                t.ColorWrite("{0} $cERROR", ip.ToString().PadRight(16));
                                            }
                                        }
                                        else
                                        {
                                            t.WriteLine("{0}", ip.ToString());
                                        }
                                    }
                                } catch(Exception)
                                {
                                    t.ColorWrite("$cInvalid host.");
                                }

                                ServicePointManager.DnsRefreshTimeout = DnsTime;
                                return null;
                            },
                        }.Save(C, new string[] { "dns" });
            #endregion
            #region Exit Command
                        new Command
                        {
                            Help = new CommandHelp
                            {
                                Description = "Closes Vulner",
                                Usage = new string[] { "{NAME}" }
                            },
                            Main = (Argumenter a) =>
                            {
                                Environment.Exit(0);
                                return null;
                            },
                        }.Save(C, new string[] { "exit" });
            #endregion
            #region Help Command
                        new Command
                        {
                            Help = new CommandHelp
                            {
                                Description = "Displays help messages",
                                Usage = new string[]
                                {
                                    "{NAME}",
                                    "{NAME} command"
                                }
                            },
                            Main = (Argumenter a) =>
                            {
                                a.CaseSensitive = false;
                                string comma = a.GetRaw(1);
                                if (a.Get(1) == string.Empty)
                                {
                                    t.WriteLine("Commands:");
                                    foreach (KeyValuePair<string, Command> c in m.Cmds)
                                    {
                                        t.SetForeColor('f');
                                        if (c.Key == string.Empty) { continue; }
                                        if (c.Value.Help.Description != string.Empty && (c.Value.Alias == c.Key || c.Value.Alias == string.Empty) && c.Key != string.Empty)
                                        {
                                            string[] al = m.Cmds.Where(b => b.Key != "" && b.Value.Alias == c.Key).Select(b => b.Key).ToArray();
                                            t.ColorWrite("$7{0} - $8{1}", string.Join(", ", al).ToUpper().ToUpper(), c.Value.Help.Description);
                                        }
                                        else if (c.Value.Alias == c.Key || c.Value.Alias == string.Empty)
                                        {
                                            string[] al = m.Cmds.Where(b => b.Key != "" && b.Value.Alias == c.Key).Select(b => b.Key).ToArray();
                                            t.ColorWrite("$7{0}", string.Join(", ", al).ToUpper().ToUpper());
                                        }
                                    }
                                }
                                else
                                {
                                    Command e = null;
                                    try
                                    {
                                        e = m.Cmds[a.GetRaw(1)];
                                    }
                                    catch (KeyNotFoundException)
                                    {
                                        t.WriteLine("'{0}' is not a recognized command.", comma);
                                        return 0;
                                    }

                                    if(!Equals(e.Help,null))
                                    {
                                        e.Help.Print(t, e);

                                        string[] al = m.Cmds.Where(b => b.Value.Alias == C[a.Get(1)].Alias).Select(b => b.Key).ToArray();
                                        t.ColorWrite("\n$8Aliases: $7{0}", string.Join(", ", al).ToUpper());
                                    }
                                    else
                                    {
                                        t.WriteLine("Command has no help attached.");
                                    }
                                }
                                return null;
                            }
                        }.Save(C, new string[] { "help" });
            #endregion
            #region Dump Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Writes the output of a command to a file.",
                    Usage = new string[]
                    {
                        "{NAME} [FileName] [Command]",
                        "{NAME} [FileName]",
                    }
                },
                Main = (Argumenter a) =>
                {
                    if (a.Get(1).Length == 0) { return null; }
                    t.StartBuffer();

                    if (a.Get(2).Length > 0)
                    {
                        m.RunCommand(a.Get(2));
                    }

                    byte[] Output = t.EndBuffer().ToCharArray().Select(b => (byte)b).ToArray();

                    try
                    {
                        FileStream fs = File.OpenWrite(a.Get(1));
                        if (Output.Length != 0)
                            for (int i = 0; i < (1 + Output.Length / 1024); i++)
                            {
                                fs.Write(Output, i * 1024, Math.Min(1024, Output.Length - 1024 * i));
                            }
                        fs.Close();
                    }
                    catch (Exception e) { MessageBox.Show(e.Message, "Vulner", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    return null;
                },
            }.Save(C, new string[] { "dump" });
            #endregion
            #region Crypto Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Simple two way cryptography",
                    Usage = new string[] { "{NAME} [filename]" },
                    Examples = new string[]
                    {
                        "{NAME} File.txt",
                        "{NAME} File.txt /pw password"
                    }
                },
                Parameters = new string[]
                {
                    "pw"
                },
                Main = (Argumenter a) =>
                {
                    byte[] ip = (a.GetPr("pw", "") + "\xF4VULNEr").ToCharArray().Select(b => (byte)b).ToArray();
                    byte[] pw = SHA1.Create().ComputeHash(ip);

                    foreach (string b in a.Parsed)
                    {
                        try
                        {
                            byte[] f = File.ReadAllBytes(b);
                            byte r = (byte)0;
                            for (int i = 0; i < f.Length; i++)
                            {
                                r = pw[i % pw.Length];
                                f[i] = (byte)(f[i] ^ (byte)(i * (13 + r)) ^ r);
                            }
                            File.WriteAllBytes(b, f);
                            t.ColorWrite("Cryptographed $a{0} $8({1} bytes)", new FileInfo(b).FullName, f.Length);
                        }
                        catch (Exception) { }
                    }
                    return null;
                },
            }.Save(C, new string[] { "crypto" });
            #endregion
            #region Insert Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Forces a file into any directory",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {

                    FileInfo In = null;
                    FileInfo Out = null;
                    try
                    {
                        In = new FileInfo(Environment.ExpandEnvironmentVariables(a.Get(1)));
                        Out = new FileInfo(Environment.ExpandEnvironmentVariables(a.Get(2)));
                    }
                    catch (Exception)
                    {
                        t.WriteLine("Syntax:\n\tINSERT [FILENAME] [FOLDER]");
                    }

                    FileInfo Tmp = new FileInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%temp%"), Funcs.RandomString(10) + ".cab"));

                    t.WriteLine("Compacting...");
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "makecab.exe",
                        Arguments = string.Format("\"{0}\" \"{1}\" /V1", In.FullName, Tmp.FullName),
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true
                    }).WaitForExit();

                    t.WriteLine("Extracting...");
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "wusa.exe",
                        Arguments = string.Format("\"{0}\" /extract:\"{1}\" /log:\"F:/wusa_log.txt\" /quiet", Tmp.FullName, Out.FullName),
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true
                    }).WaitForExit();

                    Tmp.Delete();

                    FileInfo Fo = new FileInfo(Path.Combine(Out.FullName, In.Name));
                    if (Fo.Exists)
                    {
                        t.ColorWrite("$aFile created: {0}", Fo.FullName);
                    }
                    else
                    {
                        t.ColorWrite("$cFailed to create file: {0}", Fo.FullName);
                    }
                    return null;
                },
            }.Save(C, new string[] { "insert" });
            #endregion
            #region Cat Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Print file contents to t",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {
                    FileInfo f = new FileInfo(a.Get(1));
                    if (f.Exists)
                    {
                        string r = "";
                        try
                        {
                            string s = File.ReadAllText(f.FullName);
                            if (f.Length > 3000)
                            {
                                r += string.Format("File is longer than 3000 bytes({0} bytes)", f.Length);
                            }
                            if (s.Contains('\0'))
                            {
                                if (r.Length > 0)
                                {
                                    r += " and contains binary data";
                                }
                                else
                                {
                                    r += "File contains binary data";
                                }
                            }
                            if (r.Length > 0)
                            {
                                t.ColorWrite("$c{0}.\n$f Do you want to proceed? $aYes$f/$cNo", r);
                                if ((t.ReadLine() + "n").ToLower()[0] != 'y')
                                {
                                    t.ColorWrite("$f> No");
                                    return null;
                                }
                                t.ColorWrite("$f> Yes");
                            }
                            t.SetForeColor(t.ltc['8']);
                            t.WriteLine(s);
                        }
                        catch (IOException ex)
                        {
                            t.ColorWrite("$cAn error has ocourred!\n{0}", ex.Message);
                        }
                    }
                    else
                    {
                        t.ColorWrite("$cFile not found!");
                    }
                    return null;
                },
            }.Save(C, new string[] { "cat" });
            #endregion
            // General File Stuff
            #region CD Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Changes current working directory",
                    Usage = new string[]
                    {
                                    "{NAME} path",
                                    "{NAME}",
                    },
                    Examples = new string[]
                    {
                                    "{NAME} C:/full/path/",
                                    "{NAME} relative/path/",
                                    "{NAME} system32",
                                    "{NAME} .."
                    }
                },
                Main = (Argumenter a) =>
                {
                    a.CaseSensitive = false;
                    bool changed = false;
                    if (a.Get(1) != "")
                    {
                        string[] Paths = new string[] {
                                        Path.Combine(Environment.CurrentDirectory, Environment.ExpandEnvironmentVariables(a.Get(1))),
                                        Environment.ExpandEnvironmentVariables(a.Get(1)),
                                        Environment.GetEnvironmentVariable(a.Get(1)) ?? ""
                        };
                        foreach (string s in Paths)
                        {
                            try
                            {
                                if (new DirectoryInfo(s).Exists && !changed)
                                {
                                    Environment.CurrentDirectory = new DirectoryInfo(s).FullName;
                                    changed = true;
                                    break;
                                }
                            }
                            catch (Exception) { }
                        }
                        foreach (Environment.SpecialFolder s in Enum.GetValues(typeof(Environment.SpecialFolder)))
                        {
                            try
                            {
                                if (a.Get(1) == s.ToString().ToLower() && !changed)
                                {
                                    string p = Environment.GetFolderPath(s);
                                    if (new DirectoryInfo(p).Exists)
                                    {
                                        Environment.CurrentDirectory = new DirectoryInfo(p).FullName;
                                        changed = true;
                                        break;
                                    }
                                }
                            }
                            catch (Exception) { }
                        }
                        foreach (string s in Environment.GetEnvironmentVariable("path").Split(';'))
                        {
                            try
                            {
                                if (new DirectoryInfo(s).Name.ToLower() == a.Get(1) && !changed)
                                {
                                    Environment.CurrentDirectory = new DirectoryInfo(s).FullName;
                                    changed = true;
                                    break;
                                }
                            }
                            catch (Exception) { }
                        }
                        if (!changed)
                        {
                            t.ColorWrite("$cDirectory not found!");
                        }
                    }
                    t.ColorWrite("$fWorking directory: $a{0}", Environment.CurrentDirectory);
                    return null;
                }
            }.Save(C, new string[] { "cd" });
            #endregion
            #region Ls Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Lists files and folders in a directory",
                    Usage = new string[] { "{NAME}" }
                },
                Main = (Argumenter a) =>
                {
                    string M = a.Get(1).Length == 0 ? "*" : a.Get(1);
                    t.ColorWrite("$e{0}", Path.Combine(Environment.CurrentDirectory, M));
                    string[] Af = Funcs.GetFilesSmarty(M, false);
                    Af = Funcs.GetDirsSmarty(M, false).Concat(Af).ToArray();
                    foreach (string Fil in Af)
                    {
                        FileInfo f = new FileInfo(Fil);
                        string attr = "";
                        attr += ((int)f.Attributes & (int)FileAttributes.Directory) == 0 ? '-' : 'D';
                        attr += ((int)f.Attributes & (int)FileAttributes.ReadOnly) == 0 ? '-' : 'R';
                        attr += ((int)f.Attributes & (int)FileAttributes.Hidden) == 0 ? '-' : 'H';
                        attr += ((int)f.Attributes & (int)FileAttributes.System) == 0 ? '-' : 'S';

                        t.ColorWrite("$a{0} $8{1}", attr, f.Name);
                    }
                    return null;
                },
            }.Save(C, new string[] { "ls" });
            #endregion
            #region Del Command
            new Command
            {
                Switches = new string[] { "r", "f" },
                Help = new CommandHelp
                {
                    Description = "Deletes files and folders",
                    Switches = new Dictionary<string, string> { { "r", "Recursive" } },
                    Usage = new string[] {
                        "{NAME} [filename]",
                        "{NAME} [match]",
                    },
                    Examples = new string[]
                    {
                        "{NAME} *.txt $8Deletes all .txt files in current directory",
                        "{NAME} *.txt /r $8Deletes all .txt files in current directory and in all subdirectories",
                        "{NAME} * $8Deletes everything in folder",
                        "{NAME} $8Same effect",
                    }
                },
                Main = (Argumenter a) =>
                { // DirectoryInfo
                    string h = a.Get(1).Length != 0 ? a.Get(1) : "*";

                    new Thread(()=> System.Media.SystemSounds.Hand.Play()).Start();
                    if (!a.GetSw("f"))
                    {
                        t.ColorWrite("$eAre you sure you want to delete: $f{0} $f($aYes$f/$cNo$f)", Path.Combine(Environment.CurrentDirectory, h));
                        if ((t.FancyInput() + "n").ToLower()[0] != 'y') { return null; }
                    }
                    
                    Dictionary<string, int> Exceptions = new Dictionary<string, int>{
                        { "Access", 0 },
                        { "IO", 0 },
                        { "Generic", 0 }
                    };
                    bool R = a.GetSw("r");
                    int FileJump = 1;
                    int DirJump = 1;

                    string CurDir = Environment.CurrentDirectory;

                    string fl = a.Get(1);
                    FileInfo tmpf = null;

                    try
                    {
                        if (new FileInfo(a.Get(1)).DirectoryName != new FileInfo(CurDir).DirectoryName)
                        {
                            tmpf = new FileInfo(a.Get(1));
                            fl = tmpf.Name;
                            h = tmpf.Name;
                            Environment.CurrentDirectory = tmpf.Directory.FullName;
                        }
                    } catch(Exception) { }

                    t.SetForeColor('2');
                    t.WriteLine("Indexing directory...");
                    string[] Files = Funcs.GetFilesSmarty(h, false);
                    string[] Dirs = Funcs.GetDirsSmarty(R ? "*" : h, false);
                    HashSet<string> fls = new HashSet<string>();
                    HashSet<string> drs = new HashSet<string>();
                    try
                    {
                        if (new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, h)).Exists)
                        {
                            drs.Add(Path.Combine(Environment.CurrentDirectory, h));
                        }
                    }
                    catch (Exception) { }
                    foreach (string s in Files) { fls.Add(s); }

                    Func<string[], int> Recursive = null;
                    int count = fls.Count;
                    int oldc = count;

                    t.Write("File count: ");
                    Writable w = t.GetWritable(10);
                    t.WriteLine();

                    int dcount = drs.Count;
                    int doldc = dcount;

                    t.Write("Searched directories: ");
                    Writable dw = t.GetWritable(10);
                    t.WriteLine();

                    w.Write(count);
                    dw.Write(dcount);
                    int time = Environment.TickCount;

                    string Search = R ? fl : "*";
                    Recursive = (d) =>
                    {
                        foreach (string s in d)
                        {
                            dcount++;
                            DirectoryInfo dir = new DirectoryInfo(s);
                            if (!dir.Exists) { return 0; }
                            try
                            {
                                Environment.CurrentDirectory = dir.FullName;
                                string[] F = Funcs.GetFilesSmarty(Search, false);
                                string[] D = Funcs.GetDirsSmarty(Search, false);

                                if (!R) { drs.Add(s); }
                                foreach (string r in F) { fls.Add(r); }
                                count += F.Length + D.Length;
                                if (count - oldc > FileJump)
                                {
                                    FileJump = Math.Min((int)(FileJump * 1.5 + 20), 500);
                                    w.Write(count);
                                    oldc = count;
                                }
                                if (dcount - doldc > DirJump)
                                {
                                    DirJump = Math.Min((int)(DirJump * 1.5 + 20), 500);
                                    dw.Write(dcount);
                                    doldc = dcount;
                                }
                                if (R)
                                {
                                    Recursive.Invoke(Directory.GetDirectories(Environment.CurrentDirectory));
                                }
                                else
                                {
                                    Recursive.Invoke(D);
                                }
                            }
                            catch (IOException) { Exceptions["IO"]++; }
                            catch (UnauthorizedAccessException) { Exceptions["Access"]++; }
                            catch (Exception) { Exceptions["Generic"]++; }
                        }
                        return 0;
                    };
                    Recursive(Dirs); // All indexing is done here
                    Environment.CurrentDirectory = new DirectoryInfo(CurDir).Root.FullName;
                    w.Write(count);
                    dw.Write(dcount);

                    w = null; dw = null;

                    t.WriteLine("Done indexing!");
                    t.WriteLine("The process took {0:0.00} seconds", (Environment.TickCount - time)/1000);
                    t.WriteLine();

                    int all = fls.Count + drs.Count;
                    if (all == 0)
                    {
                        t.ColorWrite("$eNothing found...");
                    }
                    else
                    {
                        t.WriteLine("Removing all attributes...");
                        foreach (string s in fls) { File.SetAttributes(s, 0); }
                        foreach (string s in drs) { File.SetAttributes(s, 0); }
                        t.WriteLine();

                        count = 0;
                        oldc = 0;
                        t.Write("Deleted: ");
                        w = t.GetWritable(20);
                        w.Write(string.Format("{0}/{1} ({2:0.00}%)", count, all, (count / all) * 100));
                        t.WriteLine();

                        foreach (string f in fls)
                        {
                            try
                            {
                                File.Delete(f);
                                count++;
                                if (count - oldc > FileJump)
                                {
                                    FileJump = Math.Min((int)(FileJump * 1.5 + 20), 500);
                                    oldc = count;
                                    w.Write(string.Format("{0}/{1} ({2:0.00}%)", count, all, (count / all) * 100));
                                }
                            }
                            catch (IOException) { Exceptions["IO"]++; }
                            catch (UnauthorizedAccessException) { Exceptions["Access"]++; }
                            catch (Exception) { Exceptions["Generic"]++; }
                        }
                        w.Write(count);
                        foreach (string f in drs.Reverse()) // Subdirectories first
                        {
                            try
                            {
                                Directory.Delete(f);
                                count++;
                                if (count - oldc > FileJump)
                                {
                                    FileJump = Math.Min((int)(FileJump * 1.5 + 20), 500);
                                    oldc = count;
                                    w.Write(string.Format("{0}/{1} ({0:2.00}%)", count, all, (count / all) * 100));
                                }
                            }
                            //catch (IOException) { Exceptions["IO"]++; }
                            //catch (UnauthorizedAccessException) { Exceptions["Access"]++; }
                            catch (Exception e) { MessageBox.Show(e.Message); Exceptions["Generic"]++; }
                        }
                        w.Write(string.Format("{0}/{1} ({2:0.00}%)", count, all, (count / all) * 100));

                        if (Exceptions["IO"] > 0 || Exceptions["Access"] > 0 || Exceptions["Generic"] > 0)
                        {
                            t.ColorWrite("$cErrors during the process:");
                            if (Exceptions["IO"] > 0) { t.ColorWrite("$c File system: {0}", Exceptions["IO"]); }
                            if (Exceptions["Access"] > 0) { t.ColorWrite("$c Unauthorized access: {0}", Exceptions["Access"]); }
                            if (Exceptions["Generic"] > 0) { t.ColorWrite("$c Generic: {0}", Exceptions["Generic"]); }
                        }
                    }
                    Environment.CurrentDirectory = CurDir;

                    return null;
                },
            }.Save(C, new string[] { "del" });
            #endregion
            #region Mv Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Moves files",
                    Usage = new string[]
                    {
                        "{NAME} [match find] [match replace]"
                    },
                    Examples = new string[]
                    {
                        "{NAME} *.* $$i.$$2 $8Renames all files to an increasing integer",
                        "{NAME} *.bin $$1.exe $8Changes all .bin files to .exe",
                        "{NAME} *.* folder/$$0 $8Moves files withour renaming them"
                    }
                },
                Main = (Argumenter a) =>
                {
                    if (a.Get(1).Length > 0 && a.Get(2).Length > 0)
                    {
                        Funcs.RegexRename(a.Get(1), a.Get(2), t);
                    } else
                    {
                        t.WriteLine("Please provide valid input.");
                    }
                    return null;
                },
            }.Save(C, new string[] { "mv" });
            #endregion
            #region Copy Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Moves files",
                    Usage = new string[]
                    {
                        "{NAME} [match find] [match replace]"
                    },
                    Examples = new string[]
                    {
                        "{NAME} *.* $$i.$$2 $8Copies and renames all files to an increasing integer",
                        "{NAME} *.bin $$1.exe $8Copies and renames all .bin files to .exe",
                        "{NAME} *.* folder/$$0 $8Copies files withour renaming them"
                    }
                },
                Main = (Argumenter a) =>
                {
                    if (a.Get(1).Length > 0 && a.Get(2).Length > 0)
                    {
                        Funcs.RegexRename(a.Get(1), a.Get(2), t, true);
                    }
                    else
                    {
                        t.WriteLine("Please provide valid input.");
                    }
                    return null;
                },
            }.Save(C, new string[] { "copy" });
            #endregion
            #region Attrib Command
            FileAttributes AllAttribs = 0;
            foreach (FileAttributes a in Enum.GetValues(typeof(FileAttributes)))
            {
                AllAttribs = AllAttribs | a;
            }
            Dictionary<Char, FileAttributes> AllAttr = new Dictionary<char, FileAttributes>
            {
                { 'r', FileAttributes.ReadOnly },
                { 's', FileAttributes.System },
                { 'h', FileAttributes.Hidden },
                { 't', FileAttributes.Temporary },
                { 'e', FileAttributes.Encrypted },
                { 'o', FileAttributes.Offline },
                { 'n', FileAttributes.Normal },
                { 'c', FileAttributes.Compressed },
                { 'd', FileAttributes.Device },
                { 'i', FileAttributes.NotContentIndexed },
                { 'R', FileAttributes.ReparsePoint },
                { 'P', FileAttributes.SparseFile },
                { '*', AllAttribs }
            };
            new Command
            {
                ParsePR = false,
                ParseSW = false,
                Help = new CommandHelp
                {
                    Description = "File attribute manager",
                    Examples = new string[]
                    {
                        "{NAME} $8Lists attributes and corresponding characters",
                        "{NAME} * -*+h $8Removes all attributes and add hidden attribute to all files",
                        "{NAME} * +hr $8Add readonly and hidden attributes to all files",
                    },
                    Usage = new string[]
                    {
                        "{NAME}",
                        "{NAME} [files]",
                        "{NAME} [files] [-attribs][+attribs]"
                    }
                },
                Main = (Argumenter a) =>
                {
                    if ( a.Get(1).Length == 0 )
                    {
                        foreach(KeyValuePair<Char, FileAttributes> kp in AllAttr)
                        {
                            t.ColorWrite("$a{0} $f- {1}", kp.Key, kp.Value);
                        }
                        return null;
                    }
                    string[] files = Funcs.GetFilesSmarty(a.Get(1), false);
                    string all = "";
                    foreach (KeyValuePair<Char, FileAttributes> kp in AllAttr) { all += kp.Key; }

                    if (a.Get(2).Length > 0)
                    {
                        foreach (string f in files)
                        {
                            FileInfo fl = new FileInfo(f);
                            FileAttributes fa = fl.Attributes;

                            char mode = '+';
                            foreach( char ch in a.Get(2) )
                            {
                                if ("+-".Contains(ch)) { mode = ch; }
                                FileAttributes cur = 0;
                                AllAttr.TryGetValue(ch, out cur);
                                if ((int)cur != 0)
                                {
                                    if (mode == '+')
                                    {
                                        fa = fa | AllAttr[ch];
                                    }
                                    if (mode == '-')
                                    {
                                        fa = (FileAttributes)(fa - (fa & AllAttr[ch]));
                                    }
                                }
                            }

                            fl.Attributes = fa;
                        }
                    }

                    t.ColorWrite( "$e{0} Filename", all );
                    foreach ( string f in files )
                    {
                        FileInfo fl = new FileInfo(f);
                        FileAttributes fa = fl.Attributes;
                        string fi = "";
                        foreach( KeyValuePair<Char,FileAttributes> kp in AllAttr )
                        {
                            if ( (fa & kp.Value) == kp.Value)
                            {
                                fi += kp.Key;
                                continue;
                            }
                            fi += "-";
                        }
                        t.ColorWrite("$a{0} {1}", fi, fl.Name );
                    }
                    return null;
                },
            }.Save(C, new string[] { "attrib" });
            #endregion
            //
            #region Rules Command
            Dictionary<string, string> Rules = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "!CMD", @"Software\Policies\Microsoft\Windows\System;DisableCMD" },
                { "!Run", @"Software\Policies\Microsoft\Windows\System;NoRun" },
                { "!TaskMGR", @"Software\Microsoft\Windows\CurrentVersion\Policies;DisableTaskMgr" },
                { "!MSI", @"Software\Policies\Microsoft\Windows\Installer;DisableMSI" },
                { "!Regedit", @"Software\Microsoft\Windows\CurrentVersion\Policies\System;DisableRegistryTools" },
                { "!ControlPanel", @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer;NoControlPanel" },
                { "!SetBG", @"Software\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop;NoChangingWallPaper" },
                { "!TrayMenu", @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer;NoTrayContextMenu" },
                { "!WindowsUpdate", @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer;NoWindowsUpdate" },
                { "!Shutdown", @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer;NoClose" },
                { "!Install", @"Software\Microsoft\Windows\CurrentVersion\Policies\Uninstall;NoAddRemovePrograms" },
                { "!SetTaskbar", @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer;NoSetTaskbar" },
                { "!Desktop", @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer;NoDesktop" },
                { "!SetPW", @"Software\Microsoft\Windows\CurrentVersion\Policies\System;Disable Change Password" },
            };
            new Command
            {
                Switches = new string[] { "s" },
                Help = new CommandHelp
                {
                    Description = "Changes windows policies",
                    Usage = new string[]
                    {
                        "{NAME}",
                        "{NAME} [rule=$atrue$7] [rule=$cfalse$7]",
                        "{NAME} [rule=$a1$7] [rule=$c0$7]",
                        "{NAME} [rule=$ay$7] [rule=$cn$7]",
                        "{NAME} [rule=$ayes$7] [rule=$cno$7]",
                        "{NAME} [rule=$aenable$7] [rule=$cdisable$7]",
                    },
                    Examples = new string[]
                    {
                        "{NAME}",
                        "{NAME} cmd=disable regedit=enable"
                    },
                    Switches = new Dictionary<string, string>
                    {
                        { "s", "System wide" }
                    }
                },
                Main = (Argumenter a) =>
                {
                    if (a.GetSw("s") && !Funcs.IsAdmin())
                    {
                        t.ColorWrite("$cAdministrator rights required.");
                        return null;
                    }
                    RegistryKey r = Registry.CurrentUser;
                    if (a.GetSw("s")) r = Registry.LocalMachine;

                    if (a.Get(1).Length == 0)
                    {
                        foreach (KeyValuePair<string,string> s in Rules)
                        {
                            bool invert = s.Key.StartsWith("!");
                            string[] v = s.Value.Split(';');
                            object vl = null;
                            string Value = "";
                            try
                            {
                                Funcs.MakeKeyTree(v[0], r);
                                try { vl = r.OpenSubKey(v[0]).GetValue(v[1]); } catch (Exception) { }
                                bool bl = int.Parse((Equals(vl, null) ? "0" : vl).ToString()) != 0;
                                if (invert) bl = !bl;
                                Value = bl ? "$aEnabled" : "$cDisabled";
                            }
                            catch (UnauthorizedAccessException) { Value = "$eAccess Denied"; }
                            catch (Exception) { Value = "$eERROR"; }
                            t.ColorWrite("$6{0} = " + Value, s.Key.Replace("!",""));
                        }
                    }
                    else
                    {
                        foreach (string rs in a.Parsed.Skip(1))
                        {
                            bool invert = false;
                            if (rs.Split('=').Length != 2) { continue; }
                            string res = rs.Split('=')[0].ToLower();
                            string val = rs.Split('=')[1].ToLower();

                            bool VAL = false;
                            if ((val == "true") || 
                                (val + "0")[0] == '1' || 
                                (val + "0")[0] == 'y' ||
                                (val.StartsWith("enable")))
                                VAL = true;

                            if (!Rules.ContainsKey(res))
                            {
                                res = "!" + res;
                                invert = true;
                            }
                            if (Rules.ContainsKey(res))
                            {
                                string s = Rules[res];
                                string[] v = s.Split(';');
                                string Value = "";
                                try
                                {
                                    Funcs.MakeKeyTree(v[0], r);
                                    r.OpenSubKey(v[0], true).SetValue(v[1], (invert ? !VAL : VAL) ? 1 : 0, RegistryValueKind.DWord);
                                    object vl = null;
                                    try { vl = r.OpenSubKey(v[0]).GetValue(v[1]); } catch (Exception) { }
                                    bool bl = int.Parse((Equals(vl, null) ? "0" : vl).ToString()) != 0;
                                    if (invert) bl = !bl;
                                    Value = bl ? "$aEnabled" : "$cDisabled";
                                }
                                catch (UnauthorizedAccessException) { Value = "$eAccess Denied"; }
                                catch (Exception) { Value = "$eERROR"; }
                                t.ColorWrite("$6{0} = " + Value, res.Replace("!", ""));
                            }
                            else
                            {
                                t.ColorWrite("$cInvalid key.");
                            }
                        }
                    }
                    return null;
                },
            }.Save(C, new string[] { "rule" });
            #endregion
            // General Process Stuff
            #region PL Command
            new Command
            {
                Switches = new string[] { "d" },
                Help = new CommandHelp
                {
                    Description = "Lists processes",
                    Usage = new string[]
                    {
                        "{NAME}",
                        "{NAME} /d"
                    },
                    Switches = new Dictionary<string, string> { { "d", "Show Command Line" } },
                },
                Main = (Argumenter a) =>
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process");
                    t.ColorWrite("$eProcess list:");
                    string match = Regex.Escape(a.Get(1)).Replace("\\*", "[.]*").Replace("\\?", ".");
                    bool useRegex = match.Length > 0;


                    foreach (ManagementObject envVar in searcher.Get())
                    {
                        if (useRegex)
                        {
                            if (Regex.Match(envVar["name"].ToString(), match, RegexOptions.IgnoreCase).Success)
                            {
                                if (a.GetSw("d"))
                                {
                                    t.ColorWrite("$f[{0}] {1} - {2} $8{3}\n", envVar["handle"], envVar["name"], envVar["description"], envVar["commandLine"]);
                                }
                                else
                                {
                                    t.ColorWrite("$f[{0}] {1} - {2}", envVar["handle"], envVar["name"], envVar["description"]);
                                }
                            }
                        }
                        else
                        {
                            if (a.GetSw("d"))
                            {
                                t.ColorWrite("$f[{0}] {1} - {2} $8{3}\n", envVar["handle"], envVar["name"], envVar["description"], envVar["commandLine"]);
                            }
                            else
                            {
                                t.ColorWrite("$f[{0}] {1} - {2}", envVar["handle"], envVar["name"], envVar["description"]);
                            }
                        }
                    }
                    t.ColorWrite("$eEnd");
                    return null;
                },
            }.Save(C, new string[] { "pl" });
            #endregion
            #region KPID Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Kills a process with the given id",
                    Usage = new string[]
                    {
                        "{NAME} [process id]"
                    },
                    Examples = new string[]
                    {
                        "{NAME} 12",
                    }
                },
                Main = (Argumenter a) =>
                {
                    foreach (string ar in a.Parsed.Skip(1))
                    {
                        Process p = null;
                        try
                        {
                            p = Process.GetProcessById(int.Parse(ar));
                        }
                        catch (Exception)
                        {
                            t.ColorWrite("$cInvalid process id.");
                            return null;
                        }
                        //t.ColorWrite("$eKilling {0}...", p.ProcessName);
                            string st = string.Format( "$aKilled {0}", p.ProcessName );

                        try { p.Kill(); }
                        catch (UnauthorizedAccessException) { st = "$cAccess Denied"; }
                        catch (Exception) { st = "$cError"; }
                        t.ColorWrite(st);
                    }
                    return null;
                },
            }.Save(C, new string[] { "kpid" });
            #endregion
            #region KP Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Kills processes that match given argument",
                    Usage = new string[]
                    {
                        "{NAME} [process name]"
                    },
                    Examples = new string[]
                    {
                        "{NAME} chrome",
                        "{NAME} * $8Be sure you know what you are doing"
                    }
                },
                Main = (Argumenter a) =>
                {
                    bool suicide = false;
                    foreach (string ar in a.Parsed.Skip(1))
                    {
                        string r = Regex.Escape(ar).Replace(@"\*", ".*").Replace(@"\?", ".");
                        foreach (Process prc in Process.GetProcesses())
                        {
                            if (Regex.Match(prc.ProcessName, r, RegexOptions.IgnoreCase).Success)
                            {
                                if (prc.Id == Process.GetCurrentProcess().Id)
                                {
                                    t.ColorWrite("$f{0}", prc.ProcessName);
                                    suicide = true;
                                    continue;
                                }
                                string st = "$aKilled";

                                try { prc.Kill(); }
                                catch (UnauthorizedAccessException) { st = "$cAccess Denied"; }
                                catch (Exception) { st = "$cError"; }

                                t.ColorWrite("$f{0} - " + st, prc.ProcessName);
                            }
                        }
                        t.WriteLine();
                    }
                    if (suicide) { Process.GetCurrentProcess().Kill(); }
                    return null;
                },
            }.Save(C, new string[] { "kp" });
            #endregion
            #region Start Command
            new Command
            {
                ParsePR = false,
                ParseSW = false,
                Help = new CommandHelp
                {
                    Description = "Starts a process",
                    Usage = new string[]
                    {
                        "{NAME} [file] [argument]"
                    }
                },
                Main = (Argumenter a) =>
                {
                    string cmd = a.Get(1);
                    string arg = a.Get(2);
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = cmd,
                            Arguments = arg,
                            WorkingDirectory = Environment.CurrentDirectory,
                            UseShellExecute = false
                        });
                    } catch(Exception ex) { t.ColorWrite("$c{0}", ex.Message); }
                    return null;
                },
            }.Save(C, new string[] { "start" });
            #endregion
            #region Runas Command
            new Command
            {
                ParsePR = false,
                ParseSW = false,
                Help = new CommandHelp
                {
                    Description = "Starts a process with administrator priviledges",
                    Usage = new string[]
                    {
                        "{NAME} [file] [argument]"
                    }
                },
                Main = (Argumenter a) =>
                {
                    string cmd = a.Get(1);
                    string arg = a.Get(2);
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = cmd,
                            Arguments = arg,
                            WorkingDirectory = Environment.CurrentDirectory,
                            UseShellExecute = false,
                            Verb = "runas",
                        });
                    }
                    catch (Exception ex) { t.ColorWrite("$c{0}", ex.Message); }
                    return null;
                },
            }.Save(C, new string[] { "runas" });
            #endregion
            #region Assoc Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = "Changes file format associations",
                    Usage = new string[]
                    {
                        "{NAME} [extension]=[name]",
                        "{NAME} '[extension].Open=[commandline]'",
                    },
                    Examples = new string[]
                    {
                        "{NAME} txt=TextFile",
                        "{NAME} 'txt.Open=Notepad.exe \"%1\" %*'",
                        "{NAME} txt=TextFile 'txt.Open=Notepad.exe \"%1\" %*'",
                        "{NAME} 'bat.Open=\"cmd.exe /c echo Bat files disabled! & pause\"'",
                    },
                    Switches = new Dictionary<string, string>()
                    {
                        { "u", "User level" },
                        { "m", "Machine level" },
                    }
                },
                Switches = new string[]
                {
                    "u",
                    "m",
                },
                Main = (Argumenter a) =>
                {
                    bool SimpleMode = false;
                    RegistryKey r = null;
                    if ( a.GetSw("u") && a.GetSw("m") )
                    {
                        t.ColorWrite("Switches /u and /m are mutually exclusive!");
                        return null;
                    }
                    if (a.GetSw("u"))
                    {
                        r = Registry.CurrentUser.OpenSubKey("Software\\Classes");
                    }
                    else if (a.GetSw("m"))
                    {
                        r = Registry.LocalMachine.OpenSubKey("Software\\Classes");
                    }
                    else
                    {
                        r = Registry.ClassesRoot;
                    }
                    foreach (string ar in a.Parsed.Skip(1))
                    {
                        string arg = ar;
                        string action = "list";
                        string attr = "";
                        string val = "";
                        bool dot = ar.Contains('.');
                        bool equ = ar.Contains('=');
                        if (dot && equ)
                        {
                            action = "setattr";
                            string[] sep = ar.Split('.');
                            arg = sep[0];
                            attr = ar.Substring(arg.Length + 1).Split('=')[0];
                            val = ar.Substring(arg.Length + 1).Split('=')[1];
                        }
                        else if (equ && !dot)
                        {
                            try
                            {
                                action = "setname";
                                string k = arg.Split('=')[0];
                                string v = arg.Split('=')[1];
                                if (r.OpenSubKey("." + k) == null) { r.CreateSubKey("." + k); }
                                if (r.OpenSubKey(v) == null) { r.CreateSubKey(v); }
                                if (r.OpenSubKey(v + "\\shell") == null) { r.OpenSubKey(v, true).CreateSubKey(v + "\\shell"); }
                                r.OpenSubKey("." + k, true).SetValue("", v);
                                t.ColorWrite("$a.{0} => $f{1}\n", k, v);
                                continue;
                            }
                            catch (Exception ex)
                            {
                                t.ColorWrite("$c{0}", ex.Message);
                            }
                        }
                        else if (dot && !equ)
                        {
                            t.ColorWrite("$cInvalid arguments.");
                        }

                        string rg = "^" + Regex.Escape("." + arg).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";

                        string[] Matched = r.GetSubKeyNames().Where(b => b.StartsWith(".") && Regex.IsMatch(b, rg)).ToArray();

                        if (Matched.Length > 50) { SimpleMode = true; } else { SimpleMode = false; }

                        foreach (string s in Matched)
                        {
                            string b = "";
                            try
                            {
                                b = r.OpenSubKey(s).GetValue("").ToString();
                                RegistryKey o = null;
                                if (r.OpenSubKey(b) == null) { r.CreateSubKey(b); }
                                if (r.OpenSubKey(b + "\\shell") == null) { r.CreateSubKey(b + "\\shell"); }
                                if (action == "setattr")
                                {
                                    o = r.OpenSubKey(b + "\\shell", true);
                                    if (o.OpenSubKey(attr) == null) { o.CreateSubKey(attr); }
                                    if (o.OpenSubKey(attr + "\\command") == null) { o.CreateSubKey(attr + "\\command"); }
                                    o.OpenSubKey(attr + "\\command", true).SetValue("", val);
                                }
                                else
                                {
                                    o = r.OpenSubKey(b + "\\shell");
                                }
                                string[] sbkn = o.GetSubKeyNames();
                                string[] vl = new string[sbkn.Length];
                                int i = 0;
                                foreach (string k in sbkn)
                                {
                                    if (SimpleMode)
                                    {
                                        vl[i++] = string.Format(" {0} -> {1}", k, o.OpenSubKey(k + "\\command").GetValue(""));
                                    }
                                    else
                                    {
                                        vl[i++] = string.Format("$e {0} -> $f{1}", k, o.OpenSubKey(k + "\\command").GetValue(""));
                                    }
                                }

                                if (SimpleMode)
                                {
                                    t.WriteLine("{0} => {1}", s, t);
                                    foreach (string ag in vl) { t.WriteLine(ag); }
                                }
                                else
                                {
                                    t.ColorWrite("$a{0} => $f{1}", s, t);
                                    foreach (string ag in vl) { t.ColorWrite(ag); }
                                }
                                t.WriteLine();
                            }
                            catch (Exception)
                            {
                                if (SimpleMode)
                                {
                                    t.WriteLine("{0} => {1}(ERROR)\n", s, t);
                                }
                                else
                                {
                                    t.ColorWrite("$a{0} => $c{1}(ERROR)\n", s, t);
                                }
                            }
                        }
                    }

                    return null;
                },
            }.Save(C, new string[] { "assoc" });
            #endregion
            #region Scan Command
            new Command
            {
                Switches = new string[] { "s", "q" },
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    bool q = a.GetSw("q");
                    Regex[] rg = new Regex[]
                    {
                        new Regex("([a-z][A-Z][a-z])"),
                        new Regex("([A-Z][a-z][A-Z])"),
                        new Regex("((.)\\2{3,4})", RegexOptions.IgnoreCase),
                        new Regex("([~\\^\\[\\]=\\+\\-;,#\\$!@¨&\\(\\)§´`])", RegexOptions.IgnoreCase),
                    };
                    Regex scrp = new Regex("^[wc]script$", RegexOptions.IgnoreCase);
                    HashSet<string> Folders = new HashSet<string>();
                    HashSet<int> ids = new HashSet<int>();
                    HashSet<string> fls = new HashSet<string>();
                    foreach ( Process p in Process.GetProcesses() )
                    {
                        int dr = 0; // Detection rate
                        List<string> rs = new List<string>(); // Reasons
                        ProcessModule pm = null;
                        string fl = "";
                        try
                        {
                            pm = p.MainModule;
                            fl = pm.FileName;
                        }
                        catch (Exception) { }
                        if (fl == "") { continue; }
                        FileInfo fil = new FileInfo(fl);

                        int cm = 0;
                        //List<string> ab = new List<string>();
                        foreach ( Regex r in rg )
                        {
                            //ab.Add(string.Format( "({0})", string.Join( "; ", mt.Groups.Cast<Group>().Select(t=>t.Value).ToArray())) );
                            foreach ( Match mt in r.Matches(fil.Name))
                            {
                                cm++;
                            }
                        }
                        if ( cm >= 6 )
                        {
                            rs.Add("Erratic file name.");
                            dr += cm * 2;
                        }

                        if (fl != "")
                        {
                            bool hta = false;
                            if (scrp.Match(p.ProcessName).Success || (hta = p.ProcessName.ToLower() == "mshta"))
                            {
                                try
                                {
                                    string str = Funcs.GetCommandline(p);
                                    Match scn = Regex.Match(str, "\"([^\"]+)\"[^\"]*$");
                                    if (scn.Success) {
                                        fls.Add(scn.Groups[1].Value);
                                        if (hta)
                                        {
                                            rs.Add(string.Format("HTA host ({0}).", scn.Groups[1].Value));
                                        }
                                        else
                                        {
                                            rs.Add(string.Format("Script host ({0}).", scn.Groups[1].Value));
                                        }
                                        Folders.Add(new FileInfo(scn.Groups[1].Value).DirectoryName);
                                    }
                                    dr += 20;
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if ( (fil.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                            {
                                rs.Add("File is hidden.");
                                dr += 20;
                            }
                            if ((new FileInfo(fil.DirectoryName).Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                            {
                                rs.Add("Parent folder is hidden.");
                                dr += 20;
                            }
                            if ( fl.ToLower().Contains( Environment.ExpandEnvironmentVariables("%windir%").ToLower()) )
                            {
                                if (pm.FileVersionInfo.CompanyName != "Microsoft Corporation")
                                {
                                    rs.Add("Running from WinDir without Microsoft company name.");
                                    dr += 10;
                                }
                            }
                        }
                        if (fl.ToLower().Contains(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToLower()))
                        {
                            rs.Add("Running from AppData.");
                            dr += 15;
                        }
                        if (fl.ToLower().Contains(Environment.ExpandEnvironmentVariables("%temp%").ToLower()))
                        {
                            rs.Add("Running from Temp.");
                            dr += 20;
                        }
                        if ( dr >= 20 )
                        {
                            ids.Add(p.Id);
                            t.ColorWrite("[{2} | {0}] $c({1})", p.ProcessName, dr, p.Id);
                            foreach( string r in rs )
                            {
                                t.ColorWrite("$e {0}", r);
                            }
                            t.WriteLine();
                        }
                    }
                    if (a.GetSw("s"))
                    {
                        List<string> Commands = new List<string>();
                        if (ids.Count > 0)
                        {
                            Commands.Add(string.Format("kpid {0}", string.Join(" ", ids.Select(b => b.ToString()).ToArray())));
                        }
                        if (fls.Count > 0)
                        {
                            foreach (string s in fls)
                            {
                                Commands.Add(string.Format("del \"{0}\" /f", s.Replace("\\","/")));
                            }
                        }
                        if (!q)
                        {
                            t.ColorWrite("$cThese commands will be run:");
                            foreach (string s in Commands)
                            {
                                t.ColorWrite("$7{0}", s);
                            }
                            t.ColorWrite("$cType Y to confirm");
                            if ((t.ReadLine().ToLower() + " ")[0] != 'y')
                            {
                                return null;
                            }
                        }
                        foreach (string s in Commands)
                        {
                            m.RunCommand(s);
                        }
                    }
                    else
                    {
                        if (Folders.Count > 0)
                        {
                            t.ColorWrite("$2Possible infection folders:");
                            foreach (string s in Folders)
                            {
                                t.ColorWrite("$f {0}", s);
                            }
                        }
                    }
                    return null;
                },
            }.Save(C, new string[] { "scan" });
            #endregion
            #region Update Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    if (Process.GetCurrentProcess().ProcessName.ToLower().Contains(".vshost."))
                    {
                        t.ColorWrite("$cRunning on VSHOST.");
                        return null;
                    }
                    string self = Environment.GetCommandLineArgs()[0];
                    string tmp = Environment.ExpandEnvironmentVariables(string.Format("%temp%/{0}.exe", Funcs.RandomString(10, 20)));
                    File.Copy(self, tmp);
                    Process.Start(tmp, string.Format("update \"{0}\"", self));
                    Process.GetCurrentProcess().Kill();
                    return null;
                },
            }.Save(C, new string[] { "update" });
            #endregion
            // String Modifying Commands
            #region Invisible Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    char[] ch = "​|‌|‍|‎|‏|‪|‬|‭|⁡|⁢|⁣|⁪|⁫|⁬|⁮|⁯".Split('|').Select(b => b[0]).ToArray();
                    foreach (string s in a.Parsed.Skip(1))
                    {
                        string r = "";
                        foreach (char c in s)
                        {
                            r += c;
                            r += ch[Funcs.Rnd(0, ch.Length - 1)];
                        }
                        t.WriteLine(r.Substring(0, r.Length - 1));
                    }
                    return null;
                },
            }.Save(C, new string[] { "invisible" }, __debug__);
            #endregion
            #region Weird Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    char[] ch = new char[] { (char)0x030D, (char)0x030E, (char)0x0304, (char)0x0305, (char)0x033F, (char)0x0311, (char)0x0306, (char)0x0310, (char)0x0352, (char)0x0357, (char)0x0351, (char)0x0307, (char)0x0308, (char)0x030A, (char)0x0342, (char)0x0343, (char)0x0344, (char)0x034A, (char)0x034B, (char)0x034C, (char)0x0303, (char)0x0302, (char)0x030C, (char)0x0350, (char)0x0300, (char)0x0301, (char)0x030B, (char)0x030F, (char)0x0312, (char)0x0313, (char)0x0314, (char)0x033D, (char)0x0309, (char)0x0363, (char)0x0364, (char)0x0365, (char)0x0366, (char)0x0367, (char)0x0368, (char)0x0369, (char)0x036A, (char)0x036B, (char)0x036C, (char)0x036D, (char)0x036E, (char)0x036F, (char)0x033E, (char)0x035B, (char)0x0346, (char)0x031A, (char)0x0315, (char)0x031B, (char)0x0340, (char)0x0341, (char)0x0358, (char)0x0321, (char)0x0322, (char)0x0327, (char)0x0328, (char)0x0334, (char)0x0335, (char)0x0336, (char)0x034F, (char)0x035C, (char)0x035D, (char)0x035E, (char)0x035F, (char)0x0360, (char)0x0362, (char)0x0338, (char)0x0337, (char)0x0361, (char)0x0489, (char)0x0316, (char)0x0317, (char)0x0318, (char)0x0319, (char)0x031C, (char)0x031D, (char)0x031E, (char)0x031F, (char)0x0320, (char)0x0324, (char)0x0325, (char)0x0326, (char)0x0329, (char)0x032A, (char)0x032B, (char)0x032C, (char)0x032D, (char)0x032E, (char)0x032F, (char)0x0330, (char)0x0331, (char)0x0332, (char)0x0333, (char)0x0339, (char)0x033A, (char)0x033B, (char)0x033C, (char)0x0345, (char)0x0347, (char)0x0348, (char)0x0349, (char)0x034D, (char)0x034E, (char)0x0353, (char)0x0354, (char)0x0355, (char)0x0356, (char)0x0359, (char)0x035A, (char)0x0323 };
                    foreach (string s in a.Parsed.Skip(1))
                    {
                        string r = "";
                        foreach (char c in s)
                        {
                            r += c;
                            for (int i = 0; i < Funcs.Rnd(5, 10); i++)
                            {
                                r += ch[Funcs.Rnd(0, ch.Length - 1)];
                            }
                        }
                        t.WriteLine(r);
                    }
                    return null;
                },
            }.Save(C, new string[] { "weird" }, __debug__);
            #endregion
            //
            #region Hosts Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    string hosts = Environment.ExpandEnvironmentVariables("%windir%\\system32\\drivers\\etc\\hosts");
                    bool write = false;
                    try
                    {
                        File.OpenWrite(hosts).Close();
                        write = true;
                    }
                    catch(Exception) { } // Unauthorized!
                    if (!File.Exists(hosts))
                    {
                        if (write)
                        {
                            File.WriteAllBytes(hosts, new byte[0]);
                        }
                        else
                        {
                            t.ColorWrite("$cFile not found: $f{0}", hosts);
                            t.ColorWrite("$cRestart as admin to fix this problem.");
                            return null;
                        }
                    }

                    Dictionary<string, string> n = new Dictionary<string, string>();
                    if ( write )
                    {
                        foreach( string st in a.Parsed.Skip(1) )
                        {
                            string[] b = null;
                            if ((b = st.Split('=')).Length != 2) continue;
                            if ( Regex.IsMatch(b[1], @"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}") || string.IsNullOrEmpty(b[1]) )
                            {
                                n[b[0]] = b[1];
                            } else
                            {
                                t.ColorWrite("$cInvalid IP Address: $f{0}", b[1] );
                            }
                        }
                    }

                    Dictionary<string, string> h = new Dictionary<string, string>();
                    Action ReadHosts = () =>
                    {
                        foreach (string s in File.ReadAllLines(hosts))
                        {
                            string str = s.Trim();
                            string[] b = null;
                            if (str.StartsWith("#")) continue;
                            if ((b = str.Split(' ').Where(c => c.Trim() != "").ToArray()).Length != 2) continue;

                            h[b[1]] = b[0]; // H[ Domain ] = IP
                        }
                    };

                    ReadHosts();

                    if (write && n.Count != 0)
                    {
                        foreach(KeyValuePair<string,string> k in n)
                        {
                            if (string.IsNullOrEmpty(k.Value))
                            {
                                h.Remove(k.Key);
                            }
                            else
                            {
                                h[k.Key] = k.Value;
                            }
                        }
                        string res = "";
                        foreach (KeyValuePair<string, string> k in h)
                        {
                            res += string.Format("{0} {1}\n", k.Value, k.Key);
                        }
                        MessageBox.Show(res);
                    }


                    foreach (KeyValuePair<string, string> k in h)
                    {
                        t.ColorWrite("$e{0} = $f{1}", k.Key, k.Value);
                    }
                    
                    return null;
                },
            }.Save(C, new string[] { "hosts" }, __debug__);
            #endregion
            #region Install Command
            new Command
            {
                Help = new CommandHelp
                {
                    Description = ""
                },
                Main = (Argumenter a) =>
                {
                    string path = Environment.ExpandEnvironmentVariables("%windir%\\Vulner.exe");
                    try
                    {
                        if (File.Exists(path)) new FileInfo(path).Delete();
                        new FileInfo(Environment.GetCommandLineArgs()[0].Replace(".vshost", "")).CopyTo(path);
                        m.RunCommand("assoc 'fal=Vulner' 'fal.Open=Vulner.exe \"%1\" %*'");
                        t.ColorWrite("$aVulner copied to Windows directory!");
                    }
                    catch (Exception e)
                    {
                        t.ColorWrite("$c{0}.", e.Message);
                    }
                    return null;
                },
            }.Save(C, new string[] { "install" }, __debug__);
            #endregion
            #region Path Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    string p = Environment.GetEnvironmentVariable("PATH");
                    foreach( string s in p.Split(';') )
                    {
                        t.ColorWrite("$a{0}", s);
                    }
                    return null;
                },
            }.Save(C, new string[] { "path" }, __debug__);
            #endregion
            #region Size Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    int w = 0;
                    int h = 0;
                    int.TryParse(a.Get(1), out w);
                    int.TryParse(a.Get(2), out h);
                    if (w == 0) w = Console.WindowHeight;
                    if (w == 0) h = Console.WindowWidth;

                    w = Math.Min(Math.Max(w, 80), Console.LargestWindowWidth);
                    h = Math.Min(Math.Max(h, 25), Console.LargestWindowHeight);

                    Console.BufferWidth = Console.WindowWidth = w;
                    Console.WindowHeight = h;
                    t.WriteLine("Size = {0},{1}", w, h);
                    return null;
                },
            }.Save(C, new string[] { "size" }, __debug__);
            #endregion
            #region Find Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    string r = "";
                    string p = a.Get(1);
                    string[] frmts = new string[] { "exe", "bat", "vbs", "vb", "com", "js", "hta" };
                    foreach (string s in Environment.GetEnvironmentVariable("path").Split(';'))
                    {
                        try
                        {
                            FileInfo f = new FileInfo(Path.Combine(s, p));
                            if (f.Exists) { r = f.FullName; break; }
                        }
                        catch (Exception) { }
                        if (!p.Contains('.'))
                        {
                            foreach (string frm in frmts)
                            {
                                try
                                {
                                    FileInfo f = new FileInfo(Path.Combine(s, p + "." + frm));
                                    if (f.Exists) { r = f.FullName; break; }
                                }
                                catch (Exception) { }
                            }
                        }
                    }
                    if (r == "")
                    {
                        foreach (Environment.SpecialFolder sf in Enum.GetValues(typeof(Environment.SpecialFolder)))
                        {
                            string s = Environment.GetFolderPath(sf);
                            try
                            {
                                FileInfo f = new FileInfo(Path.Combine(s, p));
                                if (f.Exists) { r = f.FullName; break; }
                            }
                            catch (Exception) { }
                            if (!p.Contains('.'))
                            {
                                foreach (string frm in frmts)
                                {
                                    try
                                    {
                                        FileInfo f = new FileInfo(Path.Combine(s, p + "." + frm));
                                        if (f.Exists) { r = f.FullName; break; }
                                    }
                                    catch (Exception) { }
                                }
                            }
                        }
                    }
                    t.WriteLine("{0}", r);
                    return null;
                },
            }.Save(C, new string[] { "find" }, __debug__);
            #endregion
            #region Hide Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    foreach( string c in a.Parsed.Skip(1) )
                    {
                        t.hide = true;
                        m.RunCommand(c);
                    }
                    t.hide = false;
                    return null;
                },
            }.Save(C, new string[] { "hide" }, __debug__);
            #endregion
            #region Example Command
            Dictionary<string, MessageBoxIcon> ms = new Dictionary<string, MessageBoxIcon>()
            {
                { "", MessageBoxIcon.None },
                { "none", MessageBoxIcon.None },

                { "x", MessageBoxIcon.Error },
                { "*", MessageBoxIcon.Asterisk },
                { "!", MessageBoxIcon.Exclamation },
                { "?", MessageBoxIcon.Question },

                { "info", MessageBoxIcon.Information },
                { "hand", MessageBoxIcon.Hand },
                { "stop", MessageBoxIcon.Stop },
                { "warn", MessageBoxIcon.Warning },
            };
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    if (!ms.ContainsKey(a.Get(3))) { return null; }
                    string title = a.Get(2);
                    MessageBoxIcon mbb = ms[a.Get(3)];
                    if (title.Length == 0) { title = "Vulner"; }
                    MessageBox.Show(a.Get(1), title, MessageBoxButtons.OK, mbb);
                    return null;
                },
            }.Save(C, new string[] { "msgbox" }, __debug__);
            #endregion

            return C;
        }
    }
}
