using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ORG.UIL.Desktop.Untility;
using ORGEntity;
using ORGRepository;

namespace ORG.UIL.Desktop
{
    public partial class frm_Unit : frm_TemplateData
    {
        List<Unit> _MyObjectList;
        public List<Unit> MyObjectList
        {
            get
            {
                return _MyObjectList;
            }
            set
            {
                _MyObjectList = value;
                panelUnitTool1.FillItems<Unit>(_MyObjectList.ToList());
            }
        }

        Unit _MyObject;
        public new Unit MyObject
        {
            get
            {
                if (_MyObject == null)
                    _MyObject = new Unit();
                _MyObject.Name = txtName.Text;
                return _MyObject;
            }
            set
            {
                _MyObject = value;
                if (_MyObject == null)
                    _MyObject = new Unit();
                txtName.Text = _MyObject.Name;
                UsedDelete = _MyObject.Id > 0;
            }
        }

        public frm_Unit()
        {
            InitializeComponent();
            OpenClose = false;         
        }

        public override void panelUnitTool1_SelectITem(object sender, EventArgs e)
        {
            var ob = (int)sender;
            MyObject = new UnitRepo().getById(ob);
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

            MyObjectList = new List<Unit>();
            bttnSave.Enabled = false;
            AsyncDatabaseWorker.Stop = true;
            MyObject = new UnitRepo().Save(MyObject);
            if (MyObject != null && MyObject.Id > 0)
            {
                string Title1 = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                string Msg1 = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                MessageORG.ShowConfirm(Title1, Msg1);
                OpenClose = false;
                MyObject = null;
                panelUnitTool1.CurrPage = 1;
                MyObjectList = new UnitRepo().getAll(txtSearch.Text,1);
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

            MyObjectList = new List<Unit>();
            bttnSaveNew.Enabled = false;
            AsyncDatabaseWorker.Stop = true;
            MyObject = new UnitRepo().Save(MyObject);
            if (MyObject != null && MyObject.Id > 0)
            {
                string Title1 = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                string Msg1 = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                MessageORG.ShowConfirm(Title1, Msg1);
                MyObject = null;
                panelUnitTool1.CurrPage = 1;
                MyObjectList = new UnitRepo().getAll(txtSearch.Text,1);
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
                MyObjectList = new List<Unit>();
                bttnDelete.Enabled = false;
                AsyncDatabaseWorker.Stop = true;
                if (new UnitRepo().Remove(MyObject.Id))
                {
                    string Title1 = GeneralMembers.Lang == "ar" ? "حذف" : "Delete";
                    string Msg1 = GeneralMembers.Lang == "ar" ? "تم حذف الملف بنجاح" : "The file has been deleted successfully";
                    MessageORG.ShowConfirm(Title1, Msg1);
                    OpenClose = false;
                    MyObject = null;
                    panelUnitTool1.CurrPage = 1;
                    Helper.Sound(Application.StartupPath, SoudType.DeleteClick);
                    MyObjectList = new UnitRepo().getAll(txtSearch.Text,1);
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
            MyObjectList = new UnitRepo().getAll(txtSearch.Text,1);
        }

        public override void RefrashRow(object sender, EventArgs e)
        {
            OpenClose = false;
            panelUnitTool1.CurrPage = 1;
            MyObjectList = new UnitRepo().getAll(txtSearch.Text,1);
        }

        public override bool Validation()
        {
            if (txtName.Text == "") return false;
            return true;
        }

        private void panelUnitTool1_pagging(int page)
        {
            MyObjectList = new UnitRepo().getAll(txtSearch.Text , page);
        }

        public override void Search()
        {
            panelUnitTool1.CurrPage = 1;
            MyObjectList = new UnitRepo().getAll(txtSearch.Text, 1);
            txtSearch.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            panelUnitTool1.CurrPage = 1;
            MyObjectList = new UnitRepo().getAll(txtSearch.Text, 1);
            txtSearch.Focus();
        }
    }
}