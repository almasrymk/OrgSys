using ORG.TaskbarSystray.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace ORG.TaskbarSystray
{
    public partial class frm_TaskbarSystray : Form
    {
        static string  url = "", CurrntVersion = "", newVersion = "";
        public static bool IsRuning = false;

        public frm_TaskbarSystray()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }      
     
        private void frm_TaskbarSystray_Load(object sender, EventArgs e)
        {
            label1.Visible = true;
            pictureBox3.Visible = true;
            label4.Visible = true;
            label2.Visible = false;
            label3.Visible = false;
            button1.Visible = false;
            if (CheckInternetConnection())
            {
                try
                {
                    url = ConfigurationManager.AppSettings["SysUpdate"];
                    CurrntVersion = ConfigurationManager.AppSettings["Version"];
                    newVersion = (new WebClient()).DownloadString(url + @"AppFiles/infoVersion.txt");
                    if (newVersion != CurrntVersion)
                    {
                        label2.Visible = true;
                        label3.Visible = true;
                        button1.Visible = true;
                        label1.Visible = false;
                        pictureBox3.Visible = false;
                        label4.Visible = false;
                    }
                    else
                    {
                        label1.Visible = true;
                        pictureBox3.Visible = true;
                        label4.Visible = true;
                        label2.Visible = false;
                        label3.Visible = false;
                        button1.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
      
        private void lblb2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var p = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "Organizer");
                if (p != null)
                    p.Kill();
                Thread th = new Thread(delegate () { Doanload(); });
                th.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                label2.Text = "New updates";
                pictureBox2.Visible = true;
                button1.Enabled = true;
                button1.Text = "Search";

            }
        }

        public void Doanload()
        {
            try
            {
                IsRuning = true;
                label2.Text = "Please wait ...";
                pictureBox2.Visible = false;
                button1.Enabled = false;
                button1.Text = "Loading ...";

                WebClient Client = new WebClient();
                Client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(delegate (object sender, DownloadProgressChangedEventArgs e)
                {
                    label2.Text = string.Format("Please wait ... {0}%.", e.ProgressPercentage);
                });

                Client.DownloadFileCompleted += new AsyncCompletedEventHandler(delegate (object sender, AsyncCompletedEventArgs e)
                {
                    try
                    {
                        label1.Visible = true;
                        pictureBox3.Visible = true;
                        label4.Visible = true;
                        label2.Visible = false;
                        label3.Visible = false;
                        button1.Visible = false;
                        this.WindowState = FormWindowState.Minimized;
                        string[] folders = Directory.GetDirectories(Application.StartupPath);
                        foreach (string file in folders)
                        {
                            string[] fs = Directory.GetFiles(file);
                            foreach (string f in fs)
                                File.Delete(f);
                            Directory.Delete(file);
                        }

                        string[] files = Directory.GetFiles(Application.StartupPath);
                        foreach (string file in files)
                        {
                            if (file.ToLower().Contains("updateorg") || file.ToLower().Contains("updates"))
                                continue;
                            File.Delete(file);
                        }
                        ZipFile.ExtractToDirectory(Application.StartupPath + @"\Updates.zip", Application.StartupPath);
                        File.Delete(@"\Updates.zip");

                        Configuration config;
                        config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                        config.AppSettings.Settings["Version"].Value = newVersion;
                        config.Save(ConfigurationSaveMode.Modified);

                        Process ExternalProcess = new Process();
                        ExternalProcess.StartInfo.FileName = Application.StartupPath + @"\Organizer.exe";
                        ExternalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                        ExternalProcess.Start();
                        ExternalProcess.WaitForExit();


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
                Uri u = new Uri(url + @"/AppFiles/Updates.zip");
                Client.DownloadFileAsync(u, Application.StartupPath + @"\Updates.zip");
                IsRuning = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool CheckInternetConnection()
        {
            String host = "organizersys.com";
            Ping p = new Ping();
            try
            {
                Ping myPing = new Ping();
                byte[] buffer = new byte[32];
                int timeout = 5000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch (Exception ex) { }
            return false;
        }
    }
}
