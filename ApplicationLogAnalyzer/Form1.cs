using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
                UpdateColumn();
            }
            catch (SerializationException exception)
            {
                Console.WriteLine("Error");
                throw;
            }
        }

        private void UpdateColumn()
        {
            dataGridView1.Rows.Clear();
            foreach (LogItem item in Logger.Log)
            {
                dataGridView1.Rows.Add(new object[3] {item.Level, item.DateTime, item.Message});
            }
        }

        private void UpdateColumn(List<LogItem> log)
        {
            dataGridView1.Rows.Clear();
            foreach (LogItem item in log)
            {
                dataGridView1.Rows.Add(new object[3] { item.Level, item.DateTime, item.Message });
            } 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Logger.Initialize();

            Logger.Success("test");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Logger.Error("testesttetatessefse");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void ConstraintControlChanged(object sender, EventArgs e)
        {
            LogLevel level = (LogLevel)1;
            if (radioButton2.Checked)
                level = LogLevel.Debug;
            else if (radioButton3.Checked)
                level = LogLevel.Info;
            else if (radioButton4.Checked)
                level = LogLevel.Success;
            else if (radioButton5.Checked)
                level = LogLevel.Warning;
            else if (radioButton6.Checked)
                level = LogLevel.Error;
            UpdateColumn(Logger.SortLogItemsByLogLevel(dateTimePicker1.Value, dateTimePicker2.Value, level));
        }
    }
}