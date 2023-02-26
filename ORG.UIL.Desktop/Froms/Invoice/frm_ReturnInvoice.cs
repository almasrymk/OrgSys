using ORG.UIL.Desktop.Froms.General;
using ORGEntity;
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

namespace ORG.UIL.Desktop
{
    public partial class frm_ReturnInvoice : Form
    {
        public int PageInv { get; set; }
        public int PageReInv { get; set; }

        Invoice _MyOb = new Invoice();
        public Invoice MyOb
        {
            get
            {
                if (_MyOb == null)
                    _MyOb = new Invoice();

                _MyOb.TypeId = 2;
                if (_MyOb.Id == 0)
                {
                    _MyOb.Code = new InvoiceRepo().GetMaxCode(2);
                    _MyOb.InvoiceDate = DateTime.Now.Date;
                    _MyOb.UserId = GeneralMembers.User.Id;
                }
                _MyOb.StoreId = 1;
                _MyOb.DealerId = 1;
                _MyOb.GrandTotal = decimal.Parse("0" + txtNet.Text);
                _MyOb.NetTotal = decimal.Parse("0" + txtNet.Text);

                //_MyOb.GrandTotal = toolCashier1.Total;
                //_MyOb.Discount = toolCashier1.Discount;
                //_MyOb.Tax = toolCashier1.Tax;
                //_MyOb.Service = toolCashier1.Service;
                //_MyOb.NetTotal = toolCashier1.Net;
                _MyOb.InvoiceItem = new List<InvoiceItem>();

                for (int i = 0; i < dgvDetials.Rows.Count; i++)
                {
                    InvoiceItem _invoiceD = new InvoiceItem();
                    _invoiceD.Id = int.Parse("0" + dgvDetials["colID", i].Value);
                    _invoiceD.Code = string.Format("{0:00}", i + 1);
                    _invoiceD.ItemId = int.Parse("0" + dgvDetials["colItemID", i].Value);
                    _invoiceD.UnitId = int.Parse("0" + dgvDetials["colUnitID", i].Value);
                    _invoiceD.Quantity = decimal.Parse("0" + dgvDetials["colQty", i].Value);
                    _invoiceD.Price = decimal.Parse("0" + dgvDetials["colPrice", i].Value);
                    _invoiceD.NetTotal = decimal.Parse("0" + dgvDetials["colTotal", i].Value);
                    _MyOb.InvoiceItem.Add(_invoiceD);
                }
                return _MyOb;
            }
            set
            {
                _MyOb = value;
                if (_MyOb == null)
                    _MyOb = new Invoice();

                if (_MyOb.InvoiceItem == null)
                    _MyOb.InvoiceItem = new List<InvoiceItem>();

                dgvDetials.Rows.Clear();
                for (int i = 0; i < _MyOb.InvoiceItem.Count; i++)
                {
                    int row = dgvDetials.Rows.Add();
                    dgvDetials["colID", row].Value = _MyOb.InvoiceItem[i].Id;
                    dgvDetials["colSerial", row].Value = _MyOb.InvoiceItem[i].Code;
                    dgvDetials["colItemID", row].Value = _MyOb.InvoiceItem[i].ItemId;
                    dgvDetials["colItemName", row].Value = _MyOb.InvoiceItem[i].ItemName;
                    dgvDetials["colUnitID", row].Value = _MyOb.InvoiceItem[i].UnitId;
                    dgvDetials["colQty", row].Value = string.Format("{0:0.00}", _MyOb.InvoiceItem[i].Quantity);
                    dgvDetials["colMaXQty", row].Value = string.Format("{0:0.00}", _MyOb.InvoiceItem[i].Quantity);
                    dgvDetials["colPrice", row].Value = string.Format("{0:0.00}", _MyOb.InvoiceItem[i].Price);
                    dgvDetials["colTotal", row].Value = string.Format("{0:0.00}", _MyOb.InvoiceItem[i].NetTotal);
                }

                bttnSave.Visible = bttnDelete.Visible = _MyOb.Id > 0;
            }
        }

        public frm_ReturnInvoice()
        {
            InitializeComponent();
        }

        private void frm_ReturnInvoice_Load(object sender, EventArgs e)
        {
            picSearch_Click(null, null);
        }

