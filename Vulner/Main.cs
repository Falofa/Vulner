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
        public int tid = 0;
        public static bool Hide = false;
        public void HideOutput()
        {
            Hide = true;
        }
        public void ShowOutput()
        {
            Hide = true;
        }
        public Main(TerminalController te, int tid = 0)
        {
            this.tid = tid;
            VulnerFolder = new DirectoryInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%appdata%"), "vulner"));
            if (!VulnerFolder.Exists) { VulnerFolder.Create(); }
            if (Environment.GetCommandLineArgs().Contains("root"))
            {
                Process.Start(new ProcessStartInfo
                {
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
            Cmds = new Commands().Get(this, t);
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

            fbuild = string.Format("  ║{0} $f║", fbuild.PadLeft(capt.Length - 3));

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
            

        private void K_MouseEvent(Kennedy.ManagedHooks.MouseEvents mEvent, Point point)
        {
            if (mEvent == Kennedy.ManagedHooks.MouseEvents.MouseWheel)
            {
                t.WriteLine("FUCK");
            }
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
        public object Return = null;
        public UserVar Ret = null;
        public bool RunCommand(string interp, bool ExpectsOutput = false)
        {
            if (Hide)
            {
                t.hide = true;
            }
            else
            {
                t.hide = false;
            }
            Return = null;
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
                        
                        if ( a.OutType != Argumenter.OutputType.None )
                        {
                            t.StartBuffer(true);
                        }
                        a.ExpectsOutput = ExpectsOutput;
#if (DEBUG)
                        Return = c.Run(t, a);
#else
                            try
                            {
                                Return = c.Run(t, a);
                            } catch(Exception e)
                            {
                                Trace(e);
                            }
#endif
                        if (Return == null)
                            Ret = new UserVar(new Null());
                        else
                            Ret = new UserVar(Return);

                        if (a.OutType != Argumenter.OutputType.None)
                        {
                            Stream s = t.EndStreamBuffer();
                            a.WriteOutput(s, Ret);
                            t.KillBuffer();
                        }
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
