using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ORG.Tools.Desktop.Tools;
using ORGEntity;
using ORG.UIL.Desktop.Froms.Data;
using System.Media;
using Excel = Microsoft.Office.Interop.Excel;
using ORGRepository;

namespace ORG.UIL.Desktop
{
    public partial class toolCashier : UserControl
    {
        public event _Select SelectCategroy;
        public event _SelectOb SelectItemInvoice;
        public event _SearchEvent SearchItem;
        public event _evet _Refrash;
        public event _Pagging pagging;
        public int CurrPage = 1;

        bool EditItem = false, looked = false;

        public long TypeID { get; set; }
        public int ID { get; set; }
        string _UnitDescription = "";
        public string UnitDescription
        {
            get { return _UnitDescription; }
            set
            {
                _UnitDescription = value;
                pgtItems.UnitDescription = _UnitDescription;
            }
        }

        decimal _CountItems = 0;
        public decimal CountItems
        {
            get { return decimal.Parse(txtCountItems.Text); }
            set
            {
                _CountItems = value;
                txtCountItems.Text = string.Format("{0:0.00}", _CountItems);
            }
        }

        decimal _Discount = 0;
        public decimal Discount
        {
            get { return decimal.Parse(txtDiscount.Text); }
            set
            {
                _CountItems = value;
                txtDiscount.Text = string.Format("{0:0.00}", _Discount);
            }
        }

        decimal _Service = 0;
        public decimal Service
        {
            get { return decimal.Parse(txtService.Text); }
            set
            {
                _Service = value;
                txtService.Text = string.Format("{0:0.00}", _Service);
            }
        }

        decimal _Tax = 0;
        public decimal Tax
        {
            get { return decimal.Parse(txtTax.Text); }
            set
            {
                _Tax = value;
                txtTax.Text = string.Format("{0:0.00}", _Tax);
            }
        }

        decimal _Total = 0;
        public decimal Total
        {
            get { return decimal.Parse(txtTotal.Text); }
            set
            {
                _Total = value;
                txtTotal.Text = string.Format("{0:0.00}", _Total);
            }
        }

        decimal _Net = 0;
        public decimal Net
        {
            get { return decimal.Parse(txtNet.Text); }
            set
            {
                _Net = value;
                txtNet.Text = string.Format("{0:0.00}", _Net);
            }
        }

        List<Unit> _unitList = null;
        public List<Unit> UnitList
        {
            get { return _unitList; }
            set
            {
                _unitList = value;
                cmbUnit.ValueMember = "ID";
                cmbUnit.DisplayMember = "Name";
                cmbUnit.DataSource = _unitList;
            }
        }

        public Category category { get; set; }

        Item _CurrItemView = null;
        public Item CurrItemView
        {
            get { return _CurrItemView; }
            set
            {
                _CurrItemView = value;
                if (_CurrItemView != null)
                {
                    lblItemName.Text = _CurrItemView.Name;
                    txtQty.Text = string.Format("{0:0.00}", _CurrItemView.DefaultQuantity);
                    txtPrice.Text = string.Format("{0:0.00}", _CurrItemView.SellingPrice);
                    cmbUnit.SelectedValue = _CurrItemView.DefaultUnitId;
                }
                else
                {
                    lblItemName.Text = string.Empty;
                    txtQty.Text = "0.00";
                    txtPrice.Text = "0.00";
                }
            }
        }

        List<Category> _ItemGroup = null;
        public List<Category> ItemGroup
        {
            get { return _ItemGroup; }
            set
            {
                _ItemGroup = value;
                if (_ItemGroup != null)
                {
                    Application.DoEvents();
                    pgtGroupItems.FillItems<Category>(_ItemGroup);
                }
            }
        }

        List<Item> _ItemListViewDetail = null;
        public List<Item> ItemListViewDetail
        {
            get { return _ItemListViewDetail; }
            set { _ItemListViewDetail = value; }
        }

        bool _IsAdmin = false;
        public bool IsAdmin
        {
            get { return _IsAdmin; }
            set
            {
                _IsAdmin = value;
                panel5.Visible = panel28.Visible = value;
            }
        }