        public void LoadData()
        {
            lblPaggingInv.Visible = false;
            var inv = new InvoiceRepo().getAllInvoiceByUserId(txtSearch.Text, GeneralMembers.User.RoleId <= 2 ? 0 : GeneralMembers.User.Id, PageInv);
            if (inv != null)
            {
                lblPaggingInv.Visible = inv.Count == 20;
                int RowCount = inv.Count;
                for (int i = 0; i < RowCount; i++)
                {
                    int Row = dgvTransactions.Rows.Add();
                    dgvTransactions["colTrnsID", Row].Value = inv[i].Id;
                    dgvTransactions["colNo", Row].Value = Row + 1;
                    dgvTransactions["colDate", Row].Value = inv[i].InvoiceDate;
                    dgvTransactions["colCode", Row].Value = inv[i].Code;
                    dgvTransactions["colTotalVal", Row].Value = inv[i].NetTotal;
                }
            }
        }

        private void lblPaggingInv_Click(object sender, EventArgs e)
        {
            PageInv++;
            LoadData();
        }

        private void picSearch_Click(object sender, EventArgs e)
        {
            PageInv = 1;
            dgvTransactions.Rows.Clear();
            LoadData();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                picSearch_Click(null, null);
        }

        public void NewFile(object sender, EventArgs e)
        {
            try
            {
                txtQty.Text = "0.00";
                txtPrice.Text = "0.00";
                txtNet.Text = "0.00";
                MyOb = new Invoice() { Code = new InvoiceRepo().GetMaxCode(2), InvoiceDate = DateTime.Now, CreateDate = DateTime.Now };
            }
            catch { }
        }

        public void SaveFile(object sender, EventArgs e)
        {
            try
            {
                string Title = GeneralMembers.Lang == "ar" ? "خطأ فى الحفظ" : "Errore Save";
                string Msg = GeneralMembers.Lang == "ar" ? "لا يمكن حفط هذا الملف" : "Can't Save This File ...";
                if (!Validation())
                {
                    MessageORG.Show(Title, Msg, CountButton.One, MessageBoxIcon.Error);
                }
                else
                {
                    bttnSave.Enabled = false;
                    MyOb = new InvoiceRepo().Save(MyOb);
                    string Title1 = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                    string Msg1 = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                    MessageORG.ShowConfirm(Title1, Msg1);
                    Helper.Sound(Application.StartupPath, SoudType.SaveInvoice);
                    picSearch_Click(null, null);
                    bttnSave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bttnSave.Enabled = true;
            }
        }

        public void SaveNewFile(object sender, EventArgs e)
        {
            try
            {
                string Title = GeneralMembers.Lang == "ar" ? "خطأ فى الحفظ" : "Errore Save";
                string Msg = GeneralMembers.Lang == "ar" ? "لا يمكن حفط هذا الملف" : "Can't Save This File ...";
                if (!Validation())
                {
                    MessageORG.Show(Title, Msg, CountButton.One, MessageBoxIcon.Error);
                }
                else
                {
                    bttnSaveNew.Enabled = false;
                    MyOb = new InvoiceRepo().Save(MyOb);
                    string Title1 = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                    string Msg1 = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                    MessageORG.ShowConfirm(Title1, Msg1);
                    Helper.Sound(Application.StartupPath, SoudType.SaveInvoice);
                    NewFile(sender, e);
                    picSearch_Click(null, null);
                    bttnSaveNew.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bttnSave.Enabled = true;
            }
        }

        private bool Validation()
        {
            if (dgvDetials.RowCount < 1) return false;
            return true;
        }

        private void dgvTransactions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvTransactions.Columns[e.ColumnIndex].Name == "colSelect")
            {
                int CurrRowIndex = dgvTransactions.CurrentRow.Index;
                var inv = new InvoiceRepo().getById(int.Parse("" + dgvTransactions["colTrnsID", CurrRowIndex].Value));
                Invoice ReInv = new Invoice();
                ReInv.Code = "";
                ReInv.InvoiceDate = DateTime.Now;
                ReInv.CreateDate = DateTime.Now;
                ReInv.DealerId = ReInv.DealerId;
                ReInv.StoreId = ReInv.StoreId;
                ReInv.TypeId = 2;
                ReInv.UserId = GeneralMembers.User.Id;
                ReInv.InvoiceBaseId = inv.Id;
                //ReInv.Tax = inv.Tax;
                //ReInv.Discount = inv.Discount;
                //ReInv.Service = inv.Service;
                ReInv.GrandTotal = inv.GrandTotal;
                ReInv.NetTotal = inv.NetTotal;
                ReInv.InvoiceItem = new List<InvoiceItem>();
                foreach (var item in inv.InvoiceItem)
                {
                    var Quantity = new InvoiceRepo().getRestQuantity(inv.Id, item.ItemId);
                    if (Quantity > 0)
                    {
                        var newItem = new InvoiceItem
                        {
                            Code = item.Code,
                            Discount = item.Discount,
                            GrandTotal = Quantity * item.Price,
                            ItemId = item.ItemId,
                            NetTotal = Quantity * item.Price,
                            Price = item.Price,
                            Quantity = Quantity,
                            StoreId = item.StoreId,
                            Tax = item.Tax,
                            Item = item.Item,
                            UnitId = item.UnitId,
                            Serialnumber = item.Serialnumber
                        };

                        ReInv.InvoiceItem.Add(newItem);
                    }
                }
                MyOb = ReInv;
                Calc();
                if (dgvDetials.CurrentRow != null)
                {
                    int CurrIndex = dgvDetials.CurrentRow.Index;
                    txtQty.Text = string.Format("{0:0.00}", dgvDetials["colQty", CurrIndex].Value);
                    txtPrice.Text = string.Format("{0:0.00}", dgvDetials["colPrice", CurrIndex].Value);
                }
            }
        }

