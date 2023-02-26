using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ORGRepository;
using ORGEntity;
using System.Threading;
using Microsoft.Reporting.WebForms;
using ORG.UIL.Desktop.Class;
using System.Drawing.Printing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;
using Microsoft.Reporting.WinForms;
//using Microsoft.PointOfService;
using System.IO.Ports;
using System.Linq;
using ORG.UIL.Desktop.Froms.Data;
using ORG.UIL.Desktop.Untility;

namespace ORG.UIL.Desktop
{
    public partial class frm_Invoice : frm_TemplateTransation
    {
        List<Invoice> _InvoiceListView;
        public List<Invoice> InvoiceListView
        {
            get
            {
                return _InvoiceListView;
            }
            set
            {
                _InvoiceListView = value;
                if (_InvoiceListView != null)
                    toolSearchInfo1.InvoiceView = _InvoiceListView;
                else
                    toolSearchInfo1.InvoiceView = null;
            }
        }

        Invoice _MyObject;
        public Invoice MyObject
        {
            get
            {
                if (_MyObject == null)
                    _MyObject = new Invoice();

                _MyObject.TypeId = 1;
                _MyObject.Id = (int)toolCashier1.ID;
                if (_MyObject.Id == 0)
                {
                    _MyObject.Code = new InvoiceRepo().GetMaxCode(1);
                    _MyObject.InvoiceDate = DateTime.Now;
                }
                _MyObject.StoreId = 1;
                _MyObject.DealerId = 1;
                _MyObject.GrandTotal = toolCashier1.Total;
                _MyObject.Discount = toolCashier1.Discount;
                _MyObject.Tax = toolCashier1.Tax;
                _MyObject.Service = toolCashier1.Service;
                _MyObject.NetTotal = toolCashier1.Net;
                _MyObject.InvoiceItem = new List<InvoiceItem>();
                if (_MyObject.Id == 0)
                    _MyObject.UserId = GeneralMembers.User.Id;

                for (int i = 0; i < toolCashier1.dgvDetials.Rows.Count; i++)
                {
                    int ID = int.Parse("0" + toolCashier1.dgvDetials["colID", i].Value);
                    InvoiceItem _invoiceD = new InvoiceItem();
                    _invoiceD.Id = int.Parse("0" + toolCashier1.dgvDetials["colID", i].Value);
                    _invoiceD.Code = string.Format("{0:00}", i + 1);
                    _invoiceD.ItemId = int.Parse("0" + toolCashier1.dgvDetials["colItemID", i].Value);
                    _invoiceD.UnitId = int.Parse("0" + toolCashier1.dgvDetials["colUnitID", i].Value);
                    _invoiceD.Quantity = decimal.Parse("0" + toolCashier1.dgvDetials["colQty", i].Value);
                    _invoiceD.Price = decimal.Parse("0" + toolCashier1.dgvDetials["colPrice", i].Value);
                    _invoiceD.NetTotal = decimal.Parse("0" + toolCashier1.dgvDetials["colTotal", i].Value);
                    _MyObject.InvoiceItem.Add(_invoiceD);
                }
                return _MyObject;
            }
            set
            {
                _MyObject = value;
                if (_MyObject == null || _MyObject.Id == 0)
                    _MyObject = new Invoice() { InvoiceDate = DateTime.Now, CreateDate = DateTime.Now, Tax = GeneralMembers.User.Tax, Service = GeneralMembers.User.Service };

                toolCashier1.TypeID = 1;
                toolCashier1.ID = _MyObject.Id;
                toolCashier1.Total = _MyObject.GrandTotal;
                toolCashier1.Discount = _MyObject.Discount;
                toolCashier1.Tax = _MyObject.Tax;
                toolCashier1.Service = _MyObject.Service;
                toolCashier1.Net = _MyObject.NetTotal;
                toolCashier1.ItemListViewDetail = new List<Item>();
                toolCashier1.dgvDetials.Rows.Clear();
                if (_MyObject.InvoiceItem == null)
                    _MyObject.InvoiceItem = new List<InvoiceItem>();
                toolCashier1.CountItems = _MyObject.InvoiceItem.Count;

                for (int i = 0; i < _MyObject.InvoiceItem.Count; i++)
                {
                    int row = toolCashier1.dgvDetials.Rows.Add();
                    toolCashier1.dgvDetials["colID", row].Value = _MyObject.InvoiceItem[i].Id;
                    toolCashier1.dgvDetials["colSerial", row].Value = _MyObject.InvoiceItem[i].Code;
                    toolCashier1.dgvDetials["colItemID", row].Value = _MyObject.InvoiceItem[i].ItemId;
                    toolCashier1.dgvDetials["colItemName", row].Value = _MyObject.InvoiceItem[i].ItemName;
                    toolCashier1.dgvDetials["colUnitID", row].Value = _MyObject.InvoiceItem[i].UnitId;
                    toolCashier1.dgvDetials["colQty", row].Value = string.Format("{0:0.000}", _MyObject.InvoiceItem[i].Quantity);
                    toolCashier1.dgvDetials["colPrice", row].Value = string.Format("{0:0.00}", _MyObject.InvoiceItem[i].Price);
                    toolCashier1.dgvDetials["colTotal", row].Value = string.Format("{0:0.00}", _MyObject.InvoiceItem[i].NetTotal);
                    toolCashier1.CountItems += _MyObject.InvoiceItem[i].Quantity;
                    toolCashier1.ItemListViewDetail.Add(_MyObject.InvoiceItem[i].Item);
                }

                UseSave = UsePrint = UseDelete = _MyObject.Id > 0;
            }
        }

