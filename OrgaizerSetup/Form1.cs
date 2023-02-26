using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrgaizerSetup
{
    public partial class frmSetup : Form
    {
        static string url = "";
        public static bool IsRuning = false;
        public int PathSetup { get; set; }
        public frmSetup()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ////string pathToExe = @"C:\Users\Mohamed Khaled\Desktop\Org1\New folder\Organizer.exe.lnk";
            string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
            string appStartMenuPath = Path.Combine(commonStartMenuPath, "Programs", "Organizer\\Organizer.lnk");
            //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Organizer.exe.lnk";
           
            if (System.IO.File.Exists(appStartMenuPath))
            {
                WshShell shell = new WshShell(); //Create a new WshShell Interface
                IWshShortcut link = (IWshShortcut)shell.CreateShortcut(appStartMenuPath); //Link the interface to our shortcut
                txtPath.Text = link.WorkingDirectory;
                btnRemove.Visible = true;
                btnModify.Visible = true;
                label3.Visible = true;
                pnlBrows.Visible = false;
                btnStartSetup.Visible = false;
            }
            else
            {
                txtPath.Text = Path.GetPathRoot(Environment.SystemDirectory) + "\\Organizer";
            }

            
            //if (!Directory.Exists(appStartMenuPath))
            //    Directory.CreateDirectory(appStartMenuPath);

            //string shortcutLocation = Path.Combine(appStartMenuPath, "Shortcut to Organizer" + ".lnk");
            //WshShell shell = new WshShell();
            //IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            //shortcut.Description = "Organizer xxx";
            ////shortcut.IconLocation = @"C:\Program Files (x86)\TestApp\TestApp.ico"; //uncomment to set the icon of the shortcut
            //shortcut.TargetPath = pathToExe;
            //shortcut.Save();

            //if (!Directory.Exists(Path.Combine(commonStartMenuPath, "Programs", "Organizer")))
            //    Directory.CreateDirectory(Path.Combine(commonStartMenuPath, "Programs", "Organizer"));

            //if (Directory.Exists(appStartMenuPath))
            //    Directory.Delete(appStartMenuPath);
            //if (Directory.Exists(desktopPath))
            //    Directory.Delete(desktopPath);

            //System.IO.File.Copy(pathToExe, appStartMenuPath);
            //System.IO.File.Copy(pathToExe, desktopPath);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                txtPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lnlMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnStartSetup_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPath.Text != "")
                {
                    if (!Directory.Exists(txtPath.Text))
                        Directory.CreateDirectory(txtPath.Text);

                    pnlBrows.Visible = false;
                    picLoad.Visible = true;
                    lblLoad.Visible = true;
                    btnStartSetup.Enabled = false;

                    var p = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "UpdateOrg");
                    if (p != null)
                        p.Kill();
                    var p2 = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "Organizer");
                    if (p2 != null)
                        p2.Kill();

                    Thread th = new Thread(delegate () { Doanload(); });
                    th.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SafeUpdate(Action action)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        public void Doanload()
        {
            try
            {
                IsRuning = true;

                WebClient Client = new WebClient();
                Client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(delegate (object sender, DownloadProgressChangedEventArgs e)
                {
                    if (lblLoad.InvokeRequired)
                    {
                        var txt = string.Format("Please wait ({0}%)...", e.ProgressPercentage);
                        SafeUpdate(() => lblLoad.Text = txt);
                    }
                    else
                        lblLoad.Text = string.Format("Please wait ({0}%)...", e.ProgressPercentage);
                });

                Client.DownloadFileCompleted += new AsyncCompletedEventHandler(delegate (object sender, AsyncCompletedEventArgs e)
                {
                    try
                    {                        

                        if (lblLoad.InvokeRequired)
                            SafeUpdate(() => lblLoad.Text = "Install...");
                        else
                            lblLoad.Text = "Install...";                      

                        ZipFile.ExtractToDirectory(txtPath.Text + @"\OrgApp.zip", txtPath.Text);
                        System.IO.File.Delete(txtPath.Text + @"\OrgApp.zip");

                        string pathToExe = txtPath.Text + @"\Organizer.exe";

                        string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
                        string appStartMenuPath = Path.Combine(commonStartMenuPath, "Programs", @"Organizer\Organizer.lnk");
                        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Organizer.lnk";

                        if (!Directory.Exists(commonStartMenuPath + @"\Programs\Organizer"))
                            Directory.CreateDirectory(commonStartMenuPath + @"\Programs\Organizer");

                        if (System.IO.File.Exists(appStartMenuPath))
                            System.IO.File.Delete(appStartMenuPath);

                        if (System.IO.File.Exists(desktopPath))
                            System.IO.File.Delete(desktopPath);


                    //    string shortcutLocation = Path.Combine(appStartMenuPath, "Organizer" + ".lnk");
                        WshShell shell = new WshShell();
                        IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(appStartMenuPath);

                        shortcut.Description = "Organizer";
                        shortcut.TargetPath = pathToExe;
                        shortcut.WorkingDirectory = txtPath.Text;
                        shortcut.Save();


                        //shortcutLocation = Path.Combine(desktopPath, "Organizer" + ".lnk");
                        shell = new WshShell();
                        shortcut = (IWshShortcut)shell.CreateShortcut(desktopPath);

                        shortcut.Description = "Organizer";
                        shortcut.TargetPath = pathToExe;
                        shortcut.WorkingDirectory = txtPath.Text;
                        shortcut.Save();
                        //System.IO.File.Copy(pathToExe, appStartMenuPath);
                        //System.IO.File.Copy(pathToExe, desktopPath);

                        if (lblLoad.InvokeRequired)
                        {
                            SafeUpdate(() => btnFinish.Visible = true);
                            SafeUpdate(() => btnRemove.Visible = false);
                            SafeUpdate(() => btnModify.Visible = false);
                            SafeUpdate(() => picLoad.Visible = false);
                            SafeUpdate(() => lblLoad.Visible = false);
                            SafeUpdate(() => label1.Visible = true);
                            SafeUpdate(() => pictureBox3.Visible = true);
                        }
                        else
                            btnFinish.Visible = true;

                      
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
                Uri u = new Uri(@"http://organizersys.com/AppFiles/OrgApp.zip");
                Client.DownloadFileAsync(u, txtPath.Text + @"\OrgApp.zip");
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

        private void btnFinish_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                btnRemove.Enabled = false;
                btnModify.Enabled = false;
                label3.Visible = false;
                picLoad.Visible = true;

                var p = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "UpdateOrg");
                if (p != null)
                    p.Kill();
                var p2 = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "Organizer");
                if (p2 != null)
                    p2.Kill();

                Thread th = new Thread(delegate () { DeleteFile(); });
                th.Start();
            }
            catch
            {
                btnRemove.Enabled = true;
                btnModify.Enabled = true;
                label3.Visible = true;
                picLoad.Visible = false;
            }
          
        }

        public void DeleteFile()
        {
            try
            {
                IsRuning = true;
               

                string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
                string appStartMenuPath = Path.Combine(commonStartMenuPath, "Programs", "Organizer\\Organizer.lnk");
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Organizer.lnk";

                WshShell shell = new WshShell(); //Create a new WshShell Interface
                IWshShortcut link = (IWshShortcut)shell.CreateShortcut(appStartMenuPath);

                string[] folders = Directory.GetDirectories(link.WorkingDirectory);
                foreach (string file in folders)
                {
                    string[] fs = Directory.GetFiles(file);
                    foreach (string f in fs)
                        System.IO.File.Delete(f);
                    Directory.Delete(file);
                }

                string[] files = Directory.GetFiles(link.WorkingDirectory);
                foreach (string file in files)
                {                   
                    System.IO.File.Delete(file);
                }

                Directory.Delete(link.WorkingDirectory);

                if (System.IO.File.Exists(appStartMenuPath))
                    System.IO.File.Delete(appStartMenuPath);

                if (System.IO.File.Exists(desktopPath))
                    System.IO.File.Delete(desktopPath);

                Directory.Delete(Path.Combine(commonStartMenuPath, "Programs", "Organizer"));

                if (lblLoad.InvokeRequired)
                {
                    SafeUpdate(() => btnFinish.Visible = true);
                    SafeUpdate(() => btnRemove.Visible = false);
                    SafeUpdate(() => btnModify.Visible = false);
                    SafeUpdate(() => picLoad.Visible = false);
                    SafeUpdate(() => lblLoad.Visible = false);
                    SafeUpdate(() => label1.Visible = true);
                    SafeUpdate(() => pictureBox3.Visible = true);
                }
                else
                {
                    btnModify.Visible = false;
                    btnRemove.Visible = false;
                    btnFinish.Visible = true;
                    picLoad.Visible = false;
                    lblLoad.Visible = false;
                    label1.Visible = true;
                    pictureBox3.Visible = true;
                }
                IsRuning = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPath.Text != "")
                {
                    if (!Directory.Exists(txtPath.Text))
                        Directory.CreateDirectory(txtPath.Text);

                    var p = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "UpdateOrg");
                    if (p != null)
                        p.Kill();
                    var p2 = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "Organizer");
                    if (p2 != null)
                        p2.Kill();

                    pnlBrows.Visible = false;
                    picLoad.Visible = true;
                    lblLoad.Visible = true;
                    btnModify.Enabled = false;
                    btnRemove.Enabled = false;
                    label3.Visible = false;
                    btnStartSetup.Enabled = false;
                    string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
                    string appStartMenuPath = Path.Combine(commonStartMenuPath, "Programs", "Organizer\\Organizer.lnk");

                    WshShell shell = new WshShell(); //Create a new WshShell Interface
                    IWshShortcut link = (IWshShortcut)shell.CreateShortcut(appStartMenuPath);

                    string[] folders = Directory.GetDirectories(link.WorkingDirectory);
                    foreach (string file in folders)
                    {
                        string[] fs = Directory.GetFiles(file);
                        foreach (string f in fs)
                            System.IO.File.Delete(f);
                        Directory.Delete(file);
                    }

                    string[] files = Directory.GetFiles(link.WorkingDirectory);
                    foreach (string file in files)
                    {
                        if (file.ToLower().Equals("org"))
                            continue;
                        System.IO.File.Delete(file);
                    }
                    Thread th = new Thread(delegate () { Doanload(); });
                    th.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
