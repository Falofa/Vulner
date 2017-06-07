using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Vulner
{
    public partial class Form1 : Form
    {
        static TerminalController Terminal;
        Thread MainThread = null;
        Main m;
        public Form1()
        {
            InitializeComponent();
            this.Hide();
        }

        [STAThread]
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            if ( Environment.GetCommandLineArgs().Contains("runas") )
            {
                Process prc = Process.Start(new ProcessStartInfo
                {
                    FileName = Environment.GetCommandLineArgs()[0],
                    Verb = "runas",
                });
                Environment.Exit(0);
                return;
            }

            Terminal = new TerminalController(this);
            MainThread = new Thread(()=> Start());
            MainThread.Start();
            
        }

        private void Start()
        {
            m = new Main(this, Terminal);
            m.Run();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult.Yes != MessageBox.Show("Are you sure you want to quit?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                e.Cancel = true;
                return;
            }
            m.OnExit();
            MainThread.Abort();
        }
    }
}