        List<Category> _ItemGroup;
        public List<Category> ItemGroup
        {
            get
            {
                return _ItemGroup;
            }
            set
            {
                _ItemGroup = value;
                if (_ItemGroup != null)
                    toolCashier1.ItemGroup = _ItemGroup;
            }
        }

        List<Item> _ItemListView;
        public List<Item> ItemListView
        {
            get
            {
                return _ItemListView;
            }
            set
            {
                _ItemListView = value;
                toolCashier1.ItemListView = _ItemListView;
            }
        }

        List<Unit> _unitList;
        public List<Unit> unitList
        {
            get
            {
                return _unitList;
            }
            set
            {
                _unitList = value;
                toolCashier1.UnitList = _unitList;
            }
        }


        List<PrinterSetting> _printerSettings;
        public List<PrinterSetting> printerSettings
        {
            get
            {
                return _printerSettings;
            }
            set
            {
                _printerSettings = value;
            }
        }

        public frm_Invoice()
        {
            InitializeComponent();
        }

        private void frm_Invoice_Load(object sender, EventArgs e)
        {
            // explorer = new PosExplorer(this);
            //Microsoft.PointOfService.DeviceInfo device = new PosExplorer(this).GetDevice("CashDrawer", "WASPCD");
            //myCashDrawer = (CashDrawer)new PosExplorer(this).CreateInstance(device);
            if (AddNew)
                NewFile(sender, e);
            toolCashier1.IsAdmin = GeneralMembers.User.RoleId <= 2;
        }

        public override void NewFile(object sender, EventArgs e)
        {
            try
            {
                MyObject = new Invoice() { Code = new InvoiceRepo().GetMaxCode(1), InvoiceDate = DateTime.Now, CreateDate = DateTime.Now };
                toolCashier1.CurrItemView = null;
                base.NewFile(sender, e);
                toolCashier1.txtItemSearch.Focus();
            }
            catch { }
        }

