using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OrgSysApp.Tools
{
    public partial class MainMenu : UserControl
    {
        [Category("Org")]
        public event EventHandler click;

        [Category("Org")]
        public string ButtonSelectedName { get; set; }
        public MainMenu()
        {
            InitializeComponent();
        }

        private void this_Click(object sender, EventArgs e)
        {
            foreach (var Control in this.Controls)
            {
                if (Control == sender)
                {
                    ButtonSelectedName = ((ButtonImage)Control).ButtonName;
                    ((ButtonImage)Control).active = true;
                }
                else
                    ((ButtonImage)Control).active = false;
            }

            if (click != null)
                click(this, e);
        }
    }
}
