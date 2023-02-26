using System;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using System.IO;
using ORGRepository;
using System.Threading;
using ORG.UIL.Desktop.Froms.General;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Media;
using System.Net;
using System.ComponentModel;
using System.IO.Compression;
using ORGEntity;
using ORG.UIL.Desktop.Untility;

namespace ORG.UIL.Desktop
{
    public partial class frm_Main : Form
    {
        string scearnOpened = "invoice";
        frm_Invoice _frm_Invoice;
        frm_ReturnInvoice _frm_ReturnInvoice;
        frm_PanelData _frm_PanelData;
        frm_Report _frm_Report;
        public bool Restart { get; set; } = false;

        public  bool _ShowLoadData = false;
        public  bool ShowLoadData
        {
            get
            { return _ShowLoadData; }
            set {
                _ShowLoadData = value;
                if (pnlLoadData.InvokeRequired)
                {
                    SafeUpdate(() => pnlLoadData.Visible = value);
                }
                else
                    pnlLoadData.Visible = value;
            }
        }

        public string _TextData = "";
        public string TextData
        {
            get
            { return _TextData; }
            set
            {
                _TextData = value;
                if(lblData.InvokeRequired)
                {
                    SafeUpdate(() => lblData.Text = value) ;
                }
                else
                lblData.Text= value;
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

        public frm_Main()
        {
            ChangeLanguage();
            InitializeComponent();

            if (GeneralMembers.Lang == "ar")
                lblLangAr.ForeColor = Color.FromArgb(100, 150, 250);
            else
                lblLangEn.ForeColor = Color.FromArgb(100, 150, 250);
            new SystemUpdate().StartProcessing();
        }

        public frm_Main(string Lang)
        {
            ChangeLanguage();
            InitializeComponent();
            if (GeneralMembers.Lang == "ar")
                lblLangAr.ForeColor = Color.FromArgb(100, 150, 250);
            else
                lblLangEn.ForeColor = Color.FromArgb(100, 150, 250);
            new SystemUpdate().StartProcessing();
        }

        public virtual void ChangeLanguage()
        {
            if (ConfigurationManager.AppSettings["Language"] == "A")
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar");
                GeneralMembers.Lang = "ar";
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                GeneralMembers.Lang = "en";
            }
        }

        private void EnglishClick(object sender, EventArgs e)
        {
            if (GeneralMembers.Lang == "ar")
            {
                Helper.Sound(Application.StartupPath, SoudType.LangClick);
                Configuration config;
                config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                config.AppSettings.Settings["Language"].Value = "E";
                GeneralMembers.Lang = "en";

                if (GeneralMembers.User != null && GeneralMembers.User.Id > 0)
                {
                    config.AppSettings.Settings["us"].Value = GeneralMembers.User.Email;
                    config.AppSettings.Settings["pss"].Value = GeneralMembers.User.Password;
                }
                config.Save(ConfigurationSaveMode.Modified);
                Restart = true;
                Application.Restart();
            }
        }

        private void ArabicClick(object sender, EventArgs e)
        {
            if (GeneralMembers.Lang != "ar")
            {
                Helper.Sound(Application.StartupPath, SoudType.LangClick);
                Configuration config;
                config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                config.AppSettings.Settings["Language"].Value = "A";
                GeneralMembers.Lang = "ar";

                if (GeneralMembers.User != null && GeneralMembers.User.Id > 0)
                {
                    config.AppSettings.Settings["us"].Value = GeneralMembers.User.Email;
                    config.AppSettings.Settings["pss"].Value = GeneralMembers.User.Password;
                }
                config.Save(ConfigurationSaveMode.Modified);
                Restart = true;
                Application.Restart();
            }
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            Configuration config;
            config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            if (config.AppSettings.Settings["us"].Value != "" && config.AppSettings.Settings["pss"].Value != "")
            {
                bool DatabaseOffline = File.Exists(Application.StartupPath + @"\Org");
                var us = CheckUser(config.AppSettings.Settings["us"].Value, config.AppSettings.Settings["pss"].Value, DatabaseOffline);
                if (us != null && us.Id > 0)
                {
                    GeneralMembersRepo.User = GeneralMembers.User = us;
                    if (!DatabaseOffline)
                    {
                        if (Helper.DownloadDatabaseOffline(Application.StartupPath))
                        {
                            new UserRepo().UpdateDatabase(GeneralMembers.User.SchemaName);
                            var usList = new UserRepo(false).getAllBySchema(us.SchemaName);
                            foreach (var u in usList)
                            {
                                u.Id = 0;
                                u.DbVersion = 0;
                                var res = new UserRepo().Save(u);
                                if (res.Email == us.Email)
                                    GeneralMembersRepo.User = GeneralMembers.User = res;
                            }
                        }
                    }
                    AfterLogIn();
                    if (config.AppSettings.Settings["keepLogin"].Value == "0")
                    {
                        config.AppSettings.Settings["us"].Value = "";
                        config.AppSettings.Settings["pss"].Value = "";
                        config.Save(ConfigurationSaveMode.Modified);
                    }
                }
            }

            pnlLogIn.Visible = GeneralMembers.User == null || GeneralMembers.User.Id == 0;

            Panel.CheckForIllegalCrossThreadCalls = false;
            Label.CheckForIllegalCrossThreadCalls = false;
            if (File.Exists(@"\UpdatesSysUpdate.zip"))
            {
                if (File.Exists(@"\UpdateOrg.exe.config"))
                    File.Delete(@"\UpdateOrg.exe.config");
                if (File.Exists(@"\UpdateOrg.exe"))
                    File.Delete(@"\UpdateOrg.exe");              
                ZipFile.ExtractToDirectory(Application.StartupPath + @"\UpdatesSysUpdate.zip", Application.StartupPath);
                File.Delete(@"\UpdatesSysUpdate.zip");
            }
        }

        private void MainFormClosed(object sender, FormClosedEventArgs e)
        {
            AsyncDatabaseWorker.Stop = true;
            AsyncDatabaseWorker.IsRuning = false;
            if (!Restart)
            {
                var p = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "UpdateOrg");
                if (p != null)
                    p.Kill();
                var p2 = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "Organizer");
                if (p2 != null)
                    p2.Kill();
                Environment.Exit(Environment.ExitCode);
            }
            
        }

