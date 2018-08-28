using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebCam
{
    public partial class Form1 : Form
    {
        WebCam webCam;
        public Form1()
        {
            InitializeComponent();
            webCam = new WebCam(pictureBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webCam.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webCam.Stop();
        }
    }
}
