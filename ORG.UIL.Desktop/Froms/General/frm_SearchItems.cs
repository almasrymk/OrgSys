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
using ORGEntity;

namespace ORG.UIL.Desktop.Froms.General
{
    public partial class frm_SearchItems : Form
    {
        frm_panelblack _frm_panelMessage;
        public List<Item> Items { get; set; }
        public List<int> Ids { get; set; }
        public int PageInv { get; set; }
        public frm_SearchItems(List<int> _Ids)
        {
            InitializeComponent();
            Ids = _Ids;
            _frm_panelMessage = new frm_panelblack();
            _frm_panelMessage.Name = "_frm_panelMessage";
            _frm_panelMessage.Show();
        }

        private void frm_SearchItems_Load(object sender, EventArgs e)
        {
            PageInv = 1;
            dgvTransactions.Rows.Clear();
            LoadData();
        }

        private void picSearch_Click(object sender, EventArgs e)
        {
            PageInv = 1;
            dgvTransactions.Rows.Clear();
            LoadData();
        }

        private void LoadData()
        {
            if (txtSearch.Text == null)
                txtSearch.Text = "";
            lblPaggingInv.Visible = false;
            var inv = new ItemRepo().GetAllByFilter(x =>
            (Ids.Count == 0 || !Ids.Contains(x.Id)) && (
            "" + txtSearch.Text == "" ||
            x.Code.Equals(txtSearch.Text) ||
            x.Barcode.Equals(txtSearch.Text) ||
            x.Name.Equals(txtSearch.Text)
            ), PageInv);
            if (inv != null)
            {
                lblPaggingInv.Visible = inv.Count == 20;
                int RowCount = inv.Count;
                for (int i = 0; i < RowCount; i++)
                {
                    int Row = dgvTransactions.Rows.Add();
                    dgvTransactions["colitemId", Row].Value = inv[i].Id;
                    dgvTransactions["colNo", Row].Value = Row + 1;
                    dgvTransactions["colCategory", Row].Value = inv[i].Category?.Name;
                    dgvTransactions["colCode", Row].Value = inv[i].Code;
                    dgvTransactions["colItem", Row].Value = inv[i].Name;
                    dgvTransactions["colBarcode", Row].Value = inv[i].Barcode;
                    dgvTransactions["colPrice", Row].Value = inv[i].SellingPrice;
                    dgvTransactions["colCheck", Row].Value = false;
                }
            }
        }

        private void dgvTransactions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0 || dgvTransactions.CurrentRow == null) return;
            int CurrRowIndex = dgvTransactions.CurrentRow.Index;
            Items = new List<Item>();
            Items.Add(new ItemRepo().getById(int.Parse("0" + dgvTransactions["colitemId", CurrRowIndex].Value)));
            Application.OpenForms["_frm_panelMessage"].Close();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bttnOk2_Click(object sender, EventArgs e)
        {
            Items = new List<Item>();
            foreach (DataGridViewRow row in dgvTransactions.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["colCheck"];
                if ("" + chk.Value == "1")
                {
                    Items.Add(new ItemRepo().getById(int.Parse("0" + dgvTransactions["colitemId", row.Index].Value)));
                }
            }

            Application.OpenForms["_frm_panelMessage"].Close();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void lblb1_Click(object sender, EventArgs e)
        {
            Application.OpenForms["_frm_panelMessage"].Close();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void lblPaggingInv_Click(object sender, EventArgs e)
        {
            PageInv++;
            LoadData();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                PageInv = 1;
                dgvTransactions.Rows.Clear();
                LoadData();
            }
        }
    }
}
