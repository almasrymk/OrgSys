using OrgSysApp.Forms.Data;
using OrgSysApp.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OrgSysApp.Forms
{
    public partial class Org : Form
    {
        MainData mainData;
        public Org()
        {
            InitializeComponent();
        }

        private void mainMenu_click(object sender, EventArgs e)
        {
            if (Application.OpenForms["MainData1"] != null)
            {
                pnlBody.Controls.Remove(pnlBody.Controls["MainData1"]);               
                Application.OpenForms["MainData1"].Close();
            }

            switch (((MainMenu)sender).ButtonSelectedName)
            {
                case "Data":
                    if (pnlBody.Controls["MainData1"] == null)
                    {
                        mainData = new MainData();
                        mainData.Name = "MainData1";
                        mainData.TopLevel = false;
                        mainData.Height = 0;
                        mainData.Size = pnlBody.Size;
                        mainData.Dock = DockStyle.Fill;                        
                        pnlBody.Controls.Add(mainData);
                        mainData.Show();                       
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