        private void dgvDetials_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetials.CurrentRow != null)
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
                int CurrRowIndex = dgvDetials.CurrentRow.Index;
                txtQty.Text = string.Format("{0:0.00}", dgvDetials["colQty", CurrRowIndex].Value);
                txtPrice.Text = string.Format("{0:0.00}", dgvDetials["colPrice", CurrRowIndex].Value);


                if (dgvDetials.Columns[e.ColumnIndex].Name == "colDelete")
                {
                    int ItemId = int.Parse("" + dgvDetials["colItemID", CurrRowIndex].Value);
                    var item = _MyOb.InvoiceItem.FirstOrDefault(x => x.ItemId == ItemId);
                    _MyOb.InvoiceItem.Remove(item);
                    dgvDetials.Rows.RemoveAt(CurrRowIndex);                    
                    txtQty.Text = "0.00";
                    txtPrice.Text = "0.00";
                    if (dgvDetials.CurrentRow != null)
                    {
                        int CurrIndex = dgvDetials.CurrentRow.Index;
                        txtQty.Text = string.Format("{0:0.00}", dgvDetials["colQty", CurrIndex].Value);
                        txtPrice.Text = string.Format("{0:0.00}", dgvDetials["colPrice", CurrIndex].Value);
                    }

                    Helper.Sound(Application.StartupPath, SoudType.DeleteClick);
                }
                else if (dgvDetials.Columns[e.ColumnIndex].Name == "colAddQty")
                {
                    try
                    {
                        if (!dgvDetials.Focused || dgvDetials.CurrentRow.Index < 0) return;

                        Helper.Sound(Application.StartupPath, SoudType.AddQuantity);
                        int ItemId = int.Parse("" + dgvDetials["colItemID", CurrRowIndex].Value);
                        decimal qty = decimal.Parse("" + dgvDetials["colQty", CurrRowIndex].Value);
                        decimal Maxqty = decimal.Parse("" + dgvDetials["colMaXQty", CurrRowIndex].Value);
                        ++qty;
                        if (qty <= Maxqty || _MyOb.InvoiceBaseId == 0)
                        {
                            dgvDetials["colQty", CurrRowIndex].Value = string.Format("{0:0.00}", qty);
                            txtQty.Text = string.Format("{0:0.00}", qty);
                            txtPrice.Text = string.Format("{0:0.00}", dgvDetials["colPrice", CurrRowIndex].Value);
                            dgvDetials["colTotal", CurrRowIndex].Value = string.Format("{0:0.00}", decimal.Parse(txtPrice.Text) * qty);
                        }
                        Calc();
                    }
                    catch
                    {
                    }
                }
                else if (dgvDetials.Columns[e.ColumnIndex].Name == "colSubQty")
                {
                    try
                    {
                        if (!dgvDetials.Focused || dgvDetials.CurrentRow.Index < 0) return;
                        Helper.Sound(Application.StartupPath, SoudType.AddQuantity);
                        int ItemId = int.Parse("" + dgvDetials["colItemID", CurrRowIndex].Value);
                        decimal qty = decimal.Parse("" + dgvDetials["colQty", CurrRowIndex].Value);
                        --qty;
                        if (qty > 0)
                        {
                            dgvDetials["colQty", CurrRowIndex].Value = string.Format("{0:0.00}", qty);
                            txtQty.Text = string.Format("{0:0.00}", qty);
                            txtPrice.Text = string.Format("{0:0.00}", dgvDetials["colPrice", CurrRowIndex].Value);
                            dgvDetials["colTotal", CurrRowIndex].Value = string.Format("{0:0.00}", decimal.Parse(txtPrice.Text) * qty);
                        }
                        Calc();
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void Calc()
        {
            decimal _total = 0;
            for (int i = 0; i < dgvDetials.Rows.Count; i++)
            {
                if ("" + dgvDetials["colTotal", i].Value == "") continue;
                var _Value = decimal.Parse("" + dgvDetials["colTotal", i].Value);
                _total += _Value;

            }
            txtNet.Text = string.Format("{0:0.00}", _total);
        }

        private void lblDone_Click(object sender, EventArgs e)
        {
            decimal x = 0;
            if (!decimal.TryParse(txtQty.Text, out x))
                txtQty.Text = "0.00";
            decimal x2 = 0;
            if (!decimal.TryParse(txtPrice.Text, out x2))
                txtPrice.Text = "0.00";

            txtQty.Text = string.Format("{0:0.00}", decimal.Parse(txtQty.Text));
            txtPrice.Text = string.Format("{0:0.00}", decimal.Parse(txtPrice.Text));
            if (dgvDetials.CurrentRow != null)
            {
                int CurrRowIndex = dgvDetials.CurrentRow.Index;
                var MaXQty = decimal.Parse("" + dgvDetials["colMaXQty", CurrRowIndex].Value);
                if (MaXQty < decimal.Parse(txtQty.Text))
                    txtQty.Text = string.Format("{0:0.00}", MaXQty);

                dgvDetials["colQty", CurrRowIndex].Value = string.Format("{0:0.00}", decimal.Parse(txtQty.Text));
                dgvDetials["colPrice", CurrRowIndex].Value = string.Format("{0:0.00}", decimal.Parse(txtPrice.Text));
                dgvDetials["colTotal", CurrRowIndex].Value = string.Format("{0:0.00}", decimal.Parse(txtQty.Text) * decimal.Parse(txtPrice.Text));
                Calc();
            }
        }

        private void bttnCancel_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.Visible = false;
            pnlReInv.Visible = true;
            btnSearchReInv_Click(null, null);
        }

        public void LoadDataReInv()
        {
            lblPageReInv.Visible = false;
            var inv = new InvoiceRepo().getAllReInvoiceByUserId(txtSearchReInv.Text, GeneralMembers.User.RoleId <= 2 ? 0 : GeneralMembers.User.Id, PageReInv);
            if (inv != null)
            {
                lblPageReInv.Visible = inv.Count == 20;
                int RowCount = inv.Count;
                for (int i = 0; i < RowCount; i++)
                {
                    int Row = dgvReInv.Rows.Add();
                    dgvReInv["colInvId", Row].Value = inv[i].Id;
                    dgvReInv["dataGridViewTextBoxColumn2", Row].Value = Row + 1;
                    dgvReInv["ColInvDate", Row].Value = inv[i].InvoiceDate;
                    dgvReInv["ColInvCode", Row].Value = inv[i].Code;
                    dgvReInv["ColInvTotal", Row].Value = inv[i].NetTotal;
                }
            }
        }

        private void btnSearchReInv_Click(object sender, EventArgs e)
        {
            PageReInv = 1;
            dgvReInv.Rows.Clear();
            LoadDataReInv();
        }

        private void txtSearchReInv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                btnSearchReInv_Click(null, null);
        }

