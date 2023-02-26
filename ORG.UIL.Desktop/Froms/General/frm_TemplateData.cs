using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ORG.Tools.Desktop.Tools;

namespace ORG.UIL.Desktop
{
    public partial class frm_TemplateData : Form
    {
       public bool _UsedNew = true;
        public bool UsedNew
        {
            get { return _UsedNew; }
            set { bttnNew.Visible =  _UsedNew = value; }
        }
        bool _UsedRefrash = true;
        public bool UsedRefrash
        {
            get { return _UsedRefrash; }
            set { bttnRefrach.Visible =  _UsedRefrash = value; }
        }

        bool _UsedNewSave = true;
        public bool UsedNewSave
        {
            get { return _UsedNewSave; }
            set { bttnSaveNew.Visible =  _UsedNewSave = value; }
        }

        bool _UsedSave = true;
        public bool UsedSave
        {
            get { return _UsedSave; }
            set { bttnSave.Visible =  _UsedSave = value; }
        }

        bool _UsedDelete = true;
        public bool UsedDelete
        {
            get { return _UsedDelete; }
            set { bttnDelete.Visible =  _UsedDelete = value; }
        }

        bool _UsedCancel = true;
        public bool UsedCancel
        {
            get { return _UsedCancel; }
            set { bttnCancel.Visible =  _UsedCancel = value; }
        }


        bool _UsedImportFile = false;
        public bool UsedImportFile
        {
            get { return _UsedImportFile; }
            set { bottunOrg4.Visible = bottunOrg5.Visible = _UsedImportFile = value; }
        }

        bool _UsedSearch = true;
        public bool UsedSearch
        {
            get { return _UsedSearch; }
            set { _UsedSearch = value; }
        }

       public long _ScreenID = 0;
        public long ScreenID
        {
            get { return _ScreenID; }
            set
            {
                _ScreenID = value;
            }
        }
      
        int _Paging = 0;
        public int Paging
        {
            get
            {
                return _Paging;
            }

            set
            {
                _Paging = value;
            }
        }

        Control _MainControl = null;
        public Control MainControl
        {
            get { return _MainControl; }
            set { _MainControl = value; }
        }

        #region Defind Events
        public event EventHandler LoadData;

        public event EventHandler _SaveFile;

        public event EventHandler _DeleteFile;

        public event EventHandler ClearControl;

        public event EventHandler SelectItem;

        public event EventHandler SelectGroup;
        #endregion

        #region property
        #region Object
        List<object> _MyGroupObjectList;
        public List<object> MyGroupObjectList
        {
            get
            {
                return _MyGroupObjectList;
            }
            set
            {
                _MyGroupObjectList = value;
            }
        }

        object _MyGroupObject;
        public object MyGroupObject
        {
            get
            {
                return _MyGroupObject;
            }
            set
            {
                _MyGroupObject = value;
            }
        }

        List<object> _MyObjectList;
        public List<object> MyObjectList
        {
            get
            {
                return _MyObjectList;
            }
            set
            {
                _MyObjectList = value;
            }
        }

        object _MyObject;
        public object MyObject
        {
            get
            {
                return _MyObject;
            }
            set
            {
                _MyObject = value;
            }
        }
        #endregion

        #region Other
        long _CompanyID = 0;
        public long CompanyID
        {
            get
            {
                return _CompanyID;
            }
            set
            {
                _CompanyID = value;
            }
        }

        long _BranchID = 0;
        public long BranchID
        {
            get
            {
                return _BranchID;
            }
            set
            {
                _BranchID = value;
            }
        }

        long _currId = 0;
        public long currId
        {
            get
            {
                return _currId;
            }
            set
            {
                _currId = value;
            }
        }

        long _GroupID = 0;
        public long GroupID
        {
            get { return _GroupID; }
            set { _GroupID = value; }
        }

        bool _ByImage = false;
        public bool ByImage
        {
            get { return _ByImage; }
            set
            {
                _ByImage = value;
                picImage.Visible = _ByImage;
            }
        }

        bool _ByGroup = true;
        public bool ByGroup
        {
            get { return _ByGroup; }
            set
            {
                _ByGroup = value;
                lblmyGroup.Visible = txtmyGroup.Visible = pnlLoadGroup.Visible = _ByGroup;

            }
        }