        List<Item> _ItemListView = null;
        public List<Item> ItemListView
        {
            get
            {
                return _ItemListView;
            }
            set
            {
                _ItemListView = value;
                Application.DoEvents();
                pgtItems.FillItems<Item>(_ItemListView);

            }
        }

        public toolCashier()
        {
            InitializeComponent();
            pgtGroupItems.WidthItem = pgtGroupItems.Width - 30;
            _unitList = new List<Unit>();
            _CurrItemView = new Item();
            _ItemGroup = new List<Category>();
            _ItemListViewDetail = new List<Item>();
            _ItemListView = new List<Item>();
        }

        private void txtItemSearchChange(object sender, EventArgs e)
        {
            string Code = txtItemSearch.Text, Qty = "";
            if (Code != "" && Code != null && Code.Length == 13 && Code[0] == '2' && Code[1] == '0')
            {
                Qty = Code.Substring(8, 5);
                Code = Code.Substring(2, 6);
                Qty = string.Format("{0:0.00}", decimal.Parse(Qty) / 100);
            }
        }

        private void dgvItemsCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (dgvDetials.Columns[e.ColumnIndex].Name == "colDelete")
            {
                int ItemId = int.Parse("0" + dgvDetials["colItemID", e.RowIndex].Value);
                dgvDetials.Rows.RemoveAt(e.RowIndex);
                if (_ItemListViewDetail == null)
                    _ItemListViewDetail = new List<Item>();
                var itemOb = _ItemListViewDetail.FirstOrDefault(x => x.Id == ItemId);
                if (itemOb != null)
                    _ItemListViewDetail.Remove(itemOb);
                Helper.Sound(Application.StartupPath, SoudType.DeleteClick);
            }
            else if (dgvDetials.Columns[e.ColumnIndex].Name == "colAddQty")
            {
                try
                {
                    if (!dgvDetials.Focused || dgvDetials.CurrentRow.Index < 0) return;
                    int CurrRowIndex = dgvDetials.CurrentRow.Index;
                    Helper.Sound(Application.StartupPath, SoudType.AddQuantity);
                    if (_ItemListViewDetail == null)
                        _ItemListViewDetail = new List<Item>();

                    int ItemId = int.Parse("" + dgvDetials["colItemID", CurrRowIndex].Value);
                    CurrItemView = _ItemListViewDetail.FirstOrDefault(x => x.Id == ItemId);
                    cmbUnit.SelectedValue = int.Parse("0" + dgvDetials["colUnitID", CurrRowIndex].Value);

                    decimal qty = decimal.Parse("" + dgvDetials["colQty", CurrRowIndex].Value);
                    ++qty;
                    dgvDetials["colQty", CurrRowIndex].Value = string.Format("{0:0.00}", qty);
                    txtQty.Text = string.Format("{0:0.00}", qty);
                    txtPrice.Text = string.Format("{0:0.00}", dgvDetials["colPrice", CurrRowIndex].Value);
                    dgvDetials["colTotal", CurrRowIndex].Value = string.Format("{0:0.00}", decimal.Parse(txtPrice.Text) * qty);
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
                    int CurrRowIndex = dgvDetials.CurrentRow.Index;
                    Helper.Sound(Application.StartupPath, SoudType.AddQuantity);
                    if (_ItemListViewDetail == null)
                        _ItemListViewDetail = new List<Item>();

                    int ItemId = int.Parse("" + dgvDetials["colItemID", CurrRowIndex].Value);
                    CurrItemView = _ItemListViewDetail.FirstOrDefault(x => x.Id == ItemId);
                    cmbUnit.SelectedValue = int.Parse("0" + dgvDetials["colUnitID", CurrRowIndex].Value);

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

        public virtual void pgtItems_SelectItem(object sender, EventArgs e)
        {
            if (sender == null) return;
            EditItem = false;
            CurrItemView = (Item)sender;
            cmbUnit.SelectedValue = CurrItemView.DefaultUnitId;
            lblDone_Click(null, null);
            txtQty.Focus();
            txtQty.SelectAll();
        }

        private void pgtGroupItems_SelectItem(object sender, EventArgs e)
        {
            try
            {
                if (sender == null) return;
                Category _ItemGroup = (Category)sender;
                string Code = txtItemSearch.Text, Qty = "";
                if (Code != "" && Code != null && Code.Length == 13 && Code[0] == '2' && Code[1] == '0')
                {
                    Qty = Code.Substring(8, 5);
                    Code = Code.Substring(2, 6);
                    Qty = string.Format("{0:0.00}", decimal.Parse(Qty) / 100);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lblDone_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblItemName.Text == "") return;
                Helper.Sound(Application.StartupPath, SoudType.GroupMouseSelect);
                AddItem();
                CurrItemView = null;
            }
            catch (Exception ex)
            {
            }
        }

        private void MiniOption_Click(object sender, EventArgs e)
        {
            frmPrinterSetting _frm_Message = new frmPrinterSetting();
            DialogResult _DialogResult = _frm_Message.ShowDialog();
            Application.OpenForms["frm_Main"].Activate();
            if (_Refrash != null)
                _Refrash();
        }

        private void cmbUnit_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (CurrItemView == null) return;
                long ID = long.Parse("0" + cmbUnit.SelectedValue);
                txtPrice.Text = string.Format("{0:0.00}", decimal.Parse(CurrItemView.SellingPrice.ToString()));
            }
            catch
            {

            }
        }

