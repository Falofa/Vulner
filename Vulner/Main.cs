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
        public Form Parent = null;
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
            Parent = f;
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
            Environment.CurrentDirectory = Directory.GetDirectoryRoot(Environment.CurrentDirectory);

            ProcessCheck = new Thread(() => CheckAlive());
            ProcessCheck.Start();
            
            foreach (string s in Environment.GetCommandLineArgs())
            {
                if (s.ToLower().EndsWith(".fal"))
                {
                    Funcs.RunFile(s, this);
                    break;
                }
            }
            f.Invoke( (Delegate)(Action)(() => { f.Show(); }) );
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
                t.ColorWrite("$f> {0}", s);

                Argumenter arg = new Argumenter(s, true);
                arg.SetM(this);
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
                            bool ot = !Equals(arg.Output, null) && arg.Output != "";
                            if (ot)
                            {
                                t.StartBuffer();
                            }
#if (DEBUG)
                            cmd.Run(t, arg);
#else
                            try
                            {
                                cmd.Run(t, arg);
                            } catch(Exception e)
                            {
                                Trace(e);
                            }
#endif
                            if (ot)
                            {
                                byte[] Output = t.EndBuffer().ToCharArray().Select(t => (byte)t).ToArray();

                                try
                                {
                                    string fl = arg.FormatStr.Where(u => u.Value[0] == "Output").Select(u => u.Value[1]).First<string>();
                                    if (new DirectoryInfo(fl).Exists) { throw new Exception("Output is a folder."); }
                                    File.SetAttributes(fl,FileAttributes.Normal);
                                    File.Delete(fl);
                                    FileStream fs = File.OpenWrite(fl);
                                    if (Output.Length != 0)
                                        for (int i = 0; i < (1 + Output.Length / 1024); i++)
                                        {
                                            fs.Write(Output, i * 1024, Math.Min(1024, Output.Length - 1024 * i));
                                        }
                                    fs.Close();
                                }
                                catch (Exception e) { MessageBox.Show(e.Message, "Vulner", MessageBoxButtons.OK, MessageBoxIcon.Error); }
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
                }
                catch (KeyNotFoundException)
                {
                    t.ColorWrite("$cInvalid command '$f{0}$c'", comma);
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
            t.ColorWrite("$cAn error ocourred: {0}\n{1}",e.Message, e.StackTrace);
        }
    }
}
