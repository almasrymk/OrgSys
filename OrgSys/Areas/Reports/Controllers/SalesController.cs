using System;
using Service;
using Entity.ModelView;
using Entity.ModelReport;
using OrgSys.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using OfficeOpenXml;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics.Metrics;
using static iTextSharp.text.pdf.events.IndexEvents;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using PuppeteerSharp;

namespace OrgSys.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class SalesController : BaseReportController
    {
        //public ActionResult ClientsBalancePdf(DateTime ToDate,
        //   long DealerId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
        //   int page = 1, int pageSize = 900)
        //{
        //    var data = new SalesReportService(User.GetSchema()).GetDealersBalance(1, ToDate, DealerId, ShiftId, BranchId, UserId, page, pageSize); // Fetch your data

        //    using (var ms = new MemoryStream())
        //    {
        //        var document = new Document(PageSize.A4, 50, 50, 25, 25);
        //        PdfWriter.GetInstance(document, ms);
        //        document.Open();
        //        var logoPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "logos", "logo.png");
        //        var logo = Image.GetInstance(logoPath);
        //        logo.ScaleToFit(50f, 50f); 
        //        document.Add(logo);
        //        var fontPath = Path.Combine("wwwroot", "font","cairo", "cairo-light.ttf");
        //        var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //        var titleFont = new Font(baseFont, 18, Font.BOLD);
        //        var headerFont = new Font(baseFont, 12, Font.BOLD);
        //        var bodyFont = new Font(baseFont, 12, Font.NORMAL);
        //        var titleParagraph = new Paragraph("Clients Balance", titleFont)
        //        {
        //            Alignment = Element.ALIGN_CENTER,
        //            SpacingAfter = 20
        //        };
        //        document.Add(titleParagraph); 
        //        PdfPTable table = new PdfPTable(new float[] { 1, 2, 1 })
        //        {
        //         //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
        //        };
            
             
        //        table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        table.AddCell(new PdfPCell(new Phrase("Dealer Name", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY ,});
        //        table.AddCell(new PdfPCell(new Phrase("Balance", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //        int counter = 1;
        //        foreach (var item in data)
        //        {
        //            table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));
        //            table.AddCell(new PdfPCell(new Phrase(item.DealerName, bodyFont))
        //            {
        //                HorizontalAlignment = Element.ALIGN_RIGHT, 
        //                RunDirection = PdfWriter.RUN_DIRECTION_RTL 
        //            });
        //            table.AddCell(new PdfPCell(new Phrase(item.Balance.ToString(), bodyFont)));
        //            counter++; 
        //        }
        //        document.Add(table);
        //        document.Close();

        //        var bytes = ms.ToArray();
        //        return File(bytes, "application/pdf", "ClientsBalance.pdf");
        //    }
        //}


        public async Task<ActionResult> ClientsBalancePdfAsync(DateTime ToDate,
          long DealerId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
          int page = 1, int pageSize = 900)
        {
            var data = new SalesReportService(User.GetSchema()).GetDealersBalance(1, ToDate, DealerId, ShiftId, BranchId, UserId); // Fetch your data

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

            var viewHtml = await Utility.General.RenderViewAsync<List<Entity.ModelReport.DealerBalance>>(this, "ClientsBalancePDF", data.ToList());
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

        public IActionResult ClientsBalanceExcel(DateTime ToDate ,
           long DealerId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 900)
        {
            ViewBag.pageNumber = 1;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;

            var data = new SalesReportService(User.GetSchema()).GetDealersBalance(1, ToDate, DealerId, ShiftId, BranchId, UserId, page, pageSize);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Clients Balance");

            string[] headers = { "#","Client", "Amount" }; 

           
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            int row = 2;
            int num = 1;
            foreach (var item in data) 
            {
                worksheet.Cells[row, 1].Value = num; 
                worksheet.Cells[row, 2].Value = item.DealerName; 
                worksheet.Cells[row, 3].Value = item.Balance; 
                row++;
                num++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ClientsBalance.xlsx");
        }

        public IActionResult ClientsStatment(string FromDate = null, string ToDate = null,
            long DealerId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
            int page = 1, int pageSize = 100)
        {
            ViewBag.DealerList = new SelectList(new DealerService(User.GetSchema()).GetAll(0, 1), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;
            ViewBag.DealerId = DealerId;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (FromDate != null)
                fDate = DateTime.Parse(FromDate);
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            if (DealerId == 1)
                DealerId = 0;

            ViewBag.FromDate = fDate;
            ViewBag.ToDate = tDate;

            var d = new DealerService(User.GetSchema()).Get(DealerId);
            if (d == null)
                d = new DealerModelView();

            var obList = new ReportResult<DealerStatmentData,DealerStatment> { 
                PrintMode = false , 
                Data = new DealerStatmentData {  DealerBalance = new DealerBalance { DealerName = d.Name, DealerImgPath = d.ImgPath, DealerId = d.Id }, FromDate = fDate , ToDate = tDate },
                Result = new SalesReportService(User.GetSchema()).GetDealersStatment(1, fDate, tDate, DealerId, ShiftId, BranchId, UserId, page, pageSize) 
            };
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ClientsStatmentList", obList) : View(obList);
        }

        //    public ActionResult ClientsStatmentPdf(string FromDate, string ToDate,
        //long DealerId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
        //int page = 1, int pageSize = 900)
        //    {

        //        DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
        //        DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
        //        if (FromDate != null)
        //            fDate = DateTime.Parse(FromDate);
        //        if (ToDate != null)
        //            tDate = DateTime.Parse(ToDate);

        //        var data = new ReportResult<DealerStatmentData, DealerStatment>
        //        {
        //            PrintMode = false,

        //            Result = new SalesReportService(User.GetSchema()).GetDealersStatment(1, fDate, tDate, DealerId, ShiftId, BranchId, UserId, page, pageSize)
        //        };

        //        using (var ms = new MemoryStream())
        //        {


        //            var document = new Document(PageSize.A4, 50, 50, 25, 25);
        //            PdfWriter.GetInstance(document, ms);
        //            document.Open();
        //            var logoPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "logos", "logo.png");
        //            var logo = Image.GetInstance(logoPath);
        //            logo.ScaleToFit(50f, 50f);


        //            document.Add(logo);


        //            var fontPath = Path.Combine("wwwroot", "font", "cairo", "cairo-light.ttf");

        //            var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //            var titleFont = new Font(baseFont, 18, Font.BOLD);
        //            var headerFont = new Font(baseFont, 12, Font.BOLD);
        //            var bodyFont = new Font(baseFont, 12, Font.NORMAL);


        //            var titleParagraph = new Paragraph("Clients Statment", titleFont)
        //            {
        //                Alignment = Element.ALIGN_CENTER,
        //                SpacingAfter = 20
        //            };
        //            document.Add(titleParagraph);
        //            PdfPTable table = new PdfPTable(new float[] { 1, 3, 2, 2, 3, 2 })
        //            {
        //                //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
        //            };
        //            table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //            table.AddCell(new PdfPCell(new Phrase("Client", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, });
        //            table.AddCell(new PdfPCell(new Phrase("Code", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //            table.AddCell(new PdfPCell(new Phrase("Date", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //            table.AddCell(new PdfPCell(new Phrase("Type", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
        //            table.AddCell(new PdfPCell(new Phrase("Amount", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });



        //            int counter = 1;

        //            foreach (var item in data.Result)
        //            {
        //                table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));

        //                table.AddCell(new PdfPCell(new Phrase(item.DealerName, bodyFont))
        //                {
        //                    HorizontalAlignment = Element.ALIGN_RIGHT,
        //                    RunDirection = PdfWriter.RUN_DIRECTION_RTL
        //                });
        //                table.AddCell(item.Code);
        //                table.AddCell(new PdfPCell(new Phrase(item.Date.ToString("dd/MM/yyyy"), bodyFont)));
        //                table.AddCell(new PdfPCell(new Phrase(item.TypeName.ToString(), bodyFont)));
        //                table.AddCell(new PdfPCell(new Phrase(item.Amount.ToString(), bodyFont)));
        //                counter++;
        //            }

        //            document.Add(table);
        //            document.Close();
        //            var bytes = ms.ToArray();
        //            return File(bytes, "application/pdf", "ClientsStatement.pdf");
        //        }
        //    }

        public async Task<ActionResult> ClientsStatmentPdf(string FromDate = null, string ToDate = null,
            long DealerId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0
           )
        {

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            if (FromDate != null)
                fDate = DateTime.Parse(FromDate);
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);
            var data = new ReportResult<DealerStatmentData, DealerStatment>
               {
                   PrintMode = false,

                        Result = new SalesReportService(User.GetSchema()).GetDealersStatment(1, fDate, tDate, DealerId, ShiftId, BranchId, UserId)
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

            var viewHtml = await Utility.General.RenderViewAsync<ReportResult<DealerStatmentData, DealerStatment>>(this, "ClientsStatmentPdf", data);
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



        public IActionResult ClientsStatmentExcel(string FromDate, string ToDate,
    long DealerId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
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
            var data = new ReportResult<DealerStatmentData, DealerStatment>
            {
                PrintMode = false,
                
                Result = new SalesReportService(User.GetSchema()).GetDealersStatment(1, fDate, tDate, DealerId, ShiftId, BranchId, UserId, page, pageSize)
            };

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Clients Statement");

            string[] headers = { "#", "Client", "Code", "Date", "Type", "Amount" };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            int row = 2;
            int num = 1;
            foreach (var item in data.Result) 
            {
                worksheet.Cells[row, 1].Value = num;
                worksheet.Cells[row, 2].Value = item.DealerName;
                worksheet.Cells[row, 3].Value = item.Code;
                worksheet.Cells[row, 4].Value = item.Date.ToString("dd/MM/yyyy"); 
                worksheet.Cells[row, 5].Value = item.TypeName; 
                worksheet.Cells[row, 6].Value = item.Amount;
                row++;
                num++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();


            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ClientsStatement.xlsx");
        }


        public IActionResult ClientsBalance(string ToDate = null,
           long DealerId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize =100 )
        {
            ViewBag.DealerList = new SelectList(new DealerService(User.GetSchema()).GetAll(0, 1), "Id", "Name");
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = DateTime.Now;

            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            ViewBag.ToDate = tDate;

            var obList = new SalesReportService(User.GetSchema()).GetDealersBalance(1 , tDate, DealerId, ShiftId, BranchId, UserId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("ClientsBalanceList", obList) : View(obList);
        }


        public IActionResult SalesBalance(string ToDate = null
      , long UserId = 0,
           int page = 1, int pageSize = 100)
        {
        
            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = DateTime.Now;


            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);
            ViewBag.ToDate = tDate;

            var obList = new SalesReportService(User.GetSchema()).GetSalesBalance(tDate,  UserId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("SalesBalanceList", obList) : View(obList);
        }


        public async Task<ActionResult> SalesBalancePdf(string ToDate = null
      , long UserId = 0,
           int page = 1, int pageSize = 100)
        {
            DateTime tDate = DateTime.Now;


            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);
            ViewBag.ToDate = tDate;

            var data = new SalesReportService(User.GetSchema()).GetSalesBalance(tDate, UserId, page, pageSize);

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

            var viewHtml = await Utility.General.RenderViewAsync<List<Entity.ModelReport.SalesBalance>>(this, "SalesBalancePDF", data.ToList());
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


        public IActionResult SalesBalanceExcel(string ToDate = null
      , long UserId = 0,
           int page = 1, int pageSize = 100)
        {
            ViewBag.pageNumber = 1;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = DateTime.Now;


            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);
            ViewBag.ToDate = tDate;

            var data = new SalesReportService(User.GetSchema()).GetSalesBalance(tDate, UserId, page, pageSize);
  

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Clients Balance");

            string[] headers = { "#", "Date", "InAmount" , "OutAmount", "Net" };


            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            int row = 2;
            int num = 1;
            foreach (var item in data)
            {
                worksheet.Cells[row, 1].Value = num;
                worksheet.Cells[row, 2].Value = item.Date.ToString("dd/MM/yyy");
                worksheet.Cells[row, 3].Value = item.InAmount;
                worksheet.Cells[row, 4].Value = item.OutAmount;
                worksheet.Cells[row, 5].Value = item.Net;
                row++;
                num++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalesBalance.xlsx");
        }



        public IActionResult SalesClient(string ToDate = null,long DealerId = 0
  , long UserId = 0,
       int page = 1, int pageSize = 100)
        {
            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = DateTime.Now;
            ViewBag.DealerList = new SelectList(new DealerService(User.GetSchema()).GetAll(0, 1), "Id", "Name");

            ViewBag.pageNumber = page;
            ViewBag.ParentId = 0;
            ViewBag.TypeId = 1;

            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);
            ViewBag.ToDate = tDate;

            var obList = new SalesReportService(User.GetSchema()).GetSalesClient(tDate, DealerId, UserId, page, pageSize);
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest" ? (ActionResult)PartialView("SalesClintList", obList) : View(obList);
        }
        public IActionResult SalesPerPeriod()
        {
            return View();
        }

        public IActionResult SalesProductPerPeriod()
        {
            return View();
        }
    }
}