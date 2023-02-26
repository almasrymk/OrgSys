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
    public partial class frm_TotalItemSales : Form
    {        
        public int currPage { get; set; }
        public frm_TotalItemSales()
        {
            InitializeComponent();
            dgvTransactions.Columns["colusername"].Visible = GeneralMembers.User.RoleId <= 2;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            var uslist = new ItemRepo().getAll("", 1, 10000);
            if (uslist == null)
                uslist = new List<ORGEntity.Item>();
            uslist.Insert(0, new ORGEntity.Item() { Id = 0, Name = GeneralMembers.Lang == "ar" ? "الكل" : "All" });
            cmbItem.ValueMember = "Id";
            cmbItem.DisplayMember = "Name";
            cmbItem.DataSource = uslist;
            pictureBox1_Click(null, null);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            bottunOrg1.Enabled = false;
            try
            {

                currPage = 1;
                dgvTransactions.Rows.Clear();
                var totalItemSales = new ReportRepo().totalItemSalesReport(dateTimePicker1.Value, dateTimePicker2.Value , int.Parse("0" + cmbItem.SelectedValue) , GeneralMembers.User.RoleId <= 2 ? 0 : GeneralMembers.User.Id);
                lblPagging.Visible = totalItemSales.Count == 100;
                foreach (var item in totalItemSales)
                {
                    Application.DoEvents();
                    int Row = dgvTransactions.Rows.Add();
                    dgvTransactions["colTrnsID", Row].Value = item.Id;
                    dgvTransactions["colNo", Row].Value = Row + 1;
                    dgvTransactions["colItem", Row].Value = item.Item;
                    dgvTransactions["colQuantity", Row].Value = item.Quantity;
                    dgvTransactions["colQuantityRe", Row].Value = item.QuantityRetrun;
                    dgvTransactions["colQuantityNet", Row].Value = item.NetQuantity;
                    dgvTransactions["colTotalVal", Row].Value = item.Total;
                    dgvTransactions["colTotalRe", Row].Value = item.TotalRetrun;
                    dgvTransactions["colNetAmount", Row].Value = item.Net;
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
            var totalItemSales = new ReportRepo().totalItemSalesReport(dateTimePicker1.Value, dateTimePicker2.Value, GeneralMembers.User.RoleId <= 2 ? 0 : GeneralMembers.User.Id,++currPage);
            lblPagging.Visible = totalItemSales.Count == 100;
            foreach (var item in totalItemSales)
            {
                Application.DoEvents();
                int Row = dgvTransactions.Rows.Add();
                dgvTransactions["colTrnsID", Row].Value = item.Id;
                dgvTransactions["colNo", Row].Value = Row + 1;
                dgvTransactions["colItem", Row].Value = item.Item;
                dgvTransactions["colQuantity", Row].Value = item.Quantity;
                dgvTransactions["colQuantityRe", Row].Value = item.QuantityRetrun;
                dgvTransactions["colQuantityNet", Row].Value = item.NetQuantity;
                dgvTransactions["colTotalVal", Row].Value = item.Total;
                dgvTransactions["colTotalRe", Row].Value = item.TotalRetrun;
                dgvTransactions["colNetAmount", Row].Value = item.Net;
                dgvTransactions["colusername", Row].Value = item.UserName;
            }
            bottunOrg1.Enabled = true;
        }
    }
}
