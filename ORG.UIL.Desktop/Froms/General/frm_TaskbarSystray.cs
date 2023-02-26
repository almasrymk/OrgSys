using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORG.UIL.Desktop.Froms.General
{
    public partial class frm_TaskbarSystray : Form
    {
        public frm_TaskbarSystray()
        {
            InitializeComponent(); 
          
        }

        private void frm_TaskbarSystray_Load(object sender, EventArgs e)
        {
          
        }      

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            this.Show();
            timer1.Enabled = false;
            this.Activate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void updateSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            timer1.Enabled = false;
            this.Activate();
        }

        private void lblb1_Click(object sender, EventArgs e)
        {
            this.Hide();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string title = GeneralMembers.Lang == "ar" ? "نسخة جديدة!" : "New version!", body = GeneralMembers.Lang == "ar" ? "تم العثور على نسخة جديد من البرنامج اضغط للحصول علية." : "A new version of the program has been found. Click for it.";
            notifyIcon1.ShowBalloonTip(1000, title, body, ToolTipIcon.Info);
        }

        private void lblb1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            timer1.Enabled = true;
        }
    }
}
