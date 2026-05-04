using System;
using Service;
using Entity.ModelView;
using Entity.ModelReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Entity.Model;
using OfficeOpenXml;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

namespace OrgSys.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class WarehousesController : Controller
    {
        async Task Main(string body, int id, int count)
        {
            var browserFetcher = new PuppeteerSharp.BrowserFetcher();
            await browserFetcher.DownloadAsync();
            await using var browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new PuppeteerSharp.LaunchOptions { Headless = true });
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(body);
            string path = @"PrintOut/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (System.IO.File.Exists(path + id + ".pdf"))
            { System.IO.File.Delete(path + id + ".pdf"); }
            var baseURL = Request.Scheme + "://" + Request.Host;
            //var ImagePath = System.IO.File.ReadAllText("wwwroot/logos/logos22.svg");
            await page.PdfAsync(path + id + ".pdf", new PuppeteerSharp.PdfOptions
            {
                Format = new PuppeteerSharp.Media.PaperFormat(decimal.Parse("2.24409"), 1 + (count / 4)),// PuppeteerSharp.Media.PaperFormat.A4,
                PrintBackground = false,
                OmitBackground = true,
                DisplayHeaderFooter = true,
                //FooterTemplate = "<div style=\"font-size: 8px; padding-top: 8px; text-align: center; width: 100%; \"><span class=\"pageNumber\"></span></div>",
                // HeaderTemplate = "<div style=\"text-align:center!important; margin-top:-25px; margin-left:50%; transform: translateX(-50%);\">" + ImagePath + "</div>",
                MarginOptions = new PuppeteerSharp.Media.MarginOptions
                {
                    Bottom = "90px",
                    Top = "120px",
                    Left = "10px",
                    Right = "10px"
                }
            });
        }
        #region Stock Movement


        public async Task<ActionResult> StockMovementPdf(string FromDate = null, string ToDate = null,
            long StockId = 0, long ProductId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
            int page = 1, int pageSize = 900)
        {

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (FromDate != null)
                fDate = DateTime.Parse(FromDate);
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            var data = new ReportResult<StockStatmentData, StockStatment>
            {
                PrintMode = false,

                Result = new WarehousesReportService(User.GetSchema()).GetStockStatment(fDate, tDate, StockId, ProductId, ShiftId, BranchId, UserId)
            };

            List<string> Css = new List<string>();
            Css.Add("/css/vendor/bootstrap.min.css");
            Css.Add("/css/vendor/bootstrap.rtl.only.min.css");
            Css.Add("/css/vendor/fullcalendar.min.css");
            Css.Add("/css/vendor/dataTables.bootstrap4.min.css");
            Css.Add("/css/vendor/datatables.responsive.bootstrap4.min.css");
            Css.Add("/css/vendor/select2.min.css");
            Css.Add("/css/vendor/select2-bootstrap.min.css");
            Css.Add("/css/vendor/perfect-scrollbar.css");
            Css.Add("/css/vendor/glide.core.min.css");
            Css.Add("/css/vendor/bootstrap-stars.css");
            Css.Add("/css/vendor/nouislider.min.css");
            Css.Add("/css/vendor/smart_wizard.min.css");
            Css.Add("/css/vendor/component-custom-switch.min.css");
            Css.Add("/css/main.css");
            Css.Add("/css/jquery.bonsai.css");
            Css.Add("/fontawesome-free-5.15.3-web/css/all.css");
            Css.Add("/css/vendor/bootstrap-datepicker3.min.css");

            Css = Css.Select(c =>
            {
                string output = System.IO.File.ReadAllText("wwwroot" + c, Encoding.Default);
                return output;
            }).ToList();

            ViewBag.CssFiles = Css;
            //if ("" + ob.ImageBase64String == "")
            //{
            //    if (System.IO.File.Exists(IHostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"))
            //    {
            //        var res = Convert.ToBase64String(System.IO.File.ReadAllBytes(HostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"));
            //        if (res != "")
            //            ob.ImageBase64String = "data:image/png;base64," + res;
            //    }
            //}

            var viewHtml = await Utility.General.RenderViewAsync<ReportResult<StockStatmentData, StockStatment>>(this, "StockMovementPdf", data);
            await Main(viewHtml, 0, 10);
            var cd = new System.Net.Mime.ContentDisposition
            {
                //Open In New Tap Or Download
                Inline = true
            };
            Response.Headers.Add(Microsoft.Net.Http.Headers.HeaderNames.ContentDisposition, cd.ToString());
            var stream = new FileStream("PrintOut/0.pdf", FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }
        //public ActionResult StockMovementPdf(string FromDate = null, string ToDate = null,
        //    long StockId = 0, long ProductId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
        //    int page = 1, int pageSize = 900)
        //{

        //    DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
        //    DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
        //    if (FromDate != null)
        //        fDate = DateTime.Parse(FromDate);
        //    if (ToDate != null)
        //        tDate = DateTime.Parse(ToDate);

        //    var data = new ReportResult<StockStatmentData, StockStatment>
        //    {
        //        PrintMode = false,
                
        //        Result = new WarehousesReportService(User.GetSchema()).GetStockStatment(fDate, tDate, StockId, ProductId, ShiftId, BranchId, UserId, page, pageSize)
        //    };

        //    using (var ms = new MemoryStream())
        //    {


        //        var document = new Document(PageSize.A4, 50, 50, 25, 25);
        //        PdfWriter.GetInstance(document, ms);
        //        document.Open();
        //        var logoPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "logos", "logo.png");
        //        var logo = Image.GetInstance(logoPath);
        //        logo.ScaleToFit(50f, 50f);


        //        document.Add(logo);


        //        var fontPath = Path.Combine("wwwroot", "font", "cairo", "cairo-light.ttf");

        //        var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //        var titleFont = new Font(baseFont, 18, Font.BOLD);
        //        var headerFont = new Font(baseFont, 12, Font.BOLD);
        //        var bodyFont = new Font(baseFont, 12, Font.NORMAL);


        //        var titleParagraph = new Paragraph("Stock Movement", titleFont)
        //        {
        //            Alignment = Element.ALIGN_CENTER,
        //            SpacingAfter = 20
        //        };
        //        document.Add(titleParagraph);
        //        PdfPTable table = new PdfPTable(new float[] { 1, 3, 2, 2, 2, })
        //        {
        //            //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
        //        };
        //        table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        table.AddCell(new PdfPCell(new Phrase("ProductsName", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, });
               
        //        table.AddCell(new PdfPCell(new Phrase("Date", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        table.AddCell(new PdfPCell(new Phrase("Type", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        table.AddCell(new PdfPCell(new Phrase("Quantity", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
               



        //        int counter = 1;

        //        foreach (var item in data.Result)
        //        {
        //            table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));

        //            table.AddCell(new PdfPCell(new Phrase(item.ProductName, bodyFont))
        //            {
        //                HorizontalAlignment = Element.ALIGN_RIGHT,
        //                RunDirection = PdfWriter.RUN_DIRECTION_RTL
        //            });
               
        //            table.AddCell(new PdfPCell(new Phrase(item.Date.ToString("dd/MM/yyyy"), bodyFont)));
                 
        //            table.AddCell(new PdfPCell(new Phrase(item.TypeName, bodyFont))
        //            {
        //                HorizontalAlignment = Element.ALIGN_RIGHT,
        //                RunDirection = PdfWriter.RUN_DIRECTION_RTL
        //            });
        //            table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), bodyFont)));

        //            counter++;
        //        }

        //        document.Add(table);
        //        document.Close();
        //        var bytes = ms.ToArray();
        //        return File(bytes, "application/pdf", "StockMovement.pdf");
        //    }
        //}



        public IActionResult StockMovementExcel(string FromDate = null, string ToDate = null,
            long StockId = 0, long ProductId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
            int page = 1, int pageSize = 900)
        {

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (FromDate != null)
                fDate = DateTime.Parse(FromDate);
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.pageNumber = 1;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            var data = new ReportResult<StockStatmentData, StockStatment>
            {
                PrintMode = false,

                Result = new WarehousesReportService(User.GetSchema()).GetStockStatment(fDate, tDate, StockId, ProductId, ShiftId, BranchId, UserId, page, pageSize)
            };


            ExcelPackage.License.SetNonCommercialPersonal("Your Name");
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Stock Movement");

            string[] headers = { "#", "ProductsName", "Date", "Type", "Quantity" };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            int row = 2;
            int num = 1;
            foreach (var item in data.Result)
            {
                worksheet.Cells[row, 1].Value = num;
                worksheet.Cells[row, 2].Value = item.ProductName;
                worksheet.Cells[row, 3].Value = item.Date.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 4].Value = item.TypeName;
                worksheet.Cells[row, 5].Value = item.Quantity;
                row++;
                num++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();


            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StockMovement.xlsx");
        }
        public IActionResult StockMovement(string FromDate = null, string ToDate = null,
            long StockId = 0, long ProductId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
            int page = 1, int pageSize = 100)
        {
            ViewBag.StockList = new SelectList(new StockService(User.GetSchema()).GetAll(0, 0), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            ViewBag.StockId = StockId;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (FromDate != null)
                fDate = DateTime.Parse(FromDate);
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);
            
            ViewBag.FromDate = fDate;
            ViewBag.ToDate = tDate;

            var d = new StockService(User.GetSchema()).Get(StockId);
            if (d == null)
                d = new StockModelView();

            var obList = new ReportResult<StockStatmentData, StockStatment>
            {
                PrintMode = false,
                Data = new StockStatmentData { StockBalance = new StockBalance { StockName = d.Name, StockId = d.Id }, FromDate = fDate, ToDate = tDate },
                Result = new WarehousesReportService(User.GetSchema()).GetStockStatment(fDate, tDate, StockId, ProductId, ShiftId, BranchId, UserId, page, pageSize)
            };
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("StockMovementList", obList) : View(obList);
        }

        #endregion



        #region Product Movement


        //public ActionResult ProductMovementPdf(string FromDate = null, string ToDate = null,
        //  long StockId = 0, long ProductId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
        //  int page = 1, int pageSize = 900)
        //{

        //    DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
        //    DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
        //    if (FromDate != null)
        //        fDate = DateTime.Parse(FromDate);
        //    if (ToDate != null)
        //        tDate = DateTime.Parse(ToDate);

        //    var data = new ReportResult<ProductStatmentData, ProductStatment>
        //    {
        //        PrintMode = false,
                
        //        Result = new WarehousesReportService(User.GetSchema()).GetProductStatment(fDate, tDate, StockId, ProductId, ShiftId, BranchId, UserId, page, pageSize)
        //    };

        //    using (var ms = new MemoryStream())
        //    {


        //        var document = new Document(PageSize.A4, 50, 50, 25, 25);
        //        PdfWriter.GetInstance(document, ms);
        //        document.Open();
        //        var logoPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "logos", "logo.png");
        //        var logo = Image.GetInstance(logoPath);
        //        logo.ScaleToFit(50f, 50f);


        //        document.Add(logo);


        //        var fontPath = Path.Combine("wwwroot", "font", "cairo", "cairo-light.ttf");

        //        var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //        var titleFont = new Font(baseFont, 18, Font.BOLD);
        //        var headerFont = new Font(baseFont, 12, Font.BOLD);
        //        var bodyFont = new Font(baseFont, 12, Font.NORMAL);


        //        var titleParagraph = new Paragraph("Product Movement", titleFont)
        //        {
        //            Alignment = Element.ALIGN_CENTER,
        //            SpacingAfter = 20
        //        };
        //        document.Add(titleParagraph);
        //        PdfPTable table = new PdfPTable(new float[] { 1, 3, 2, 2, 2, })
        //        {
        //            //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
        //        };
        //        table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        table.AddCell(new PdfPCell(new Phrase("ProductsName", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, });

        //        table.AddCell(new PdfPCell(new Phrase("Date", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        table.AddCell(new PdfPCell(new Phrase("Type", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        table.AddCell(new PdfPCell(new Phrase("Quantity", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });




        //        int counter = 1;

        //        foreach (var item in data.Result)
        //        {
        //            table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));

        //            table.AddCell(new PdfPCell(new Phrase(item.ProductName, bodyFont))
        //            {
        //                HorizontalAlignment = Element.ALIGN_RIGHT,
        //                RunDirection = PdfWriter.RUN_DIRECTION_RTL
        //            });

        //            table.AddCell(new PdfPCell(new Phrase(item.Date.ToString("dd/MM/yyyy"), bodyFont)));

        //            table.AddCell(new PdfPCell(new Phrase(item.TypeName, bodyFont))
        //            {
        //                HorizontalAlignment = Element.ALIGN_RIGHT,
        //                RunDirection = PdfWriter.RUN_DIRECTION_RTL
        //            });
        //            table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), bodyFont)));

        //            counter++;
        //        }

        //        document.Add(table);
        //        document.Close();
        //        var bytes = ms.ToArray();
        //        return File(bytes, "application/pdf", "ProductMovement.pdf");
        //    }
        //}


        public async Task<ActionResult> ProductMovementPdf(string FromDate = null, string ToDate = null,
          long StockId = 0, long ProductId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
          int page = 1, int pageSize = 900)
        {

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (FromDate != null)
                fDate = DateTime.Parse(FromDate);
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            var data = new ReportResult<ProductStatmentData, ProductStatment>
            {
                PrintMode = false,

                Result = new WarehousesReportService(User.GetSchema()).GetProductStatment(fDate, tDate, StockId, ProductId, ShiftId, BranchId, UserId)
            };

            List<string> Css = new List<string>();
            Css.Add("/css/vendor/bootstrap.min.css");
            Css.Add("/css/vendor/bootstrap.rtl.only.min.css");
            Css.Add("/css/vendor/fullcalendar.min.css");
            Css.Add("/css/vendor/dataTables.bootstrap4.min.css");
            Css.Add("/css/vendor/datatables.responsive.bootstrap4.min.css");
            Css.Add("/css/vendor/select2.min.css");
            Css.Add("/css/vendor/select2-bootstrap.min.css");
            Css.Add("/css/vendor/perfect-scrollbar.css");
            Css.Add("/css/vendor/glide.core.min.css");
            Css.Add("/css/vendor/bootstrap-stars.css");
            Css.Add("/css/vendor/nouislider.min.css");
            Css.Add("/css/vendor/smart_wizard.min.css");
            Css.Add("/css/vendor/component-custom-switch.min.css");
            Css.Add("/css/main.css");
            Css.Add("/css/jquery.bonsai.css");
            Css.Add("/fontawesome-free-5.15.3-web/css/all.css");
            Css.Add("/css/vendor/bootstrap-datepicker3.min.css");

            Css = Css.Select(c =>
            {
                string output = System.IO.File.ReadAllText("wwwroot" + c, Encoding.Default);
                return output;
            }).ToList();

            ViewBag.CssFiles = Css;
            //if ("" + ob.ImageBase64String == "")
            //{
            //    if (System.IO.File.Exists(IHostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"))
            //    {
            //        var res = Convert.ToBase64String(System.IO.File.ReadAllBytes(HostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"));
            //        if (res != "")
            //            ob.ImageBase64String = "data:image/png;base64," + res;
            //    }
            //}

            var viewHtml = await Utility.General.RenderViewAsync<ReportResult<ProductStatmentData, ProductStatment>>(this, "ProductMovementPdf", data);
            await Main(viewHtml, 0, 10);
            var cd = new System.Net.Mime.ContentDisposition
            {
                //Open In New Tap Or Download
                Inline = true
            };
            Response.Headers.Add(Microsoft.Net.Http.Headers.HeaderNames.ContentDisposition, cd.ToString());
            var stream = new FileStream("PrintOut/0.pdf", FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }

        public IActionResult ProductMovementExcel(string FromDate = null, string ToDate = null,
            long StockId = 0, long ProductId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
            int page = 1, int pageSize = 900)
        {

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (FromDate != null)
                fDate = DateTime.Parse(FromDate);
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.pageNumber = 1;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            var data = new ReportResult<ProductStatmentData, ProductStatment>
            {
                PrintMode = false,

                Result = new WarehousesReportService(User.GetSchema()).GetProductStatment(fDate, tDate, StockId, ProductId, ShiftId, BranchId, UserId, page, pageSize)
            };


            ExcelPackage.License.SetNonCommercialPersonal("Your Name");
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Product Movement");

            string[] headers = { "#", "ProductsName", "Date", "Type", "Quantity" };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            int row = 2;
            int num = 1;
            foreach (var item in data.Result)
            {
                worksheet.Cells[row, 1].Value = num;
                worksheet.Cells[row, 2].Value = item.ProductName;
                worksheet.Cells[row, 3].Value = item.Date.ToString("dd/MM/yyyy");
                worksheet.Cells[row, 4].Value = item.TypeName;
                worksheet.Cells[row, 5].Value = item.Quantity;
                row++;
                num++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();


            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProductMovement.xlsx");
        }
        public IActionResult ProductMovement(string FromDate = null, string ToDate = null,
            long StockId  = 0, long ProductId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
            int page = 1, int pageSize = 100)
        {
            ViewBag.StockList = new SelectList(new StockService(User.GetSchema()).GetAll(0, 0), "Id", "Name");
            ViewBag.ProductList = new SelectList(new ProductService(User.GetSchema()).GetAll(0, 0), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            ViewBag.ProductId = ProductId;
            ViewBag.StockId = StockId;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (FromDate != null)
                fDate = DateTime.Parse(FromDate);
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.FromDate = fDate;
            ViewBag.ToDate = tDate;

            var d = new ProductService(User.GetSchema()).Get(ProductId);
            var s = new StockService(User.GetSchema()).Get(StockId);
            if (d == null || s == null)
            {
                d = new ProductModelView();
                s = new StockModelView();

            }

            var obList = new ReportResult<ProductStatmentData, ProductStatment>
            {
                PrintMode = false,
                Data = new ProductStatmentData { ProductBalance = new ProductBalance { ProductName = d.Name, ProductId = d.Id  , StockId = s.Id , StockName = s.Name, ClassificationId = d.ClassificationId, ClassificationName = d.ClassificationName }, FromDate = fDate, ToDate = tDate },
                Result = new WarehousesReportService(User.GetSchema()).GetProductStatment(fDate, tDate, StockId, ProductId, ShiftId, BranchId, UserId, page, pageSize)
            };
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ProductMovementList", obList) : View(obList);
        }



        #endregion

        #region StockBalanc

        public async Task<ActionResult> StockBalancePdf(string ToDate = null,
           long StockId = 0, long ProductId = 0, long ClassificationId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 900)
        {

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);

            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            var data = new WarehousesReportService(User.GetSchema()).GetStocksBalance(1, tDate, ProductId, StockId, ClassificationId, ShiftId, BranchId, UserId, page, pageSize);

            List<string> Css = new List<string>();
            Css.Add("/css/vendor/bootstrap.min.css");
            Css.Add("/css/vendor/bootstrap.rtl.only.min.css");
            Css.Add("/css/vendor/fullcalendar.min.css");
            Css.Add("/css/vendor/dataTables.bootstrap4.min.css");
            Css.Add("/css/vendor/datatables.responsive.bootstrap4.min.css");
            Css.Add("/css/vendor/select2.min.css");
            Css.Add("/css/vendor/select2-bootstrap.min.css");
            Css.Add("/css/vendor/perfect-scrollbar.css");
            Css.Add("/css/vendor/glide.core.min.css");
            Css.Add("/css/vendor/bootstrap-stars.css");
            Css.Add("/css/vendor/nouislider.min.css");
            Css.Add("/css/vendor/smart_wizard.min.css");
            Css.Add("/css/vendor/component-custom-switch.min.css");
            Css.Add("/css/main.css");
            Css.Add("/css/jquery.bonsai.css");
            Css.Add("/fontawesome-free-5.15.3-web/css/all.css");
            Css.Add("/css/vendor/bootstrap-datepicker3.min.css");

            Css = Css.Select(c =>
            {
                string output = System.IO.File.ReadAllText("wwwroot" + c, Encoding.Default);
                return output;
            }).ToList();

            ViewBag.CssFiles = Css;
            //if ("" + ob.ImageBase64String == "")
            //{
            //    if (System.IO.File.Exists(IHostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"))
            //    {
            //        var res = Convert.ToBase64String(System.IO.File.ReadAllBytes(HostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"));
            //        if (res != "")
            //            ob.ImageBase64String = "data:image/png;base64," + res;
            //    }
            //}

            var viewHtml = await Utility.General.RenderViewAsync <List<Entity.ModelReport.StockBalance>> (this, "StockBalancePdf", data.ToList());
            await Main(viewHtml, 0, 10);
            var cd = new System.Net.Mime.ContentDisposition
            {
                //Open In New Tap Or Download
                Inline = true
            };
            Response.Headers.Add(Microsoft.Net.Http.Headers.HeaderNames.ContentDisposition, cd.ToString());
            var stream = new FileStream("PrintOut/0.pdf", FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }
        //public ActionResult StockBalancePdf(string ToDate = null,
        //   long StockId = 0, long ProductId = 0, long ClassificationId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
        //   int page = 1, int pageSize = 900)
        //{

        //    DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
        //    DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            
        //    if (ToDate != null)
        //        tDate = DateTime.Parse(ToDate);

        //    var data = new WarehousesReportService(User.GetSchema()).GetStocksBalance(1, tDate, ProductId, StockId, ClassificationId, ShiftId, BranchId, UserId, page, pageSize);
        //    using (var ms = new MemoryStream())
        //    {


        //        var document = new Document(PageSize.A4, 50, 50, 25, 25);
        //        PdfWriter.GetInstance(document, ms);
        //        document.Open();
        //        var logoPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "logos", "logo.png");
        //        var logo = Image.GetInstance(logoPath);
        //        logo.ScaleToFit(50f, 50f);


        //        document.Add(logo);


        //        var fontPath = Path.Combine("wwwroot", "font", "cairo", "cairo-light.ttf");

        //        var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //        var titleFont = new Font(baseFont, 18, Font.BOLD);
        //        var headerFont = new Font(baseFont, 12, Font.BOLD);
        //        var bodyFont = new Font(baseFont, 12, Font.NORMAL);


        //        var titleParagraph = new Paragraph("Stock Movement", titleFont)
        //        {
        //            Alignment = Element.ALIGN_CENTER,
        //            SpacingAfter = 20
        //        };
        //        document.Add(titleParagraph);
        //        PdfPTable table = new PdfPTable(new float[] { 1, 3, 2, 2, 2, })
        //        {
        //            //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
        //        };
        //        table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        table.AddCell(new PdfPCell(new Phrase("Product", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, });

        //        table.AddCell(new PdfPCell(new Phrase("Classification", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        table.AddCell(new PdfPCell(new Phrase("Quantity", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        table.AddCell(new PdfPCell(new Phrase("Stock", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });




        //        int counter = 1;

        //        foreach (var item in data)
        //        {
        //            table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));

        //            table.AddCell(new PdfPCell(new Phrase(item.ProductName, bodyFont))
        //            {
        //                HorizontalAlignment = Element.ALIGN_RIGHT,
        //                RunDirection = PdfWriter.RUN_DIRECTION_RTL
        //            });
                    
        //            table.AddCell(new PdfPCell(new Phrase(item.ClassificationName, bodyFont))
        //            {
        //                HorizontalAlignment = Element.ALIGN_RIGHT,
        //                RunDirection = PdfWriter.RUN_DIRECTION_RTL
        //            });

                   

        //            table.AddCell(new PdfPCell(new Phrase(item.Balance.ToString(), bodyFont))
        //            {
        //                HorizontalAlignment = Element.ALIGN_RIGHT,
        //                RunDirection = PdfWriter.RUN_DIRECTION_RTL
        //            });
        //            table.AddCell(new PdfPCell(new Phrase(item.StockName, bodyFont))
        //            {
        //                HorizontalAlignment = Element.ALIGN_RIGHT,
        //                RunDirection = PdfWriter.RUN_DIRECTION_RTL
        //            });

        //            counter++;
        //        }

        //        document.Add(table);
        //        document.Close();
        //        var bytes = ms.ToArray();
        //        return File(bytes, "application/pdf", "StockMovement.pdf");
        //    }
        //}



        public IActionResult StockBalanceExcel(string ToDate = null,
           long StockId = 0, long ProductId = 0, long ClassificationId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 900)
        {

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
         
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.pageNumber = 1;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            var data = new WarehousesReportService(User.GetSchema()).GetStocksBalance(1, tDate, ProductId, StockId, ClassificationId, ShiftId, BranchId, UserId, page, pageSize);

            ExcelPackage.License.SetNonCommercialPersonal("Your Name");
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Stock Movement");

            string[] headers = { "#", "Product", "Classification", "Quantity", "Stock" };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            int row = 2;
            int num = 1;
            foreach (var item in data)
            {
                worksheet.Cells[row, 1].Value = num;
                worksheet.Cells[row, 2].Value = item.ProductName;
                worksheet.Cells[row, 3].Value = item.ClassificationName;
                worksheet.Cells[row, 4].Value = item.Balance;
                worksheet.Cells[row, 5].Value = item.StockName;
                row++;
                num++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();


            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StockMovement.xlsx");
        }

        public IActionResult StockBalance(string ToDate = null,
           long StockId = 0, long ProductId = 0 , long ClassificationId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 100)
        {
            ViewBag.ClassificationList = new SelectList(new ClassificationService(User.GetSchema()).GetAll(0, 0), "Id", "Name");
            ViewBag.ProductList = new SelectList(new ProductService(User.GetSchema()).GetAll(0, 0), "Id", "Name");
            ViewBag.StockList = new SelectList(new StockService(User.GetSchema()).GetAll(0, 1), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            ViewBag.ProductId = ProductId;
            ViewBag.StockId = StockId;
            ViewBag.ClassificationId = ClassificationId;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = DateTime.Now;

            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.ToDate = tDate;

            var obList = new WarehousesReportService(User.GetSchema()).GetStocksBalance(1, tDate, ProductId, StockId, ClassificationId, ShiftId, BranchId, UserId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("StockBalanceList", obList) : View(obList);
        }


        #endregion


        #region Product Balance

        public async Task<ActionResult> ProductBalancePdf(string ToDate = null, long ProductId = 0, long StockId = 0, long ClassificationId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 900)
        {

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
          
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            var data = new WarehousesReportService(User.GetSchema()).GetProductsBalance(1, tDate, ProductId, StockId, ClassificationId, ShiftId, BranchId, UserId, page, pageSize);

            List<string> Css = new List<string>();
            Css.Add("/css/vendor/bootstrap.min.css");
            Css.Add("/css/vendor/bootstrap.rtl.only.min.css");
            Css.Add("/css/vendor/fullcalendar.min.css");
            Css.Add("/css/vendor/dataTables.bootstrap4.min.css");
            Css.Add("/css/vendor/datatables.responsive.bootstrap4.min.css");
            Css.Add("/css/vendor/select2.min.css");
            Css.Add("/css/vendor/select2-bootstrap.min.css");
            Css.Add("/css/vendor/perfect-scrollbar.css");
            Css.Add("/css/vendor/glide.core.min.css");
            Css.Add("/css/vendor/bootstrap-stars.css");
            Css.Add("/css/vendor/nouislider.min.css");
            Css.Add("/css/vendor/smart_wizard.min.css");
            Css.Add("/css/vendor/component-custom-switch.min.css");
            Css.Add("/css/main.css");
            Css.Add("/css/jquery.bonsai.css");
            Css.Add("/fontawesome-free-5.15.3-web/css/all.css");
            Css.Add("/css/vendor/bootstrap-datepicker3.min.css");

            Css = Css.Select(c =>
            {
                string output = System.IO.File.ReadAllText("wwwroot" + c, Encoding.Default);
                return output;
            }).ToList();

            ViewBag.CssFiles = Css;
            //if ("" + ob.ImageBase64String == "")
            //{
            //    if (System.IO.File.Exists(IHostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"))
            //    {
            //        var res = Convert.ToBase64String(System.IO.File.ReadAllBytes(HostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"));
            //        if (res != "")
            //            ob.ImageBase64String = "data:image/png;base64," + res;
            //    }
            //}

            var viewHtml = await Utility.General.RenderViewAsync<List<Entity.ModelReport.ProductBalance>>(this, "ProductBalancePdf", data.ToList());
            await Main(viewHtml, 0, 10);
            var cd = new System.Net.Mime.ContentDisposition
            {
                //Open In New Tap Or Download
                Inline = true
            };
            Response.Headers.Add(Microsoft.Net.Http.Headers.HeaderNames.ContentDisposition, cd.ToString());
            var stream = new FileStream("PrintOut/0.pdf", FileMode.Open);
            return new FileStreamResult(stream, "application/pdf");

        }
     



        public IActionResult ProductBalanceExcel(string ToDate = null, long ProductId = 0, long StockId = 0, long ClassificationId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 900)
        {

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);

            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.pageNumber = 1;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            var data = new WarehousesReportService(User.GetSchema()).GetProductsBalance(1, tDate, ProductId, StockId, ClassificationId, ShiftId, BranchId, UserId, page, pageSize);

            ExcelPackage.License.SetNonCommercialPersonal("Your Name");
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Product Balance");

            string[] headers = { "#", "Product", "Classification", "Quantity", "Stock" };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            int row = 2;
            int num = 1;
            foreach (var item in data)
            {
                worksheet.Cells[row, 1].Value = num;
                worksheet.Cells[row, 2].Value = item.ProductName;
                worksheet.Cells[row, 3].Value = item.ClassificationName;
                worksheet.Cells[row, 4].Value = item.Balance;
                worksheet.Cells[row, 5].Value = item.StockName;
                row++;
                num++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();


            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProductBalance.xlsx");
        }
        public IActionResult ProductBalance(string ToDate = null, long ProductId = 0, long StockId = 0, long ClassificationId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 100)
        {
            ViewBag.ClassificationList = new SelectList(new ClassificationService(User.GetSchema()).GetAll(0, 0), "Id", "Name");
            ViewBag.ProductList = new SelectList(new ProductService(User.GetSchema()).GetAll(0, 0), "Id", "Name");
            ViewBag.StockList = new SelectList(new StockService(User.GetSchema()).GetAll(0, 1), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            ViewBag.ProductId = ProductId;
            ViewBag.StockId = StockId;
            ViewBag.ClassificationId = ClassificationId;


            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = DateTime.Now;

            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.ToDate = tDate;

            var obList = new WarehousesReportService(User.GetSchema()).GetProductsBalance(1, tDate, ProductId, StockId , ClassificationId ,ShiftId, BranchId, UserId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ProductBalanceList", obList) : View(obList);
        }

        #endregion
        public IActionResult TransaferReport()
        {
            return View();
        }

        public IActionResult ReceivedReport()
        {
            return View();
        }

        public IActionResult ProductStockInventoryList()
        {
            return View();
        }

        public IActionResult InventoryReport()
        {
            return View();
        }
        
    }
}
