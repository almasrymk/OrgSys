using System;
using Service;
using Entity.ModelView;
using Entity.ModelReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Entity.Model;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OfficeOpenXml;
using System.IO;

namespace OrgSys.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class WarehousesController : Controller
    {

        #region Stock Movement
        public ActionResult StockMovementPdf(string FromDate = null, string ToDate = null,
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
                
                Result = new WarehousesReportService(User.GetSchema()).GetStockStatment(fDate, tDate, StockId, ProductId, ShiftId, BranchId, UserId, page, pageSize)
            };

            using (var ms = new MemoryStream())
            {


                var document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter.GetInstance(document, ms);
                document.Open();
                var logoPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "logos", "logo.png");
                var logo = Image.GetInstance(logoPath);
                logo.ScaleToFit(50f, 50f);


                document.Add(logo);


                var fontPath = Path.Combine("wwwroot", "font", "cairo", "cairo-light.ttf");

                var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var titleFont = new Font(baseFont, 18, Font.BOLD);
                var headerFont = new Font(baseFont, 12, Font.BOLD);
                var bodyFont = new Font(baseFont, 12, Font.NORMAL);


                var titleParagraph = new Paragraph("Stock Movement", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20
                };
                document.Add(titleParagraph);
                PdfPTable table = new PdfPTable(new float[] { 1, 3, 2, 2, 2, })
                {
                    //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                };
                table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("ProductsName", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, });
               
                table.AddCell(new PdfPCell(new Phrase("Date", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Type", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Quantity", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
               



                int counter = 1;

                foreach (var item in data.Result)
                {
                    table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));

                    table.AddCell(new PdfPCell(new Phrase(item.ProductName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });
               
                    table.AddCell(new PdfPCell(new Phrase(item.Date.ToString("dd/MM/yyyy"), bodyFont)));
                 
                    table.AddCell(new PdfPCell(new Phrase(item.TypeName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });
                    table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), bodyFont)));

                    counter++;
                }

                document.Add(table);
                document.Close();
                var bytes = ms.ToArray();
                return File(bytes, "application/pdf", "StockMovement.pdf");
            }
        }



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


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
        public ActionResult ProductMovementPdf(string FromDate = null, string ToDate = null,
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
                
                Result = new WarehousesReportService(User.GetSchema()).GetProductStatment(fDate, tDate, StockId, ProductId, ShiftId, BranchId, UserId, page, pageSize)
            };

            using (var ms = new MemoryStream())
            {


                var document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter.GetInstance(document, ms);
                document.Open();
                var logoPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "logos", "logo.png");
                var logo = Image.GetInstance(logoPath);
                logo.ScaleToFit(50f, 50f);


                document.Add(logo);


                var fontPath = Path.Combine("wwwroot", "font", "cairo", "cairo-light.ttf");

                var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var titleFont = new Font(baseFont, 18, Font.BOLD);
                var headerFont = new Font(baseFont, 12, Font.BOLD);
                var bodyFont = new Font(baseFont, 12, Font.NORMAL);


                var titleParagraph = new Paragraph("Product Movement", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20
                };
                document.Add(titleParagraph);
                PdfPTable table = new PdfPTable(new float[] { 1, 3, 2, 2, 2, })
                {
                    //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                };
                table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("ProductsName", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, });

                table.AddCell(new PdfPCell(new Phrase("Date", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Type", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Quantity", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });




                int counter = 1;

                foreach (var item in data.Result)
                {
                    table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));

                    table.AddCell(new PdfPCell(new Phrase(item.ProductName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });

                    table.AddCell(new PdfPCell(new Phrase(item.Date.ToString("dd/MM/yyyy"), bodyFont)));

                    table.AddCell(new PdfPCell(new Phrase(item.TypeName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });
                    table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), bodyFont)));

                    counter++;
                }

                document.Add(table);
                document.Close();
                var bytes = ms.ToArray();
                return File(bytes, "application/pdf", "ProductMovement.pdf");
            }
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


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
        public ActionResult StockBalancePdf(string ToDate = null,
           long StockId = 0, long ProductId = 0, long ClassificationId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 900)
        {

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);
            
            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);

            var data = new WarehousesReportService(User.GetSchema()).GetStocksBalance(1, tDate, ProductId, StockId, ClassificationId, ShiftId, BranchId, UserId, page, pageSize);
            using (var ms = new MemoryStream())
            {


                var document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter.GetInstance(document, ms);
                document.Open();
                var logoPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "logos", "logo.png");
                var logo = Image.GetInstance(logoPath);
                logo.ScaleToFit(50f, 50f);


                document.Add(logo);


                var fontPath = Path.Combine("wwwroot", "font", "cairo", "cairo-light.ttf");

                var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var titleFont = new Font(baseFont, 18, Font.BOLD);
                var headerFont = new Font(baseFont, 12, Font.BOLD);
                var bodyFont = new Font(baseFont, 12, Font.NORMAL);


                var titleParagraph = new Paragraph("Stock Movement", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20
                };
                document.Add(titleParagraph);
                PdfPTable table = new PdfPTable(new float[] { 1, 3, 2, 2, 2, })
                {
                    //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                };
                table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Product", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, });

                table.AddCell(new PdfPCell(new Phrase("Classification", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Quantity", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Stock", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });




                int counter = 1;

                foreach (var item in data)
                {
                    table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));

                    table.AddCell(new PdfPCell(new Phrase(item.ProductName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });
                    
                    table.AddCell(new PdfPCell(new Phrase(item.ClassificationName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });

                   

                    table.AddCell(new PdfPCell(new Phrase(item.Balance.ToString(), bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });
                    table.AddCell(new PdfPCell(new Phrase(item.StockName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });

                    counter++;
                }

                document.Add(table);
                document.Close();
                var bytes = ms.ToArray();
                return File(bytes, "application/pdf", "StockMovement.pdf");
            }
        }



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

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
        public ActionResult ProductBalancePdf(string ToDate = null, long ProductId = 0, long StockId = 0, long ClassificationId = 0, long ShiftId = 0, long BranchId = 0, long UserId = 0,
           int page = 1, int pageSize = 900)
        {

            DateTime fDate = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime tDate = new DateTime(DateTime.Now.Year, 12, 31);

            if (ToDate != null)
                tDate = DateTime.Parse(ToDate);
            var data = new WarehousesReportService(User.GetSchema()).GetProductsBalance(1, tDate, ProductId, StockId, ClassificationId, ShiftId, BranchId, UserId, page, pageSize);
            using (var ms = new MemoryStream())
            {


                var document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter.GetInstance(document, ms);
                document.Open();
                var logoPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "logos", "logo.png");
                var logo = Image.GetInstance(logoPath);
                logo.ScaleToFit(50f, 50f);


                document.Add(logo);


                var fontPath = Path.Combine("wwwroot", "font", "cairo", "cairo-light.ttf");

                var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var titleFont = new Font(baseFont, 18, Font.BOLD);
                var headerFont = new Font(baseFont, 12, Font.BOLD);
                var bodyFont = new Font(baseFont, 12, Font.NORMAL);


                var titleParagraph = new Paragraph("Product Balance", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20
                };
                document.Add(titleParagraph);
                PdfPTable table = new PdfPTable(new float[] { 1, 3, 2, 2, 2, })
                {
                    //   RunDirection = PdfWriter.RUN_DIRECTION_RTL,
                };
                table.AddCell(new PdfPCell(new Phrase("No.", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Product", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, });

                table.AddCell(new PdfPCell(new Phrase("Classification", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Quantity", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Stock", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });




                int counter = 1;

                foreach (var item in data)
                {
                    table.AddCell(new PdfPCell(new Phrase(counter.ToString(), bodyFont)));

                    table.AddCell(new PdfPCell(new Phrase(item.ProductName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });

                    table.AddCell(new PdfPCell(new Phrase(item.ClassificationName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });



                    table.AddCell(new PdfPCell(new Phrase(item.Balance.ToString(), bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });
                    table.AddCell(new PdfPCell(new Phrase(item.StockName, bodyFont))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        RunDirection = PdfWriter.RUN_DIRECTION_RTL
                    });

                    counter++;
                }

                document.Add(table);
                document.Close();
                var bytes = ms.ToArray();
                return File(bytes, "application/pdf", "ProductBalance.pdf");
            }
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

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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
