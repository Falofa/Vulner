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
using Shell32;

namespace Vulner
{
    class Commands
    {
        public Dictionary<string, Command> Get(Main m, TerminalController Console)
        {
            Dictionary<string, Command> C = new Dictionary<string, Command>();

            /*
             * Standard colors:
             * Info: 8
             * Warning: E
             * Sucess: A
             * Error: C
             */

#if (DEBUG)
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
            }.Save(C, new string[] { "example" });
            #endregion
            #region Example Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    var rng = RandomNumberGenerator.Create();
                    int i = 0;
                    byte[] bt = new byte[256].Select(t => (byte)i++).ToArray();
                    byte[] vl = new byte[1];
                    bt = bt.OrderBy(t => { rng.GetBytes(vl); return vl[0]; }).ToArray();
                    Console.ColorWrite("{0}", string.Join(", ", bt.Select(t => t.ToString()).ToArray()));
                    return null;
                },
            }.Save(C, new string[] { "rnd" });
            #endregion
            #region Todo Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    Console.ColorWrite("$cTHINGS TO DO:\n");
                    foreach (KeyValuePair<string, Command> c in m.Cmds)
                    {
                        if (c.Key.Length==0) { continue; }
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
                            Console.ColorWrite("$a{0}", c.Value.Name.ToUpper());
                            foreach (string str in s) { Console.ColorWrite("$e {0}", str); }
                            Console.WriteLine();
                        }
                    }
                    return null;
                },
            }.Save(C, new string[] { "todo" });
            #endregion
