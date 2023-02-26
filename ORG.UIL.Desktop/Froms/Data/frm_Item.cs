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
using Excel = Microsoft.Office.Interop.Excel;

namespace ORG.UIL.Desktop.Froms.Data
{
    public partial class frm_Item : frm_TemplateData
    {
        List<Category> _MyGroupList;
        public List<Category> MyGroupList
        {
            get
            {
                return _MyGroupList;
            }
            set
            {
                _MyGroupList = value;
                if (_MyGroupList == null)
                    _MyGroupList = new List<Category>();
                panelGroupTool1.FillItems<Category>(_MyGroupList.ToList());
            }
        }

        Category _MyGroup;
        public new Category MyGroup
        {
            get
            {
                if (_MyGroup == null)
                    _MyGroup = new Category();
                return _MyGroup;
            }
            set
            {
                _MyGroup = value;
                if (_MyGroup == null)
                    _MyGroup = new Category();
                UsedNew = _MyGroup.Id > 0;
            }
        }

        List<Item> _MyObjectList;
        public List<Item> MyObjectList
        {
            get
            {
                return _MyObjectList;
            }
            set
            {
                _MyObjectList = value;
                panelUnitTool1.FillItems<Item>(_MyObjectList.ToList());
            }
        }

        Item _MyObject;
        public new Item MyObject
        {
            get
            {
                if (_MyObject == null)
                    _MyObject = new Item();
                _MyObject.CategoryId = MyGroup.Id;
                _MyObject.Code = txtCode.Text;
                _MyObject.Name = txtName.Text;
                _MyObject.Barcode = txtBarcode.Text;
                _MyObject.Description = txtDescription.Text;
                _MyObject.DefaultUnitId = int.Parse("0" + txtUnit.SelectedValue);
                _MyObject.DefaultQuantity = decimal.Parse(txtQuantity.Text);
                _MyObject.SellingPrice = decimal.Parse(txtPrice.Text);
                _MyObject.ImageFile = picImage.ShowImage;
                return _MyObject;
            }
            set
            {
                _MyObject = value;
                if (_MyObject == null)
                    _MyObject = new Item();
                txtCode.Text = _MyObject.Id > 0 ? _MyObject.Code : new ItemRepo().GetMaxCode();
                txtName.Text = _MyObject.Name;
                txtBarcode.Text = _MyObject.Barcode;
                txtDescription.Text = _MyObject.Description;
                txtUnit.SelectedValue = _MyObject.DefaultUnitId;
                txtQuantity.Text = _MyObject.Id > 0 ? string.Format("{0}", _MyObject.DefaultQuantity) : "1";
                txtPrice.Text = string.Format("{0}", _MyObject.SellingPrice);
                picImage.ShowImage = _MyObject.ImageFile;
                UsedDelete = _MyObject.Id > 0;
            }
        }

        public frm_Item()
        {
            InitializeComponent();
            OpenClose = false;
            panelGroupTool1.WidthItem = panelGroupTool1.Width - 30;
        }

        public override void panelGroupTool1_SelectITem(object sender, EventArgs e)
        {
            MyGroup = (Category)sender;
            panelUnitTool1.CurrPage = 1;
            MyObjectList = new ItemRepo().GetAllGroupId(MyGroup.Id , txtSearch.Text , 1);
        }

