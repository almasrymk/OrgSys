using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using ORGEntity;

namespace ORG.UIL.Desktop.Froms.General
{
    public partial class toolSearchInfo : UserControl
    {
        public event _SearchEvent SearchEvent;
        public event _Pagging pagging;
        public event _Select SelectEdit;
        public event _Select PrintInvoice;
        public event _Select DeleteInvoice;
        public int CurrPage = 1;

        #region Properties      

        int _Paging = 0;
        public int Paging
        {
            get { return _Paging; }
            set
            {
                _Paging = value;
                CurrPage = 1;
                pnlPaging.Controls.Clear();
                Application.DoEvents();
                if (_Paging > 1)
                    for (int i = _Paging; i > 0; i--)
                    {
                        Application.DoEvents();
                        Label lbl = new Label();
                        lbl.TextAlign = ContentAlignment.MiddleCenter;
                        lbl.Font = new Font("Arial", 8, FontStyle.Bold);
                        if (i > 1)
                        {
                            lbl.ForeColor = Color.FromArgb(100, 150, 250);
                            lbl.BackColor = Color.FromKnownColor(KnownColor.Control);
                        }
                        else
                        {
                            lbl.ForeColor = Color.DimGray;
                            lbl.BackColor = Color.FromArgb(100, 150, 250);
                        }
                        lbl.Dock = GeneralMembers.Lang == "en" ? DockStyle.Left : DockStyle.Right; ;
                        lbl.Width = 25;
                        lbl.Text = i.ToString();
                        lbl.Click += Clicklbl;
                        pnlPaging.Controls.Add(lbl);
                    }
            }
        }

        Image _DefulteImage = null;
        public Image DefulteImage
        {
            get { return _DefulteImage; }
            set { _DefulteImage = value; }
        }

        Image _FromImage = null;
        public Image FromImage
        {
            get { return _FromImage; }
            set { _FromImage = value; }
        }

        Image _ToImage = null;
        public Image ToImage
        {
            get { return _ToImage; }
            set { _ToImage = value; }
        }

