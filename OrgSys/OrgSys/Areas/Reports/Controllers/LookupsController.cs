using Service;
using OrgSys.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
 
namespace OrgSys.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class LookupsController : BaseReportController
    {       
        public IActionResult Clients(string search, int page = 1 , int pageSize = 100)
        {
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            var obList = new LookupsReportService(User.GetSchema()).GetDealers(1, search, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ClientsList", obList) : View(obList);
        }

        public IActionResult Suppliers(string search, int page = 1, int pageSize = 100)
        {
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 2;
            var obList = new LookupsReportService(User.GetSchema()).GetDealers(2, search, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ClientsList", obList) : View(obList);
        }


        #region Products
        //public ActionResult ProductsPdf(long ClassificationId, int page = 1, int pageSize = 900)
        //{
        //    var data = new LookupsReportService(User.GetSchema()).GetProducts(ClassificationId, page, pageSize);

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
        //        var titleParagraph = new Paragraph("Products", titleFont)
        //        {
        //            Alignment = Element.ALIGN_CENTER,
        //            SpacingAfter = 20
        //        };
        //        document.Add(titleParagraph);
        //        PdfPTable table = new PdfPTable(new float[] { 1, 2, 1,2 })
        //        {
        //            //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
        //        };


        //        table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        table.AddCell(new PdfPCell(new Phrase("Name", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, });
        //        table.AddCell(new PdfPCell(new Phrase("Barcode", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        table.AddCell(new PdfPCell(new Phrase("Classification", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        int counter = 1;
        //        foreach (var item in data)
        //        {
        //            table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));
        //            table.AddCell(new PdfPCell(new Phrase(item.ItemName, bodyFont))
        //            {
        //                HorizontalAlignment = Element.ALIGN_RIGHT,
        //                RunDirection = PdfWriter.RUN_DIRECTION_RTL
        //            });
        //            table.AddCell(new PdfPCell(new Phrase(item.BarCode.ToString(), bodyFont)));
        //            table.AddCell(new PdfPCell(new Phrase(item.ClassificationName, bodyFont))
        //            {
        //                HorizontalAlignment = Element.ALIGN_RIGHT,
        //                RunDirection = PdfWriter.RUN_DIRECTION_RTL
        //            });
        //            counter++;
        //        }
        //        document.Add(table);
        //        document.Close();

        //        var bytes = ms.ToArray();
        //        return File(bytes, "application/pdf", "Products.pdf");
        //    }
        //}


        // public async Task<ActionResult> ProductsPdf(long ClassificationId, int page = 1, int pageSize = 900)
        // {


        //     var data = new LookupsReportService(User.GetSchema()).GetProducts(ClassificationId);

        //     List<string> Css = new List<string>();
        //     Css.Add("/css/vendor/bootstrap.min.css");
        //     Css.Add("/css/vendor/bootstrap.rtl.only.min.css");
        //     Css.Add("/css/vendor/fullcalendar.min.css");
        //     Css.Add("/css/vendor/dataTables.bootstrap4.min.css");
        //     Css.Add("/css/vendor/datatables.responsive.bootstrap4.min.css");
        //     Css.Add("/css/vendor/select2.min.css");
        //     Css.Add("/css/vendor/select2-bootstrap.min.css");
        //     Css.Add("/css/vendor/perfect-scrollbar.css");
        //     Css.Add("/css/vendor/glide.core.min.css");
        //     Css.Add("/css/vendor/bootstrap-stars.css");
        //     Css.Add("/css/vendor/nouislider.min.css");
        //     Css.Add("/css/vendor/smart_wizard.min.css");
        //     Css.Add("/css/vendor/component-custom-switch.min.css");
        //     Css.Add("/css/main.css");
        //     Css.Add("/css/jquery.bonsai.css");
        //     Css.Add("/fontawesome-free-5.15.3-web/css/all.css");
        //     Css.Add("/css/vendor/bootstrap-datepicker3.min.css");

        //     Css = Css.Select(c =>
        //     {
        //         string output = System.IO.File.ReadAllText("wwwroot" + c, Encoding.Default);
        //         return output;
        //     }).ToList();

        //     ViewBag.CssFiles = Css;
        //     //if ("" + ob.ImageBase64String == "")
        //     //{
        //     //    if (System.IO.File.Exists(IHostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"))
        //     //    {
        //     //        var res = Convert.ToBase64String(System.IO.File.ReadAllBytes(HostingEnvironment.ContentRootPath + "/wwwroot/img/ClientCard.png"));
        //     //        if (res != "")
        //     //            ob.ImageBase64String = "data:image/png;base64," + res;
        //     //    }
        //     //}

        //     var viewHtml = await Utility.General.RenderViewAsync<List<Entity.ModelReport.ProductList>>(this, "ProductsPdf", data.ToList());
        //     await Main(viewHtml, 0, 10);
        //     var cd = new System.Net.Mime.ContentDisposition
        //     {
        //         //Open In New Tap Or Download
        //         Inline = true
        //     };
        //     Response.Headers.Add(Microsoft.Net.Http.Headers.HeaderNames.ContentDisposition, cd.ToString());
        //     var stream = new FileStream("PrintOut/0.pdf", FileMode.Open);
        //     return new FileStreamResult(stream, "application/pdf");

        // }

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
        public IActionResult ProductsExcel(long ClassificationId, int page = 1, int pageSize = 900)
        {
            ViewBag.pageNumber = 1;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
         
            var data = new LookupsReportService(User.GetSchema()).GetProducts(ClassificationId, page, pageSize);

            ExcelPackage.License.SetNonCommercialPersonal("Your Name");
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Products");

            string[] headers = { "#", "Name", "Barcode" , "Classification" };


            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            int row = 2;
            int num = 1;
            foreach (var item in data)
            {
                worksheet.Cells[row, 1].Value = num;
                worksheet.Cells[row, 2].Value = item.ItemName;
                worksheet.Cells[row, 3].Value = item.BarCode;
                worksheet.Cells[row, 4].Value = item.ClassificationName;
                row++;
                num++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Products.xlsx");
        }
        public IActionResult Products(long ClassificationId , int page = 1, int pageSize = 100)
        {
            ViewBag.ClassificationList = new SelectList(new ClassificationService(User.GetSchema()).GetAll(0, 1), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.ClassificationId = ClassificationId;
            var obList = new LookupsReportService(User.GetSchema()).GetProducts(ClassificationId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ProductsList", obList) : View(obList);
        }


        #endregion

          #region Stocks
        public IActionResult Stocks(string search, int page = 1, int pageSize = 100)
        {
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;            
            var obList = new LookupsReportService(User.GetSchema()).GetStocks(search, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("StocksList", obList) : View(obList);
        }

        //public ActionResult StocksPdf(string search, int page = 1, int pageSize = 900)
        //{
        //    var data = new LookupsReportService(User.GetSchema()).GetStocks(search, page, pageSize);

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
        //        var titleParagraph = new Paragraph("Stocks", titleFont)
        //        {
        //            Alignment = Element.ALIGN_CENTER,
        //            SpacingAfter = 20
        //        };
        //        document.Add(titleParagraph);
        //        PdfPTable table = new PdfPTable(new float[] { 1, 6 })
        //        {
        //            //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
        //        };


        //        table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        table.AddCell(new PdfPCell(new Phrase("Name", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, });
              
        //        int counter = 1;
        //        foreach (var item in data)
        //        {
        //            table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));
        //            table.AddCell(new PdfPCell(new Phrase(item.Name, bodyFont))
        //            {
        //                HorizontalAlignment = Element.ALIGN_RIGHT,
        //                RunDirection = PdfWriter.RUN_DIRECTION_RTL
        //            });
                
                   
        //            counter++;
        //        }
        //        document.Add(table);
        //        document.Close();

        //        var bytes = ms.ToArray();
        //        return File(bytes, "application/pdf", "Stocks.pdf");
        //    }
        //}


        public async Task<ActionResult> StocksPdf(string search, int page = 1, int pageSize = 900)
        {

            var data = new LookupsReportService(User.GetSchema()).GetStocks(search);

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

            var viewHtml = await Utility.General.RenderViewAsync<List<Entity.ModelReport.StockList>>(this, "StocksPdf", data.ToList());
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
        public IActionResult StocksExcel(string search, int page = 1, int pageSize = 900)
        {
            ViewBag.pageNumber = 1;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;

            var data = new LookupsReportService(User.GetSchema()).GetStocks(search, page, pageSize);

            ExcelPackage.License.SetNonCommercialPersonal("Your Name");
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Stocks");

            string[] headers = { "#", "Name" };


            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            int row = 2;
            int num = 1;
            foreach (var item in data)
            {
                worksheet.Cells[row, 1].Value = num;
                worksheet.Cells[row, 2].Value = item.Name;
              
                row++;
                num++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Stocks.xlsx");
        }


        #endregion
        public IActionResult Safes(string search, int page = 1, int pageSize = 100)
        {
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            var obList = new LookupsReportService(User.GetSchema()).GetSafes(search, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("StocksList", obList) : View(obList);
        }
    }
}