        bool _OpenClose = false;
        public bool OpenClose
        {
            get { return _OpenClose; }
            set
            {
                _OpenClose = value;
                if (_OpenClose)
                {
                    pnlDisplay.Visible = false;
                    pnlMainData.Visible = true;
                    pnlLoadGroup.Visible = _ByGroup;
                    pnlControl1.Visible = false;
                    pnlControl2.Visible = true;
                }
                else
                {

                    pnlDisplay.Visible = true;
                    pnlMainData.Visible = false;
                    pnlLoadGroup.Visible = _ByGroup;
                    pnlControl1.Visible = true;
                    pnlControl2.Visible = false;
                }
            }
        }
           
        Image _NullGroupImage = null;
        public Image NullGroupImage
        {
            get { return _NullGroupImage; }
            set { _NullGroupImage = value; }
        }

        Image _NullUnitImage = null;
        public Image NullUnitImage
        {
            get { return _NullUnitImage; }
            set { _NullUnitImage = value; }
        }

        //Image _MyImage = null;
        //public Image MyImage
        //{
        //    get
        //    {
        //        _MyImage = picImage.MyImage;
        //        return _MyImage;

        //    }
        //    set
        //    {
        //        _MyImage = value;
        //        picImage.MyImage = _MyImage;
        //    }
        //}
        #endregion
        #endregion

        #region Methods
        public void SelectGroupAfterEvent(long ID)
        {
            for (int i = 0; i < panelGroupTool1.Controls.Count; i++)
            {
                if (((GroupUnitTool)panelGroupTool1.Controls[i]).ID == ID)
                {
                    panelGroupTool1.selectGroup(((GroupUnitTool)panelGroupTool1.Controls[i]), ((GroupUnitTool)panelGroupTool1.Controls[i]).MyObject, i);
                    break;
                }
            }
        }
        #endregion

        #region EventsButton
        public virtual void NewRow(object sender, EventArgs e)
        {
            bttnNew.Enabled = false;
            long ID = _GroupID;
            if (ClearControl != null)
                ClearControl(sender, e);
            OpenClose = true;
            if (ByGroup)
                if (ID == 0 && txtmyGroup.DataSource != null)
                    txtmyGroup.SelectedValue = 0;
                else
                    txtmyGroup.SelectedValue = ID;
            bttnNew.Enabled = true;
            bttnSaveNew.Visible =  bttnSave.Visible =  true;
            if (_MainControl != null)
                _MainControl.Focus();
        }

        public virtual void SaveNewRow(object sender, EventArgs e)
        {
            string Title = GeneralMembers.Lang == "ar" ? "خطأ فى الحفظ" : "Errore Save";
            string Msg = GeneralMembers.Lang == "ar" ? "لا يمكن حفط هذا الملف" : "Can't Save This File ...";
            if (_SaveFile != null)
            {
                if (!Validation())
                {
                    MessageORG.Show(Title, Msg, CountButton.One);
                    return;
                }
                bttnSaveNew.Enabled = false;
                _SaveFile(sender, e);
                NewRow(sender, e);
                bttnSaveNew.Enabled = true;
            }
        }

        public virtual void SaveRow(object sender, EventArgs e)
        {
            string Title = GeneralMembers.Lang == "ar" ? "خطأ فى الحفظ" : "Errore Save";
            string Msg = GeneralMembers.Lang == "ar" ? "لا يمكن حفط هذا الملف" : "Can't Save This File ...";
            if (!Validation())
            {
                MessageORG.Show(Title, Msg, CountButton.One);
                return;
            }
            bttnSave.Enabled = false;
            long ID = _GroupID;
            if (_SaveFile != null)
                _SaveFile(sender, e);
            if (ByGroup)
                SelectGroupAfterEvent(ID);
            if (ClearControl != null)
                ClearControl(sender, e);

            bttnSave.Enabled = true;
            OpenClose = false;        
        }

        public virtual void PrintRow(object sender, EventArgs e)
        {

        }

        public virtual void SavePrintRow(object sender, EventArgs e)
        {

        }

        public virtual void NewSavePrintRow(object sender, EventArgs e)
        {

        }

