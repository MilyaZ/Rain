using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rain
{
    public partial class Form1 : Form
    {
        Animator a;
        public Form1()
        {
            InitializeComponent();
            a = new Animator(panel1.CreateGraphics(), panel1.ClientRectangle);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            a.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            a.Stop();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (a == null) return;
            a.Update(panel1.CreateGraphics(), panel1.ClientRectangle);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = String.Format("Текущее значение: {0}", trackBar1.Value);
            Program.trackBar1_Value = trackBar1.Value;
            a.Wind();
           
        }

        
    }
}
