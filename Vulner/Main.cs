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
        public List<Proc> procs = new List<Proc>();
        Thread ProcessCheck;
        public void OnExit()
        {
            if (ProcessCheck.IsAlive) ProcessCheck.Abort();
            foreach( Proc p in procs )
            {
                if (p.alive)
                {
                    try
                    {
                        p.main.Kill();
                    }
                    catch (Exception) { }
                }
            }
        }
        public Main(Form f, TerminalController te)
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

            t = te;
            Cmds = new Commands().Get(this,t);
            Cmds[""] = new Command();

            Environment.SetEnvironmentVariable("startup", Environment.GetFolderPath(Environment.SpecialFolder.Startup));
            Environment.SetEnvironmentVariable("startmenu", Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));

            string name = "Vulner";
            string asciiName = @"
  $f║$c  ____   ____     __                           $f║
  $f║$c  \   \ /   __ __|  |   ____   ___________     $f║
  $f║$c   \   Y   |  |  |  |  /    \_/ __ \_  __ \    $f║
  $f║$c    \     /|  |  |  |_|   |  \  ___/|  | \/    $f║
  $f║$c     \___/ |____/|____|___|__/\____/|__|       $f║".Substring(2);     
            string capt = "  ╔═══════════════════════════Developed=by=Falofa═╗";
            string capb = "  ╚═══════════════════════════════════════════════╝";

#if (DEBUG)
            string build = "Debug Build";
#else
            string build = "Release Build";
#endif

            string fbuild = string.Format("$a[[ {0} ]]", build);

            fbuild = string.Format("  ║{0} $f║", fbuild.PadLeft(asciiName.Split('\n')[0].Length-10));

            t.WriteLine();
            t.ColorWrite(capt);
            t.ColorWrite(asciiName);
            t.ColorWrite(fbuild);
            t.ColorWrite(capb);
            t.WriteLine();

            t.ColorWrite("$2Type $eHELP$2 for a list of commands");
            Environment.CurrentDirectory = Directory.GetDirectoryRoot(Environment.CurrentDirectory);

            ProcessCheck = new Thread(() => CheckAlive());
            ProcessCheck.Start();
        }
        public void CheckAlive()
        {
            while (true)
            {
                foreach (Proc p in procs)
                {
                    p.CheckAlive();
                }
                Thread.Sleep(100);
            }
        }
        public void Run()
        {
            while(true)
            {
                string s = t.ReadLine();

                t.SetForeColor('f');
                t.SetBackColor('0');
                t.WriteLine("> {0}", s);

                Argumenter arg = new Argumenter(s);
                string comma = arg.GetRaw(0);

                try
                {
                    Command cmd = Cmds[comma.ToLower()];
                    arg.Switches = cmd.Switches;
                    arg.Params = cmd.Parameters;
                    if (cmd.Valid())
                    {
                        if (arg.Parse(cmd.ParseSW, cmd.ParsePR))
                        {
                            t.SetForeColor('8');
#if (DEBUG)
                            cmd.Run(t, arg);
#else
                            try {
                                cmd.Run(t, arg);
                            } catch (Exception e) { Trace(e); }
#endif
                        }
                        else
                        {
                            t.WriteLine("Malformed arguments.");
                        }
                    }
                }
                catch (KeyNotFoundException)
                {
                    try
                    {
                        Process.Start(arg.GetRaw(0), arg.GetRaw(1)).WaitForExit();
                        continue;
                    }
                    catch (Exception) { }
                    t.WriteLine("'{0}' is not a recognized command or an executable file.", comma);
                }
            }
        }
        public bool RunCommand(string interp)
        {
            Argumenter a = new Argumenter(interp);
            Command c = null;
            try
            {
                c = Cmds[a.GetRaw(0)];
                a.Switches = c.Switches;
                a.Params = c.Parameters;
            }
            catch (Exception) { }
            if (!Equals(c, null))
            {
                if (c.Valid() & a.Parse())
                {
                    try { c.Main.Invoke(a); } catch(Exception e) { Trace(e); }
                    return true;
                }
            }
            return false;
        }
        public void Trace(Exception e)
        {
            t.SetBackColor(Color.Black);
            t.SetForeColor(Color.Red);
            t.WriteLine("An error ocourred: {0}",e.Message);
            t.WriteLine(e.StackTrace);
        }
    }
}