        private void dgvDetials_Click(object sender, EventArgs e)
        {

        }

        private void KeyMouseDown(object sender, MouseEventArgs e)
        {
            ((Label)sender).BackColor = Color.FromArgb(100, 150, 250);
        }

        private void KeyMouseUp(object sender, MouseEventArgs e)
        {
            ((Label)sender).BackColor = Color.White;
        }

        private void KeyMouseClick(object sender, MouseEventArgs e)
        {
            TextBox txt;
            if (txtPrice.Focused)
                txt = txtPrice;
            else
                txt = txtQty;

            int point = txt.SelectionStart;
            if (point == 0)
            {
                txt.Text = ((Label)sender).Text + ".00";
            }
            else
            {
                txt.Text = txt.Text.Insert(point, ((Label)sender).Text) + (txt.Text.Contains('.') ? "" : ".00");
            }
            txt.SelectionStart = point + 1;
        }

        private void Dot_Click(object sender, EventArgs e)
        {

        }

        private void Delete_Click(object sender, EventArgs e)
        {

        }

        private void txtItemSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                if (SearchItem != null)
                {
                    SearchItem(txtItemSearch.Text);
                    txtItemSearch.Focus();
                }
            if (e.KeyData == Keys.F2)
            {
                if (tableLayoutPanel1.ColumnStyles[0].Width == 0)
                    OpenItems(true);
                else
                    OpenItems(false);
            }

        }


        public void Calc()
        {
            decimal _total = 0, count = 0;
            for (int i = 0; i < dgvDetials.Rows.Count; i++)
            {
                if ("" + dgvDetials["colTotal", i].Value == "") continue;
                var _Value = decimal.Parse("" + dgvDetials["colTotal", i].Value);
                _total += _Value;

                if ("" + dgvDetials["colQty", i].Value == "") continue;
                var _count = decimal.Parse("" + dgvDetials["colQty", i].Value);
                count += _count;
            }
            Total = _total;
            Net = (_total + (_total * Tax / 100) + (_total * Service / 100)) - Discount;
            CountItems = count;
        }

        private void SerialRow()
        {
            int Count = 0;
            for (int i = 0; i < dgvDetials.Rows.Count; i++)
            {
                if (dgvDetials.Rows[i].IsNewRow || !dgvDetials.Rows[i].Visible) continue;
                dgvDetials["colSerial", i].Value = string.Format("{0:00}", ++Count);
            }
        }