        public override void SaveFile(object sender, EventArgs e)
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
                    AsyncDatabaseWorker.Stop = true;
                    MyObject = new InvoiceRepo().Save(MyObject);
                    string Title1 = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                    string Msg1 = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                    MessageORG.ShowConfirm(Title1, Msg1);
                    Helper.Sound(Application.StartupPath, SoudType.SaveInvoice);
                    bttnSave.Enabled = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bttnSave.Enabled = true;
            }
        }

        public override void NewSaveFile(object sender, EventArgs e)
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
                    AsyncDatabaseWorker.Stop = true;
                    new InvoiceRepo().Save(MyObject);
                    string Title1 = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                    string Msg1 = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                    MessageORG.ShowConfirm(Title1, Msg1);
                    MyObject = new Invoice() { Code = new InvoiceRepo().GetMaxCode(1), InvoiceDate = DateTime.Now, CreateDate = DateTime.Now };
                    toolCashier1.CurrItemView = null;
                    //toolSearchInfo1.CurrPage = 1;
                    //InvoiceListView = new InvoiceRepo().getAll(toolSearchInfo1.txtSearch.Text, 1);
                    base.NewSaveFile(sender, e);
                    NewFile(sender, e);
                    Helper.Sound(Application.StartupPath, SoudType.SaveInvoice);
                    bttnSaveNew.Enabled = true;

                }
            }
            catch { }
        }

        public override void SavePrintFile(object sender, EventArgs e)
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
                    bttnSavePrint.Enabled = false;
                    AsyncDatabaseWorker.Stop = true;
                    MyObject = new InvoiceRepo().Save(MyObject);
                    string Title1 = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                    string Msg1 = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                    MessageORG.ShowConfirm(Title1, Msg1);
                    PrintFile(sender, e);
                    Helper.Sound(Application.StartupPath, SoudType.SaveInvoice);
                    //toolCashier1.CurrItemView = null;
                    //toolSearchInfo1.CurrPage = 1;
                    //InvoiceListView = new InvoiceRepo().getAll(toolSearchInfo1.txtSearch.Text, 1);
                    //base.SavePrintFile(sender, e);
                    bttnSavePrint.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bttnSavePrint.Enabled = true;
            }
        }

        public override void toolCashier1__Refrash()
        {
            printerSettings = new PrinterSettingRepo().getAll();
        }

        public override void NewSavePrintFile(object sender, EventArgs e)
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
                    bttnNewSavePrint.Enabled = false;
                    AsyncDatabaseWorker.Stop = true;
                    var ob = new InvoiceRepo().Save(MyObject);
                    string Title1 = GeneralMembers.Lang == "ar" ? "الحفظ" : "Save";
                    string Msg1 = GeneralMembers.Lang == "ar" ? "تم حفط الملف بنجاح" : "The file has been saved successfully";
                    MessageORG.ShowConfirm(Title1, Msg1);
                    Thread th = new Thread(delegate () { DoPrint(ob); });
                    th.Start();

                    //toolSearchInfo1.CurrPage = 1;
                    //InvoiceListView = new InvoiceRepo().getAll(toolSearchInfo1.txtSearch.Text, 1);
                    //base.NewSavePrintFile(sender, e);
                    NewFile(null, null);
                    Helper.Sound(Application.StartupPath, SoudType.SaveInvoice);
                    bttnNewSavePrint.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bttnNewSavePrint.Enabled = true;
            }
        }

        public override void PrintFile(object sender, EventArgs e)
        {
            try
            {
                if (_MyObject == null || _MyObject.Id == 0) return;
                bttnPrint.Enabled = false;
                Thread th = new Thread(delegate () { DoPrint(MyObject); });
                th.Start();
                base.PrintFile(sender, e);
                bttnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bttnPrint.Enabled = true;
            }
        }

        public override void DeleteFile(object sender, EventArgs e)
        {
            try
            {
                string Title = GeneralMembers.Lang == "ar" ? "حذف ملف" : "Delete File";
                string Msg = GeneralMembers.Lang == "ar" ? "... هل تريد حذف الملف" : "Are you want Delete this file ...";
                if (MessageORG.Show(Title, Msg, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel) return;

                bttnDelete.Enabled = false;
                AsyncDatabaseWorker.Stop = true;
                new InvoiceRepo().Remove(toolCashier1.ID);
                string Title1 = GeneralMembers.Lang == "ar" ? "حذف" : "Delete";
                string Msg1 = GeneralMembers.Lang == "ar" ? "تم حذف الملف بنجاح" : "The file has been deleted successfully";
                MessageORG.ShowConfirm(Title1, Msg1);
                NewFile(null, null);
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

        public override void CancelFile(object sender, EventArgs e)
        {
            toolCashier1.CurrItemView = null;
            Application.DoEvents();
            toolSearchInfo1.CurrPage = 1;
            InvoiceListView = new InvoiceRepo().getAllByUserId(toolSearchInfo1.txtSearch.Text, GeneralMembers.User.RoleId <= 2 ? 0 : GeneralMembers.User.Id, 1);
            base.CancelFile(sender, e);
        }

        private void toolSearchInfo1_SearchEvent(string textSearch)
        {
            InvoiceListView = new InvoiceRepo().getAllByUserId(textSearch, GeneralMembers.User.RoleId <= 2 ? 0 : GeneralMembers.User.Id, 1);
        }

        private void toolSearchInfo1_pagging(string textSearch, int page)
        {
            InvoiceListView = new InvoiceRepo().getAllByUserId(textSearch, GeneralMembers.User.RoleId <= 2 ? 0 : GeneralMembers.User.Id, page);
        }

        public void toolSearchInfo1_SelectEdit(int Id)
        {
            MyObject = new InvoiceRepo().getById(Id);
            AddNew = false;
            OpenClose = true;
        }

        private bool Validation()
        {
            if (toolCashier1.dgvDetials.RowCount < 1) return false;

            for (int i = 0; i < toolCashier1.dgvDetials.RowCount; i++)
            {
                if (int.Parse("0" + toolCashier1.dgvDetials["colUnitID", i].Value) == 0)
                    return false;
                if (decimal.Parse("0" + toolCashier1.dgvDetials["colQty", i].Value) == 0)
                    return false;
            }

            return true;
        }

        private void toolCashier1_SelectCategroy(int Id)
        {
            ItemListView = new ItemRepo().GetAllGroupId(Id, toolCashier1.txtItemSearch.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            toolSearchInfo1.CurrPage = 1;
            Application.DoEvents();
            printerSettings = new PrinterSettingRepo().getAll();
            if (GeneralMembers.User.TypeSystem == 1)
            {
                Application.DoEvents();
                //InvoiceListView = new InvoiceRepo().getAll("", 1);
                toolCashier1.OpenItems(true);
            }
            else if (GeneralMembers.User.TypeSystem == 2)
            {
                Application.DoEvents();
                //InvoiceListView = new InvoiceRepo().getAll("", 1);
                toolCashier1.OpenItems(false);
            }

            toolCashier1.Visible = true;
            toolCashier1.txtItemSearch.Focus();
            Application.DoEvents();
            toolCashier1.ItemGroup = new CategoryRepo().getAllCategories();
            Application.DoEvents();
            unitList = new UnitRepo().getAll();
        }

        public void DoPrint(Invoice inv)
        {
            try
            {
                List<InvoiceReportMv> invoiceReportMv = new List<InvoiceReportMv>();
                foreach (var item in inv.InvoiceItem)
                    invoiceReportMv.Add(new InvoiceReportMv()
                    {
                        TitleCompany = GeneralMembers.User.CompanyName,
                        Address = GeneralMembers.User.Address,
                        Telephone = GeneralMembers.User.Telephone,
                        Code = inv.Code,
                        Date = inv.InvoiceDate,
                        ItemName = new ItemRepo().getById(item.ItemId).Name,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Total = item.NetTotal,
                        Discount = inv.GrandTotal * (inv.Discount / 100.00m),
                        Tax = inv.GrandTotal * (inv.Tax / 100.00m),
                        Service = inv.GrandTotal * (inv.Service / 100.00m),
                        GrandTotal = inv.NetTotal,
                        EmpName = new UserRepo().getById(inv.UserId)?.UserName
                    });

                LocalReport report = new LocalReport();
                report.DataSources.Add(new ReportDataSource("DataSet1", invoiceReportMv));

                if (printerSettings != null && printerSettings.Count > 0)
                    foreach (var printerSetting in printerSettings)
                    {
                        report.ReportPath = Application.StartupPath + @"\InvoicePrint\Invoice_" + (printerSetting.TypePrinter + 1) + (printerSetting.PrintLang == 1 ? "" : "_en") + ".rdlc";
                        PrintToPrinter(report, printerSetting.PrinterName);
                    }
                else
                {
                    report.ReportPath = Application.StartupPath + @"\InvoicePrint\Invoice_1" + (GeneralMembers.User.PrintLang == 1 ? "" : "_en") + ".rdlc";
                    PrintToPrinter(report, new PrinterSettings().PrinterName);
                }
                OpenCashDrawer();
            }
            catch (Exception ex)
            {

            }
        }

        private static List<Stream> m_streams;
        private static int m_currentPageIndex = 0;

        public void PrintToPrinter(LocalReport report, string printerName = "")
        {
            string deviceInfo =
            @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>3in</PageWidth>
                <PageHeight>8.3in</PageHeight>
                <MarginTop>0in</MarginTop>
                <MarginLeft>0.1in</MarginLeft>
                <MarginRight>0.1in</MarginRight>
                <MarginBottom>0in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();

            report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            // ---------------
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            if (printerName != "")
                printDoc.PrinterSettings.PrinterName = printerName;

            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }

        public Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        public void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }




        public void OpenCashDrawer()
        {
            //myCashDrawer.Open();
            //myCashDrawer.Claim(1000);
            //myCashDrawer.DeviceEnabled = true;
            //myCashDrawer.OpenDrawer();
            //myCashDrawer.DeviceEnabled = false;
            //myCashDrawer.Release();
            //myCashDrawer.Close();
            try
            {
                Encoding enc = Encoding.Unicode;
                SerialPort sp = new SerialPort();
                sp.PortName = "COM2";

                sp.Encoding = enc;
                sp.BaudRate = 38400;
                sp.Parity = System.IO.Ports.Parity.None;
                sp.DataBits = 8;
                sp.StopBits = System.IO.Ports.StopBits.One;
                sp.DtrEnable = true;
                if (!sp.IsOpen)
                    sp.Open();
                sp.Write(char.ConvertFromUtf32(28699) + char.ConvertFromUtf32(9472) + char.ConvertFromUtf32(3365));
                sp.Close();
            }
            catch (Exception)
            {

            }
        }

        public static void DisposePrint()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }

        private void toolCashier1_pagging(string textSearch, int page)
        {
            ItemListView = new ItemRepo().GetAllGroupId(toolCashier1.category.Id, textSearch, page);
        }

        private void toolCashier1_SearchItem(string textSearch)
        {
            if ("" + textSearch != "")
            {
                string code = textSearch;
                decimal qty = 0;

                if ("" + GeneralMembers.User.ElectronicScaleCode != "" && textSearch.StartsWith(GeneralMembers.User.ElectronicScaleCode))
                {
                    if (("" + textSearch).Length == 13)
                    {
                        code = textSearch.Replace(GeneralMembers.User.ElectronicScaleCode, "").Substring(0, 5);
                        qty = decimal.Parse(textSearch.Remove(0, 7).Substring(0, 5)) / 1000;

                        var item = new ItemRepo().GetByBarcode(code);
                        if (item != null && item.Id > 0)
                        {
                            toolCashier1.CurrItemView = item;
                            if (qty > 0)
                            {
                                toolCashier1.txtQty.Text = "" + qty;
                            }
                            toolCashier1.AddItem();
                            toolCashier1.txtItemSearch.Text = "";

                            return;
                        }
                    }
                }



                var items = new ItemRepo().GetAllGroupId(0, code);
                if (items != null && items.Count > 0)
                {
                    if (items.Count == 1)
                    {
                        toolCashier1.CurrItemView = items[0];
                        if (qty > 0)
                        {
                            toolCashier1.txtQty.Text = "" + qty;
                        }
                        toolCashier1.AddItem();
                        toolCashier1.txtItemSearch.Text = "";
                    }
                    else
                    {
                        toolCashier1.ItemGroup = new CategoryRepo().GetAllByItemName(code);
                        if (toolCashier1.ItemGroup != null && toolCashier1.ItemGroup.Count > 0)
                        {
                            toolCashier1.category = toolCashier1.ItemGroup[0];
                            ItemListView = new ItemRepo().GetAllGroupId(toolCashier1.ItemGroup[0].Id, code);
                        }
                    }
                }
                else
                {


                    ItemInvoice _frm_Message = new ItemInvoice(GeneralMembers.Lang == "ar" ? "صنف جديد" : "New Item", toolCashier1.txtItemSearch.Text);
                    DialogResult _DialogResult = _frm_Message.ShowDialog();
                    Application.OpenForms["frm_Main"].Activate();
                    if (_DialogResult == DialogResult.OK)
                    {
                        toolCashier1.CurrItemView = _frm_Message.MyObject;
                        toolCashier1.AddItem();
                        toolCashier1.txtItemSearch.Text = "";
                    }
                    else
                    {
                        ItemGroup = null;
                        ItemListView = null;
                    }
                }

            }
            else
            {
                toolCashier1.ItemGroup = new CategoryRepo().getAllCategories();
                if (toolCashier1.ItemGroup != null && toolCashier1.ItemGroup.Count > 0)
                {
                    toolCashier1.category = toolCashier1.ItemGroup[0];
                    ItemListView = new ItemRepo().GetAllGroupId(toolCashier1.ItemGroup[0].Id, textSearch);
                }
            }

            Application.DoEvents();
            unitList = new UnitRepo().getAll();

        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F10:
                case Keys.S | Keys.Control:
                    NewSavePrintFile(null, null);
                    break;
                case Keys.F9:
                case Keys.W | Keys.Control:
                    SavePrintFile(null, null);
                    break;
                case Keys.F3:
                case Keys.P | Keys.Control:
                    if (MyObject != null && MyObject.Id > 0)
                        PrintFile(null, null);
                    else
                        SavePrintFile(null, null);
                    break;
                case Keys.N | Keys.Control:
                    NewFile(null, null);
                    break;
                case Keys.D | Keys.Control:
                    DeleteFile(null, null);
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private object toolCashier1_SelectItemInvoice(int Id)
        {
            ItemInvoice _frm_Message = new ItemInvoice(GeneralMembers.Lang == "ar" ? "تعديل صنف" : "Edit Item", new ItemRepo().getById(Id));
            DialogResult _DialogResult = _frm_Message.ShowDialog();
            Application.OpenForms["frm_Main"].Activate();
            return _frm_Message.MyObject;
        }

        private void toolSearchInfo1_PrintInvoice(int Id)
        {
            DoPrint(new InvoiceRepo().getById(Id));
        }

        private void toolSearchInfo1_DeleteInvoice(int Id)
        {
            try
            {
                string Title = GeneralMembers.Lang == "ar" ? "حذف ملف" : "Delete File";
                string Msg = GeneralMembers.Lang == "ar" ? "... هل تريد حذف الملف" : "Are you want Delete this file ...";
                if (MessageORG.Show(Title, Msg, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel) return;

                bttnDelete.Enabled = false;
                new InvoiceRepo().Remove(Id);
                string Title1 = GeneralMembers.Lang == "ar" ? "حذف" : "Delete";
                string Msg1 = GeneralMembers.Lang == "ar" ? "تم حذف الملف بنجاح" : "The file has been deleted successfully";
                MessageORG.ShowConfirm(Title1, Msg1);
                InvoiceListView = new InvoiceRepo().getAllByUserId(toolSearchInfo1.txtSearch.Text, GeneralMembers.User.RoleId <= 2 ? 0 : GeneralMembers.User.Id, toolCashier1.CurrPage);
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
    }
}