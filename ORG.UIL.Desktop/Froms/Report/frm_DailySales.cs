using ORGRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORG.UIL.Desktop
{
    public partial class frm_DailySales : Form
    {
        
        public frm_DailySales()
        {
            InitializeComponent();
            dgvTransactions.Columns["colusername"].Visible = GeneralMembers.User.RoleId <= 2;
            label4.Visible = cmbUser.Visible = GeneralMembers.User.RoleId <= 2;
            if (GeneralMembers.User.RoleId == 3)
            {
                if (GeneralMembers.Lang == "ar")
                    pictureBox1.Left = label4.Right - pictureBox1.Width;
                else
                    pictureBox1.Left = label4.Left;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            var uslist = new UserRepo().getAll("", 1, 10000, true, GeneralMembers.User.SchemaName);
            if (uslist == null)
                uslist = new List<ORGEntity.UsersApp>();
            uslist.Insert(0, new ORGEntity.UsersApp() { Id = 0, UserName = GeneralMembers.Lang == "ar" ? "الكل" : "All" });
            cmbUser.ValueMember = "Id";
            cmbUser.DisplayMember = "UserName";
            cmbUser.DataSource = uslist;
            cmbUser.SelectedValue = GeneralMembers.User.Id;
            pictureBox1_Click(null, null);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Enabled = false;
            try
            {
                dgvTransactions.Rows.Clear();
                var salesDetail = new ReportRepo().dailySalesReport(dateTimePicker1.Value, dateTimePicker2.Value , int.Parse("0" + cmbUser.SelectedValue));
                foreach (var item in salesDetail)
                {
                    Application.DoEvents();
                    int Row = dgvTransactions.Rows.Add();
                    dgvTransactions["colNo", Row].Value = Row + 1;
                    dgvTransactions["colDate", Row].Value = item.Date;
                    dgvTransactions["colTotalVal", Row].Value = item.TotalInvoice;
                    dgvTransactions["colTotalReVal", Row].Value = item.TotalRetrun;
                    dgvTransactions["colNetAmount", Row].Value = item.Net;
                    dgvTransactions["colusername", Row].Value = item.UserName;
                    dgvTransactions["colUserId", Row].Value = item.UserId;                    
                }
            }
            catch (Exception ex)
            {

            }
            pictureBox1.Enabled = true;
        }

        private void dgvTransactions_DoubleClick(object sender, EventArgs e)
        {
            Helper.Sound(Application.StartupPath, SoudType.GroupMouseEnter);
            var d = DateTime.Parse("" + dgvTransactions["colDate", dgvTransactions.CurrentRow.Index].Value);
            var u = int.Parse("0" + dgvTransactions["colUserId", dgvTransactions.CurrentRow.Index].Value);

            ((frm_Report)Application.OpenForms["_frm_Report1"]).SelectMenuItem("dailySales", false);
            ((frm_Report)Application.OpenForms["_frm_Report1"]).SelectMenuItem("salesDetail", true);
            if (Application.OpenForms["frm_salesDetail1"] != null)
            {
                ((frm_SalesDetail)Application.OpenForms["frm_salesDetail1"]).Date = d;
                ((frm_SalesDetail)Application.OpenForms["frm_salesDetail1"]).UserId =  u;
            }
        }
    }
}