        private void lblPageReInv_Click(object sender, EventArgs e)
        {
            PageReInv++;
            LoadDataReInv();
        }

        private void bottunOrg1_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.Visible = true;
            pnlReInv.Visible = false;
            NewFile(null, null);
            PageInv = 1;
            dgvTransactions.Rows.Clear();
            LoadData();
        }

        private void dgvReInv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            int CurrRowIndex = dgvReInv.CurrentRow.Index;
            MyOb = new InvoiceRepo().getById(int.Parse("0" + dgvReInv["colInvId", CurrRowIndex].Value));                
            txtQty.Text = string.Format("{0:0.00}", dgvDetials["colQty", CurrRowIndex].Value);
            txtPrice.Text = string.Format("{0:0.00}", dgvDetials["colPrice", CurrRowIndex].Value);
            Calc();
            tableLayoutPanel1.Visible = true;
            pnlReInv.Visible = false;
        }

        private void bttnDelete_Click(object sender, EventArgs e)
        {
            Delete(_MyOb.Id);
            NewFile(null, null);
            picSearch_Click(null, null);
        }

        private void Delete(int id)
        {
            try
            {
                string Title = GeneralMembers.Lang == "ar" ? "حذف ملف" : "Delete File";
                string Msg = GeneralMembers.Lang == "ar" ? "... هل تريد حذف الملف" : "Are you want Delete this file ...";
                if (MessageORG.Show(Title, Msg, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel) return;

                bttnDelete.Enabled = false;
                new InvoiceRepo().Remove(id);
                string Title1 = GeneralMembers.Lang == "ar" ? "حذف" : "Delete";
                string Msg1 = GeneralMembers.Lang == "ar" ? "تم حذف الملف بنجاح" : "The file has been deleted successfully";
                MessageORG.ShowConfirm(Title1, Msg1);
                Helper.Sound(Application.StartupPath, SoudType.DeleteClick);
                bttnDelete.Enabled = true;
            }
            catch (Exception)
            {
                string Title = GeneralMembers.Lang == "ar" ? "خطأ فى حذف" : "Errore Delete";
                string Msg = GeneralMembers.Lang == "ar" ? "لا يمكن حذف هذا الملف" : "Can't delete This File ...";
                MessageORG.Show(Title, Msg, CountButton.One, MessageBoxIcon.Error);
                bttnDelete.Enabled = true;
            }
        }

        private void dgvReInv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvReInv.Columns[e.ColumnIndex].Name == "ColInvDelete")
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
                int CurrRowIndex = dgvReInv.CurrentRow.Index;
                Delete(int.Parse("0" + dgvReInv["colInvId", CurrRowIndex].Value));
                PageReInv = 1;
                dgvReInv.Rows.Clear();
                LoadDataReInv();
            }
        }

        private void bottunOrg2_Click(object sender, EventArgs e)
        {

            int index = 0;
            List<int> ids = new List<int>();
            if (_MyOb.InvoiceItem != null)
                ids = _MyOb.InvoiceItem.Select(x => x.ItemId).ToList();
            var items = MessageORG.ShowItemSearch(ids);
            if (items != null && items.Count > 0)
            {
                Invoice invoice = _MyOb;

                if (invoice == null || invoice.InvoiceBaseId > 0)
                    invoice = new Invoice() { InvoiceDate = DateTime.Now, CreateDate = DateTime.Now, Code = new InvoiceRepo().GetMaxCode(2) };

                if (invoice.InvoiceItem == null)
                    invoice.InvoiceItem = new List<InvoiceItem>();

                index = invoice.InvoiceItem.Count;
                foreach (var item in items)
                {
                    index++;
                    invoice.InvoiceItem.Add(new InvoiceItem
                    {
                        Code = "" + index,
                        ItemId = item.Id,
                        Item = item,
                        UnitId = item.DefaultUnitId,
                        Quantity = 1,
                        Price = item.SellingPrice,
                        NetTotal = item.SellingPrice,
                        GrandTotal = item.SellingPrice,
                        Serialnumber = "" + index,
                        StoreId = 1
                    });
                }
                MyOb = invoice;
                Calc();
                if (dgvDetials.CurrentRow != null)
                {
                    txtQty.Text = string.Format("{0:0.00}", dgvDetials["colQty", dgvDetials.CurrentRow.Index].Value);
                    txtPrice.Text = string.Format("{0:0.00}", dgvDetials["colPrice", dgvDetials.CurrentRow.Index].Value);
                }
            }
        }

        public void SelectEdit(int Id)
        {
            MyOb = new InvoiceRepo().getById(Id);
            if (dgvDetials.CurrentRow == null) return;
            int CurrRowIndex = dgvDetials.CurrentRow.Index;
            txtQty.Text = string.Format("{0:0.00}", dgvDetials["colQty", CurrRowIndex].Value);
            txtPrice.Text = string.Format("{0:0.00}", dgvDetials["colPrice", CurrRowIndex].Value);
            Calc();
        }
    }
}