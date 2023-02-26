using ORG.UIL.Desktop.Froms.Data;
using ORGEntity;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ORG.UIL.Desktop
{
    public partial class frm_PanelData : Form
    {     
        string scearnOpenedItem = "";
        frm_Unit _frm_Unit;
        frm_ItemGroup _frm_ItemGroup;
        frm_Item _frm_Item;
        frm_Users _frm_Users;
        frmSetting _frm_Setting;

        public frm_PanelData()
        {
            Application.DoEvents();
            InitializeComponent();
            panel2.Visible = GeneralMembers.User.RoleId < 2;
        }
      
        private void SelectMenuItem(string Text, bool Open)
        {
            if (Open)
                scearnOpenedItem = Text;
            else
                scearnOpenedItem = "";

            if (!Open)
            {                
                if (Application.OpenForms["frm_Unit1"] != null)
                {
                    ((frm_TemplateData)Application.OpenForms["frm_Unit1"]).panelGroupTool1.Controls.Clear();
                    ((frm_TemplateData)Application.OpenForms["frm_Unit1"]).panelUnitTool1.pnlMain.Controls.Clear();
                    Application.OpenForms["frm_Unit1"].Close();
                }
                if (Application.OpenForms["frm_ItemGroup1"] != null)
                {
                    ((frm_TemplateData)Application.OpenForms["frm_ItemGroup1"]).panelGroupTool1.Controls.Clear();
                    ((frm_TemplateData)Application.OpenForms["frm_ItemGroup1"]).panelUnitTool1.pnlMain.Controls.Clear();
                    Application.OpenForms["frm_ItemGroup1"].Close();
                }
                if (Application.OpenForms["frm_Item1"] != null)
                {
                    ((frm_TemplateData)Application.OpenForms["frm_Item1"]).panelGroupTool1.Controls.Clear();
                    ((frm_TemplateData)Application.OpenForms["frm_Item1"]).panelUnitTool1.pnlMain.Controls.Clear();
                    Application.OpenForms["frm_Item1"].Close();
                }
                if (Application.OpenForms["frm_Users1"] != null)
                {
                    ((frm_TemplateData)Application.OpenForms["frm_Users1"]).panelGroupTool1.Controls.Clear();
                    ((frm_TemplateData)Application.OpenForms["frm_Users1"]).panelUnitTool1.pnlMain.Controls.Clear();
                    Application.OpenForms["frm_Users1"].Close();
                }
                if (Application.OpenForms["frm_Setting1"] != null)
                {                   
                    Application.OpenForms["frm_Setting1"].Close();
                }

                pnlItemBody.Controls.Clear();
            }

            switch (Text.ToLower())
            {
             
               
                case "unit":
                    if (pnlItemBody.Controls["frm_Unit1"] == null)
                    {
                        _frm_Unit = new frm_Unit();
                        _frm_Unit.Name = "frm_Unit1";
                        _frm_Unit.TopLevel = false;
                        _frm_Unit.Height = 0;
                        _frm_Unit.Size = pnlItemBody.Size;
                        _frm_Unit.Dock = DockStyle.Fill;
                        _frm_Unit.ScreenID = 52;
                        pnlItemBody.Controls.Add(_frm_Unit);
                        if (Open)
                        {                           
                            _frm_Unit.panelUnitTool1.NullImage = btnUnit.ImageTitleEnter;
                            _frm_Unit.Show();
                        }
                    }
                    if (!Open)
                    {
                        _frm_Unit.panelUnitTool1.FillItems<Unit>(null);
                        _frm_Unit.Close();
                        pnlItemBody.Controls.Remove(pnlItemBody.Controls["frm_Unit1"]);
                    }
                    btnUnit.Selected = _frm_Unit.Visible = Open;
                    break;
                case "itemgroup":
                    if (pnlItemBody.Controls["frm_ItemGroup1"] == null)
                    {
                        _frm_ItemGroup = new frm_ItemGroup();
                        _frm_ItemGroup.Name = "frm_ItemGroup1";
                        _frm_ItemGroup.TopLevel = false;
                        _frm_ItemGroup.Size = pnlItemBody.Size;
                        _frm_ItemGroup.Dock = DockStyle.Fill;
                        _frm_ItemGroup.ScreenID = 55;
                        pnlItemBody.Controls.Add(_frm_ItemGroup);
                        if (Open)
                        {
                            _frm_ItemGroup.panelUnitTool1.NullImage = btnItemGroup.ImageTitleEnter;
                            _frm_ItemGroup.Show();
                        }
                    }
                    if (!Open)
                    {
                        _frm_ItemGroup.panelUnitTool1.FillItems<Category>(null);
                        _frm_ItemGroup.Close();
                        pnlItemBody.Controls.Remove(pnlItemBody.Controls["frm_ItemGroup1"]);
                    }
                    btnItemGroup.Selected = _frm_ItemGroup.Visible = Open;
                    break;
                case "item":
                    if (pnlItemBody.Controls["frm_Item1"] == null)
                    {
                        _frm_Item = new frm_Item();
                        _frm_Item.Name = "frm_Item1";
                        _frm_Item.TopLevel = false;
                        _frm_Item.Size = pnlItemBody.Size;
                        _frm_Item.Dock = DockStyle.Fill;
                        _frm_Item.ScreenID = 56;
                        pnlItemBody.Controls.Add(_frm_Item);
                        if (Open)
                        {
                            _frm_Item.panelGroupTool1.NullImage = btnItemGroup.ImageTitleEnter;
                            _frm_Item.panelUnitTool1.NullImage = btnItem.ImageTitleEnter;
                            _frm_Item.Show();
                        }
                    }
                    if (!Open)
                    {
                        _frm_Item.panelUnitTool1.FillItems<Item>(null);
                        _frm_Item.Close();
                        pnlItemBody.Controls.Remove(pnlItemBody.Controls["frm_Item1"]);
                    }
                    btnItem.Selected = _frm_Item.Visible = Open;
                    break;
                case "user":
                    if (pnlItemBody.Controls["frm_Users1"] == null)
                    {
                        _frm_Users = new frm_Users();
                        _frm_Users.Name = "frm_Users1";
                        _frm_Users.TopLevel = false;
                        _frm_Users.Size = pnlItemBody.Size;
                        _frm_Users.Dock = DockStyle.Fill;
                        _frm_Users.ScreenID = 56;
                        pnlItemBody.Controls.Add(_frm_Users);
                        if (Open)
                        {
                            _frm_Users.panelGroupTool1.NullImage = menuItemTool1.ImageTitleEnter;
                            _frm_Users.panelUnitTool1.NullImage = menuItemTool1.ImageTitleEnter;
                            _frm_Users.Show();
                        }
                    }
                    if (!Open)
                    {
                        _frm_Users.panelUnitTool1.FillItems<UsersApp>(null);
                        _frm_Users.Close();
                        pnlItemBody.Controls.Remove(pnlItemBody.Controls["frm_Users1"]);
                    }
                    menuItemTool1.Selected = _frm_Users.Visible = Open;
                    break;
                case "setting":
                    if (pnlItemBody.Controls["frm_Setting1"] == null)
                    {
                        _frm_Setting = new  frmSetting();
                        _frm_Setting.Name = "frm_Setting1";
                        _frm_Setting.TopLevel = false;
                        _frm_Setting.Size = pnlItemBody.Size;
                        _frm_Setting.Dock = DockStyle.Fill;                        
                        pnlItemBody.Controls.Add(_frm_Setting);
                        if (Open)
                        {                            
                            _frm_Setting.Show();
                        }
                    }
                    if (!Open)
                    {
                        _frm_Setting.Close();
                        pnlItemBody.Controls.Remove(pnlItemBody.Controls["frm_Setting1"]);
                    }
                    btnSetting.Selected = _frm_Setting.Visible = Open;
                    break;
                case "close":
                    break;
                default:
                    break;
            }
        }

        private void MenuItem_SelectFromMenu(object sender, EventArgs e)
        {
            SelectMenuItem(scearnOpenedItem, false);
            SelectMenuItem(((Control)sender).Tag.ToString().ToLower(), true);
        }
    }
}