#endif
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
                    Console.ColorWrite("$7User Name: $a{0}", Environment.UserName);
                    Console.ColorWrite("$7Domain Name: $a{0}", Environment.UserDomainName);
                    Console.ColorWrite("$7Name: $a{0}", c.Name);
                    Console.ColorWrite("$7Groups:");
                    foreach (var v in c.Groups) {
                        Console.ColorWrite("$8 {0}", v.Translate(typeof(NTAccount)).Value );
                    }
                    Console.ColorWrite("\n$7Administrator: $a{0}", Funcs.IsAdmin());
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
                    Console.ColorWrite("$eUsers in this computer:");
                    foreach (ManagementObject envVar in searcher.Get())
                    {
                       Console.WriteLine(" {0}", envVar["Name"]);
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
                    if (Funcs.CheckExploit())
                    {
                        Funcs.Exploit(string.Format("\"{0}\" root", Command));
                        Environment.Exit(0);
                        return null;
                    }
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = Command,
                            Arguments = "runas",
                            Verb = "runas"
                        });
                        Environment.Exit(0);
                        return null;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Failed to get admin rights");
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
                                Console.ColorWrite("Ip config renewed and dns cache flushed.");
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
                                Console.WriteLine(new string('\n', 50));
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
                                    if (a.GetSw("r")) { Console.ColorWrite("$e{0} {1}", "[ IP ADDRESS ]".PadRight(16), "[ HOSTNAME ]".PadRight(16)); }
                                    foreach (IPAddress ip in i)
                                    {
                                        if (a.GetSw("r"))
                                        {
                                            try
                                            {
                                                IPHostEntry iph = Dns.GetHostEntry(ip);
                                                Console.WriteLine("{0} {1}", ip.ToString().PadRight(16), iph.HostName);
                                            }catch(Exception)
                                            {
                                                Console.ColorWrite("{0} $cERROR", ip.ToString().PadRight(16));
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("{0}", ip.ToString());
                                        }
                                    }
                                } catch(Exception)
                                {
                                    Console.ColorWrite("$cInvalid host.");
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
                                    Console.WriteLine("Commands:");
                                    foreach (KeyValuePair<string, Command> c in m.Cmds)
                                    {
                                        Console.SetForeColor(Color.White);
                                        if (c.Key == string.Empty) { continue; }
                                        if (c.Value.Help.Description != string.Empty && (c.Value.Alias == c.Key || c.Value.Alias == string.Empty) && c.Key != string.Empty)
                                        {
                                            string[] al = m.Cmds.Where(t => t.Key != "" && t.Value.Alias == c.Key).Select(t => t.Key).ToArray();
                                            Console.ColorWrite("$7{0} - $8{1}", string.Join(", ", al).ToUpper().ToUpper(), c.Value.Help.Description);
                                        }
                                        else if (c.Value.Alias == c.Key || c.Value.Alias == string.Empty)
                                        {
                                            string[] al = m.Cmds.Where(t => t.Key != "" && t.Value.Alias == c.Key).Select(t => t.Key).ToArray();
                                            Console.ColorWrite("$7{0}", string.Join(", ", al).ToUpper().ToUpper());
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
                                        Console.WriteLine("'{0}' is not a recognized command.", comma);
                                        return 0;
                                    }

                                    if(!Equals(e.Help,null))
                                    {
                                        e.Help.Print(Console, e);

                                        string[] al = m.Cmds.Where(t => t.Value.Alias == C[a.Get(1)].Alias).Select(t => t.Key).ToArray();
                                        Console.ColorWrite("\n$8Aliases: $7{0}", string.Join(", ", al).ToUpper());
                                    }
                                    else
                                    {
                                        Console.WriteLine("Command has no help attached.");
                                    }
                                }
                                return null;
                            }
                        }.Save(C, new string[] { "help" });
            #endregion
            #region Out Command
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
                    Console.StartBuffer();

                    if (a.Get(2).Length > 0)
                    {
                        m.RunCommand(a.Get(2));
                    }

                    byte[] Output = Console.EndBuffer().ToCharArray().Select(t => (byte)t).ToArray();

                    try
                    {
                        FileStream fs = File.OpenWrite(a.Get(1));
                        if (Output.Length != 0)
                            for (int i = 0; i < (1 + Output.Length / 1024); i++)
                            {
                                fs.Write(Output, i * 1024, Math.Min(1024, Output.Length - 1024 * i));
                            }
                    }
                    catch (Exception e) { MessageBox.Show(e.Message, "Vulner", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    return null;
                },
            }.Save(C, new string[] { "out" });
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
                    byte[] ip = (a.GetPr("pw", "") + "\xF4VULNEr").ToCharArray().Select(t => (byte)t).ToArray();
                    byte[] pw = SHA1.Create().ComputeHash(ip);

                    foreach (string b in a.Full)
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
                            Console.ColorWrite("Cryptographed $a{0} $8({1} bytes)", new FileInfo(b).FullName, f.Length);
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
                        Console.WriteLine("Syntax:\n\tINSERT [FILENAME] [FOLDER]");
                    }

                    FileInfo Tmp = new FileInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%temp%"), Funcs.RandomString(10) + ".cab"));

                    Console.WriteLine("Compacting...");
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "makecab.exe",
                        Arguments = string.Format("\"{0}\" \"{1}\" /V1", In.FullName, Tmp.FullName),
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true
                    }).WaitForExit();

                    Console.WriteLine("Extracting...");
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
                        Console.ColorWrite("$aFile created: {0}", Fo.FullName);
                    }
                    else
                    {
                        Console.ColorWrite("$cFailed to create file: {0}", Fo.FullName);
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
                    Description = "Print file contents to console",
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
                                Console.ColorWrite("$c{0}.\n$f Do you want to proceed? $aYes$f/$cNo", r);
                                if ((Console.ReadLine() + "n").ToLower()[0] != 'y')
                                {
                                    Console.ColorWrite("$f> No");
                                    return null;
                                }
                                Console.ColorWrite("$f> Yes");
                            }
                            Console.SetForeColor(Console.ltc['8']);
                            Console.WriteLine(s);
                        }
                        catch (IOException ex)
                        {
                            Console.ColorWrite("$cAn error has ocourred!\n{0}", ex.Message);
                        }
                    }
                    else
                    {
                        Console.ColorWrite("$cFile not found!");
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
                            Console.ColorWrite("$cDirectory not found!");
                        }
                    }
                    Console.ColorWrite("$fWorking directory: $a{0}", Environment.CurrentDirectory);
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
                    string M = a.FullArg.Length == 0 ? "*" : a.FullArg;
                    Console.ColorWrite("$e{0}", Path.Combine(Environment.CurrentDirectory, M));
                    string[] Af = Funcs.GetFilesSmarty(M, false);
                    Af = Funcs.GetDirsSmarty(M, false).Concat(Af).ToArray();
                    Console.StartInputBuffer();
                    foreach (string Fil in Af)
                    {
                        FileInfo f = new FileInfo(Fil);
                        string attr = "";
                        attr += ((int)f.Attributes & (int)FileAttributes.Directory) == 0 ? '-' : 'D';
                        attr += ((int)f.Attributes & (int)FileAttributes.ReadOnly) == 0 ? '-' : 'R';
                        attr += ((int)f.Attributes & (int)FileAttributes.Hidden) == 0 ? '-' : 'H';
                        attr += ((int)f.Attributes & (int)FileAttributes.System) == 0 ? '-' : 'S';

                        Console.ColorWrite("$a{0} $8{1}", attr, f.Name);
                    }
                    Console.WriteInputBuffer();
                    return null;
                },
            }.Save(C, new string[] { "ls" });
            #endregion
            #region Del Command
            new Command
            {
                Switches = new string[] { "r" },
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
                    Console.ColorWrite("$eAre you sure you want to delete: $f{0} $f($aYes$f/$cNo$f)", Path.Combine(Environment.CurrentDirectory, h));
                    if ((Console.ReadLine() + "n").ToLower()[0] != 'y') { return null; }
                    
                    Dictionary<string, int> Exceptions = new Dictionary<string, int>{
                        { "Access", 0 },
                        { "IO", 0 },
                        { "Generic", 0 }
                    };
                    bool R = a.GetSw("r");
                    int FileJump = 1;
                    int DirJump = 1;
                    Console.SetForeColor(Console.ltc['6']);
                    Console.WriteLine("Indexing directory...");
                    string[] Files = Funcs.GetFilesSmarty(h, false);
                    string[] Dirs = Funcs.GetDirsSmarty(R ? "*" : h, false);
                    HashSet<string> fls = new HashSet<string>();
                    HashSet<string> drs = new HashSet<string>();
                    foreach (string s in Files) { fls.Add(s); }

                    string CurDir = Environment.CurrentDirectory;

                    Func<string[], int> Recursive = null;
                    int count = fls.Count;
                    int oldc = count;

                    Console.Write("File count: ");
                    Writable w = Console.GetWritable(10);
                    Console.WriteLine();

                    int dcount = drs.Count;
                    int doldc = dcount;

                    Console.Write("Searched directories: ");
                    Writable dw = Console.GetWritable(10);
                    Console.WriteLine();

                    w.Write(count);
                    dw.Write(dcount);
                    int time = Environment.TickCount;

                    string Search = R ? a.Get(1) : "*";
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
                    Environment.CurrentDirectory = CurDir;
                    w.Write(count);
                    dw.Write(dcount);

                    w = null; dw = null;

                    Console.WriteLine("Done indexing!");
                    Console.WriteLine("The process took {0:0.00} seconds", (Environment.TickCount - time)/1000);
                    Console.WriteLine();

                    int all = fls.Count + drs.Count;
                    if (all == 0)
                    {
                        Console.ColorWrite("$eNothing found...");
                    }
                    else
                    {
                        Console.WriteLine("Removing all attributes...");
                        foreach (string s in fls) { File.SetAttributes(s, 0); }
                        foreach (string s in drs) { File.SetAttributes(s, 0); }
                        Console.WriteLine();

                        count = 0;
                        oldc = 0;
                        Console.Write("Deleted: ");
                        w = Console.GetWritable(20);
                        w.Write(string.Format("{0}/{1} ({2:0.00}%)", count, all, (count / all) * 100));
                        Console.WriteLine();

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
                            Console.ColorWrite("$cErrors during the process:");
                            if (Exceptions["IO"] > 0) { Console.ColorWrite("$c File system: {0}", Exceptions["IO"]); }
                            if (Exceptions["Access"] > 0) { Console.ColorWrite("$c Unauthorized access: {0}", Exceptions["Access"]); }
                            if (Exceptions["Generic"] > 0) { Console.ColorWrite("$c Generic: {0}", Exceptions["Generic"]); }
                        }
                    }

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
                        Funcs.RegexRename(a.Get(1), a.Get(2), Console);
                    } else
                    {
                        Console.WriteLine("Please provide valid input.");
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
                        Funcs.RegexRename(a.Get(1), a.Get(2), Console, true);
                    }
                    else
                    {
                        Console.WriteLine("Please provide valid input.");
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
                            Console.ColorWrite("$a{0} $f- {1}", kp.Key, kp.Value);
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

                    Console.ColorWrite( "$e{0} Filename", all );
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
                        Console.ColorWrite("$a{0} {1}", fi, fl.Name );
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
                        Console.ColorWrite("$cAdministrator rights required.");
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
                            Console.ColorWrite("$6{0} = " + Value, s.Key.Replace("!",""));
                        }
                    }
                    else
                    {
                        foreach (string rs in a.Full.Skip(1))
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
                                Console.ColorWrite("$6{0} = " + Value, res.Replace("!", ""));
                            }
                            else
                            {
                                Console.ColorWrite("$cInvalid key.");
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
                    Console.ColorWrite("$eProcess list:");
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
                                    Console.ColorWrite("$f[{0}] {1} - {2} $8{3}\n", envVar["handle"], envVar["name"], envVar["description"], envVar["commandLine"]);
                                }
                                else
                                {
                                    Console.ColorWrite("$f[{0}] {1} - {2}", envVar["handle"], envVar["name"], envVar["description"]);
                                }
                            }
                        }
                        else
                        {
                            if (a.GetSw("d"))
                            {
                                Console.ColorWrite("$f[{0}] {1} - {2} $8{3}\n", envVar["handle"], envVar["name"], envVar["description"], envVar["commandLine"]);
                            }
                            else
                            {
                                Console.ColorWrite("$f[{0}] {1} - {2}", envVar["handle"], envVar["name"], envVar["description"]);
                            }
                        }
                    }
                    Console.ColorWrite("$eEnd");
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
                    Process p = null;
                    try
                    {
                        p = Process.GetProcessById(int.Parse(a.Get(1)));
                    }
                    catch (Exception)
                    {
                        Console.ColorWrite("$cInvalid process id.");
                        return null;
                    }
                    Console.ColorWrite("$eKilling {0}...", p.ProcessName);
                    try
                    {
                        p.Kill();
                        if (p.WaitForExit(5000))
                        {
                            Console.ColorWrite("$aProcess killed!");
                        }
                        else
                        {
                            Console.ColorWrite("$cUnknown error...");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.ColorWrite("$cError: {0}", e.Message);
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
                    string r = Regex.Escape(a.Get(1)).Replace(@"\*", ".*").Replace(@"\?", ".");
                    bool suicide = false;
                    foreach( Process prc in Process.GetProcesses() )
                    {
                        if ( Regex.Match(prc.ProcessName, r, RegexOptions.IgnoreCase).Success )
                        {
                            if (prc.Id == Process.GetCurrentProcess().Id)
                            {
                                Console.ColorWrite("$f{0}", prc.ProcessName);
                                suicide = true;
                                continue;
                            }
                            string st = "$aKilled";

                            try { prc.Kill(); }
                            catch (UnauthorizedAccessException) { st = "$cAccess Denied"; }
                            catch (Exception) { st = "$cError"; }

                            Console.ColorWrite("$f{0} - " + st, prc.ProcessName);
                        }
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
                    } catch(Exception ex) { Console.ColorWrite("$c{0}", ex.Message); }
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
                    catch (Exception ex) { Console.ColorWrite("$c{0}", ex.Message); }
                    return null;
                },
            }.Save(C, new string[] { "runas" });
            #endregion
            #region Assoc Command
            new Command
            {
                Help = new CommandHelp
                {

                },
                Main = (Argumenter a) =>
                {
                    Console.StartInputBuffer();
                    foreach (string ar in a.Full.Skip(1))
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
                            attr = ar.Substring(arg.Length+1).Split('=')[0];
                            val = ar.Substring(arg.Length+1).Split('=')[1];
                        }
                        else if (equ && !dot)
                        {
                            try
                            {
                                action = "setname";
                                string k = arg.Split('=')[0];
                                string v = arg.Split('=')[1];
                                if (Registry.ClassesRoot.OpenSubKey("." + k) == null) { Registry.ClassesRoot.CreateSubKey("." + k); }
                                if (Registry.ClassesRoot.OpenSubKey(v) == null) { Registry.ClassesRoot.CreateSubKey(v); }
                                if (Registry.ClassesRoot.OpenSubKey(v + "\\shell") == null) { Registry.ClassesRoot.OpenSubKey(v, true).CreateSubKey(v + "\\shell"); }
                                Registry.ClassesRoot.OpenSubKey("." + k, true).SetValue("", v);
                                Console.ColorWrite("$a.{0} => $f{1}\n", k, v);
                                continue;
                            }
                            catch (Exception ex)
                            {
                                Console.ColorWrite("$c{0}", ex.Message);
                            }
                        }
                        else if (dot && !equ)
                        {
                            Console.ColorWrite("$cInvalid arguments.");
                        }

                        string r = "^" + Regex.Escape("." + arg).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
                        foreach (string s in Registry.ClassesRoot.GetSubKeyNames().Where(b => b.StartsWith(".")))
                        {
                            if (Regex.Match(s, r).Success)
                            {
                                string t = "";
                                try
                                {
                                    t = Registry.ClassesRoot.OpenSubKey(s).GetValue("").ToString();
                                    RegistryKey o = null;
                                    if (Registry.ClassesRoot.OpenSubKey(t) == null) { Registry.ClassesRoot.CreateSubKey(t); }
                                    if (Registry.ClassesRoot.OpenSubKey(t + "\\shell") == null) { Registry.ClassesRoot.CreateSubKey(t + "\\shell"); }
                                    if (action == "setattr")
                                    {
                                        o = Registry.ClassesRoot.OpenSubKey(t + "\\shell", true);
                                        if (o.OpenSubKey(attr) == null) { o.CreateSubKey(attr); }
                                        if (o.OpenSubKey(attr + "\\command") == null) { o.CreateSubKey(attr + "\\command"); }
                                        o.OpenSubKey(attr + "\\command", true).SetValue("", val);
                                    } else
                                    {
                                        o = Registry.ClassesRoot.OpenSubKey(t + "\\shell");
                                    }
                                    string[] sbkn = o.GetSubKeyNames();
                                    string[] vl = new string[sbkn.Length];
                                    int i = 0;
                                    foreach (string k in sbkn)
                                    {
                                        vl[i++] = string.Format("$e {0} -> $f{1}", k, o.OpenSubKey(k + "\\command").GetValue(""));
                                    }

                                    Console.ColorWrite("$a{0} => $f{1}", s, t);
                                    foreach (string ag in vl) { Console.ColorWrite(ag); }
                                    Console.WriteLine();
                                }
                                catch (Exception)
                                {
                                    Console.ColorWrite("$a{0} => $c{1}(ERROR)\n", s, t);
                                }
                            }
                        }
                    }
                    Console.WriteInputBuffer();

                    return null;
                },
            }.Save(C, new string[] { "assoc" });
            #endregion

            return C;
        }
    }
}
