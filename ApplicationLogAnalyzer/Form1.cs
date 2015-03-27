using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Forms;
using ApplicationLogger;

namespace ApplicationLogAnalyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Logger.OnNewLogItem += Logger_OnNewLogItem;
        }

        private void Logger_OnNewLogItem(LogItem item)
        {
            UpdateColumn();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openLogFileDialog.ShowDialog(this);
        }

        private void openLogFileDialog_FileOk_1(object sender, CancelEventArgs e)
        {
            Console.WriteLine("Opening file: " + openLogFileDialog.FileName);

            loadConfig(openLogFileDialog.FileName);
        }

        private void loadConfig(string configName)
        {
            try
            {
                Logger.Initialize(openLogFileDialog.FileName);
            }
            catch (SerializationException exception)
            {
                Console.WriteLine("Error");
                throw;
            }

            UpdateColumn();
        }

        private void UpdateColumn()
        {
            dataGridView1.Rows.Clear();
            foreach (LogItem item in Logger.Log)
            {
                dataGridView1.Rows.Add(new object[3] {item.Level, item.Time, item.Message});
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Logger.Initialize();

            Logger.Success("test");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Logger.Destruct("test.file");
        }
    }
}