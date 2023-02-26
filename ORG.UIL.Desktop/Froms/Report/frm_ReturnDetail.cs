using ORGRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORG.UIL.Desktop.Froms.Report
{
    public partial class frm_ReturnDetail : Form
    {
        DateTime _Date = DateTime.Now;

        public DateTime Date
        {
            get
            {
                return _Date;
            }
            set
            {
                _Date = dateTimePicker1.Value = dateTimePicker2.Value = value;
            }
        }


        int _UserId = GeneralMembers.User.Id;
        public int UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                _UserId = value;
            }
        }

        public int currPage { get; set; }
        public frm_ReturnDetail()
        {
            InitializeComponent();
            dgvTransactions.Columns["colusername"].Visible = GeneralMembers.User.RoleId <= 2;
            label4.Visible = cmbUser.Visible = GeneralMembers.User.RoleId <= 2;
            if (GeneralMembers.User.RoleId == 3)
            {
                if (GeneralMembers.Lang == "ar")
                    bottunOrg1.Left = label4.Right - bottunOrg1.Width;
                else
                    bottunOrg1.Left = label4.Left;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            dateTimePicker1.Value = Date;
            dateTimePicker2.Value = Date;
            var uslist = new UserRepo().getAll("", 1, 10000, true, GeneralMembers.User.SchemaName);
            if (uslist == null)
                uslist = new List<ORGEntity.UsersApp>();
            uslist.Insert(0, new ORGEntity.UsersApp() { Id = 0, UserName = GeneralMembers.Lang == "ar" ? "الكل" : "All" });
            cmbUser.ValueMember = "Id";
            cmbUser.DisplayMember = "UserName";
            cmbUser.DataSource = uslist;
            cmbUser.SelectedValue = UserId;
            pictureBox1_Click(null, null);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            bottunOrg1.Enabled = false;
            try
            {

                currPage = 1;
                dgvTransactions.Rows.Clear();
                var salesDetail = new ReportRepo().returnDetailsReport(dateTimePicker1.Value, dateTimePicker2.Value, int.Parse("0" + cmbUser.SelectedValue));
                lblPagging.Visible = salesDetail.Count == 100;
                foreach (var item in salesDetail)
                {
                    Application.DoEvents();
                    int Row = dgvTransactions.Rows.Add();
                    dgvTransactions["colTrnsID", Row].Value = item.Id;
                    dgvTransactions["colNo", Row].Value = Row + 1;
                    dgvTransactions["colDate", Row].Value = item.Date;
                    dgvTransactions["colCode", Row].Value = item.Code;
                    dgvTransactions["colTotalVal", Row].Value = item.Net;                   
                    dgvTransactions["colusername", Row].Value = item.UserName;
                }
            }
            catch (Exception ex)
            {
            }
            bottunOrg1.Enabled = true;
        }

        private void lblPagging_Click(object sender, EventArgs e)
        {
            bottunOrg1.Enabled = false;
            var salesDetail = new ReportRepo().returnDetailsReport(dateTimePicker1.Value, dateTimePicker2.Value, int.Parse("0" + cmbUser.SelectedValue), ++currPage);
            lblPagging.Visible = salesDetail.Count == 100;
            foreach (var item in salesDetail)
            {
                Application.DoEvents();
                int Row = dgvTransactions.Rows.Add();
                dgvTransactions["colTrnsID", Row].Value = item.Id;
                dgvTransactions["colNo", Row].Value = Row + 1;
                dgvTransactions["colDate", Row].Value = item.Date;
                dgvTransactions["colCode", Row].Value = item.Code;
                dgvTransactions["colTotalVal", Row].Value = item.Net;
                dgvTransactions["colusername", Row].Value = item.UserName;
            }
            bottunOrg1.Enabled = true;
        }

        private void dgvTransactions_DoubleClick(object sender, EventArgs e)
        {
            var id = int.Parse("0" + dgvTransactions["colTrnsID", dgvTransactions.CurrentRow.Index].Value);
            Helper.Sound(Application.StartupPath, SoudType.GroupMouseEnter);
            ((frm_Main)Application.OpenForms["frm_Main"]).SelectMenu("report", false);
            ((frm_Main)Application.OpenForms["frm_Main"]).SelectMenu("return", true, id);
        }
    }
}
