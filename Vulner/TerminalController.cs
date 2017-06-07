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

namespace Vulner
{
    class TerminalController
    {
        #region Build region
        static public RichTextBox Otp = null;
        static public TextBox Inp = null;
        static public Form Frm = null;
        static Color[] ResetColor;
        public Color Back = Color.Black;
        public Color Fore = Color.White;
        public Dictionary<char, Color> ltc = new Dictionary<char, Color>(); // Letter To Color

        static public RichTextBox OtpBuffer = null;
        static public string InputBuffer = null;
        static public bool IpBuff = false;

        public void StartInputBuffer()
        {
            if (OtpBuffer!=null) { return; }
            InputBuffer = "";
            IpBuff = true;
        }

        public void WriteInputBuffer()
        {
            if (OtpBuffer != null) { return; }
            IpBuff = false;
            string[] lines = InputBuffer.Split('\n');
            
            int i = 0;
            while (true)
            {
                string[] ln = lines.Skip(50*i++).Take(50).ToArray();

                if (ln.Length == 0) { break; }

                ColorWrite(string.Join("\n", ln));
            }


            /*int BlockSize = 1000;
            for( int i = 0; i < 1+(InputBuffer.Length / BlockSize); i++ )
            {
                ColorWrite( InputBuffer.Substring( i*BlockSize, Math.Min( (i+1)*BlockSize,InputBuffer.Length ) ) );
            }*/
        }

        public void StartBuffer()
        {
            OtpBuffer = Otp;
            Otp = new RichTextBox();
            Frm.Invoke((MethodInvoker)(() => {
                Otp.Parent = Frm;
                Otp.Visible = false;
                IntPtr h = Otp.Handle;
            }));
            /*  int i = 0;
            while( !Otp.IsHandleCreated || i++ > 100 ) { Thread.Sleep(5); } */
        }
        public string EndBuffer()
        {
            string s = null;
            Frm.Invoke((MethodInvoker)(() =>
            {
                s = Otp.Text;
                Otp.Dispose();
            }));
            Otp = OtpBuffer;
            OtpBuffer = null;
            return s;
        }

        public RichTextBox GetOutput() { return Otp; }
        public TerminalController( Form Main )
        {
            ResetColor = new Color[] { Fore, Back };

            ltc['a'] = Color.GreenYellow;
            ltc['b'] = Color.Aqua;
            ltc['c'] = Color.Red;
            ltc['d'] = Color.Violet;
            ltc['e'] = Color.Yellow;
            ltc['f'] = Color.White;

            ltc['1'] = Color.Blue;
            ltc['2'] = Color.Green;
            ltc['3'] = Color.Aquamarine;
            ltc['4'] = Color.DarkRed;
            ltc['5'] = Color.DarkViolet;
            ltc['6'] = Color.Gold;
            ltc['7'] = Color.LightGray;
            ltc['8'] = Color.DimGray;
            ltc['0'] = Color.Black;

            Frm = Main;
            Otp = new RichTextBox();
            Inp = new TextBox();

            Frm.Invoke((MethodInvoker)(() => {
                Frm.Controls.Add(Otp);
                Frm.Controls.Add(Inp);

                Otp.Font = new Font("Consolas", 12f, FontStyle.Bold);
                Otp.BackColor = Color.Black;
                Otp.ForeColor = Color.White;
                Otp.SelectionColor = Color.White;
                Otp.ReadOnly = true;
                Otp.BorderStyle = BorderStyle.None;
                Otp.AutoWordSelection = false;

                Inp.Font = new Font("Consolas", 12f, FontStyle.Bold);
                Inp.BorderStyle = BorderStyle.FixedSingle;
                Inp.BackColor = Color.Black;
                Inp.ForeColor = Color.White;
                Inp.ReadOnly = true;

                this.Resize(this, new EventArgs());

                Frm.Resize += Resize;
                Frm.Load += (o, e) => { Inp.Focus(); };
                Otp.KeyPress += ListenForKey;
            }));
        }

        public event EventHandler CancelKey;

