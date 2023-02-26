using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ORG.UIL.Desktop.Untility;
using ORGEntity;
using ORGRepository;
using Excel = Microsoft.Office.Interop.Excel;

namespace ORG.UIL.Desktop
{
    public partial class frm_ItemGroup : frm_TemplateData
    {
        List<Category> _MyObjectList;
        public List<Category> MyObjectList
        {
            get
            {
                return _MyObjectList;
            }
            set
            {
                _MyObjectList = value;
                panelUnitTool1.FillItems<Category>(_MyObjectList.ToList());
            }
        }

        Category _MyObject;
        public new Category MyObject
        {
            get
            {
                if (_MyObject == null)
                    _MyObject = new Category();
                _MyObject.Name = txtName.Text;
                _MyObject.ImageFile = picImage.ShowImage;
                return _MyObject;
            }
            set
            {
                _MyObject = value;
                if (_MyObject == null)
                    _MyObject = new Category();
                txtName.Text = _MyObject.Name;
                picImage.ShowImage = _MyObject.ImageFile;
                UsedDelete = _MyObject.Id > 0;               
            }
        }

        public frm_ItemGroup()
        {
            InitializeComponent();
            OpenClose = false;
        }

        public override void panelUnitTool1_SelectITem(object sender, EventArgs e)
        {
            var ob = (int)sender;
            MyObject = new CategoryRepo().getById(ob);
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

            MyObjectList = new List<Category>();
            bttnSave.Enabled = false;
            AsyncDatabaseWorker.Stop = true;
            MyObject = new CategoryRepo().Save(MyObject);
            if (MyObject != null && MyObject.Id > 0)
            {
                string Title1 = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                string Msg1 = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                MessageORG.ShowConfirm(Title1, Msg1);
                OpenClose = false;
                MyObject = null;
                panelUnitTool1.CurrPage = 1;
                MyObjectList = new CategoryRepo().getAll(txtSearch.Text, 1);
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

            MyObjectList = new List<Category>();
            bttnSave.Enabled = false;
            AsyncDatabaseWorker.Stop = true;
            MyObject = new CategoryRepo().Save(MyObject);
            if (MyObject != null && MyObject.Id > 0)
            {
                SaveImage();
                string Title1 = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                string Msg1 = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                MessageORG.ShowConfirm(Title1, Msg1);
                MyObject = null;
                panelUnitTool1.CurrPage = 1;
                MyObjectList = new CategoryRepo().getAll(txtSearch.Text, 1);
            }
            bttnSave.Enabled = true;
        }

        public override void DeleteRow(object sender, EventArgs e)
        {
            try
            {

                string Title = GeneralMembers.Lang == "ar" ? "حذف ملف" : "Delete File";
                string Msg = GeneralMembers.Lang == "ar" ? "... هل تريد حفظ الملف" : "Are you want Delete this file ...";
                if (MessageORG.Show(Title, Msg, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel) return;
                MyObjectList = new List<Category>();
                bttnDelete.Enabled = false;
                AsyncDatabaseWorker.Stop = true;
                if (new CategoryRepo().Remove(MyObject.Id))
                {
                    string Title1 = GeneralMembers.Lang == "ar" ? "حذف" : "Delete";
                    string Msg1 = GeneralMembers.Lang == "ar" ? "تم حذف الملف بنجاح" : "The file has been deleted successfully";
                    MessageORG.ShowConfirm(Title1, Msg1);
                    OpenClose = false;
                    MyObject = null;
                    panelUnitTool1.CurrPage = 1;
                    Helper.Sound(Application.StartupPath, SoudType.DeleteClick);
                    MyObjectList = new CategoryRepo().getAll(txtSearch.Text, 1);
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
            MyObjectList = new CategoryRepo().getAll(txtSearch.Text, 1);
        }


        public override void RefrashRow(object sender, EventArgs e)
        {
            OpenClose = false;
            panelUnitTool1.CurrPage = 1;
            MyObjectList = new CategoryRepo().getAll(txtSearch.Text, 1);
        }


        public override bool Validation()
        {
            if (txtName.Text == "") return false;
            return true;
        }

        private void panelUnitTool1_pagging(int page)
        {
            MyObjectList = new CategoryRepo().getAll(txtSearch.Text, page);
        }

        public override void Search()
        {
            panelUnitTool1.CurrPage = 1;
            MyObjectList = new CategoryRepo().getAll(txtSearch.Text, 1);
            txtSearch.Focus();
        }

        public override void TemplateRow(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(Application.StartupPath + @"/Template/CategoryTemplate.csv");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save Template File";
            saveFileDialog1.FileName = "Category Template";
            saveFileDialog1.Filter = "Excel files csv (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                xlWorkbook.SaveCopyAs(saveFileDialog1.FileName);
            }
            xlWorkbook.Close();
            xlApp.Quit();
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
                    if ("" + xlWorkSheet.Cells[iRow, 1].value == "" && "" + xlWorkSheet.Cells[iRow, 2].value == "")
                        break;
                    Category cat = new CategoryRepo().getByName(xlWorkSheet.Cells[iRow, 2].value + "");
                    if (cat == null || cat.Id == 0)
                        new CategoryRepo().Save(new Category() { Code = xlWorkSheet.Cells[iRow, 1].value + "", Name = xlWorkSheet.Cells[iRow, 2].value + "" });
                }

                xlWorkBook.Close();
                xlApp.Quit();
            }
            RefrashRow(null, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            panelUnitTool1.CurrPage = 1;
            MyObjectList = new CategoryRepo().getAll(txtSearch.Text, 1);
            txtSearch.Focus();
        }

        private void SaveImage()
        {
            //if(picImage.MyImage != null)
            //{
            //    if (File.Exists("http://organizersys.com/images/9a107fe8-79be-40f0-a3e9-4576d94e1202.jpg")) //+ GeneralMembers.User.SchemaName + "__" + _MyObject.Id + "." + picImage.MyImage.RawFormat))
            //    {
            //        File.Delete("http://organizersys.com/images/9a107fe8-79be-40f0-a3e9-4576d94e1202.jpg");
            //    }
                
            //}
        }
    }
}