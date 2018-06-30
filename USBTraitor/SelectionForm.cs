using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Speech;
using System.Speech.Synthesis;
using System.Media;



using System.Data.OleDb;

namespace USBTraitor
{
    public partial class SelectionForm : Form
    {
        int i = 0,n=1,x=0;
        private OleDbConnection connection = new OleDbConnection();
        public SelectionForm()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=LoginInfo.accdb;
    Persist Security Info=False;";
        }
        SpeechSynthesizer s = new SpeechSynthesizer();
       
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
            
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                string addFormat = "insert into Table3(Format) values ('"+textBox1.Text+"')";
                command.CommandText = addFormat;
                command.ExecuteNonQuery();

                listBox1.Items.Add(textBox1.Text);
            }

            catch (Exception ex)
            {
                MessageBox.Show("already exists");
            }

            textBox1.Text = "";
            connection.Close();
        }



        private void SelectionForm_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            
            connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            

            ////adding into listbox1///
            string formatAdd = "SELECT * FROM Formats";
            command.CommandText = formatAdd;

            OleDbDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                listBox1.Items.Add(reader["Format"].ToString());
                listBox2.Items.Add(reader["Vformat"].ToString());
            }
            
            
            

            connection.Close();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browse = new FolderBrowserDialog();
            if (browse.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = browse.SelectedPath;
               
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        void getUSB()
        {
            int i = 0;
            var drives = from drive in DriveInfo.GetDrives() where drive.DriveType == DriveType.Removable select drive;
            comboBox2.DataSource = drives.ToList();
            string dName = comboBox2.Text;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                comboBox2.Items.Clear();
                comboBox2.Text = "";
            }
            catch (Exception)
            {

            }

            getUSB();

            if (comboBox2.Items.Count > 0)
            {
                label2.Visible = true;
                label2.Text = "USB Inserted";
                try
                {
                    if (listBox1.SelectedItem != null && x==1)
                    {
                        copyFile();
                    }
                    
                }
                catch (Exception ex)
                {
                    
                }

            }
            else
            {
                label2.Visible = true;
                label2.Text = "USB Removed Or\n not Inserted";
                comboBox2.Text = "";
            }
        }

        private void label2_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            x = 1;
            if (comboBox2.Items.Count > 0)
            {
                    MessageBox.Show("Everything Saved,programme copied '"
                        + listBox1.SelectedItem + "' format files automatically in the background");

                    //this.Hide();
            }
            else
            {
                
                
                    MessageBox.Show("No USB Found but Everything Saved,programme will copy '"
                   + listBox1.SelectedItem + "' format files automatically in the background when it gets an USB");

                  //  this.Hide();
            }
        }




        void copyFile()
        {
            string s = textBox2.Text;
            string[] files = Directory.GetFiles(@comboBox2.Text, "*" + listBox1.SelectedItem,SearchOption.AllDirectories);
            foreach (string file in files)
            {
                File.Copy(file, Path.Combine(@textBox2.Text + "\\", Path.GetFileName(file)), true);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox2.Items.Count > 0)
            {
                foreach (string item in listBox2.Items)
                {
                    RemoveVirus(item);
                }

                MessageBox.Show("Scan complete!! All the threats were deleted");
            }

            else
                MessageBox.Show("NO USB FOUND");
        }

        public void RemoveVirus(string ex)
        {
            DirectoryInfo di = new DirectoryInfo(@comboBox2.Text);
            FileInfo[] files = di.GetFiles("*" +ex).Where(p => p.Extension == ex ).ToArray();

            foreach (FileInfo item in files)
            {
                try
                {
                    item.Attributes = FileAttributes.Normal;
                    item.Delete();
                }
                catch
                {

                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void selectionSizeChanged(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Icon = SystemIcons.Shield;
                //notifyIcon1.BalloonTipText = "Application is Running in System tray";
                //notifyIcon1.ShowBalloonTip(1000);
                this.ShowInTaskbar = false;
                i = 0;
                timer2.Enabled = true;
            }

            else if (this.WindowState == FormWindowState.Normal)
            {
                if (i<=n)
                {
                    this.Show();
                    notifyIcon1.BalloonTipText = "USBTraitor has come back";
                    this.ShowInTaskbar = true;
                    notifyIcon1.ShowBalloonTip(1000);
                }
                else
                {
                    this.Hide();
                    LoginForm lf = new LoginForm();
                    lf.Show();
                }

            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Underconstruction,being developed by Shujon & Topu");
        }

        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("NOt yet published");
        }

        

        private void SelectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {

                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                string addFormat = "insert into Table3(Vformat) values ('" + textBox3.Text + "')";
                command.CommandText = addFormat;
                command.ExecuteNonQuery();

                listBox2.Items.Add(textBox3.Text);
                MessageBox.Show("ADDED Successfully..");
            }

            catch (Exception ex)
            {
                MessageBox.Show("already exists");
            }
            textBox3.Text = "";
            connection.Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            i++;
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            label4.Visible = false;
            textBox1.Visible = false;
            button3.Visible = false;
            pictureBox2.Visible = false;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            label5.Visible = false;
            button5.Visible = false;
            textBox3.Visible = false;
            pictureBox3.Visible = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Underconstruction By Shujon & Topu");
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            label4.Visible = true;
            textBox1.Visible = true;
            button3.Visible = true;
            pictureBox2.Visible = true;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            label5.Visible = true;
            textBox3.Visible = true;
            button5.Visible = true;
            pictureBox3.Visible = true;
        }

        private void minutesToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            n = 2;
            MessageBox.Show("AUTOLOCK TIMER SET");
        }

        private void minutesToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            n = 5;
            MessageBox.Show("AUTOLOCK TIMER SET");
        }

        private void minutesToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            n = 10;
            MessageBox.Show("AUTOLOCK TIMER SET");
        }

        private void minutesToolStripMenuItem8_Click(object sender, EventArgs e)
        {
            n = 15;
            MessageBox.Show("AUTOLOCK TIMER SET");
        }

        private void minutesToolStripMenuItem9_Click(object sender, EventArgs e)
        {
            n = 30;
            MessageBox.Show("AUTOLOCK TIMER SET");
        }

        
        

        

    }
}
