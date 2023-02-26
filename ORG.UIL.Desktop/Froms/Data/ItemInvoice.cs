using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ORG.UIL.Desktop.Untility;
using ORGEntity;
using ORGRepository;

namespace ORG.UIL.Desktop.Froms.Data
{
    public partial class ItemInvoice : Form
    {
        frm_panelblack _frm_panelMessage;
        Item _MyObject;
        public new Item MyObject
        {
            get
            {
                if (_MyObject == null)
                    _MyObject = new Item();
                _MyObject.CategoryId = int.Parse("0" + cmbGroupItem.SelectedValue);
                _MyObject.Code = txtCode.Text;
                _MyObject.Name = txtName.Text;
                _MyObject.Barcode = txtBarcode.Text;
                _MyObject.Description = txtDescription.Text;
                _MyObject.DefaultUnitId = int.Parse("0" + txtUnit.SelectedValue);
                _MyObject.DefaultQuantity = decimal.Parse(txtQuantity.Text);
                _MyObject.SellingPrice = decimal.Parse(txtPrice.Text);
                return _MyObject;
            }
            set
            {
                _MyObject = value;
                if (_MyObject == null)
                    _MyObject = new Item();
                cmbGroupItem.SelectedValue = _MyObject.CategoryId;
                txtCode.Text = _MyObject.Id > 0 ? _MyObject.Code : new ItemRepo().GetMaxCode();
                txtName.Text = _MyObject.Name;
                txtBarcode.Text = _MyObject.Barcode;
                txtDescription.Text = _MyObject.Description;
                txtUnit.SelectedValue = _MyObject.DefaultUnitId;
                txtQuantity.Text = _MyObject.Id > 0 ? string.Format("{0}", _MyObject.DefaultQuantity) : "1";
                txtPrice.Text = string.Format("{0}", _MyObject.SellingPrice);               
            }
        }

        public ItemInvoice()
        {
            InitializeComponent();
            _frm_panelMessage = new frm_panelblack();
            _frm_panelMessage.Name = "_frm_panelMessage2";
            _frm_panelMessage.Show();
        }

        public ItemInvoice(string title , Item item)
        {
            InitializeComponent();
            label1.Text = title;
            MyObject = item;
            txtName.Focus();
            _frm_panelMessage = new frm_panelblack();
            _frm_panelMessage.Name = "_frm_panelMessage2";
            _frm_panelMessage.Show();
        }

        public ItemInvoice(string title , string barcode)
        {
            InitializeComponent();
            label1.Text = title;
            MyObject = new Item() { Barcode = barcode};
            txtName.Focus();
            _frm_panelMessage = new frm_panelblack();
            _frm_panelMessage.Name = "_frm_panelMessage2";
            _frm_panelMessage.Show();
        }

        public ItemInvoice(string title)
        {
            InitializeComponent();
            label1.Text = title;
            MyObject = new Item();
            txtName.Focus();
            _frm_panelMessage = new frm_panelblack();
            _frm_panelMessage.Name = "_frm_panelMessage2";
            _frm_panelMessage.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            txtUnit.DataSource = new UnitRepo().getAll();
            txtUnit.DisplayMember = "Name";
            txtUnit.ValueMember = "Id";
            txtUnit.SelectedValue = _MyObject.DefaultUnitId;

            cmbGroupItem.DataSource = new CategoryRepo().getAllCategories();
            cmbGroupItem.DisplayMember = "Name";
            cmbGroupItem.ValueMember = "Id";
            cmbGroupItem.SelectedValue = _MyObject.CategoryId;
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
                MessageORG.Show(Title, Msg , this.Name , CountButton.One , MessageBoxIcon.Error);
              
                return;
            }

            bttnNew.Enabled = false;
            AsyncDatabaseWorker.Stop = true;
            MyObject = new ItemRepo().Save(MyObject);
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
            if (txtCode.Text == "" || txtName.Text == "" || txtQuantity.Text == "" || txtPrice.Text == "") return false;
            decimal x = 0;
            if (!decimal.TryParse(txtPrice.Text, out x) || !decimal.TryParse(txtQuantity.Text, out x)) return false;
            if (int.Parse("0" + cmbGroupItem.SelectedValue) == 0) return false;
            if (int.Parse("0" + txtUnit.SelectedValue) == 0) return false;
            return true;
        }
    }
}
