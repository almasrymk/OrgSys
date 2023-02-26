using ORG.UIL.Desktop.Froms.Data;
using ORG.UIL.Desktop.Froms.Report;
using ORGEntity;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ORG.UIL.Desktop
{
    public partial class frm_Report : Form
    {
        string ScreenOpened = string.Empty;
        string scearnOpenedsalesDetail = "salesDetail";
        
        frm_SalesDetail _frm_SalesDetail;
        frm_ReturnDetail _frm_ReturnDetail;
        frm_DailySales _frm_DailySales;
        frm_TotalItemSales _frm_TotalItemSales;
        public frm_Report()
        {
            InitializeComponent();
            ScreenOpened = "salesDetail";
            OpenClose(0, 3, 0, 0, 0);
        }

        private void EnterControl(object sender, EventArgs e)
        {
            OpenCloseScreen(((Control)sender).Tag.ToString(), true, false, false);
        }

        private void LeaveControl(object sender, EventArgs e)
        {
            OpenCloseScreen(((Control)sender).Tag.ToString(), false, false, false);
        }

        private void ClickControl(object sender, EventArgs e)
        {
            Close_btn();
            OpenCloseScreen(((Control)sender).Tag.ToString(), false, true, true);
        }

        private void frm_PanelData_Load(object sender, EventArgs e)
        {           

        }

        public void OpenCloseScreen(string ScreenName, bool MouseEnter, bool Opened, bool Clikced)
        {
            switch (ScreenName)
            {
                case "salesDetail":
                    if (Opened)
                    {
                        if (ScreenOpened == "salesDetail")
                        {
                            ScreenOpened = "";
                            OpenClose(2, 0, 0, 0, 0);
                        }
                        else
                        {
                            ScreenOpened = "salesDetail";
                            OpenClose(3, 0, 0, 0, 0);
                        }
                    }
                    else if (ScreenOpened != "salesDetail")
                        OpenClose((MouseEnter ? 1 : 0), 0, 0, 0, 0);
                    break;
                case "returnDetail":
                    if (Opened)
                    {
                        if (ScreenOpened == "returnDetail")
                        {
                            ScreenOpened = "";
                            OpenClose(2, 0, 0, 0, 0);
                        }
                        else
                        {
                            ScreenOpened = "returnDetail";
                            OpenClose(3, 0, 0, 0, 0);
                        }
                    }
                    else if (ScreenOpened != "returnDetail")
                        OpenClose((MouseEnter ? 1 : 0), 0, 0, 0, 0);
                    break;
                case "totalItemSales":
                    if (Opened)
                    {
                        if (ScreenOpened == "totalItemSales")
                        {
                            ScreenOpened = "";
                            OpenClose(2, 0, 0, 0, 0);
                        }
                        else
                        {
                            ScreenOpened = "totalItemSales";
                            OpenClose(3, 0, 0, 0, 0);
                        }
                    }
                    else if (ScreenOpened != "totalItemSales")
                        OpenClose((MouseEnter ? 1 : 0), 0, 0, 0, 0);
                    break;
                case "dailySales":
                    if (Opened)
                    {
                        if (ScreenOpened == "dailySales")
                        {
                            ScreenOpened = "";
                            OpenClose(2, 0, 0, 0, 0);
                        }
                        else
                        {
                            ScreenOpened = "dailySales";
                            OpenClose(3, 0, 0, 0, 0);
                        }
                    }
                    else if (ScreenOpened != "dailySales")
                        OpenClose((MouseEnter ? 1 : 0), 0, 0, 0, 0);
                    break;                    
            }
        }

        private void OpenClose(int _Company, int _Item, int _Dealer, int _Employee, int _Developer)
        {
            #region Item
            //if (_Item == 0)
            //{
            //    if (ScreenOpened != "salesDetail")
            //    {
            //        lblItem.Font = new Font(lblItem.Font.FontFamily, 10, FontStyle.Bold);
            //        lblItem.ForeColor = Color.Silver;
            //        picKeyItem.Image = ORG.UIL.Desktop.Properties.Resources._05df;
            //        pnlItem.Height = 100;

            //        if (Application.OpenForms["frm_salesDetail1"] != null)                    
            //            Application.OpenForms["frm_salesDetail1"].Close();

            //        pnlItemBody.Controls.Clear();
            //    }

            //    if (ScreenOpened != "totalItemSales")
            //    {
            //        lblItem.Font = new Font(lblItem.Font.FontFamily, 10, FontStyle.Bold);
            //        lblItem.ForeColor = Color.Silver;
            //        picKeyItem.Image = ORG.UIL.Desktop.Properties.Resources._05df;
            //        pnlItem.Height = 100;

            //        if (Application.OpenForms["frm_totalItemSales1"] != null)
            //            Application.OpenForms["frm_totalItemSales1"].Close();

            //        pnlItemBody.Controls.Clear();
            //    }

            //    if (ScreenOpened != "dailySales")
            //    {
            //        lblItem.Font = new Font(lblItem.Font.FontFamily, 10, FontStyle.Bold);
            //        lblItem.ForeColor = Color.Silver;
            //        picKeyItem.Image = ORG.UIL.Desktop.Properties.Resources._05df;
            //        pnlItem.Height = 100;

            //        if (Application.OpenForms["frm_dailySales1"] != null)
            //            Application.OpenForms["frm_dailySales1"].Close();

            //        pnlItemBody.Controls.Clear();
            //    }                
            //}
            //else
            //{
            //    lblItem.Font = new Font(lblItem.Font.FontFamily, 12, FontStyle.Bold);
            //    lblItem.ForeColor = Color.FromArgb(100, 150, 250);

            //    if (_Item < 3)
            //    {
            //        picKeyItem.Image = ORG.UIL.Desktop.Properties.Resources._051;
            //        if (_Item == 2)
            //        {
            //            pnlItem.Height = 44;
            //        }
            //    }
            //    else
            //    {
            //        picKeyItem.Image = ORG.UIL.Desktop.Properties.Resources._05df;
            //        pnlItem.Height = this.Height - 205;
            //    }
            //}
            #endregion
        }

        public void SelectMenuItem(string Text, bool Open , object Id = null)
        {
            if (Open)
                scearnOpenedsalesDetail = Text;
            else
                scearnOpenedsalesDetail = "";

            if (!Open)
            {
                if (Application.OpenForms["frm_salesDetail1"] != null)                
                    Application.OpenForms["frm_salesDetail1"].Close();
                if (Application.OpenForms["frm_returnDetail1"] != null)
                    Application.OpenForms["frm_returnDetail1"].Close();
                if (Application.OpenForms["frm_totalItemSales1"] != null)
                    Application.OpenForms["frm_totalItemSales1"].Close();
                if (Application.OpenForms["frm_dailySales1"] != null)
                    Application.OpenForms["frm_dailySales1"].Close();
                
                pnlItemBody.Controls.Clear();
            }
            
            switch (Text)
            {
             
               
                case "salesDetail":
                    if (pnlItemBody.Controls["frm_salesDetail1"] == null)
                    {
                        _frm_SalesDetail = new frm_SalesDetail();
                        _frm_SalesDetail.Name = "frm_salesDetail1";
                        _frm_SalesDetail.TopLevel = false;
                        _frm_SalesDetail.Height = 0;
                        _frm_SalesDetail.Size = pnlItemBody.Size;
                        _frm_SalesDetail.Dock = DockStyle.Fill;                        
                        pnlItemBody.Controls.Add(_frm_SalesDetail);
                        if (Open)
                        {                                                       
                            _frm_SalesDetail.Show();
                        }
                    }
                    if (!Open)
                    {
                        _frm_SalesDetail.Close();
                        pnlItemBody.Controls.Remove(pnlItemBody.Controls["frm_salesDetail1"]);
                    }
                    btnUnit.Selected = _frm_SalesDetail.Visible = Open;
                    break;
                case "returnDetail":
                    if (pnlItemBody.Controls["frm_returnDetail1"] == null)
                    {
                        _frm_ReturnDetail = new frm_ReturnDetail();
                        _frm_ReturnDetail.Name = "frm_returnDetail1";
                        _frm_ReturnDetail.TopLevel = false;
                        _frm_ReturnDetail.Height = 0;
                        _frm_ReturnDetail.Size = pnlItemBody.Size;
                        _frm_ReturnDetail.Dock = DockStyle.Fill;
                        pnlItemBody.Controls.Add(_frm_ReturnDetail);
                        if (Open)
                        {
                            _frm_ReturnDetail.Show();
                        }
                    }
                    if (!Open)
                    {
                        _frm_ReturnDetail.Close();
                        pnlItemBody.Controls.Remove(pnlItemBody.Controls["frm_returnDetail1"]);
                    }
                    menuItemTool2.Selected = _frm_ReturnDetail.Visible = Open;
                    break;
                case "totalItemSales":
                    if (pnlItemBody.Controls["frm_totalItemSales1"] == null)
                    {
                        _frm_TotalItemSales = new frm_TotalItemSales();
                        _frm_TotalItemSales.Name = "frm_totalItemSales1";
                        _frm_TotalItemSales.TopLevel = false;
                        _frm_TotalItemSales.Height = 0;
                        _frm_TotalItemSales.Size = pnlItemBody.Size;
                        _frm_TotalItemSales.Dock = DockStyle.Fill;
                        pnlItemBody.Controls.Add(_frm_TotalItemSales);
                        if (Open)
                        {
                            _frm_TotalItemSales.Show();
                        }
                    }
                    if (!Open)
                    {
                        _frm_TotalItemSales.Close();
                        pnlItemBody.Controls.Remove(pnlItemBody.Controls["frm_totalItemSales1"]);
                    }
                    menuItemTool1.Selected = _frm_TotalItemSales.Visible = Open;
                    break;                    
                case "dailySales":
                    if (pnlItemBody.Controls["frm_dailySales1"] == null)
                    {
                        _frm_DailySales = new frm_DailySales();
                        _frm_DailySales.Name = "frm_dailySales1";
                        _frm_DailySales.TopLevel = false;
                        _frm_DailySales.Height = 0;
                        _frm_DailySales.Size = pnlItemBody.Size;
                        _frm_DailySales.Dock = DockStyle.Fill;
                        pnlItemBody.Controls.Add(_frm_DailySales);
                        if (Open)
                        {
                            _frm_DailySales.Show();
                        }
                    }
                    if (!Open)
                    {
                        _frm_DailySales.Close();
                        pnlItemBody.Controls.Remove(pnlItemBody.Controls["frm_dailySales1"]);
                    }
                    btnItemGroup.Selected = _frm_DailySales.Visible = Open;
                    break;
                case "close":
                    break;
                default:
                    break;
            }
        }

        public void MenuItem_SelectFromMenu(object sender, EventArgs e)
        {
            SelectMenuItem(scearnOpenedsalesDetail, false);
            SelectMenuItem(((Control)sender).Tag.ToString(), true);
        }

        private void Close_btn()
        {
           btnUnit.Selected = btnItemGroup.Selected  =  false;
        }
    }
}