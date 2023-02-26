using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORG.UIL.Desktop
{
    public partial class frm_TemplateTransation : Form
    {
        #region Varible
        public bool AddNew = true, falg = false;
        #endregion

        #region Porperties            

        #region Controls
        long _ScreenID = 0;
        public long ScreenID
        {
            get { return _ScreenID; }
            set
            {
                _ScreenID = value;

            }
        }

        bool _UseNew = true;
        public bool UseNew
        {
            get { return _UseNew; }
            set { _UseNew = value; }
        }

        bool _UseSave = true;
        public bool UseSave
        {
            get { return _UseSave; }
            set
            {
                _UseSave = value;
                bttnSave.Visible = _UseSave;
            }
        }

        bool _UseNewSave = true;
        public bool UseNewSave
        {
            get { return _UseNewSave; }
            set
            {
                _UseNewSave = value;
                bttnSaveNew.Visible = _UseNewSave;
            }
        }

        bool _UseSavePrint = true;
        public bool UseSavePrint
        {
            get { return _UseSavePrint; }
            set
            {
                _UseSavePrint = value;
                 bttnSavePrint.Visible = _UseSavePrint;
            }
        }

        bool _UsePrint = true;
        public bool UsePrint
        {
            get { return _UsePrint; }
            set
            {
                _UsePrint = value;
                 bttnPrint.Visible = _UsePrint;
            }
        }

        bool _UseNewSavePrint = true;
        public bool UseNewSavePrint
        {
            get { return _UseNewSavePrint; }
            set
            {
                _UseNewSavePrint = value;
                bttnNewSavePrint.Visible = _UseNewSavePrint;
            }
        }

        bool _UseDelete = true;
        public bool UseDelete
        {
            get { return _UseDelete; }
            set
            {
                _UseDelete = value;
                bttnDelete.Visible =  _UseDelete;
            }
        }

      
        bool _UseCancel = true;
        public bool UseCancel
        {
            get { return _UseCancel; }
            set
            {
                _UseCancel = value;
                bttnCancel.Visible = _UseCancel;
            }
        }
        #endregion

        bool _OpenClose = true;
        public bool OpenClose
        {
            get { return _OpenClose; }
            set
            {              
                _OpenClose = value;
                falg = true;
                if (_OpenClose)
                {                    
                    bttnNew.Visible = false;
                    toolSearchInfo1.Visible = false;
                    pnlControls.Visible = true;
                    toolCashier1.Visible = true;
                }
                else
                {                   
                    bttnNew.Visible = true;
                    toolSearchInfo1.Visible = true;
                    pnlControls.Visible = false;
                    toolCashier1.Visible = false;
                }
                falg = false;
            }
        }
        #endregion

        #region EventControls
        public frm_TemplateTransation()
        {
            InitializeComponent();
            AddNew = true;
        }

        public virtual void NewFile(object sender, EventArgs e)
        {
            AddNew = OpenClose = true;         
        }

        public virtual void NewSaveFile(object sender, EventArgs e)
        {
           
                AddNew = true;
        }

        public virtual void SaveFile(object sender, EventArgs e)
        {
           
                AddNew = OpenClose = false;
        }

        public virtual void PrintFile(object sender, EventArgs e)
        {

        }

        public virtual void PreviewFile(object sender, EventArgs e)
        {

        }

        public virtual void SavePrintFile(object sender, EventArgs e)
        {
            AddNew = OpenClose = false;
        }

        public virtual void NewSavePrintFile(object sender, EventArgs e)
        {
            AddNew = true;
        }

        public virtual void DeleteFile(object sender, EventArgs e)
        {
            string Title = GeneralMembers.Lang == "ar" ? "حذف ملف" : "Delete File";
            string Msg = GeneralMembers.Lang == "ar" ? "... هل تريد حذف الملف" : "Are you want Delete this file ...";
            if (MessageORG.Show(Title, Msg) == System.Windows.Forms.DialogResult.Cancel) return;
            AddNew = OpenClose = false;
        }

        public virtual void CancelFile(object sender, EventArgs e)
        {
            AddNew = OpenClose = false;           
        }
      
        public virtual object InvioceClick(object _object)
        {
            OpenClose = true;
            AddNew = false;
             bttnPrint.Visible = true;           
            return default(object);
        }

        public virtual void TypeCheckedChanged(object sender, EventArgs e)
        {
            if (!((Control)sender).Focused) return;
            AddNew = OpenClose = false;
        }

        public virtual object toolSelectPageing(object _object)
        {
            return default(object);
        }

        public virtual void toolCashier1__Refrash()
        {
           
        }
        private void LoadForm(object sender, EventArgs e)
        {

        }

        private void bttnNew_MouseEnter(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = Color.White;
        }

       
     
        #endregion
    }
}