        private void ListenForKey(object o, KeyPressEventArgs e)
        {
            //MessageBox.Show(((byte)e.KeyChar).ToString());
            if (e.KeyChar == (char)3)
            {
                try
                {
                    CancelKey(this, new EventArgs());
                }
                catch (Exception) { }
            }
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

        private void Resize(object sender, EventArgs e)
        {
            Otp.Location = new Point(0, 0);
            Otp.Size = new Size(Frm.DisplayRectangle.Size.Width, Frm.DisplayRectangle.Size.Height - Inp.ClientRectangle.Height);

            Inp.Location = new Point(0, Frm.DisplayRectangle.Size.Height - Inp.ClientRectangle.Height); 
            Inp.Size = new Size(Frm.DisplayRectangle.Size.Width, Inp.ClientRectangle.Height);
        }

        public Writable GetWritable(int Length, int Start = -1)
        {
            return new Writable(Frm, this, Length, Start);
        }

        public void SetBackColor(Color c)
        {
            Back = c;
        }

        public void SetForeColor(Color c)
        {
            Fore = c;
        }

        public void SetBackColor(char c)
        {
            try
            {
                Back = ltc[c];
            }
            catch (Exception) { }
        }

        public void SetForeColor(char c)
        {
            try
            {
                Fore = ltc[c];
            }
            catch (Exception) { }
        }

        public void Clear()
        {
            Frm.Invoke((MethodInvoker)(() =>
            {
                Otp.Text = string.Empty;
            }));
        }

        private void iWrite( string s )
        {
            Frm.Invoke((MethodInvoker)(() =>
            {
                //SuspendDrawing(Otp);
                Otp.SelectionStart = Otp.Text.Length;
                Otp.SelectionLength = 0;
                Otp.SelectionColor = Fore;
                Otp.SelectionBackColor = Back;
                Otp.SelectedText = s;
                //Otp.SelectionLength = 0;
                Otp.SelectionStart = Otp.Text.Length;
                Otp.ScrollToCaret();
                //ResumeDrawing(Otp);
            }));
        }
        private void sWrite( string s )
        {
            //SuspendDrawing(Otp);
            Otp.SelectionStart = Otp.Text.Length;
            Otp.SelectionLength = 0;
            Otp.SelectionColor = Fore;
            Otp.SelectionBackColor = Back;
            Otp.SelectedText = s;
            //Otp.SelectionLength = 0;
            Otp.SelectionStart = Otp.Text.Length;
            //ResumeDrawing(Otp);
        }

        public void WriteLine(object o = null)
        {
            if (Equals(o,null)) { o = ""; }
            string s = (string)Convert.ChangeType(o, typeof(String));
            if (IpBuff)
            {
                InputBuffer += s.Replace("$", "$$") + "\n";
            }
            else
            {
                iWrite(s + "\n");
            }
        }

        public void ColorWrite(object o, object a = null, object b = null, object c = null, object d = null)
        {
            bool skiponce = false;
            string s = (string)Convert.ChangeType(o, typeof(String));
            if (IpBuff)
            {
                string sa = (string)Convert.ChangeType(a, typeof(String));
                string sb = (string)Convert.ChangeType(b, typeof(String));
                string sc = (string)Convert.ChangeType(c, typeof(String));
                string sd = (string)Convert.ChangeType(d, typeof(String));
                sa = Equals(sa, null) ? "" : sa;
                sb = Equals(sb, null) ? "" : sb;
                sc = Equals(sc, null) ? "" : sc;
                sd = Equals(sd, null) ? "" : sd;
                InputBuffer += string.Format(s, sa.Replace("$", "$$"), sb.Replace("$", "$$"), sc.Replace("$", "$$"), sd.Replace("$", "$$")) + "\n";
                return;
            }
            Frm.Invoke((Delegate)(Action)(() =>
            {
                bool first = true;
                Color reset = Fore;
                foreach (string se in s.Split('$'))
                {
                    string ss = se;
                    if (!first && !skiponce)
                    {
                        if (se.Length == 0) { skiponce = true; continue; }
                        char ch = ss.ToLower()[0];
                        Fore = ltc[ch];
                        if (Equals(Fore, null)) { Fore = reset; }
                        ss = ss.Substring(1);
                    }
                    if (skiponce)
                    {
                        ss = "$" + ss;
                    }
                    skiponce = false;
                    sWrite(string.Format(ss, new object[] { a, b, c, d }));
                    first = false;
                }
                sWrite("\n");
                Otp.ScrollToCaret();
                Fore = reset;
            }));
        }

        public void WriteLine(object o, object a = null, object b = null, object c = null, object d = null)
        {
            string s = string.Format((string)Convert.ChangeType(o, typeof(String)), new object[] { a, b, c, d });
            if (IpBuff)
            {
                InputBuffer += s.Replace("$", "$$") + "\n";
            }
            else
            {
                iWrite(s + "\n");
            }
        }
        public void NL()
        {
            iWrite("\n");
        }

        public void Write(object o)
        {
            string s = (string)Convert.ChangeType(o, typeof(String));
            if (IpBuff)
            {
                InputBuffer += s.Replace("$", "$$");
            }
            else
            {
                iWrite(s);
            }
        }

        public void Write(object o, object a = null, object b = null, object c = null, object d = null)
        {
            string s = string.Format((string)Convert.ChangeType(o, typeof(String)), new object[] { a, b, c, d });
            if (IpBuff)
            {
                InputBuffer += s.Replace("$", "$$");
            }
            else
            {
                iWrite(s);
            }
        }

        public void Lock()
        {
            Frm.Invoke((MethodInvoker)(() =>
            {
                Inp.ReadOnly = false;
                Inp.Enabled = true;
            }));
        }

        public void Unlock()
        {
            Frm.Invoke((MethodInvoker)(() =>
            {
                Inp.ReadOnly = false;
                Inp.Enabled = true;
            }));
        }
        
        public Form GetControl()
        {
            return Frm;
        }

        static void I( Action a )
        {
            Frm.Invoke((Delegate)a);
        }

        string[] Older = new string[50];
        int i = 0;
        public string ReadLine()
        {
            Frm.Invoke((MethodInvoker)(() =>
            {
                Unlock();
                Inp.Focus();
            }));
            string Str = "";
            bool Pr = true;
            Action Submit = () =>
            {
                Str = Inp.Text;
                if (i == 49)
                {
                    Older = Older.Reverse().Take(49).Reverse().Concat(new string[] { "" }).ToArray();
                    i--;
                }
                else
                {
                    Older[i++] = Str;
                }
                Inp.Clear();
                Pr = false;
                Lock();
            };
            EventHandler bc = (o, e) =>
            {
                Submit.Invoke();
            };
            int a = i;
            KeyEventHandler kp = (o, e) =>
            {
                if ( e.KeyCode == Keys.Up | e.KeyCode == Keys.PageUp )
                {
                    a = Math.Max(a - 1, 0);
                    Inp.Text = Older[a];
                    Inp.SelectionStart = Inp.Text.Length;
                    Inp.SelectionLength = 0;
                    e.Handled = true;
                }
                if (e.KeyCode == Keys.Down | e.KeyCode == Keys.Down)
                {
                    a = Math.Min(a + 1, i-1);
                    Inp.Text = Older[a];
                    Inp.SelectionStart = Inp.Text.Length;
                    Inp.SelectionLength = 0;
                    e.Handled = true;
                }
                if ( e.KeyCode == Keys.Return )
                {
                    Submit.Invoke();
                    e.Handled = true;
                }
            };
            Inp.KeyDown += kp;
            
            while ( Pr )
            {
                Thread.Sleep(10);
            }
            Inp.KeyDown -= kp;
            return Str;
        }

        public int SelectStart() { return Otp.SelectionStart; }
        public int SelectEnd() { return Otp.SelectionStart + Otp.SelectionLength; }
    }
    class Writable
    {
        int Start = 0;
        int Length = 0;
        static Form Frm;
        static RichTextBox Otp;
        static void I(Action a)
        {
            Frm.Invoke((Delegate)a);
        }
        public Writable(Form fr, TerminalController Terminal, int Len, int Pos = -1)
        {
            Frm = fr;
            I(() =>
            {
                Otp = Terminal.GetOutput();
                if (Pos == -1)
                {
                    Pos = Otp.Text.Length;
                }
                Start = Pos;
                Length = Len;
                Otp.SelectionStart = Pos;
                Otp.SelectionLength = 0;
                Otp.SelectedText = "".PadRight(Len);
                Otp.SelectionLength = 0;
            });
        }
        public void Write( object o )
        {
            string s = (string)Convert.ChangeType(o, typeof(String));
            if (s.Length > Length)
            {
                throw new ArgumentOutOfRangeException(string.Format("Maximum length is {0}, but input of length {1} was given.", Length, s.Length));
            }
            I(() =>
            {
                TerminalController.SuspendDrawing(Otp);
                int a = Otp.SelectionStart;
                int b = Otp.SelectionLength;
                Otp.SelectionStart = Start;
                Otp.SelectionLength = Length;
                Otp.SelectedText = s.PadRight(Length);
                Otp.SelectionLength = 0;
                Otp.SelectionStart = a;
                Otp.SelectionLength = b;
                TerminalController.ResumeDrawing(Otp);
            });
        }
    }
}
