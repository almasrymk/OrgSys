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
    public partial class frm_Users : frm_TemplateData
    {
        List<UsersApp> _MyObjectList;
        public List<UsersApp> MyObjectList
        {
            get
            {
                return _MyObjectList;
            }
            set
            {
                _MyObjectList = value;
                panelUnitTool1.FillItems<UsersApp>(_MyObjectList.ToList());
            }
        }

        UsersApp _MyObject;
        public new UsersApp MyObject
        {
            get
            {
                if (_MyObject == null)
                    _MyObject = new UsersApp();

                _MyObject.UserName = txtName.Text;
                _MyObject.Telephone = textBox1.Text;
                _MyObject.Address = textBox2.Text;
                _MyObject.Email = textBox3.Text;
                _MyObject.Password = textBox4.Text;
                _MyObject.RoleId = 2;
                _MyObject.SchemaName = GeneralMembers.User.SchemaName;
                _MyObject.CompanyName = GeneralMembers.User.CompanyName;
                _MyObject.TypeSystem = GeneralMembers.User.TypeSystem;
                _MyObject.ElectronicScaleCode = GeneralMembers.User.ElectronicScaleCode;

                if (cmbPrinterName.SelectedValue != null)
                    _MyObject.RoleId = int.Parse("0" + cmbPrinterName.SelectedValue);
                return _MyObject;
            }
            set
            {
                _MyObject = value;
                if (_MyObject == null)
                    _MyObject = new UsersApp();

                txtName.Text = _MyObject.UserName;
                textBox1.Text = _MyObject.Telephone;
                textBox2.Text = _MyObject.Address;
                textBox3.Text = _MyObject.Email;
                textBox4.Text = _MyObject.Password;
                if (_MyObject.RoleId != null)
                    cmbPrinterName.SelectedValue = _MyObject.RoleId;
                UsedDelete = _MyObject.Id > 0;
            }
        }

        public frm_Users()
        {
            InitializeComponent();
            OpenClose = false;
            List<InvoiceTemplate> invoiceTemplates = new List<InvoiceTemplate>();
            invoiceTemplates.Add(new InvoiceTemplate() { Id = 2, Name = GeneralMembers.Lang != "ar" ? "Admin" : "مدير" });
            invoiceTemplates.Add(new InvoiceTemplate() { Id = 3, Name = GeneralMembers.Lang != "ar" ? "Casher" : "كاشير" });

            cmbPrinterName.ValueMember = "Id";
            cmbPrinterName.DisplayMember = "Name";
            cmbPrinterName.DataSource = invoiceTemplates;
        }

        public override void panelUnitTool1_SelectITem(object sender, EventArgs e)
        {
            var ob = (int)sender;
            MyObject = new UserRepo().getById(ob);
            OpenClose = true;
        }

        public override void NewRow(object sender, EventArgs e)
        {
            MyObject = null;
            OpenClose = true;
        }

        public override void SaveRow(object sender, EventArgs e)
        {
            string Title = GeneralMembers.Lang == "ar" ? "خطأ فى الحفظ" : "Errore Save";
            string Msg = GeneralMembers.Lang == "ar" ? "لا يمكن حفط هذا الملف" : "Can't Save This File ...";
            if (!Validation())
            {
                MessageORG.Show(Title, Msg, CountButton.One, MessageBoxIcon.Error);
                return;
            }

            MyObjectList = new List<UsersApp>();
            bttnSave.Enabled = false;
            AsyncDatabaseWorker.Stop = true;
            MyObject = new UserRepo().Save(MyObject);
            if (MyObject != null && MyObject.Id > 0)
            {
                if (MyObject.Id == GeneralMembers.User.Id)
                    GeneralMembers.User = MyObject;
                string Title1 = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                string Msg1 = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                MessageORG.ShowConfirm(Title1, Msg1);
                OpenClose = false;
                MyObject = null;
                panelUnitTool1.CurrPage = 1;
                MyObjectList = new UserRepo().getAll(txtSearch.Text, 1 , 100 , false , GeneralMembers.User.SchemaName);
            }
            bttnSave.Enabled = true;
        }

        public override void SaveNewRow(object sender, EventArgs e)
        {
            string Title = GeneralMembers.Lang == "ar" ? "خطأ فى الحفظ" : "Errore Save";
            string Msg = GeneralMembers.Lang == "ar" ? "لا يمكن حفط هذا الملف" : "Can't Save This File ...";
            if (!Validation())
            {
                MessageORG.Show(Title, Msg, CountButton.One, MessageBoxIcon.Error);
                return;
            }

            MyObjectList = new List<UsersApp>();
            bttnSaveNew.Enabled = false;
            AsyncDatabaseWorker.Stop = true;
            MyObject = new UserRepo().Save(MyObject);
            if (MyObject != null && MyObject.Id > 0)
            {
                string Title1 = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                string Msg1 = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                MessageORG.ShowConfirm(Title1, Msg1);
                MyObject = null;
                panelUnitTool1.CurrPage = 1;
                MyObjectList = new UserRepo().getAll(txtSearch.Text, 1 , 100 , false , GeneralMembers.User.SchemaName);
            }
            bttnSaveNew.Enabled = true;
        }

        public override void DeleteRow(object sender, EventArgs e)
        {
            try
            {
                string Title = GeneralMembers.Lang == "ar" ? "حذف ملف" : "Delete File";
                string Msg = GeneralMembers.Lang == "ar" ? "... هل تريد حفظ الملف" : "Are you want Delete this file ...";
                if (MessageORG.Show(Title, Msg, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel) return;
                MyObjectList = new List<UsersApp>();
                bttnDelete.Enabled = false;
                AsyncDatabaseWorker.Stop = true;
                if (new UserRepo().Remove(MyObject.Id))
                {
                    string Title1 = GeneralMembers.Lang == "ar" ? "حذف" : "Delete";
                    string Msg1 = GeneralMembers.Lang == "ar" ? "تم حذف الملف بنجاح" : "The file has been deleted successfully";
                    MessageORG.ShowConfirm(Title1, Msg1);
                    OpenClose = false;
                    MyObject = null;
                    panelUnitTool1.CurrPage = 1;
                    Helper.Sound(Application.StartupPath, SoudType.DeleteClick);
                    MyObjectList = new UserRepo().getAll(txtSearch.Text, 1 , 100 , false , GeneralMembers.User.SchemaName);
                }
                bttnDelete.Enabled = true;
            }
            catch (Exception)
            {
                string Title = GeneralMembers.Lang == "ar" ? "خطأ فى حذف" : "Errore Delete";
                string Msg = GeneralMembers.Lang == "ar" ? "لا يمكن حذف هذا الملف" : "Can't delete This File ...";
                MessageORG.Show(Title, Msg, CountButton.One, MessageBoxIcon.Error);
                bttnDelete.Enabled = true;
            }
        }

        public override void CancelRow(object sender, EventArgs e)
        {
            OpenClose = false;
            panelUnitTool1.CurrPage = 1;
            MyObjectList = new UserRepo().getAll(txtSearch.Text, 1 , 100 , false , GeneralMembers.User.SchemaName);
        }

        public override void RefrashRow(object sender, EventArgs e)
        {
            OpenClose = false;
            panelUnitTool1.CurrPage = 1;
            MyObjectList = new UserRepo().getAll(txtSearch.Text, 1 , 100 , false , GeneralMembers.User.SchemaName);
        }

        public override bool Validation()
        {
            if (txtName.Text == "") return false;
            return true;
        }

        private void panelUnitTool1_pagging(int page)
        {
            MyObjectList = new UserRepo().getAll(txtSearch.Text, page , 100 , false , GeneralMembers.User.SchemaName);
        }

        public override void Search()
        {
            panelUnitTool1.CurrPage = 1;
            MyObjectList = new UserRepo().getAll(txtSearch.Text, 1, 100, false, GeneralMembers.User.SchemaName);
            txtSearch.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            panelUnitTool1.CurrPage = 1;
            MyObjectList = new UserRepo().getAll(txtSearch.Text, 1, 100, false, GeneralMembers.User.SchemaName);
            txtSearch.Focus();
        }
    }
}
