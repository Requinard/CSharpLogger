using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ApplicationLogger;

namespace ApplicationLogAnalyzer
{
    public partial class LogItemForm : Form
    {
        private string message;

        public LogItemForm(string item)
        {
            InitializeComponent();
            this.message = item;

            textBox1.Text = message;

            textBox1.Location = new Point(0,0);

            textBox1.Size = this.Size;
        }

        private void LogItemForm_Load(object sender, EventArgs e)
        {

        }

        private void LogItemForm_ResizeEnd(object sender, EventArgs e)
        {
            textBox1.Size = this.Size;
        }
    }
}
