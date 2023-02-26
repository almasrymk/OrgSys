using ORG.UIL.Desktop.Untility;
using ORGEntity;
using ORGRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORG.UIL.Desktop.Froms.Data
{
    public partial class frmSetting : Form
    {
        UsersApp _User;
        public UsersApp User
        {
            get
            {
                if (_User == null)
                    _User = new UsersApp();

                _User.Email = txtEmail.Text;
                _User.Password = txtPassword.Text;
                _User.CompanyName = txtCompanyName.Text;
                _User.Telephone = txtTelephones.Text;
                _User.Address = txtAddress.Text;
                _User.ElectronicScaleCode = txtElectronicScaleCode.Text;
                _User.TypeSystem = chkInvoiceClassic.Checked ? 2 : 1;
                _User.Tax = decimal.Parse("0" + textBox1.Text);
                _User.Service = decimal.Parse("0" + textBox2.Text);
                _User.ButtonSound = checkBox1.Checked;
                _User.MusicSound = checkBox2.Checked;
                _User.Volume = trackBar1.Value;
                _User.PrintLang = int.Parse("0" + comboBox1.SelectedValue);
                return _User;
            }
            set
            {
                _User = value;
                if (_User == null)
                    _User = new UsersApp();
                txtEmail.Text = _User.Email;
                txtPassword.Text = _User.Password;
                txtCompanyName.Text = _User.CompanyName;
                txtTelephones.Text = _User.Telephone;
                txtAddress.Text = _User.Address;
                textBox1.Text = "" + _User.Tax;
                textBox2.Text = "" + _User.Service;
                txtElectronicScaleCode.Text = _User.ElectronicScaleCode;
                chkInvoiceClassic.Checked  = _User.TypeSystem == 1 ? false : true;
                checkBox1.Checked = _User.ButtonSound;
                checkBox2.Checked = _User.MusicSound;
                trackBar1.Value = _User.Volume;
                comboBox1.SelectedValue = _User.PrintLang;
            }
        }

        public frmSetting()
        {
            InitializeComponent();
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            List<InvoiceTemplate> invoiceTemplates2 = new List<InvoiceTemplate>();
            invoiceTemplates2.Add(new InvoiceTemplate() { Id = 1, Name = GeneralMembers.Lang == "ar" ? "عربي" : "Arabic" });
            invoiceTemplates2.Add(new InvoiceTemplate() { Id = 2, Name = GeneralMembers.Lang == "ar" ? "انجليزي" : "English" });

            comboBox1.ValueMember = "Id";
            comboBox1.DisplayMember = "Name";
            comboBox1.DataSource = invoiceTemplates2;

            User = GeneralMembers.User;           
        }

        private void bttnSave_Click(object sender, EventArgs e)
        {
            string Titleerror = GeneralMembers.Lang == "ar" ? "خطأ فى الحفظ" : "Errore Save";
            string Msgerror = GeneralMembers.Lang == "ar" ? "لا يمكن حفط هذا الملف" : "Can't Save This File ...";
            if (!Validation())
            {
                MessageORG.Show(Titleerror, Msgerror, CountButton.One, MessageBoxIcon.Error);
                return;
            }
            AsyncDatabaseWorker.Stop = true;
            var us = new UserRepo().Save(User);
            if (us != null && us.Id >0)
            {
                GeneralMembers.User = User;
                string Title = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                string Msg = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                MessageORG.ShowConfirm(Title, Msg);

            }
        }

        public bool Validation()
        {
            decimal x = 0;
            if (textBox1.Text == "" || !decimal.TryParse(textBox1.Text , out x) ) return false;
            x = 0;
            if (textBox2.Text == "" || !decimal.TryParse(textBox2.Text, out x)) return false;
            return true;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (trackBar1.Focused)
            {
                Helper.SetVolume(trackBar1.Value);
                if (GeneralMembers.User.Id == _User.Id)
                    GeneralMembers.User.Volume = trackBar1.Value;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Helper.ButtonSound = checkBox1.Checked;
            if (GeneralMembers.User.Id == _User.Id)
                GeneralMembers.User.ButtonSound = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Helper.MusicSound = checkBox2.Checked;
            Helper.PlayStop(Application.StartupPath , checkBox2.Checked);
            if (GeneralMembers.User.Id == _User.Id)
                GeneralMembers.User.MusicSound = checkBox2.Checked;
        }
    }   
}
