using Application.Commands.Org.Setting.Preference.Commands;
using AutoMapper;
using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OrgSys.Controllers;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class PreferenceController(IConfiguration configuration, IMapper mapper)
        :  MainController<PreferenceModelView, CreatePreferenceCommand , UpdatePreferenceCommand>(configuration, mapper)
    {


        public async Task<ActionResult> Show(string Resource = "", int type = 0)
        {
            ViewBag.Resource = Resource;
            ViewBag.type = type;
            var Service = await GetListApi<PreferenceModelView>(TypeId:type , TextSearch: Resource);
            if (Resource == "Invoice")
            {
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Amount" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Percentage" });
                ViewBag.Stocks = new SelectList(await GetListApi<StockModelView>(TypeId: 1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultStock")?.Value);

                ViewBag.Customers = new SelectList(await GetListApi<DealerModelView>(TypeId:1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCustomer")?.Value);
                ViewBag.Suppliers = new SelectList(await GetListApi<DealerModelView>(TypeId: 2), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultSupplier")?.Value);
                ViewBag.PaymentTypes = new SelectList(await GetListApi<PaymentTypeModelView>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultPaymentType")?.Value);
                ViewBag.DiscountType = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "DefaultDiscountType")?.Value);
                ViewBag.ServiceType = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "DefaultServiceType")?.Value);
                ViewBag.TaxType = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "DefaultTaxType")?.Value);
                ViewBag.Currencys = new SelectList(await GetListApi<CurrencyModelView>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCurrency")?.Value);

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
                ViewBag.CodeElectronicScale = Service.FirstOrDefault(e => e.Key == "CodeElectronicScale")?.Value;
                ViewBag.LengthElectronicScale = Service.FirstOrDefault(e => e.Key == "LengthElectronicScale")?.Value;
                ViewBag.LengthQtyElectronicScale = Service.FirstOrDefault(e => e.Key == "LengthQtyElectronicScale")?.Value;

            }

            if (Resource == "Transaction")
            {
                ViewBag.Customers = new SelectList(await GetListApi<DealerModelView>(TypeId: 1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCustomer")?.Value);
                ViewBag.Suppliers = new SelectList(await GetListApi<DealerModelView>(TypeId: 2), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultSupplier")?.Value);
                ViewBag.Stocks = new SelectList(await GetListApi<StockModelView>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultStock")?.Value);

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
                ViewBag.SaveLastStatusSetting = Service.FirstOrDefault(e => e.Key == "SaveLastStatusSetting")?.Value == "1";

                selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "0", Text = "Yes" });
                selectListItems.Add(new SelectListItem { Value = "1", Text = "No" });
                ViewBag.AutoReceived = Service.FirstOrDefault(e => e.Key == "AutoReceived")?.Value == "1";
            }

            if (Resource == "Order")
            {
                ViewBag.Customers = new SelectList(await GetListApi<DealerModelView>(TypeId: 1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCustomer")?.Value);

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
                ViewBag.Clients = new SelectList(await GetListApi<DealerModelView>(TypeId: 1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultClient")?.Value);
                ViewBag.Suppliers = new SelectList(await GetListApi<DealerModelView>(TypeId: 2), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultSupplier")?.Value);
                ViewBag.Safes = new SelectList( await GetListApi<SafeModelView>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultSafe")?.Value);
                ViewBag.PaymentTypes = new SelectList(await GetListApi<PaymentTypeModelView>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultPaymentType")?.Value);
                ViewBag.Currencys = new SelectList(await GetListApi<CurrencyModelView>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCurrency")?.Value);
                ViewBag.Outlays = new SelectList(await GetListApi<OutlayModelView>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultOutlay")?.Value);

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

            if (Resource == "Inventory")
            {
                ViewBag.Stocks = new SelectList( await GetListApi<StockModelView>(TypeId: 1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultStock")?.Value);

                var selectListItems = new List<SelectListItem>();
                selectListItems.Add(new SelectListItem { Value = "1", Text = "Allow" });
                selectListItems.Add(new SelectListItem { Value = "2", Text = "Not allowed" });

                ViewBag.TypeSerial = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "TypeSerial")?.Value);
                ViewBag.AutoSave = Service.FirstOrDefault(e => e.Key == "AutoSave")?.Value == "1";
            }
            return View(Service);
        }

        [HttpPost]
        public async Task<JsonResult> SavePreference([FromBody] PreferenceModelView list)
        {
          await base.Save(list);

            return Json("oK");
        }
    }


}
