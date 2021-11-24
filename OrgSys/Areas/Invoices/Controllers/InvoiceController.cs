using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrgSys.Controllers;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace OrgSys.Areas.Invoices.Controllers
{
    [Area("Invoices")]
    public class InvoiceController : BaseController<InvoiceModelView>
    {
        public override void LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {
            var type = new InvoiceTypeService(User.GetSchema()).Get(TypeId);
            ViewBag.InvoicesType = type.Name;
            ViewBag.InvoicesGroup = type.Group;
            ViewBag.InvoicesIcon = type.Icon;

            base.LoadViewBagIndex();
        }

        public override void LoadViewBag(InvoiceModelView model)
        {
            ViewBag.CurrencyId = new SelectList(new CurrencyService(User.GetSchema()).GetAll(model.ParentId, 0, 1, 20), "Id", "Name", model.CurrencyId);
            ViewBag.PaymentTypeId = new SelectList(new PaymentTypeService(User.GetSchema()).GetAll(model.ParentId, model.PaymentTypeId, 1, 20), "Id", "Name", model.PaymentTypeId);

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "1", Text = Translate.GetTranslate("Amount") });
            selectListItems.Add(new SelectListItem { Value = "2", Text = Translate.GetTranslate("Ratio") });

            ViewBag.DiscountType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.ServiceType = new SelectList(selectListItems, "Value", "Text");
            ViewBag.TaxType = new SelectList(selectListItems, "Value", "Text");

            var type = new InvoiceTypeService(User.GetSchema()).Get(model.TypeId);
            ViewBag.InvoicesType = type.Name;
            ViewBag.InvoicesGroup = type.Group;
            ViewBag.InvoicesIcon = type.Icon;
        }

        public override InvoiceModelView InitializeData(InvoiceModelView ob)
        {
            var setting = new PreferenceService(User.GetSchema());
            var StoreId = long.Parse("0" + setting.GetByKey("DefaultStore", "Invoice", ob.TypeId, 0)?.Value);

            long DealerId = 0;
            if (ob.TypeId == 1 || ob.TypeId == 3)
                DealerId = long.Parse("0" + setting.GetByKey("DefaultCustomer", "Invoice", ob.TypeId, 0)?.Value);
            else if (ob.TypeId == 2 || ob.TypeId == 4)
                DealerId = long.Parse("0" + setting.GetByKey("DefaultSupplier", "Invoice", ob.TypeId, 0)?.Value);

            var PaymentTypeId = long.Parse("0" + setting.GetByKey("DefaultPaymentType", "Invoice", ob.TypeId, 0)?.Value);
            var DefaultCurrencyId = long.Parse("0" + setting.GetByKey("DefaultCurrency", "Invoice", ob.TypeId, 0)?.Value);
            var DefaultDiscountType = int.Parse("0" + setting.GetByKey("DefaultDiscountType", "Invoice", ob.TypeId, 0)?.Value);
            var DefaultServiceType = int.Parse("0" + setting.GetByKey("DefaultServiceType", "Invoice", ob.TypeId, 0)?.Value);
            var DefaultTaxType = int.Parse("0" + setting.GetByKey("DefaultTaxType", "Invoice", ob.TypeId, 0)?.Value);
            var DiscountValue = decimal.Parse("0" + setting.GetByKey("DiscountValue", "Invoice", ob.TypeId, 0)?.Value);
            var ServiceValue = decimal.Parse("0" + setting.GetByKey("ServiceValue", "Invoice", ob.TypeId, 0)?.Value);
            var TaxValue = decimal.Parse("0" + setting.GetByKey("TaxValue", "Invoice", ob.TypeId, 0)?.Value);
            ViewBag.NumberLine = int.Parse("0" + setting.GetByKey("NumberLine", "Invoice", ob.TypeId, 0)?.Value);
            ViewBag.OrderTabe = int.Parse("0" + setting.GetByKey("OrderTabe", "Invoice", ob.TypeId, 0)?.Value);
            ViewBag.AutoSave = int.Parse("0" + setting.GetByKey("AutoSave", "Invoice", ob.TypeId, 0)?.Value);
            var TypeCode = int.Parse("0" + setting.GetByKey("TypeSerial", "Invoice", ob.TypeId, 0)?.Value);
            ViewBag.TypeSerial = TypeCode;
            ViewBag.AllowRepeated = int.Parse("0" + setting.GetByKey("AllowRepeated", "Invoice", ob.TypeId, 0)?.Value);

            if (ob == null)
                ob = new InvoiceModelView();

            if (ob.Id == 0)
            {
                ob.CodeNumber = new InvoiceService(User.GetSchema()).GetMaxCode(ob.TypeId);
                ob.Code = "" + ob.CodeNumber;
                ob.StoreId = StoreId;
                ob.DealerId = DealerId;
                ob.PaymentTypeId = PaymentTypeId;
                ob.CurrencyId = DefaultCurrencyId;
                ob.Date = DateTime.Now;
                ob.DiscountType = DefaultDiscountType;
                ob.ServiceType = DefaultServiceType;
                ob.TaxType = DefaultTaxType;
                ob.Discount = DiscountValue;
                ob.Service = ServiceValue;
                ob.Tax = TaxValue;
                ob.InvoiceProducts = new List<InvoiceProductModelView>();
            }

            ob.StoreName = new StoreService(User.GetSchema()).Get(ob.StoreId??0).Name;
            ob.DealerName = new DealerService(User.GetSchema()).Get(ob.DealerId).Name;
            ob.ParentCode = new InvoiceService(User.GetSchema()).Get(ob.ParentId).Code;
            ob.Rate = new CurrencyService(User.GetSchema()).Get(ob.CurrencyId).Rate;
            return ob;
        }

        public ActionResult CreateTransaction(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            new IntegrationServics(User.GetSchema()).CreateTransactionByInvoice(new InvoiceService(User.GetSchema()).Get(id));
            return Redirect("/Invoices/Invoice/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
        }

        public ActionResult CreateFinancial(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1, string dir = "Index")
        {
            new IntegrationServics(User.GetSchema()).CollectPaidInvoice(new InvoiceService(User.GetSchema()).Get(id));
            if (dir == "Index")
                return Redirect("/Invoices/Invoice/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
            else
                return Redirect("/Invoices/Invoice/Save?Id=" + id + "&ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
        }

        public JsonResult CollectInvoice(long id)
        {
            new IntegrationServics(User.GetSchema()).CreateFinancialByInvoice(new InvoiceService(User.GetSchema()).Get(id));
            return Json("Ok");
        }

        public JsonResult GetInvoicesNotReturn(string txtSearch = "", long TypeId = 0, long InvId = 0, int page = 1, int pageSize = 10)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = new InvoiceService(User.GetSchema()).GetInvoicesNotReturn(txtSearch, TypeId, InvId, page, pageSize);
            var list = itemsList.Distinct().OrderBy(_ => _.Code)
                .Select(_ => new
                {
                    _.Id,
                    _.Code
                })
                .ToList();
            return Json(list);
        }

        public ActionResult SearchInvoices(string txt = "", long dealerId = 0, long currencyId = 0, int page = 1, long typeId = 1, int Type = 1, int index = 0, string ids = "")
        {
            ViewBag.index = index;
            ViewBag.dealerId = dealerId;
            ViewBag.currencyId = currencyId;
            ViewBag.Type = Type;
            ViewBag.ids = ids;
            var list = new InvoiceService(User.GetSchema()).GetCreditAllByDealerId(txt, dealerId, currencyId, ids, 0, typeId, page, 10);
            return Type != 1 ? (ActionResult)PartialView("SearchInvoicesList", list) : View("SearchInvoices", list);
        }

        public JsonResult checkStock(int id)
        {
            var invoice = new InvoiceService(User.GetSchema()).Get(id);
            var data = new
            {
                code = invoice.Code,
                dealer = invoice.DealerName,
                date = invoice.Date,
                net = invoice.Net,
                credit = invoice.Credit
            };
            return Json(data);
        }

        public ActionResult Cancel(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            new InvoiceService(User.GetSchema()).Cancel(id);
            return Redirect("/Invoices/Invoice/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
        }

        public ActionResult Redo(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            new InvoiceService(User.GetSchema()).Redo(id);
            return Redirect("/Invoices/Invoice/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + ResultStatus.success + "&MsgError=Success");
        }

        //public async Task<IActionResult> Print(int Id = 0)
        //{
        //    var inv = new InvoiceService(User.GetSchema()).Get(Id);

        //    //List<string> Css = new List<string>();
        //    //Css.Add("/css/main.css");
        //    //Css.Add("/font/iconsmind-s/css/iconsminds.css");
        //    //Css.Add("/font/simple-line-icons/css/simple-line-icons.css");
        //    //Css.Add("/css/vendor/bootstrap.min.css");
        //    //Css.Add("/css/vendor/bootstrap.rtl.only.min.css");
        //    //Css.Add("/css/vendor/dataTables.bootstrap4.min.css");
        //    //Css.Add("/css/vendor/datatables.responsive.bootstrap4.min.css");
        //    //Css.Add("/css/vendor/select2.min.css");
        //    //Css.Add("/css/vendor/select2-bootstrap.min.css");
        //    //Css.Add("/css/vendor/perfect-scrollbar.css");
        //    //Css.Add("/css/vendor/glide.core.min.css");
        //    //Css.Add("/css/vendor/bootstrap-stars.css");
        //    //Css.Add("/css/vendor/nouislider.min.css");
        //    //Css.Add("/css/vendor/bootstrap-datepicker3.min.css");
        //    //Css.Add("/css/vendor/component-custom-switch.min.css");
        //    //Css.Add("/css/vendor/bootstrap-float-label.min.css");
        //    //Css.Add("/css/vendor/smart_wizard.min.css");
        //    //Css.Add("/css/vendor/bootstrap-tagsinput.css");
        //    //Css.Add("/font-awesome/css/all.css");
        //    //Css.Add("/lib/main.css");
        //    //Css.Add("/alertify.js/alertify.core.css");
        //    //Css.Add("/alertify.js/alertify.default.css");
        //    //Css = Css.Select(c =>
        //    //{
        //    //    string output = System.IO.File.ReadAllText("wwwroot" + c, Encoding.Default);
        //    //    return output;
        //    //}).ToList();
        //    //reviewer.CssFiles = Css;

        //    //if ("" + reviewer.ImageBase64String == "")
        //    //{
        //    //    if (System.IO.File.Exists(_hostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"))
        //    //    {
        //    //        var res = Convert.ToBase64String(System.IO.File.ReadAllBytes(_hostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"));
        //    //        if (res != "")
        //    //            reviewer.ImageBase64String = "data:image/png;base64," + res;
        //    //    }
        //    //}

        //    var viewHtml = Utility.General.RenderViewAsync<InvoiceModelView>(this, "InvoicePrint", inv).Result;

        //    var browserFetcher = new PuppeteerSharp.BrowserFetcher();
        //    await browserFetcher.DownloadAsync();
        //    await using var browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new PuppeteerSharp.LaunchOptions { Headless = true });

        //    await using var page = await browser.NewPageAsync();
        //    //await page.DeleteCookieAsync(cookieParams);
        //    //await page.SetCookieAsync(cookieParams);
        //    //await page.GoToAsync(url);
        //    //await page.PdfAsync(filename);
        //    await page.SetContentAsync(viewHtml);
        //    string path = @"PrintOut/";
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);

        //    }
        //    if (System.IO.File.Exists(path + inv.Id + ".pdf"))
        //    { System.IO.File.Delete(path + inv.Id + ".pdf"); }
        //    var baseURL = Request.Scheme + "://" + Request.Host;
        //    var ImagePath = System.IO.File.ReadAllText("wwwroot/logos/black2.svg");
        //    await page.PdfAsync(path + inv.Id + ".pdf", new PuppeteerSharp.PdfOptions
        //    {
        //        Format = PuppeteerSharp.Media.PaperFormat.A4,
        //        PrintBackground = false,
        //        OmitBackground = true,

        //        DisplayHeaderFooter = true,
        //        FooterTemplate = "<div style=\"font-size: 8px; padding-top: 8px; text-align: center; width: 100%; \"><span class=\"pageNumber\"></span></div>",
        //        HeaderTemplate = "<div style=\"text-align:center!important; margin-top:-25px; margin-left:50%; transform: translateX(-50%);\">" + ImagePath + "</div>",
        //        MarginOptions = new PuppeteerSharp.Media.MarginOptions
        //        {
        //            Bottom = "90px",
        //            Top = "120px",
        //            Left = "10px",
        //            Right = "10px"
        //        }
        //    });



        //   // Main(viewHtml, inv.Id);
        //    var cd = new System.Net.Mime.ContentDisposition
        //    {
        //        //Open In New Tap Or Download
        //        Inline = true
        //    };
        //    Response.Headers.Add(Microsoft.Net.Http.Headers.HeaderNames.ContentDisposition, cd.ToString());
        //    var stream = new FileStream("PrintOut/" + inv.Id.ToString() + ".pdf", FileMode.Open);
        //    return new FileStreamResult(stream, "application/pdf");
        //}

        // Task Main(string body, long id)
        //{
        //    var browserFetcher = new PuppeteerSharp.BrowserFetcher();
        //    await browserFetcher.DownloadAsync();

        //    await using var browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new PuppeteerSharp.LaunchOptions { Headless = true });

        //    await using var page = await browser.NewPageAsync();
        //    //await page.DeleteCookieAsync(cookieParams);
        //    //await page.SetCookieAsync(cookieParams);
        //    //await page.GoToAsync(url);
        //    //await page.PdfAsync(filename);
        //    await page.SetContentAsync(body);
        //    string path = @"PrintOut/";
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);

        //    }
        //    if (System.IO.File.Exists(path + id + ".pdf"))
        //    { System.IO.File.Delete(path + id + ".pdf"); }
        //    var baseURL = Request.Scheme + "://" + Request.Host;
        //    var ImagePath = System.IO.File.ReadAllText("wwwroot/logos/black2.svg");
        //    await page.PdfAsync(path + id + ".pdf", new PuppeteerSharp.PdfOptions
        //    {
        //        Format = PuppeteerSharp.Media.PaperFormat.A4,
        //        PrintBackground = false,
        //        OmitBackground = true,

        //        DisplayHeaderFooter = true,
        //        FooterTemplate = "<div style=\"font-size: 8px; padding-top: 8px; text-align: center; width: 100%; \"><span class=\"pageNumber\"></span></div>",
        //        HeaderTemplate = "<div style=\"text-align:center!important; margin-top:-25px; margin-left:50%; transform: translateX(-50%);\">" + ImagePath + "</div>",
        //        MarginOptions = new PuppeteerSharp.Media.MarginOptions
        //        {
        //            Bottom = "90px",
        //            Top = "120px",
        //            Left = "10px",
        //            Right = "10px"
        //        }
        //    });
        //}
    }
}