using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace Vulner
{
    class TerminalController
    {
        #region Build region
        static ConsoleColor[] ResetColor;
        public ConsoleColor Back = ConsoleColor.Black;
        public ConsoleColor Fore = ConsoleColor.White;
        public Dictionary<char, ConsoleColor> ltc = new Dictionary<char, ConsoleColor>(); // Letter To Color

        public bool buffer = false;
        public bool hide = false;
        public MemoryStream stream = null;
        public StreamWriter r = null;

        public void StartBuffer(bool hide = false)
        {
            if (hide) this.hide = hide;
            buffer = true;
            stream = new MemoryStream();
            r = new StreamWriter(stream);
            r.AutoFlush = true;
        }
        public string EndBuffer()
        {
            hide = false;
            buffer = false;
            StreamReader sr = new StreamReader(stream);
            stream.Position = 0;
            return sr.ReadToEnd();
        }

        public TerminalController()
        {
            ResetColor = new ConsoleColor[] { Fore, Back };

            ltc['a'] = ConsoleColor.Green;
            ltc['b'] = ConsoleColor.Cyan;
            ltc['c'] = ConsoleColor.Red;
            ltc['d'] = ConsoleColor.Magenta;
            ltc['e'] = ConsoleColor.Yellow;
            ltc['f'] = ConsoleColor.White;

            ltc['1'] = ConsoleColor.Blue;
            ltc['2'] = ConsoleColor.Green;
            ltc['3'] = ConsoleColor.DarkBlue;
            ltc['4'] = ConsoleColor.DarkRed;
            ltc['5'] = ConsoleColor.DarkMagenta;
            ltc['6'] = ConsoleColor.DarkYellow;
            ltc['7'] = ConsoleColor.Gray;
            ltc['8'] = ConsoleColor.DarkGray;
            ltc['0'] = ConsoleColor.Black;

            Console.BackgroundColor = Back = ConsoleColor.Black;
            Console.ForegroundColor = Fore = ConsoleColor.White;
        }
        #endregion

        #region DLL
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        public static void SuspendDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }
        #endregion

        public void ColorReset()
        {
            Fore = ResetColor[0];
            Back = ResetColor[1];
        }

        public void SetBackColor(ConsoleColor c)
        {
            Console.BackgroundColor = Back = c;
        }

        public void SetForeColor(ConsoleColor c)
        {
            Console.ForegroundColor = Fore = c;
        }

        public void SetBackColor(char c)
        {
            try
            {
                Console.BackgroundColor = Back = ltc[c];
            }
            catch (Exception) { }
        }

        public void SetForeColor(char c)
        {
            try
            {
                Console.ForegroundColor = Fore = ltc[c];
            }
            catch (Exception) { }
        }

        public void Clear()
        {
            if (!hide) WriteLine("".PadRight(Console.BufferHeight, '\n'));
        }

        public void ColorWrite(object o, params object[] obj)
        {
            bool skiponce = false;
            string s = (string)Convert.ChangeType(o, typeof(String));

            bool first = true;
            ConsoleColor reset = Fore;
            foreach (string se in s.Split('$'))
            {
                string ss = se;
                if (!first && !skiponce)
                {
                    if (se.Length < 2) { skiponce = true; continue; }
                    char ch = ss.ToLower()[0];
                    SetForeColor(ltc[ch]);
                    if (Equals(Fore, null)) { SetForeColor(reset); }
                    ss = ss.Substring(1);
                }
                if (skiponce)
                {
                    ss = "$" + ss;
                }
                skiponce = false;
                try
                {
                    string str = string.Format(ss, obj);
                    if (!hide) Write(str);
                } catch (Exception)
                {
                    // It appears that CLSIDS for example: {BB64F8A7-BEE7-4E1A-AB8D-7D8273F7FDB6} make string.Format throw an exception, rest in pieces
                    if (!hide) Write(ss);
                }
                first = false;
            }
            if (!hide) WriteLine();
            SetForeColor(reset);
        }

        public void WriteLine(object o = null, params object[] obj)
        {
            if ( Equals(o,null) ) { o = ""; }
            string s = string.Format((string)Convert.ChangeType(o, typeof(String)), obj);
            if (buffer) { r.WriteLine(s); }
            if (!hide) Console.WriteLine(s);
        }

        public void Write(object o = null, params object[] obj)
        {
            if (Equals(o, null)) { o = ""; }
            string s = string.Format((string)Convert.ChangeType(o, typeof(String)), obj);
            if (buffer) { r.Write(s); }
            if (!hide) Console.Write(s);
        }

        public string FancyInput()
        {
            SetForeColor('f');
            Write("> ");
            string s = ReadLine();
            if (buffer) { r.WriteLine("{0}", s); }
            return s;
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public Writable GetWritable( int Length )
        {
            return new Writable(Length, this);
        }

        public string EscapeColor( string s )
        {
            return s.Replace("$", "$$");
        }
    }
    class Writable {
        int X = 0;
        int Y = 0;
        int RP = 0;
        int Len = 0;
        TerminalController t = null;
        public Writable(int Length, TerminalController t)
        {
            this.t = t;
            X = Console.CursorLeft;
            Y = Console.CursorTop;
            RP = (int)t.stream.Position;
            Len = Length;
            Clear();
        }

        public void Clear()
        {
            Write("".PadRight(Len), true);
        }

        public void Write(object o, bool clearing = false)
        {
            string s = (string)Convert.ChangeType(o, typeof(String));
            string wrt = s.Substring(0, Math.Min(s.Length, Len)).PadRight(Len);
            int[] temp = new int[] { Console.CursorLeft, Console.CursorTop };
            Console.CursorLeft = X;
            Console.CursorTop = Y;
            if (!t.hide) Console.Write(wrt);
            Console.CursorLeft = temp[0];
            Console.CursorTop = temp[1];
            if (t.buffer)
            {
                int tempb = (int)t.stream.Position;
                t.stream.Position = RP;
                t.r.Write(wrt);
                if (!clearing) // Why is this something that needs to exist? We may never know...
                {
                    t.stream.Position = tempb;
                }
            }
        }
    }
}
