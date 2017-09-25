using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Security;
using System.Security.Permissions;

namespace Vulner
{
    class Main
    {
        public String Name = null;
        public String FileName = null;
        public DirectoryInfo VulnerFolder = null;
        public Argumenter CurrentArgumenter = null;

        public Dictionary<string, Command> Cmds = new Dictionary<string,Command>();
        public TerminalController t = null;
        public Int32 tid = 0;
        public String Version = "1.0";
        public static Boolean Hide = false;
        public static Boolean killthread = false;
        int FirstKey = 0;
        int LastKey = 0;
        public void HideOutput()
        {
            Hide = true;
        }
        public void ShowOutput()
        {
            Hide = true;
        }
        public void Config()
        {
            this.Name = "Vulner";
            this.FileName = Environment.GetCommandLineArgs()[0];
            FileInfo fi = new FileInfo(FileName);
            if (fi.Name.Contains( ".vshost" ) )
            {
                FileName = Path.Combine(fi.Directory.FullName, fi.Name.Replace(".vshost", ""));
            }
            fi = null;
            Console.WindowWidth = 80;
            Console.WindowHeight = 25;
            Console.BufferWidth = 80;
            Console.BufferHeight = 500;
        }
        public Main(TerminalController te, int tid = 0)
        {
            this.Config();
            this.tid = tid;
            VulnerFolder = new DirectoryInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%appdata%"), Name.ToLower()));
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
                Process.GetProcesses().Where(t => t.ProcessName.ToLower().Contains(Name.ToLower()) && t.Id != id).Select(t => { t.Kill(); return 0; });
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

            Environment.SetEnvironmentVariable(Name, Environment.GetCommandLineArgs()[0]);
            Environment.SetEnvironmentVariable("startup", Environment.GetFolderPath(Environment.SpecialFolder.Startup));
            Environment.SetEnvironmentVariable("startmenu", Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));
            
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
                t.ColorWrite("$a{0} was updated!", Name);
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
            Funcs.EnableRightClick();

            ConsoleCancelEventHandler ce = (o, e) =>
            {
                if ((Environment.TickCount - LastKey) > 500)
                {
                    FirstKey = Environment.TickCount;
                }
                LastKey = Environment.TickCount;
                if ((e.SpecialKey & ConsoleSpecialKey.ControlC) == ConsoleSpecialKey.ControlC)
                {
                    killthread = true;
                    if (CurrentArgumenter != null)
                        CurrentArgumenter.Quit = true;
                }
                e.Cancel = true;
            };
            Console.CancelKeyPress += ce;

            if (Environment.GetCommandLineArgs().Contains("emergency"))
            {
                Funcs.Emergency(te, this, true);
            }
        }

        public void Error(string s)
        {
            if (!t.hide)
            {
                t.ColorWrite("$c{0}", s);
            } else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(s);
            }
        }

        public void Run()
        {
            FirstKey = Environment.TickCount;
            LastKey = Environment.TickCount;
            while (true)
            {
                Console.CursorVisible = true;
                if ( Console.CursorLeft != 0 ) { Console.WriteLine(); }
                if ( LastKey > FirstKey + 2000)
                {
                    t.SetForeColor('c');
                    Console.WriteLine("\nEmergency mode triggered!");
                    Funcs.Emergency(t, this);
                }
                t.SetForeColor('f');
                Console.Write("> ");
                string s = t.ReadLine();
                killthread = false;
                if (s != null)
                {
                    this.RunCommand(s);
                } else
                {
                    t.WriteLine();
                }
            }
        }
        public object Return = null;
        public UserVar Ret = null;
        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public bool RunCommand(string interp, bool ExpectsOutput = false, bool pretty = false)
        {
            if (pretty)
            {
                t.ColorWrite("$f> {0}", interp);
            }
            if (Hide)
            {
                t.hide = true;
            }
            else
            {
                t.hide = false;
            }
            Return = null;
            Command cmd = null;
            Thread ct = new Thread(() =>
            {
                Argumenter a = new Argumenter(interp, true);
                CurrentArgumenter = a;
                a.SetM(this);
                if (a.GetRaw(0).Trim().Length == 0)
                {
                    return;
                }
                a.SetM(this);
                Command c = null;
                try
                {
                    c = Cmds[a.GetRaw(0).ToLower()];
                    a.Switches = c.Switches;
                    a.Params = c.Parameters;
                    a.AllSP = c.AllSP;
                    if (!c.Valid())
                    {
                        t.ColorWrite("$cInvalid command.");
                        return;
                    }
                    cmd = c;
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
                        t.SetForeColor('f');
                        
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
                        if (!Equals(cmd.Exit, null))
                        {
                            cmd.Exit.Invoke();
                        }
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
            ct.SetApartmentState(ApartmentState.STA);
            ct.Start();
            while(ct.IsAlive)
            {
                Thread.Sleep(100);
                if (killthread)
                {
                    ct.Abort();
                    ct.Interrupt();
                    if (!Equals(cmd.Exit, null))
                    {
                        cmd.Exit.Invoke();
                    }
                }
            }
            return false;
        }
        public void Trace(Exception e)
        {
            t.ColorWrite("$cAn error ocourred: {0}\n{1}",e.Message, e.StackTrace);
        }
    }
}
