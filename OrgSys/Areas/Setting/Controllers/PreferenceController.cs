using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class PreferenceController : Controller
    {
        PreferenceService service;
        public PreferenceController()
        {
            service = new PreferenceService();
        }
        [HttpGet]
        public ActionResult Show(string Resource = "", int type = 0)
        {
            ViewBag.Resource = Resource;
            ViewBag.type = type;

            var Service = service.GetAll(Resource, 0, type);
            if (Resource == "Invoice")
            {
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Amount" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Percentage" });
                
                ViewBag.Customers = new SelectList(new DealerService().GetAll(0, 1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCustomer")?.Value);
                ViewBag.Suppliers = new SelectList(new DealerService().GetAll(0, 2), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultSupplier")?.Value);                
                ViewBag.Stores = new SelectList(new StoreService().GetAll(0, 0), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultStore")?.Value);
                ViewBag.PaymentTypes = new SelectList(new PaymentTypeService().GetAll(0, 0), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultPaymentType")?.Value);
                ViewBag.DiscountType = new SelectList(selectListItems, "Value", "Text" , Service.FirstOrDefault(e => e.Key == "DefaultDiscountType")?.Value);
                ViewBag.ServiceType = new SelectList(selectListItems, "Value", "Text" , Service.FirstOrDefault(e => e.Key == "DefaultServiceType")?.Value);
                ViewBag.TaxType = new SelectList(selectListItems, "Value", "Text" , Service.FirstOrDefault(e => e.Key == "DefaultTaxType")?.Value);
                ViewBag.DiscountValue = Service.FirstOrDefault(e => e.Key == "DiscountValue")?.Value;
                ViewBag.ServiceValue = Service.FirstOrDefault(e => e.Key == "ServiceValue")?.Value;
                ViewBag.TaxValue = Service.FirstOrDefault(e => e.Key == "TaxValue")?.Value;

                selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Data after product" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Product after Data" });

                ViewBag.OrderTabe = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "OrderTabe")?.Value);

                selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Allow Repeated" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Increasing the quantity" });
                selectListItems.Add(new SelectListItem { Value = "3", Text = "Not allowed" });

                ViewBag.AllowRepeated = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "AllowRepeated")?.Value);

                selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Allow" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Not allowed" });

                ViewBag.TypeSerial = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "TypeSerial")?.Value);

                ViewBag.NumberLine = Service.FirstOrDefault(e => e.Key == "NumberLine")?.Value;
                ViewBag.AutoSave = Service.FirstOrDefault(e => e.Key == "AutoSave")?.Value == "1";
                ViewBag.AutoCreateTransaction = Service.FirstOrDefault(e => e.Key == "AutoCreateTransaction")?.Value == "1";
                ViewBag.SaveLastStatusSetting = Service.FirstOrDefault(e => e.Key == "SaveLastStatusSetting")?.Value == "1";

            }

            if (Resource == "Transaction")
            {
                ViewBag.Customers = new SelectList(new DealerService().GetAll(0, 1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCustomer")?.Value);
                ViewBag.Suppliers = new SelectList(new DealerService().GetAll(0, 2), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultSupplier")?.Value);
                ViewBag.Stores = new SelectList(new StoreService().GetAll(0, 0), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultStore")?.Value);

                List<SelectListItem>  selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Data after product" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Product after Data" });

                ViewBag.OrderTabe = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "OrderTabe")?.Value);

                selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Allow Repeated" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Increasing the quantity" });
                selectListItems.Add(new SelectListItem { Value = "3", Text = "Not allowed" });

                ViewBag.AllowRepeated = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "AllowRepeated")?.Value);

                selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Allow" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Not allowed" });

                ViewBag.TypeSerial = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "TypeSerial")?.Value);

                ViewBag.NumberLine = Service.FirstOrDefault(e => e.Key == "NumberLine")?.Value;
                ViewBag.AutoSave = Service.FirstOrDefault(e => e.Key == "AutoSave")?.Value == "1";
                ViewBag.SaveLastStatusSetting = Service.FirstOrDefault(e => e.Key == "SaveLastStatusSetting")?.Value == "1";

                selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "0", Text = "Yes" });
                selectListItems.Add(new SelectListItem { Value = "1", Text = "No" });
                ViewBag.AutoReceived = Service.FirstOrDefault(e => e.Key == "AutoReceived")?.Value == "1";
            }

            if (Resource == "Order")
            {
                ViewBag.Customers = new SelectList(new DealerService().GetAll(0, 1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCustomer")?.Value);

                List<SelectListItem> selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Data after product" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Product after Data" });

                ViewBag.OrderTabe = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "OrderTabe")?.Value);

                selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Allow Repeated" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Increasing the quantity" });
                selectListItems.Add(new SelectListItem { Value = "3", Text = "Not allowed" });

                ViewBag.AllowRepeated = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "AllowRepeated")?.Value);

                selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Allow" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Not allowed" });

                ViewBag.TypeSerial = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "TypeSerial")?.Value);

                ViewBag.NumberLine = Service.FirstOrDefault(e => e.Key == "NumberLine")?.Value;
                ViewBag.AutoSave = Service.FirstOrDefault(e => e.Key == "AutoSave")?.Value == "1";
                ViewBag.AutoCreateInvoice = Service.FirstOrDefault(e => e.Key == "AutoCreateInvoice")?.Value == "1";
                //ViewBag.SaveLastStatusSetting = Service.FirstOrDefault(e => e.Key == "SaveLastStatusSetting")?.Value == "1";

                selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Amount" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Percentage" });

                ViewBag.DiscountType = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "DefaultDiscountType")?.Value);
                ViewBag.ServiceType = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "DefaultServiceType")?.Value);
                ViewBag.TaxType = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "DefaultTaxType")?.Value);
                ViewBag.DiscountValue = Service.FirstOrDefault(e => e.Key == "DiscountValue")?.Value;
                ViewBag.ServiceValue = Service.FirstOrDefault(e => e.Key == "ServiceValue")?.Value;
                ViewBag.TaxValue = Service.FirstOrDefault(e => e.Key == "TaxValue")?.Value;
            }

            if (Resource == "Financial")
            {
                ViewBag.Clients = new SelectList(new DealerService().GetAll(0, 1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultClient")?.Value);
                ViewBag.Suppliers = new SelectList(new DealerService().GetAll(0, 2), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultSupplier")?.Value);
                ViewBag.Safes = new SelectList(new SafeService().GetAll(0, 0), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultSafe")?.Value);
                ViewBag.PaymentTypes = new SelectList(new PaymentTypeService().GetAll(0, 0), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultPaymentType")?.Value);
                ViewBag.Currencys = new SelectList(new CurrencyService().GetAll(0, 0), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCurrency")?.Value);
                ViewBag.Outlays = new SelectList(new OutlayService().GetAll(0, 0), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultOutlay")?.Value);

                List<SelectListItem> selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Data after product" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Product after Data" });

                ViewBag.OrderTabe = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "OrderTabe")?.Value);
              
                selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Allow" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Not allowed" });

                ViewBag.TypeSerial = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "TypeSerial")?.Value);
                ViewBag.AutoSave = Service.FirstOrDefault(e => e.Key == "AutoSave")?.Value == "1";
            }
            return View(Service);
        }

        [HttpPost]
        public JsonResult Save(string list)
        {
            try
            {
                var obList = JsonSerializer.Deserialize<List<PreferenceModelView>>(list);
                foreach (var ob in obList)
                {
                    if ("" + ob.Key != "")
                    {
                        var pr = service.GetByKey(ob.Key, ob.Reference, ob.TypeId, 0);
                        if (pr != null && pr.Id > 0)
                        {
                            pr.Value = ob.Value;
                            service.Save(pr);
                        }
                    }
                }

                return Json("Ok");
            }
            catch(Exception ex) {
                var ss = ex.Message;
            }
            return Json("Error");
        }
    }
}
