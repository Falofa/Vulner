using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace Vulner
{
    class Proc
    {
        public Process main;
        public string id;
        public string name;
        public bool alive;
        TerminalController tc;

        public Proc( ProcessStartInfo p, TerminalController stdout )
        {
            main = Process.Start(p);
            name = main.ProcessName;
            alive = true;
            id = GenID();
            tc = stdout;
        }
        public void CheckAlive()
        {
            if ( alive )
            {
                if ( main.HasExited )
                {
                    alive = false;
                    tc.WriteLine("Child process ( [{0}] {1} ) has exited!", id, name);
                }
            }
        }
        public string GenID()
        {
            string a = "abcdefxyz";
            string r = "";
            Random rng = new Random();
            for ( int i = 0; i < 4; i++ )
            {
                r = r + a[ rng.Next() % a.Length ];
            }
            return r;
        }
    }
}
