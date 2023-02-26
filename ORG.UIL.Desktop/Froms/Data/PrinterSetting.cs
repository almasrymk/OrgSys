using ORGEntity;
using ORGRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORG.UIL.Desktop.Froms.Data
{
    public partial class frmPrinterSetting : frm_TemplateData
    {
        frm_panelblack _frm_panelMessage;
        public frmPrinterSetting()
        {
            InitializeComponent();
            _frm_panelMessage = new frm_panelblack();
            _frm_panelMessage.Name = "_frm_panelMessage2";
            _frm_panelMessage.Show();      
            
        }

        List<PrinterSetting> _MyObjectList;
        public List<PrinterSetting> MyObjectList
        {
            get
            {
                return _MyObjectList;
            }
            set
            {
                _MyObjectList = value;
                panelUnitTool1.FillItems<PrinterSetting>(_MyObjectList.ToList());
            }
        }

        PrinterSetting _MyObject;
        public new PrinterSetting MyObject
        {
            get
            {
                if (_MyObject == null)
                    _MyObject = new PrinterSetting();
                _MyObject.TypeInvoiceId = 1;
                _MyObject.TypePrinter = int.Parse("0" + cmbInvoiceTemplate.SelectedValue);
                _MyObject.PrintLang = int.Parse("0" + comboBox1.SelectedValue);
                _MyObject.PrinterName = cmbPrinterName.Text;             
                return _MyObject;
            }
            set
            {
                _MyObject = value;
                if (_MyObject != null && _MyObject.Id > 0)
                {
                    cmbInvoiceTemplate.SelectedValue = _MyObject.TypePrinter;
                    comboBox1.SelectedValue = _MyObject.PrintLang;
                    cmbPrinterName.Text = _MyObject.PrinterName;
                }
                else
                {
                    cmbInvoiceTemplate.SelectedValue = 0;
                    cmbPrinterName.SelectedValue = 0;
                    comboBox1.SelectedValue = 0;
                }
            }
        }

        public override void LoadForm(object sender, EventArgs e)
        {
            pnlDisplay.Dock = DockStyle.Fill;
            pnlControl1.Dock = DockStyle.Fill;
            OpenClose = false;
            MyObjectList = new PrinterSettingRepo().getAll();

            List<InvoiceTemplate> invoiceTemplates = new List<InvoiceTemplate>();
            invoiceTemplates.Add(new InvoiceTemplate() { Id = 0, Name = GeneralMembers.Lang != "ar" ? "Invoice Total Price & Quantity" : "إجمالى سعر الفاتورة و الكميات"});
            invoiceTemplates.Add(new InvoiceTemplate() { Id = 2, Name = GeneralMembers.Lang != "ar" ? "Invoice Total Price Only" : "إجمالى سعر الفاتورة فقط"});
            invoiceTemplates.Add(new InvoiceTemplate() { Id = 1, Name = GeneralMembers.Lang != "ar" ? "Invoice Quantity Only" : "كميات الفانورة فقط" });

            cmbInvoiceTemplate.ValueMember = "Id";
            cmbInvoiceTemplate.DisplayMember = "Name";
            cmbInvoiceTemplate.DataSource = invoiceTemplates;

            List<InvoiceTemplate> invoiceTemplates2 = new List<InvoiceTemplate>();
            invoiceTemplates2.Add(new InvoiceTemplate() { Id = 1, Name = GeneralMembers.Lang == "ar" ? "عربي" : "Arabic" });
            invoiceTemplates2.Add(new InvoiceTemplate() { Id = 2, Name = GeneralMembers.Lang == "ar" ? "انجليزي" : "English" });

            comboBox1.ValueMember = "Id";
            comboBox1.DisplayMember = "Name";
            comboBox1.DataSource = invoiceTemplates2;
            

            foreach (var strPrinter in PrinterSettings.InstalledPrinters)
            {
                cmbPrinterName.Items.Add(strPrinter);
                if (strPrinter != null)
                    cmbPrinterName.Text = strPrinter.ToString();
            }
            txtSearch.Focus();
        }

        public override void panelUnitTool1_SelectITem(object sender, EventArgs e)
        {
            var ob = (int)sender;
            MyObject = new PrinterSettingRepo().getById(ob);
            OpenClose = true;
        }

        public override void SaveRow(object sender, EventArgs e)
        {
            string Title = GeneralMembers.Lang == "ar" ? "خطأ فى الحفظ" : "Errore Save";
            string Msg = GeneralMembers.Lang == "ar" ? "لا يمكن حفط هذا الملف" : "Can't Save This File ...";
           
            MyObjectList = new List<PrinterSetting>();
            bttnSave.Enabled = false;
            MyObject = new PrinterSettingRepo().Save(MyObject);
            if (MyObject != null && MyObject.Id > 0)
            {
                MyObject = null;
                OpenClose = false;
                MyObjectList = new PrinterSettingRepo().getAll();
            }
            bttnSave.Enabled = true;
        }

        public override void SaveNewRow(object sender, EventArgs e)
        {
            string Title = GeneralMembers.Lang == "ar" ? "خطأ فى الحفظ" : "Errore Save";
            string Msg = GeneralMembers.Lang == "ar" ? "لا يمكن حفط هذا الملف" : "Can't Save This File ...";

            MyObjectList = new List<PrinterSetting>();
            bttnSaveNew.Enabled = false;
            MyObject = new PrinterSettingRepo().Save(MyObject);
            if (MyObject != null && MyObject.Id > 0)
            {
                MyObject = null;
                MyObjectList = new PrinterSettingRepo().getAll();
            }
            bttnSaveNew.Enabled = true;
        }

        public override void DeleteRow(object sender, EventArgs e)
        {
            string Title = GeneralMembers.Lang == "ar" ? "حذف ملف" : "Delete File";
            string Msg = GeneralMembers.Lang == "ar" ? "... هل تريد حفظ الملف" : "Are you want Delete this file ...";
            //if (MessageORG.Show(Title, Msg) == System.Windows.Forms.DialogResult.Cancel) return;
            MyObjectList = new List<PrinterSetting>();
            bttnDelete.Enabled = false;
            if (new PrinterSettingRepo().Remove(MyObject.Id))
            {                
                MyObject = null;
                OpenClose = false;
                Helper.Sound(Application.StartupPath, SoudType.DeleteClick);
                MyObjectList = new PrinterSettingRepo().getAll();
            }
            bttnDelete.Enabled = true;
        }

        public override void RefrashRow(object sender, EventArgs e)
        {
            OpenClose = false;
            MyObjectList = new PrinterSettingRepo().getAll();
        }

        private void lblb1_Click(object sender, EventArgs e)
        {
            Application.OpenForms["_frm_panelMessage2"].Close();                        
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

    class InvoiceTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
