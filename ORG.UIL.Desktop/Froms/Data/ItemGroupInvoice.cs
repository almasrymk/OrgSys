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
    public partial class ItemGroupInvoice : Form
    {
        frm_panelblack _frm_panelMessage;
        Category _MyObject;
        public new Category MyObject
        {
            get
            {
                if (_MyObject == null)
                    _MyObject = new Category();
                _MyObject.Code = "1";
                _MyObject.Name = txtName.Text;
                return _MyObject;
            }
            set
            {
                _MyObject = value;
                if (_MyObject == null)
                    _MyObject = new Category();
                txtName.Text = _MyObject.Name;
            }
        }

        public ItemGroupInvoice()
        {
            InitializeComponent();
            _frm_panelMessage = new frm_panelblack();
            _frm_panelMessage.Name = "_frm_panelMessage2";
            _frm_panelMessage.Show();
        }
      
        public ItemGroupInvoice(string title)
        {
            InitializeComponent();
            label1.Text = title;
            MyObject = new Category();
            txtName.Focus();
            _frm_panelMessage = new frm_panelblack();
            _frm_panelMessage.Name = "_frm_panelMessage2";
            _frm_panelMessage.Show();
        }
      
        private void bttnRefrach_Click(object sender, EventArgs e)
        {
            Application.OpenForms["_frm_panelMessage2"].Close();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void bttnNew_Click(object sender, EventArgs e)
        {
            string Title = GeneralMembers.Lang == "ar" ? "خطأ فى الحفظ" : "Errore Save";
            string Msg = GeneralMembers.Lang == "ar" ? "لا يمكن حفط هذا الملف" : "Can't Save This File ...";
            if (!Validation())
            {
                MessageORG.Show(Title, Msg, this.Name, CountButton.One, MessageBoxIcon.Error);

                return;
            }

            bttnNew.Enabled = false;
            AsyncDatabaseWorker.Stop = true;
            MyObject = new CategoryRepo().Save(MyObject);
            if (MyObject != null && MyObject.Id > 0)
            {
                Application.OpenForms["_frm_panelMessage2"].Close();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            bttnNew.Enabled = true;
        }

        public bool Validation()
        {
            if ( txtName.Text == "") return false;
            return true;
        }
    }
}
