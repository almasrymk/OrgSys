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
        public MainData()
        {
            InitializeComponent();
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

            //if (Application.OpenForms["MainData1"] != null)
            //{
            //    pnlBody.Controls.Remove(pnlBody.Controls["MainData1"]);
            //    Application.OpenForms["MainData1"].Close();
            //}

            //switch (((MainMenu)sender).ButtonSelectedName)
            //{
            //    case "Data":
            //        if (pnlBody.Controls["MainData1"] == null)
            //        {
            //            mainData = new MainData();
            //            mainData.Name = "MainData1";
            //            mainData.TopLevel = false;
            //            mainData.Height = 0;
            //            mainData.Size = pnlBody.Size;
            //            mainData.Dock = DockStyle.Fill;
            //            pnlBody.Controls.Add(mainData);
            //            mainData.Show();
            //        }
            //        break;
            //    case "Invoice":
            //        break;
            //    case "Return":
            //        break;
            //    case "Report":
            //        break;
            //    default:
            //        break;
            //}
        }
    }
}
