using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Forms;
using ApplicationLogger;

namespace ApplicationLogAnalyzer
{
    public partial class AnalyzerForm : Form
    {
        public AnalyzerForm()
        {
            InitializeComponent();
            Logger.OnNewLogItem += Logger_OnNewLogItem;
            openLogFileDialog.InitialDirectory = Environment.SpecialFolder.Personal.ToString();
            dataGridView1.Columns[0].Width = 75;
            dataGridView1.Columns[1].Width = 125;
            dataGridView1.Columns[2].Width = dataGridView1.Width - 200;

            dpEnd.Value = System.DateTime.Now;

            dpEnd.ValueChanged += ConstraintControlChanged;
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
                dataGridView1.Rows.Add(new object[3] {item.Level, item.DateTime, item.Message});
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
            var level = (LogLevel) 1;
            if (rbDebug.Checked)
                level = LogLevel.Debug;
            else if (rbInfo.Checked)
                level = LogLevel.Info;
            else if (rbSuccess.Checked)
                level = LogLevel.Success;
            else if (rbWarning.Checked)
                level = LogLevel.Warning;
            else if (rbError.Checked)
                level = LogLevel.Error;
            UpdateColumn(Logger.SortLogItemsByLogLevel(dpStart.Value, dpEnd.Value, level));
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string message = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            
            LogItemForm form =  new LogItemForm(message);

            form.Show();
        }
    }
}