        public int AddItem()
        {
            int Index = -1;
            if (_CurrItemView == null)
                _CurrItemView = new Item();
            if (_ItemListViewDetail == null)
                _ItemListViewDetail = new List<Item>();

            for (int i = 0; i < dgvDetials.RowCount; i++)
            {
                if (!dgvDetials.Rows[i].Visible) continue;
                if (int.Parse("0" + dgvDetials["colItemID", i].Value) == _CurrItemView.Id)
                {
                    dgvDetials.Rows[i].Selected = true; Index = i;
                }
            }

            if (Index < 0 && !_ItemListViewDetail.Any(e => e.Id == _CurrItemView.Id))
            {
                _ItemListViewDetail.Add(_CurrItemView);
                Index = dgvDetials.Rows.Add();
            }

            //if(_CurrItemView.DefaultQuantity > 0)
            //{
            //    txtQty.Text = "" + _CurrItemView.DefaultQuantity;
            //    //txtPrice.Text = "" + (decimal.Parse(txtPrice.Text) *  _CurrItemView.DefaultQuantity);
            //}

            dgvDetials["colItemID", Index].Value = _CurrItemView.Id;
            dgvDetials["colItemName", Index].Value = _CurrItemView.Name;
            dgvDetials["colUnitID", Index].Value = cmbUnit.SelectedValue;
            dgvDetials["colPrice", Index].Value = txtPrice.Text;
            dgvDetials["colQty", Index].Value = txtQty.Text;
            dgvDetials["colTotal", Index].Value = string.Format("{0:0.00}", decimal.Parse(txtPrice.Text) * decimal.Parse(txtQty.Text));

            Calc();
            dgvDetials.Rows[Index].Selected = true;


            txtQty.SelectAll();
            txtItemSearch.Focus();
            return Index;
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            int x = 0;
            if ((e.KeyChar == '.' && ((TextBox)sender).Text.Contains(".")))
            {
                e.Handled = true;
                return;
            }
            if ((!int.TryParse(e.KeyChar.ToString(), out x) && e.KeyChar != '.' && e.KeyChar != '\b'))
            {
                e.Handled = true;
                return;
            }
        }

        private void lblC_Click(object sender, EventArgs e)
        {

        }

        private void txtInvoice_TextChanged(object sender, EventArgs e)
        {
            if (!((Control)sender).Focused) return;
            if (((Control)sender).Text == "")
            {
                ClearGrid();
                dgvDetials.Columns["colLastQty"].Visible = false;
            }
        }

        private void ClearGrid()
        {
            for (int i = 0; i < dgvDetials.RowCount; i++)
            {
                dgvDetials.Rows.Remove(dgvDetials.Rows[i]);
                _ItemListViewDetail = new List<Item>();
            }
            Calc();
        }

