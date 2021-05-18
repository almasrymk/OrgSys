using OrgSysApp.Forms.Data.Unit;
using OrgSysApp.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OrgSysApp.Forms.Data
{
    public partial class MainData : Form
    {
        frmUnit frmUnit;
        public MainData()
        {
            InitializeComponent();
            frmUnit = new frmUnit();
        }

        private void btn_click(object sender, EventArgs e)
        {
            foreach (var Control in pnlMainDataMenu.Controls)
            {
                if (Control == sender)
                {                    
                    ((ButtonImage)Control).active = true;
                }
                else
                    ((ButtonImage)Control).active = false;
            }

            if (Application.OpenForms["frmUnit1"] != null)
            {
                pnlDataPages.Controls.Remove(pnlDataPages.Controls["frmUnit1"]);
                Application.OpenForms["frmUnit1"].Close();
            }

            switch (((ButtonImage)sender).ButtonName)
            {
                case "Unit":
                    if (pnlDataPages.Controls["frmUnit1"] == null)
                    {
                        frmUnit = new frmUnit();
                        frmUnit.Name = "frmUnit1";
                        frmUnit.TopLevel = false;
                        frmUnit.Height = 0;
                        frmUnit.Size = pnlDataPages.Size;
                        frmUnit.Dock = DockStyle.Fill;
                        pnlDataPages.Controls.Add(frmUnit);
                        frmUnit.Show();
                    }
                    break;
                case "Invoice":
                    break;
                case "Return":
                    break;
                case "Report":
                    break;
                default:
                    break;
            }
        }
    }
}