        public override void panelUnitTool1_SelectITem(object sender, EventArgs e)
        {
            var ob = (int)sender;
            MyObject = new ItemRepo().getById(ob);
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

            MyObjectList = new List<Item>();
            bttnSave.Enabled = false;
            AsyncDatabaseWorker.Stop = true;
            MyObject = new ItemRepo().Save(MyObject);
            if (MyObject != null && MyObject.Id > 0)
            {
                string Title1 = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                string Msg1 = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                MessageORG.ShowConfirm(Title1, Msg1);
                OpenClose = false;
                MyObject = null;
                panelUnitTool1.CurrPage = 1;
                MyObjectList = new ItemRepo().GetAllGroupId(MyGroup.Id, txtSearch.Text, 1);
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

            MyObjectList = new List<Item>();
            bttnSave.Enabled = false;
            AsyncDatabaseWorker.Stop = true;
            MyObject = new ItemRepo().Save(MyObject);
            if (MyObject != null && MyObject.Id > 0 )
            {
                string Title1 = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                string Msg1 = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                MessageORG.ShowConfirm(Title1, Msg1);
                MyObject = null;
                panelUnitTool1.CurrPage = 1;
                MyObjectList = new ItemRepo().GetAllGroupId(MyGroup.Id , txtSearch.Text , 1);
            }
            bttnSave.Enabled = true;
        }

        public override void DeleteRow(object sender, EventArgs e)
        {
            try
            {
                string Title = GeneralMembers.Lang == "ar" ? "حذف ملف" : "Delete File";
                string Msg = GeneralMembers.Lang == "ar" ? "... هل تريد حفظ الملف" : "Are you want Delete this file ...";
                if (MessageORG.Show(Title, Msg , MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel) return;

                MyObjectList = new List<Item>();
                bttnDelete.Enabled = false;
                AsyncDatabaseWorker.Stop = true;
                if (new ItemRepo().Remove(MyObject.Id))
                {
                    string Title1 = GeneralMembers.Lang == "ar" ? "حذف" : "Delete";
                    string Msg1 = GeneralMembers.Lang == "ar" ? "تم حذف الملف بنجاح" : "The file has been deleted successfully";
                    MessageORG.ShowConfirm(Title1, Msg1);
                    OpenClose = false;
                    MyObject = null;
                    panelUnitTool1.CurrPage = 1;
                    MyObjectList = new ItemRepo().GetAllGroupId(MyGroup.Id, txtSearch.Text, 1);
                    Helper.Sound(Application.StartupPath, SoudType.DeleteClick);
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
            MyObjectList = new ItemRepo().GetAllGroupId(MyGroup.Id, txtSearch.Text, 1);
        }


        public override void RefrashRow(object sender, EventArgs e)
        {
            OpenClose = false;
            panelUnitTool1.CurrPage = 1;
            MyObjectList = new ItemRepo().GetAllGroupId(MyGroup.Id , txtSearch.Text , 1);
            txtUnit.DataSource = new UnitRepo().getAll();
            txtUnit.DisplayMember = "Name";
            txtUnit.ValueMember = "Id";
        }

        public override void TemplateRow(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(Application.StartupPath + @"/Template/ItemTemplate.csv");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save Template File";
            saveFileDialog1.FileName = "Item Template";
            saveFileDialog1.Filter = "Excel files csv (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                xlWorkbook.SaveCopyAs(saveFileDialog1.FileName);
            }
            xlWorkbook.Close();
        }

        public override void ImportRow(object sender, EventArgs e)
        {
            AsyncDatabaseWorker.Stop = true;
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;

            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            OpenFileDialog1.Title = "Excel File to Edit";
            OpenFileDialog1.FileName = "";
            OpenFileDialog1.Filter = "Excel File|*.csv;*.xlsx;*.xls";

            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(OpenFileDialog1.FileName);
                xlWorkSheet = xlWorkBook.Worksheets[1];

                for (int iRow = 2; iRow <= xlWorkSheet.Rows.Count; iRow++)
                {
                    if ("" + xlWorkSheet.Cells[iRow, 3].value == "")
                        break;

                    Category cat = new CategoryRepo().getByName(xlWorkSheet.Cells[iRow, 1].value + "");
                    if (cat == null || cat.Id == 0)
                        cat = new CategoryRepo().Save(new Category() { Code = "1", Name = xlWorkSheet.Cells[iRow, 1].value + "" });

                    Unit un = new UnitRepo().getByName(xlWorkSheet.Cells[iRow, 5].value + "");
                    if (un == null || un.Id == 0)
                        un = new UnitRepo().Save(new Unit() {Name = xlWorkSheet.Cells[iRow, 5].value + "" });

                    decimal price = 0;
                    decimal.TryParse(xlWorkSheet.Cells[iRow, 6].value + "", out price);

                    Item  it = new ItemRepo().getByName(xlWorkSheet.Cells[iRow, 3].value + "");
                    if (it == null || it.Id == 0)
                        new ItemRepo().Save(new Item() { 
                            CategoryId = cat.Id ,  
                            Code = xlWorkSheet.Cells[iRow, 2].value + "",
                            Name = xlWorkSheet.Cells[iRow, 3].value + "" ,
                            Barcode = xlWorkSheet.Cells[iRow, 4].value + "",
                            DefaultUnitId = un.Id,
                            PurchasePrice = price,
                            SellingPrice = price,
                            DefaultQuantity = 1,
                            IsSerialized = false,
                            ReorderPoint = 0 ,
                            TaxPercent = 0                            
                        });
                }

                xlWorkBook.Close();
                xlApp.Quit();
            }            
            RefrashRow(null, null);
        }

        public override bool Validation()
        {
            if (txtCode.Text == "" || txtName.Text == "" || txtQuantity.Text == "" || txtPrice.Text == "") return false;
            decimal x = 0;
            if (!decimal.TryParse(txtPrice.Text, out x) || !decimal.TryParse(txtQuantity.Text, out x)) return false;
            if (int.Parse("0" + txtUnit.SelectedValue) == 0) return false;
            return true;
        }

        private void panelUnitTool1_pagging(int page)
        {
            MyObjectList = new ItemRepo().GetAllGroupId(MyGroup.Id , txtSearch.Text , page);
        }

        public override void Search()
        {
            panelUnitTool1.CurrPage = 1;
            MyObjectList = new List<Item>();
            MyGroupList = new CategoryRepo().GetAllByItemName(txtSearch.Text);
            if (MyGroupList != null && MyGroupList.Count > 0)
                MyObjectList = new ItemRepo().GetAllGroupId(MyGroupList[0].Id , txtSearch.Text, 1);
            txtSearch.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            MyGroupList = new CategoryRepo().getAllCategories();
            UsedNew = _MyGroup != null && _MyGroup.Id > 0;
            txtUnit.DataSource = new UnitRepo().getAll();
            txtUnit.DisplayMember = "Name";
            txtUnit.ValueMember = "Id";
            txtSearch.Focus();
        }
    }
}