        public virtual void DeleteRow(object sender, EventArgs e)
        {
            string Title = GeneralMembers.Lang == "ar" ? "حذف ملف" : "Delete File";
            string Msg = GeneralMembers.Lang == "ar" ? "... هل تريد حفظ الملف" : "Are you want Delete this file ...";
            if (MessageORG.Show(Title, Msg) == System.Windows.Forms.DialogResult.Cancel) return;
            bttnDelete.Enabled = false;
            long ID = _GroupID;
            if (_DeleteFile != null)
                _DeleteFile(sender, e);
            if (ByGroup)
                SelectGroupAfterEvent(ID);
            if (ClearControl != null)
                ClearControl(sender, e);

            OpenClose = false;
            bttnDelete.Enabled = true;
        }

        public virtual void FirstRow(object sender, EventArgs e)
        {

        }

        public virtual void PreviousRow(object sender, EventArgs e)
        {

        }

        public virtual void NextRow(object sender, EventArgs e)
        {

        }

        public virtual void LastRow(object sender, EventArgs e)
        {

        }

        public virtual void RefrashRow(object sender, EventArgs e)
        {
            bttnRefrach.Enabled = false;
            if (LoadData != null)
                LoadData(null, null);
            bttnRefrach.Enabled = true;
        }

        public virtual void CancelRow(object sender, EventArgs e)
        {
            bttnCancel.Enabled = false;
            OpenClose = false;
            long ID = _GroupID;
            //if (ByGroup)
            //    SelectGroupAfterEvent(ID);
            bttnCancel.Enabled = true;
        }

        public virtual void LoadForm(object sender, EventArgs e)
        {
            pnlDisplay.Dock = DockStyle.Fill;
            pnlControl1.Dock = DockStyle.Fill;
            OpenClose = false;
            if (LoadData != null)
                LoadData(sender, e);           
            txtSearch.Focus();
        }

        private void SearchRow(object sender, EventArgs e)
        {
            Search();
        }

        public virtual bool Validation()
        {
            return true;
        }
        #endregion

        #region EventControls
        public frm_TemplateData()
        {
            InitializeComponent();
        }

        public virtual void panelGroupTool1_SelectITem(object sender, EventArgs e)
        {
            SelectGroup(sender, e);
        }

        public virtual void panelUnitTool1_SelectITem(object sender, EventArgs e)
        {
            SelectItem(sender, e);
            OpenClose = true;           
        }

        private void lblKeyInfo_Click(object sender, EventArgs e)
        {
            if (lblKeyInfo.Text == "<")
            {
                lblKeyInfo.Text = ">";
                pnlInfo.Width = 100;
            }
            else
            {
                lblKeyInfo.Text = "<";
                pnlInfo.Width = 15;
            }
        }

        private void txtmyGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!txtmyGroup.Focused) return;
                _GroupID = long.Parse("0" + txtmyGroup.SelectedValue);
                if (ByGroup)
                    SelectGroupAfterEvent(_GroupID);
            }
            catch
            {
            }
        }
        #endregion


      
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            switch (keyData)
            {
                case Keys.W | Keys.Control:
                case Keys.F9:
                    if (_OpenClose)
                        SaveRow(null, null);
                    break;
                case Keys.Control | Keys.S:
                case Keys.F10:
                    if (_OpenClose)
                        SaveNewRow(null, null);
                    break;
                case Keys.Control | Keys.O:
                case Keys.F7:
                    if (!_OpenClose)
                        NewRow(null, null);
                    break;
                case Keys.Control | Keys.D:
                case Keys.Delete:
                    if (_OpenClose)
                        DeleteRow(null, null);
                    break;
                case Keys.Control | Keys.P:
                case Keys.F2:
                    if (_OpenClose)
                        PrintRow(null, null);
                    break;
                case Keys.F5:
                    if (!_OpenClose)
                        RefrashRow(null, null);
                    break;
                case Keys.Escape:
                    if (_OpenClose)
                        CancelRow(null, null);
                    break;
                case Keys.Enter:
                    SendKeys.Send("{Tab}");
                    break;
                default:
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
       
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                Search();
        }

        public virtual void Search()
        {

        }

        public virtual void TemplateRow(object sender, EventArgs e)
        {

        }

        public virtual void ImportRow(object sender, EventArgs e)
        {

        }
    }
}