        private void ColseAppliction(object sender, EventArgs e)
        {
            Helper.Sound(Application.StartupPath, SoudType.MenuMouseSelect);
            Application.Exit();
        }

        private void MinimizedAppliction(object sender, EventArgs e)
        {
            Helper.Sound(Application.StartupPath, SoudType.MenuMouseSelect);
            this.WindowState = FormWindowState.Minimized;
        }

        public void SelectMenu(string Text, bool Open, int Id = 0)
        {
            try
            {
                if (pnlAllScrean.Controls["_frm_Invoice1"] != null)
                    pnlAllScrean.Controls.Remove(pnlAllScrean.Controls["_frm_Invoice1"]);

                if (pnlAllScrean.Controls["_frm_ReturnInvoice1"] != null)
                    pnlAllScrean.Controls.Remove(pnlAllScrean.Controls["_frm_ReturnInvoice1"]);

                if (pnlAllScrean.Controls["_frm_PanelData1"] != null)
                    pnlAllScrean.Controls.Remove(pnlAllScrean.Controls["_frm_PanelData1"]);

                if (pnlAllScrean.Controls["_frm_Report1"] != null)
                    pnlAllScrean.Controls.Remove(pnlAllScrean.Controls["_frm_Report1"]);

                if (Application.OpenForms["_frm_Invoice1"] != null)
                    Application.OpenForms["_frm_Invoice1"].Close();

                if (Application.OpenForms["_frm_ReturnInvoice1"] != null)
                    Application.OpenForms["_frm_ReturnInvoice1"].Close();

                if (Application.OpenForms["_frm_PanelData1"] != null)
                    Application.OpenForms["_frm_PanelData1"].Close();

                if (Application.OpenForms["_frm_Report1"] != null)
                    Application.OpenForms["_frm_Report1"].Close();

                if (Open)
                    scearnOpened = Text;
                else
                    scearnOpened = "";


                switch (Text.ToLower())
                {
                    case "invoice":
                        if (this.pnlAllScrean.Controls["_frm_Invoice1"] == null && Open)
                        {
                            _frm_Invoice = new frm_Invoice();
                            _frm_Invoice.Name = "_frm_Invoice1";
                            _frm_Invoice.TopLevel = false;
                            _frm_Invoice.Dock = DockStyle.Fill;
                            if (Id > 0)
                                _frm_Invoice.toolSearchInfo1_SelectEdit(Id);
                            this.pnlAllScrean.Controls.Add(_frm_Invoice);
                        }
                        if (_frm_Invoice != null)
                            btnInvoice.Selected = _frm_Invoice.Visible = Open;
                        break;
                    case "return":
                        if (this.pnlAllScrean.Controls["_frm_ReturnInvoice1"] == null && Open)
                        {
                            _frm_ReturnInvoice = new frm_ReturnInvoice();
                            _frm_ReturnInvoice.Name = "_frm_ReturnInvoice1";
                            _frm_ReturnInvoice.TopLevel = false;
                            _frm_ReturnInvoice.Dock = DockStyle.Fill;
                            if (Id > 0)
                                _frm_ReturnInvoice.SelectEdit(Id);
                            this.pnlAllScrean.Controls.Add(_frm_ReturnInvoice);
                        }
                        if (_frm_ReturnInvoice != null)
                            menuItemTool1.Selected = _frm_ReturnInvoice.Visible = Open;
                        break;
                    case "data":
                        if (this.pnlAllScrean.Controls["_frm_PanelData1"] == null && Open)
                        {
                            _frm_PanelData = new frm_PanelData();
                            _frm_PanelData.Name = "_frm_PanelData1";
                            _frm_PanelData.TopLevel = false;
                            _frm_PanelData.Dock = DockStyle.Fill;
                            _frm_PanelData.Visible = false;
                            this.pnlAllScrean.Controls.Add(_frm_PanelData);
                        }
                        if (_frm_PanelData != null)
                            btnData.Selected = _frm_PanelData.Visible = Open;
                        break;
                    case "report":
                        if (this.pnlAllScrean.Controls["_frm_Report1"] == null && Open)
                        {
                            _frm_Report = new frm_Report();
                            _frm_Report.Name = "_frm_Report1";
                            _frm_Report.TopLevel = false;
                            _frm_Report.Dock = DockStyle.Fill;
                            _frm_Report.Visible = false;
                            this.pnlAllScrean.Controls.Add(_frm_Report);
                        }
                        if (_frm_Report != null)
                            btnReport.Selected = _frm_Report.Visible = Open;
                        break;
                    case "min":
                        break;
                    case "close":
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Menu_SelectFromMenu(object sender, EventArgs e)
        {
            try
            {
                SelectMenu(scearnOpened, false);
                SelectMenu(((Control)sender).Tag.ToString().ToLower(), true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LogInClick(object sender, EventArgs e)
        {
            Helper.Sound(Application.StartupPath, SoudType.InClick);
            if (ValidationLogin())
            {
                bool DatabaseOffline = File.Exists(Application.StartupPath + @"\Org");
                var us = CheckUser(txtUserName.Text, txtPassword.Text, DatabaseOffline);
                if (us != null && us.Id > 0)
                {
                    GeneralMembersRepo.User = GeneralMembers.User = us;
                    if (checkBox1.Checked)
                    {
                        Configuration config;
                        config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                        config.AppSettings.Settings["us"].Value = GeneralMembers.User.Email;
                        config.AppSettings.Settings["pss"].Value = GeneralMembers.User.Password;
                        config.AppSettings.Settings["keepLogin"].Value = "1";
                        config.Save(ConfigurationSaveMode.Modified);
                    }

                    if (!DatabaseOffline)
                    {
                        if (Helper.DownloadDatabaseOffline(Application.StartupPath))
                        {
                            new UserRepo().UpdateDatabase(GeneralMembers.User.SchemaName);
                            var usList = new UserRepo(false).getAllBySchema(us.SchemaName);
                            foreach (var u in usList)
                            {
                                u.Id = 0;
                                u.DbVersion = 0;
                                var res = new UserRepo().Save(u);
                                if (res.Email == us.Email)
                                    GeneralMembersRepo.User = GeneralMembers.User = res;
                            }
                        }
                    }

                    AfterLogIn();
                }
            }
        }

        public void LogOff(object sender, EventArgs e)
        {
            Helper.Sound(Application.StartupPath, SoudType.OutClick);
            Configuration config;
            config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.AppSettings.Settings["us"].Value = "";
            config.AppSettings.Settings["pss"].Value = "";
            config.AppSettings.Settings["keepLogin"].Value = "0";
            config.Save(ConfigurationSaveMode.Modified);
            Restart = true;
            Application.Restart();
        }

        private void txtUserNameKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                SendKeys.Send("{tab}");
        }

        private void txtPassw0rdKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                LogInClick(null, null);
        }

        private void ForgotPassword(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://organizersys.com/Home/ForgotPassword");
        }

        private void ButtonsMouseEnter(object sender, EventArgs e)
        {
            Helper.Sound(Application.StartupPath, SoudType.AddQuantity);
        }

        private UsersApp CheckUser(string email, string password, bool offLine = true)
        {
            if (!offLine && !Helper.CheckInternetConnection())
            {
                string Title = GeneralMembers.Lang == "ar" ? "غير متصل بالانترنت" : "Not connected to the internet";
                string Msg = GeneralMembers.Lang == "ar" ? "تأكد من الاتصال بالانترنت" : "Make sure to connect to the Internet";
                MessageORG.Show(Title, Msg, CountButton.One, MessageBoxIcon.Error);
                txtUserName.Focus();
                return null;
            }

            var usRepo = new UserRepo(offLine);
            var us = usRepo.CheckEmail(email);
            if (us == null || us.Id == 0)
            {
                string Title = GeneralMembers.Lang == "ar" ? "خطأ فى تسجيل الدخول" : "Errore Login";
                string Msg = GeneralMembers.Lang == "ar" ? "تأكد من البريد الالكتروني" : "Check the email";
                MessageORG.Show(Title, Msg, CountButton.One, MessageBoxIcon.Error);
                txtUserName.Focus();
                return null;
            }

            us = usRepo.Login(email, password);
            if (us == null || us.Id == 0)
            {
                string Title = GeneralMembers.Lang == "ar" ? "خطأ فى تسجيل الدخول" : "Errore Login";
                string Msg = GeneralMembers.Lang == "ar" ? "تأكد من كلمة المرور" : "Check the password";
                MessageORG.Show(Title, Msg, CountButton.One, MessageBoxIcon.Error);
                txtPassword.Focus();
                return null;
            }

            return us;
        }

        private bool ValidationLogin()
        {
            if (txtUserName.Text == "")
            {
                string Title = GeneralMembers.Lang == "ar" ? "خطأ فى تسجيل الدخول" : "Errore Login";
                string Msg = GeneralMembers.Lang == "ar" ? "ادخل البريد الالكتروني" : "Enter your email";
                MessageORG.Show(Title, Msg, CountButton.One, MessageBoxIcon.Error);
                txtUserName.Focus();
                return false;
            }
            if (txtPassword.Text == "")
            {
                string Title = GeneralMembers.Lang == "ar" ? "خطأ فى تسجيل الدخول" : "Errore Login";
                string Msg = GeneralMembers.Lang == "ar" ? "ادخل كلمة المرور" : "Enter your password";
                MessageORG.Show(Title, Msg, CountButton.One, MessageBoxIcon.Error);
                txtPassword.Focus();
                return false;
            }
            return true;
        }

        private void AfterLogIn()
        {
            new AsyncDatabaseWorker().StartProcessing(GeneralMembers.User);

            Helper.ButtonSound = GeneralMembers.User.ButtonSound;
            Helper.MusicSound = GeneralMembers.User.MusicSound;
            Helper.Volume = GeneralMembers.User.Volume;
            Helper.Music(Application.StartupPath);

            lblFooter.TextAlign = ContentAlignment.MiddleCenter;
            pnlMainMenu.Visible = true;
            pnlLogIn.Visible = false;
            label1.Visible = true;
            groupBox3.Visible = true;
            pnlUserName.Visible = true;
            label5.Text = "  " + GeneralMembers.User.UserName;
            string Job = "";
            if(GeneralMembers.User.RoleId > 2)
                Job =  GeneralMembers.Lang != "ar" ? "Cashier" : "كاشير";
            else
                Job = GeneralMembers.Lang != "ar" ? "Admin" : "مدير";
            label4.Text = "  ( " + Job + " ) " + string.Format("{0:dd-MM-yyyy}", DateTime.Now);
            if (GeneralMembers.User.RoleId > 2)
                btnData.Visible = false;
            Menu_SelectFromMenu(btnInvoice, null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var p = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "UpdateOrg");
            if (p != null)
                p.Kill();
            Application.Exit();
        }

        private void updateSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var pup = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "UpdateOrg");
            if (pup != null)
                pup.Kill();
            var p = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "UpdateOrg");
            if (p == null)
            {              
                Process ExternalProcess = new Process();
                ExternalProcess.StartInfo.FileName = Application.StartupPath + @"\UpdateOrg.exe";
                ExternalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                ExternalProcess.StartInfo.UseShellExecute = true;
                ExternalProcess.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                ExternalProcess.StartInfo.Verb = "runas";
                ExternalProcess.Start();
                ExternalProcess.WaitForExit();
            }
        }
    }
}