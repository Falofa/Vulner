using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulner
{
    class Command
    {
        public string Name = string.Empty;
        public string Alias = string.Empty;
        public bool ParseSW = true;
        public bool ParsePR = true;
        public bool Debug = false;
        public bool AllSP = false;
        public Func<Argumenter, object> Main { get ; set; }
        public Action Exit { get; set; }
        public CommandHelp Help = null;
        public string[] Parameters = new string[0];
        public string[] Switches = new string[0];
        public Dictionary<string, object> Memory = new Dictionary<string, object>();
        public object Run(TerminalController Console, Argumenter Arg)
        {
            return Main.Invoke(Arg);
        }
        public void Think()
        {
            if (Parameters.Length == 0)
            {
                ParsePR = false;
            }
            if (Switches.Length == 0)
            {
                ParseSW = false;
            }
        }
    }
    class CommandHelp
    {
        public string Description = null;
        public string LongDesc = null;
        public string[] Usage = null;
        public string[] Examples = null;
        public Dictionary<string,string> Switches = null;
        public Dictionary<string, string> Param = null;

        public void Print(TerminalController Console, Command c)
        {
            if (!Equals(LongDesc, null))
            {
                Console.ColorWrite("\n$7{0} - $8{1}\n", c.Name.ToUpper(), Description);
                Console.ColorWrite("$e" + LongDesc + "\n");
            } else
            {
                Console.ColorWrite("\n$7{0}:", c.Name.ToUpper());
                Console.ColorWrite("$e" + Description + "\n");
            }
            if (!Equals(Usage, null) && Usage.Length > 0)
            {
                Console.ColorWrite("$8Usage:");
                foreach (string a in Usage)
                {
                    Console.ColorWrite("$7" + a.Replace("{NAME}", c.Name));
                }
                Console.WriteLine();
            }

            if (!Equals(Switches, null) && Switches.Count > 0)
            {
                Console.ColorWrite("$8Switches:");
                foreach (KeyValuePair<string, string> a in Switches)
                {
                    Console.ColorWrite(" $7/{0} $8- {1}", a.Key, a.Value.Replace("{NAME}", c.Name));
                }
                Console.WriteLine();
            }

            if (!Equals(Param, null) && Param.Count > 0)
            {
                Console.ColorWrite("$8Parameters:");
                foreach (KeyValuePair<string, string> a in Param)
                {
                    Console.ColorWrite(" $7-{0} [value] $8- {1}", a.Key, a.Value.Replace("{NAME}", c.Name));
                }
                Console.WriteLine();
            }

            if (!Equals(Examples, null) && Examples.Length > 0)
            {
                Console.ColorWrite("$8Examples:");
                foreach (string a in Examples)
                {
                    Console.ColorWrite("$7" + a.Replace("{NAME}", c.Name));
                }
                Console.WriteLine();
            }
        }
    }
    static class CommandExtension
    {
        public static bool Valid( this Command e )
        {
            return !Equals(e.Main, null);
        }

        public static Command Save(this Command e, Dictionary<string, Command> c, string[] s, bool debug = false)
        {
            if (s.Length == 0) { return e; }
            e.Name = s[0];
            e.Alias = s[0];
            e.Debug = debug;
            foreach (string st in s)
            {
                c[st] = e;
            }
            return e;
        }
        public static Command Save(this Command e, Dictionary<string, Command> c, string s, bool debug = false)
        {
            e.Name = s;
            e.Alias = s;
            e.Debug = debug;
            c[s] = e;
            return e;
        }

        public static T Get<T>(this Command e, string s)
        {
            return (T)Convert.ChangeType(e.Memory[s], typeof(T));
        }
        public static void Set(this Command e, string s, object o)
        {
            e.Memory[s] = o;
        }
    }

}
