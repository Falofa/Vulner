using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Vulner
{
    class Main
    {
        public Dictionary<string, Command> Cmds = new Dictionary<string,Command>();
        public DirectoryInfo VulnerFolder = null;
        public TerminalController t = null;
        public Main(TerminalController te)
        {
            VulnerFolder = new DirectoryInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%appdata%"), "vulner"));
            if (!VulnerFolder.Exists) { VulnerFolder.Create(); }
            if (Environment.GetCommandLineArgs().Contains("root"))
            {
                Process.Start(new ProcessStartInfo {
                    FileName = Environment.GetCommandLineArgs()[0],
                    Arguments = "runas",
                    Verb = "runas",
                    WindowStyle = ProcessWindowStyle.Normal,
                    CreateNoWindow = false,
                });
                Environment.Exit(0);
                return;
            }
            if (Environment.GetCommandLineArgs().Contains("update"))
            {
                int id = Process.GetCurrentProcess().Id;
                Process.GetProcesses().Where(t => t.ProcessName.ToLower().Contains("vulner") && t.Id != id).Select(t => { t.Kill(); return 0; });
                string self = Environment.GetCommandLineArgs().Skip(1).Where(t => t != "update").First();
#if (DEBUG)
                    string vr = "Debug";
#else
                string vr = "Release";
#endif
                string dl = "https://github.com/Falofa/Vulner/blob/master/Vulner/bin/{0}/Vulner.exe?raw=true";
                System.Net.WebClient wb = new System.Net.WebClient();
                File.WriteAllBytes(self, wb.DownloadData(string.Format(dl, vr)));
                Process.Start(self, "updated");
                Process.GetCurrentProcess().Kill();
                return;
            }

            t = te;
            Cmds = new Commands().Get(this,t);
            Cmds[""] = new Command();

            Environment.SetEnvironmentVariable("vulner", Environment.GetCommandLineArgs()[0]);
            Environment.SetEnvironmentVariable("startup", Environment.GetFolderPath(Environment.SpecialFolder.Startup));
            Environment.SetEnvironmentVariable("startmenu", Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));

            //string name = "Vulner";
            string asciiName = @"
  $f║$c  ____   ____     __       $fDeveloped by Falofa $f║
  $f║$c  \   \ /   __ __|  |   ____   ___________     $f║
  $f║$c   \   Y   |  |  |  |  /    \_/ __ \_  __ \    $f║
  $f║$c    \     /|  |  |  |_|   |  \  ___/|  | \/    $f║
  $f║$c     \___/ |____/|____|___|__/\____/|__|       $f║".Substring(2);     
            string capt = "  ╔═══════════════════════════════════════════════╗";
            string capb = "  ╚═══════════════════════════════════════════════╝";

#if (DEBUG)
            string build = "Debug Build";
#else
            string build = "Release Build";
#endif

            string fbuild = string.Format("$a[[ {0} ]]", build);

            fbuild = string.Format("  ║{0} $f║", fbuild.PadLeft(capt.Length-3));

            t.WriteLine();
            t.ColorWrite(capt);
            t.ColorWrite(asciiName);
            t.ColorWrite(fbuild);
            t.ColorWrite(capb);
            t.WriteLine();

            t.ColorWrite("$2Type $eHELP$2 for a list of commands");

            if (Environment.GetCommandLineArgs().Contains("updated"))
            {
                t.ColorWrite("$aVulner was updated!");
            }
            
            foreach (string s in Environment.GetCommandLineArgs())
            {
                if (s.ToLower().EndsWith(".fal"))
                {
                    if (!File.Exists(s.ToLower())) { Environment.Exit(1); }
                    Environment.CurrentDirectory = new FileInfo(s.ToLower()).DirectoryName;
                    Funcs.RunFile(s, this);
                    break;
                }
            }
            Funcs.ShowConsole();
        }
        public void Run()
        {
            while (true)
            {
                t.SetForeColor('f');
                Console.Write("> ");
                string s = t.ReadLine();
                this.RunCommand(s);
            }
        }
        public string[] IgnoreFileNames = new string[] { "CON", "PRN", "AUX", "NUL",
                                                         "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
                                                         "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };
        public bool RunCommand(string interp)
        {
            Thread ct = new Thread(() =>
            {
                Argumenter a = new Argumenter(interp, true);
                a.SetM(this);
                if (a.GetRaw(0).Trim().Length == 0)
                {
                    return;
                }
                a.SetM(this);
                Command c = null;
                try
                {
                    c = Cmds[a.GetRaw(0)];
                    a.Switches = c.Switches;
                    a.Params = c.Parameters;
                    if (!c.Valid())
                    {
                        t.ColorWrite("$cInvalid command.");
                        return;
                    }
                }
                catch (Exception)
                {
                    t.ColorWrite("$cInvalid command.");
                    return;
                }
                if (!Equals(c, null))
                {
                    if (a.Parse(c.ParseSW, c.ParsePR))
                    {
                        t.SetForeColor('8');
                        bool ot = !Equals(a.Output, null) && a.Output != "";
                        if (ot)
                        {
                            t.StartBuffer();
                        }
#if (DEBUG)
                        c.Run(t, a);
#else
                            try
                            {
                                c.Run(t, a);
                            } catch(Exception e)
                            {
                                Trace(e);
                            }
#endif
                        if (ot)
                        {
                            byte[] Output = t.EndBuffer().ToCharArray().Select(t => (byte)t).ToArray();

                            //try
                            //{
                                string fl = Path.Combine(Environment.CurrentDirectory, a.FormatStr.Where(u => u.Value[0] == "Output").Select(u => u.Value[1]).First<string>());
                                //string fl = Path.Combine(Environment.CurrentDirectory, a.Parsed.Last());
                                //Console.WriteLine(fl);
                                bool IgnoreWrite = false;
                                if (IgnoreFileNames.Contains(new FileInfo(fl).Name.ToUpper())) { IgnoreWrite = true; }
                                if (!IgnoreWrite)
                                {
                                    if (new DirectoryInfo(fl).Exists) { throw new Exception("Output is a folder."); }
                                    if (File.Exists(fl))
                                    {
                                        File.SetAttributes(fl, FileAttributes.Normal);
                                        File.Delete(fl);
                                    }
                                    FileStream fs = File.OpenWrite(fl);
                                    if (Output.Length != 0)
                                        for (int i = 0; i < (1 + Output.Length / 1024); i++)
                                        {
                                            fs.Write(Output, i * 1024, Math.Min(1024, Output.Length - 1024 * i));
                                        }
                                    fs.Close();
                                }
                            //}
                            //catch (Exception e) { MessageBox.Show(e.Message, "Vulner", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                        /* DEBUG
                        foreach (KeyValuePair<int, string[]> b in arg.FormatStr)
                        {
                            t.ColorWrite("$a{0} - $f{1}", b.Value[0], b.Value[1]);
                        }/* */
                    }
                    else
                    {
                        t.WriteLine("Malformed arguments.");
                    }
                }
            });
            ConsoleCancelEventHandler ce = (o, e) =>
            {
                if ((e.SpecialKey & ConsoleSpecialKey.ControlC) == ConsoleSpecialKey.ControlC)
                {
                    t.ColorWrite("$e-> Terminating thread...");
                    int i = 0;
                    while (ct.IsAlive && i < 50)
                    {
                        i++;
                        ct.Abort();
                        Thread.Sleep(50);
                    }
                }
                e.Cancel = true;

            };
            Console.CancelKeyPress += ce;
            ct.Start();
            ct.Join();
            Console.CancelKeyPress -= ce;
            return false;
        }
        public void Trace(Exception e)
        {
            t.ColorWrite("$cAn error ocourred: {0}\n{1}",e.Message, e.StackTrace);
        }
    }
}