        Image _EMPImage = null;
        public Image EMPImage
        {
            get { return _EMPImage; }
            set { _EMPImage = value; }
        }
        List<Invoice> _invoiceView = null;
        public List<Invoice> InvoiceView
        {
            get { return _invoiceView; }
            set
            {
                try
                {
                    if (_invoiceView == null || CurrPage == 1)
                    {
                        _invoiceView = new List<Invoice>();
                        dgvTransactions.Rows.Clear();
                    }

                    lblPagging.Visible = value.Count == 20;

                    _invoiceView.AddRange(value);
                    if (_invoiceView != null)
                    {
                        //Application.DoEvents();
                        int RowCount = value.Count;
                        for (int i = 0; i < RowCount; i++)
                        {
                            int Row = dgvTransactions.Rows.Add();                                                        //Application.DoEvents();

                            dgvTransactions["colTrnsID", Row].Value = value[i].Id;
                            if (DefulteImage != null)
                                dgvTransactions["colImage", Row].Value = DefulteImage;
                            dgvTransactions["colNo", Row].Value = Row + 1;
                            //dgvTransactions["colFrom", Row].Value = GeneralMembers.Lang == "en" ? _invoiceView[i].sideFromNameF : _invoiceView[i].sideFromNameF;
                            //dgvTransactions["colTo", Row].Value = GeneralMembers.Lang == "en" ? _invoiceView[i].sideToNameF : _invoiceView[i].sideToNameF;
                            dgvTransactions["colDate", Row].Value = value[i].InvoiceDate;
                            dgvTransactions["colCode", Row].Value = value[i].Code;
                            dgvTransactions["colTotalVal", Row].Value = value[i].NetTotal;
                            //dgvTransactions["colEmplayee", Row].Value = GeneralMembers.Lang == "en" ? _invoiceView[i].sideEmpNameF : _invoiceView[i].sideEmpNameF;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        #endregion

        #region Events
        public toolSearchInfo()
        {
            InitializeComponent();
            dgvSelectInfoItems.AutoGenerateColumns = false;
        }

        private void dgvTransactions_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Helper.Sound(Application.StartupPath, SoudType.ItemMouseEnter);
                int id = dgvTransactions.CurrentRow.Index;
                if (_invoiceView == null || _invoiceView.Count == 0)
                {
                    lblSelectInfoDate.Text = string.Format("{0:dd/mm/yyyy}", DateTime.Now);
                    lblSelectInfoTime.Text = string.Format("{0:hh:mm:ss}", DateTime.Now);
                    lblSelectInfoCountItems.Text = "";
                    lblSelectInfoTotal.Text = string.Format("{0:0.00}", 0);
                    dgvSelectInfoItems.DataSource = null;
                    return;
                }

                lblSelectInfoDate.Text = string.Format("{0:dd/mm/yyyy}", _invoiceView[id].InvoiceDate);
                lblSelectInfoTime.Text = string.Format("{0:hh:mm:ss}", _invoiceView[id].InvoiceDate);
                decimal count = 0;
                if (_invoiceView[id].InvoiceItem != null)
                {
                    foreach (var item in _invoiceView[id].InvoiceItem)
                    {
                        count += item.Quantity;
                    }
                }
                lblSelectInfoCountItems.Text = "" + count;

                lblSelectInfoTotal.Text = string.Format("{0:0.00}", _invoiceView[id].NetTotal);
                if (_invoiceView[id].InvoiceItem != null)
                    dgvSelectInfoItems.DataSource = _invoiceView[id].InvoiceItem;
                else
                    dgvSelectInfoItems.DataSource = null;
            }
            catch
            {

            }
        }

        private void OpenCloseSelectInfo(object sender, EventArgs e)
        {
            switch (((Control)sender).Tag.ToString())
            {
                case "Information":
                    if (pnlSelectInfoInfo.Height == 40)
                        pnlSelectInfoInfo.Height = 250;
                    else
                        pnlSelectInfoInfo.Height = 40;
                    break;
                case "Items":
                    if (pnlSelectInfoItems.Height == 40)
                        pnlSelectInfoItems.Height = (pnlSelectInfo.Height - (pnlSelectInfoInfo.Height) > 225 ? pnlSelectInfo.Height - (pnlSelectInfoInfo.Height) : 225);
                    else
                        pnlSelectInfoItems.Height = 40;
                    break;
                default:
                    break;
            }
            if (pnlSelectInfoItems.Height != 40)
            {
                pnlSelectInfoItems.Height = (pnlSelectInfo.Height - (pnlSelectInfoInfo.Height) > 225 ? pnlSelectInfo.Height - (pnlSelectInfoInfo.Height) : 225);
            }
        }

        private void dgvTransactions_DoubleClick(object sender, EventArgs e)
        {
            if (SelectEdit != null)
            {
                Helper.Sound(Application.StartupPath, SoudType.GroupMouseEnter);
                SelectEdit(int.Parse("0" + dgvTransactions["colTrnsID", dgvTransactions.CurrentRow.Index].Value));               
            }
        }

        private void Clicklbl(object sender, EventArgs e)
        {
            for (int i = 0; i < pnlPaging.Controls.Count; i++)
            {
                Application.DoEvents();
                pnlPaging.Controls[i].ForeColor = Color.FromArgb(100, 150, 250);
                pnlPaging.Controls[i].BackColor = Color.FromKnownColor(KnownColor.Control);
            }
            ((Control)sender).ForeColor = Color.DimGray;
            ((Control)sender).BackColor = Color.FromArgb(100, 150, 250);
        }
        #endregion

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            //     lblPagging.Visible = true;
            CurrPage = 1;
            if (e.KeyData == Keys.Enter && SearchEvent != null)
                SearchEvent(txtSearch.Text);
        }

        private void lblPagging_Click(object sender, EventArgs e)
        {
            if (pagging != null)
                pagging(txtSearch.Text, ++CurrPage);
        }

        private void picSearch_Click(object sender, EventArgs e)
        {
            //      lblPagging.Visible = true;
            CurrPage = 1;
            SearchEvent(txtSearch.Text);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                CurrPage = 1;
                SearchEvent(txtSearch.Text);
            }
        }

        private void dgvTransactions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!dgvTransactions.Focused || dgvTransactions.CurrentRow.Index < 0) return;
            int CurrRowIndex = dgvTransactions.CurrentRow.Index;
            if (dgvTransactions.Columns[e.ColumnIndex].Name == "colp")
            {
                if (PrintInvoice != null)
                {
                    Helper.Sound(Application.StartupPath, SoudType.GroupMouseSelect);
                    PrintInvoice(int.Parse("" + dgvTransactions["colTrnsID", CurrRowIndex].Value));                    
                }
            }
            else if (dgvTransactions.Columns[e.ColumnIndex].Name == "cold")
            {
                if (DeleteInvoice != null)
                {
                    DeleteInvoice(int.Parse("" + dgvTransactions["colTrnsID", CurrRowIndex].Value));
                    Helper.Sound(Application.StartupPath, SoudType.DeleteClick);
                }
            }
        }
    }
}
