using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Vulner
{
    class Program
    {
        static void Main(string[] args)
        {
            //Funcs.HideConsole();
            Funcs.EnableRightClick();
            Console.BufferHeight = 500;
            Console.Title = "Vulner";
            TerminalController t = new TerminalController();
            Main m = new Main(t, Funcs.GetCurrentThreadId());
            Thread main = new Thread(() => m.Run());
            main.Start();
        }
    }
}