        private void txtQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPrice_KeyDown(sender, e);
            }
        }

        private void txtPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lblDone_Click(null, null);
                txtItemSearch.Focus();
                txtItemSearch.SelectAll();
            }
        }

        private void dgvDetials_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetials.Columns[e.ColumnIndex].Name == "colDiscount" || dgvDetials.Columns[e.ColumnIndex].Name == "colTax")
            {
                dgvDetials["colNetAmount", e.RowIndex].Value = string.Format("{0:0.00}", decimal.Parse("0" + dgvDetials["colTotal", e.RowIndex].Value) - decimal.Parse("0" + dgvDetials["colDiscount", e.RowIndex].Value) + (decimal.Parse("0" + dgvDetials["colTax", e.RowIndex].Value) / 100 * decimal.Parse("0" + dgvDetials["colTotal", e.RowIndex].Value)));
                Calc();
            }
        }

        private void dgvDetials_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox txt = e.Control as TextBox;
            txt.KeyPress += new KeyPressEventHandler(txt1_KeyPress);
        }

        void txt1_KeyPress(object sender, KeyPressEventArgs e)
        {
            /* Code run. */
            if (e.KeyChar == (char)Keys.Enter)
            {
                dgvDetials.EndEdit();
                SendKeys.Send("{Tab}");
            }
        }

        private void txtQty_Leave(object sender, EventArgs e)
        {
            decimal x = 0;
            if (decimal.TryParse(((TextBox)sender).Text, out x))
            {
                ((TextBox)sender).Text = string.Format("{0:0.00}", decimal.Parse(((TextBox)sender).Text));
            }
            Calc();
        }


        private void textBox1_Leave(object sender, EventArgs e)
        {
            Calc();
        }


        private void pgtGroupItems_SelectITem_1(object sender, EventArgs e)
        {
            if (SelectCategroy != null)
            {
                pgtItems.CurrPage = CurrPage = 1;
                category = (Category)sender;
                SelectCategroy(((Category)sender).Id);
            }
        }

        private void pgtItems_SelectITem_1(object sender, EventArgs e)
        {
            if (pgtItems.ObjectSelected != null)
            {
                CurrItemView = (Item)pgtItems.ObjectSelected;
                AddItem();
                txtItemSearch.Focus();
            }
        }

        private void pgtItems_pagging(int page)
        {
            if (pagging != null)
                pagging(txtItemSearch.Text, ++CurrPage);
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (SearchItem != null)
                SearchItem(txtItemSearch.Text);
        }

        private void dgvDetials_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!dgvDetials.Focused || dgvDetials.CurrentRow.Index < 0) return;
                int CurrRowIndex = dgvDetials.CurrentRow.Index;
                Helper.Sound(Application.StartupPath, SoudType.ItemMouseEnter);
                if (_ItemListViewDetail == null)
                    _ItemListViewDetail = new List<Item>();

                int ItemId = int.Parse("" + dgvDetials["colItemID", CurrRowIndex].Value);
                CurrItemView = _ItemListViewDetail.FirstOrDefault(x => x.Id == ItemId);
                cmbUnit.SelectedValue = int.Parse("0" + dgvDetials["colUnitID", CurrRowIndex].Value);
                txtPrice.Text = string.Format("{0:0.00}", dgvDetials["colPrice", CurrRowIndex].Value);
                txtQty.Text = string.Format("{0:0.00}", dgvDetials["colQty", CurrRowIndex].Value);
            }
            catch
            {
            }
        }

        private void txtItemSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '+' || e.KeyChar == '-' || e.KeyChar == '=')
            {
                e.Handled = true;
                return;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Helper.Sound(Application.StartupPath, SoudType.MenuMouseSelect);
            if (tableLayoutPanel1.ColumnStyles[0].Width == 0)
                OpenItems(true);
            else
                OpenItems(false);

        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (CurrItemView == null || CurrItemView.Id == 0)
                return false;

            int CurrRowIndex = 0, ItemId = 0;
            decimal qty = 0;
            switch (keyData)
            {
                case Keys.Oemplus:                    
                    if (dgvDetials.CurrentRow.Index < 0) return false;
                    CurrRowIndex = dgvDetials.CurrentRow.Index;
                    Helper.Sound(Application.StartupPath, SoudType.AddQuantity);
                    if (_ItemListViewDetail == null)
                        _ItemListViewDetail = new List<Item>();

                    ItemId = int.Parse("" + dgvDetials["colItemID", CurrRowIndex].Value);
                    CurrItemView = _ItemListViewDetail.FirstOrDefault(x => x.Id == ItemId);
                    cmbUnit.SelectedValue = int.Parse("0" + dgvDetials["colUnitID", CurrRowIndex].Value);

                    qty = decimal.Parse("" + dgvDetials["colQty", CurrRowIndex].Value);
                    ++qty;
                    dgvDetials["colQty", CurrRowIndex].Value = string.Format("{0:0.00}", qty);
                    txtQty.Text = string.Format("{0:0.00}", qty);
                    txtPrice.Text = string.Format("{0:0.00}", dgvDetials["colPrice", CurrRowIndex].Value);
                    dgvDetials["colTotal", CurrRowIndex].Value = string.Format("{0:0.00}", decimal.Parse(txtPrice.Text) * qty);
                    Calc();
                    txtItemSearch.Clear();
                    break;
                case Keys.OemMinus:
                    if (dgvDetials.CurrentRow.Index < 0) return false;
                    CurrRowIndex = dgvDetials.CurrentRow.Index;
                    Helper.Sound(Application.StartupPath, SoudType.AddQuantity);
                    if (_ItemListViewDetail == null)
                        _ItemListViewDetail = new List<Item>();

                    ItemId = int.Parse("" + dgvDetials["colItemID", CurrRowIndex].Value);
                    CurrItemView = _ItemListViewDetail.FirstOrDefault(x => x.Id == ItemId);
                    cmbUnit.SelectedValue = int.Parse("0" + dgvDetials["colUnitID", CurrRowIndex].Value);

                    qty = decimal.Parse("" + dgvDetials["colQty", CurrRowIndex].Value);
                    --qty;
                    if (qty > 0)
                    {
                        dgvDetials["colQty", CurrRowIndex].Value = string.Format("{0:0.00}", qty);
                        txtQty.Text = string.Format("{0:0.00}", qty);
                        txtPrice.Text = string.Format("{0:0.00}", dgvDetials["colPrice", CurrRowIndex].Value);
                        dgvDetials["colTotal", CurrRowIndex].Value = string.Format("{0:0.00}", decimal.Parse(txtPrice.Text) * qty);
                    }
                    Calc();
                    txtItemSearch.Text = "";
                    break;
                case Keys.F2:
                    if (!txtItemSearch.Focused)
                        if (tableLayoutPanel1.ColumnStyles[0].Width == 0)
                            OpenItems(true);
                        else
                            OpenItems(false);
                    break;

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dgvDetials_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetials.Columns[e.ColumnIndex].Name == "colDelete" || 
                dgvDetials.Columns[e.ColumnIndex].Name == "colAddQty" ||
                dgvDetials.Columns[e.ColumnIndex].Name == "colSubQty")
                return;
            int CurrRowIndex = dgvDetials.CurrentRow.Index;
            if (SelectItemInvoice != null)
            {
                Helper.Sound(Application.StartupPath, SoudType.MenuMouseSelect);
                var ItemId = int.Parse("" + dgvDetials["colItemID", CurrRowIndex].Value);
                var item = SelectItemInvoice(ItemId);
                if (item != null)
                {
                    CurrItemView = _ItemListViewDetail.FirstOrDefault(x => x.Id == ItemId);
                    CurrItemView.Name = ((Item)item).Name;
                    CurrItemView.Barcode = ((Item)item).Barcode;
                    CurrItemView.Code = ((Item)item).Code;
                    CurrItemView.SellingPrice = ((Item)item).SellingPrice;
                    CurrItemView.CategoryId = ((Item)item).CategoryId;
                    CurrItemView.DefaultUnitId = ((Item)item).DefaultUnitId;
                    CurrItemView.DefaultQuantity = ((Item)item).DefaultQuantity;

                    dgvDetials["colItemName", CurrRowIndex].Value = ((Item)item).Name;
                    dgvDetials["colPrice", CurrRowIndex].Value = ((Item)item).SellingPrice;
                    txtPrice.Text = string.Format("{0:0.00}", ((Item)item).SellingPrice);
                    cmbUnit.SelectedValue = ((Item)item).DefaultUnitId;
                    dgvDetials["colTotal", CurrRowIndex].Value = string.Format("{0:0.00}", ((Item)item).SellingPrice * decimal.Parse("" + dgvDetials["colQty", CurrRowIndex].Value));

                    Calc();
                }
            }

        }

        private void txtRecalc(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Calc();
        }
     
        private void lblDone_MouseEnter(object sender, EventArgs e)
        {
            lblDone.BackColor = Color.Green;
        }

        private void lblDone_MouseLeave(object sender, EventArgs e)
        {
            lblDone.BackColor = Color.FromArgb(100, 150, 250);
        }

        private void bottunOrg4_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(Application.StartupPath + @"/Template/ItemTemplate.csv");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save Template File";
            saveFileDialog1.FileName = "Item Template";
            saveFileDialog1.Filter = "Excel files csv (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                xlWorkbook.SaveCopyAs(saveFileDialog1.FileName);
            }
            xlWorkbook.Close();
        }

        private void bottunOrg5_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;

            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            OpenFileDialog1.Title = "Excel File to Edit";
            OpenFileDialog1.FileName = "";
            OpenFileDialog1.Filter = "Excel File|*.csv;*.xlsx;*.xls";

            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(OpenFileDialog1.FileName);
                xlWorkSheet = xlWorkBook.Worksheets[1];

                for (int iRow = 2; iRow <= xlWorkSheet.Rows.Count; iRow++)
                {
                    if ("" + xlWorkSheet.Cells[iRow, 3].value == "")
                        break;

                    Category cat = new CategoryRepo().getByName(xlWorkSheet.Cells[iRow, 1].value + "");
                    if (cat == null || cat.Id == 0)
                        cat = new CategoryRepo().Save(new Category() { Code = "1", Name = xlWorkSheet.Cells[iRow, 1].value + "" });

                    Unit un = new UnitRepo().getByName(xlWorkSheet.Cells[iRow, 5].value + "");
                    if (un == null || un.Id == 0)
                        un = new UnitRepo().Save(new Unit() { Name = xlWorkSheet.Cells[iRow, 5].value + "" });

                    decimal price = 0;
                    decimal.TryParse(xlWorkSheet.Cells[iRow, 6].value + "", out price);

                    Item it = new ItemRepo().getByName(xlWorkSheet.Cells[iRow, 3].value + "");
                    if (it == null || it.Id == 0)
                        new ItemRepo().Save(new Item()
                        {
                            CategoryId = cat.Id,
                            Code = xlWorkSheet.Cells[iRow, 2].value + "",
                            Name = xlWorkSheet.Cells[iRow, 3].value + "",
                            Barcode = xlWorkSheet.Cells[iRow, 4].value + "",
                            DefaultUnitId = un.Id,
                            PurchasePrice = price,
                            SellingPrice = price,
                            DefaultQuantity = 1,
                            IsSerialized = false,
                            ReorderPoint = 0,
                            TaxPercent = 0
                        });
                }

                ItemGroup = new CategoryRepo().getAllCategories();
                if (ItemGroup != null && ItemGroup.Count > 0)
                {
                    category = ItemGroup[0];
                    ItemListView = new ItemRepo().GetAllGroupId(ItemGroup[0].Id, "");
                }
                xlWorkBook.Close();
                xlApp.Quit();
            }
        }

        private void bottunOrg3_Click(object sender, EventArgs e)
        {
            ItemInvoice _frm_Message = new ItemInvoice(GeneralMembers.Lang == "ar" ? "صنف جديد" : "New Item");
            DialogResult _DialogResult = _frm_Message.ShowDialog();
            Application.OpenForms["frm_Main"].Activate();
            if (_DialogResult == DialogResult.OK)
            {
                ItemGroup = new CategoryRepo().getAllCategories();
                if (ItemGroup != null && ItemGroup.Count > 0)
                {
                    category = ItemGroup[0];
                    ItemListView = new ItemRepo().GetAllGroupId(ItemGroup[0].Id, "");
                }
            }           
        }

        private void bottunOrg6_Click(object sender, EventArgs e)
        {
            ItemGroupInvoice _frm_Message = new ItemGroupInvoice(GeneralMembers.Lang == "ar" ? "مجموعة صنف جديد" : "New Category");
            DialogResult _DialogResult = _frm_Message.ShowDialog();
            Application.OpenForms["frm_Main"].Activate();
            if (_DialogResult == DialogResult.OK)
            {
                ItemGroup = new CategoryRepo().getAllCategories();
                if (ItemGroup != null && ItemGroup.Count > 0)
                {
                    category = ItemGroup[0];
                    ItemListView = new ItemRepo().GetAllGroupId(ItemGroup[0].Id, "");
                }
            }
        }

        private void toolCashier_Load(object sender, EventArgs e)
        {

        }

        public void OpenItems(bool open)
        {

            if (open)
            {
                tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Percent;
                tableLayoutPanel1.ColumnStyles[0].Width = 65;
                tableLayoutPanel1.ColumnStyles[2].SizeType = SizeType.Percent;
                tableLayoutPanel1.ColumnStyles[2].Width = 35;
                tableLayoutPanel3.Visible = true;
                tableLayoutPanel5.Visible = true;
                lblDone.Visible = true;
                label1.Text = "<<";
            }
            else
            {
                tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Percent;
                tableLayoutPanel1.ColumnStyles[0].Width = 0;
                tableLayoutPanel1.ColumnStyles[2].SizeType = SizeType.Percent;
                tableLayoutPanel1.ColumnStyles[2].Width = 100;
                tableLayoutPanel3.Visible = false;
                tableLayoutPanel5.Visible = false;
                lblDone.Visible = false;
                label1.Text = ">>";
            }
        }
    }
}