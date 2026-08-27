using Application.Commands.Org.Setting.Preference.Commands;
using AutoMapper;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OrgSys.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgSys.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class PreferenceController(IConfiguration configuration, IMapper mapper)
        :  MainController<PreferenceDto, CreatePreferenceCommand , UpdatePreferenceCommand>(configuration, mapper)
    {         
        public async Task<ActionResult> Show(string Resource = "", int type = 0)
        {
            ViewBag.Resource = Resource;
            ViewBag.type = type;
            var Service = await GetListApi<PreferenceDto>(TypeId:type , TextSearch: Resource);
            if (Resource == "Invoice")
            {
                await ConfigureInvoicePreferences(Service, type);
            }

            if (Resource == "Transaction")
            {
                await ConfigureTransactionPreferences(Service);
            }

            if (Resource == "Order")
            {
                await ConfigureOrderPreferences(Service);
            }

            if (Resource == "Financial")
            {
                await ConfigureFinancialPreferences(Service);
            }

            if (Resource == "Inventory")
            {
                await ConfigureInventoryPreferences(Service);
            }

            if (Resource == "Journal")
            {
                await ConfigureJournalPreferences(Service);
            }
            return View(Service);
        }

        private async Task ConfigureInvoicePreferences(List<PreferenceDto> Service, int type)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "1", Text = "Amount" });
            selectListItems.Add(new SelectListItem { Value = "2", Text = "Percentage" });

            var Accounts = await GetListApi<AccountDto>(TypeId: 1);

            ViewBag.Stocks = new SelectList(await GetListApi<StockDto>(TypeId: 1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultStock")?.Value);
            ViewBag.Customers = new SelectList(await GetListApi<DealerDto>(TypeId:1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCustomer")?.Value);
            ViewBag.Suppliers = new SelectList(await GetListApi<DealerDto>(TypeId: 2), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultSupplier")?.Value);
            ViewBag.PaymentTypes = new SelectList(await GetListApi<PaymentTypeDto>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultPaymentType")?.Value);
            ViewBag.DiscountType = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "DefaultDiscountType")?.Value);
            ViewBag.ServiceType = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "DefaultServiceType")?.Value);
            ViewBag.TaxType = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "DefaultTaxType")?.Value);
            ViewBag.Currencys = new SelectList(await GetListApi<CurrencyDto>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCurrency")?.Value);
            ViewBag.SalesAccounts = new SelectList(Accounts, "Id", "Name", Service.FirstOrDefault(e => e.Key == "SalesAccount")?.Value);
            var invoiceAccountKey = type == 2 || type == 4 ? "PurchaseAccount" : "SalesAccount";
            ViewBag.InvoiceAccountLabel = type == 2 || type == 4 ? "Purchase Account" : "Sales Account";
            ViewBag.InvoiceAccounts = new SelectList(Accounts, "Id", "Name", Service.FirstOrDefault(e => e.Key == invoiceAccountKey)?.Value);
            ViewBag.DealerAccounts = new SelectList(Accounts, "Id", "Name", Service.FirstOrDefault(e => e.Key == "DealerAccount")?.Value);
            ViewBag.TaxAccounts = new SelectList(Accounts, "Id", "Name", Service.FirstOrDefault(e => e.Key == "TaxAccount")?.Value);

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
            ViewBag.AccountsIntegration = Service.FirstOrDefault(e => e.Key == "AccountsIntegration")?.Value == "1";
            ViewBag.AutoCreateJournalEntry = Service.FirstOrDefault(e => e.Key == "AutoCreateJournalEntry")?.Value == "1";
        }

        private async Task ConfigureTransactionPreferences(List<PreferenceDto> Service)
        {
            var Accounts = await GetListApi<AccountDto>(TypeId: 1, PageSize: 100000);

            ViewBag.Customers = new SelectList(await GetListApi<DealerDto>(TypeId: 1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCustomer")?.Value);
            ViewBag.Suppliers = new SelectList(await GetListApi<DealerDto>(TypeId: 2), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultSupplier")?.Value);
            ViewBag.Stocks = new SelectList(await GetListApi<StockDto>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultStock")?.Value);
            ViewBag.StockAccount = new SelectList(Accounts, "Id", "Name", Service.FirstOrDefault(e => e.Key == "StockAccount")?.Value);
            ViewBag.SalesAccount = new SelectList(Accounts, "Id", "Name", Service.FirstOrDefault(e => e.Key == "SalesAccount")?.Value);
            ViewBag.PurchaseReturnAccount = new SelectList(Accounts, "Id", "Name", Service.FirstOrDefault(e => e.Key == "PurchaseReturnAccount")?.Value);
            ViewBag.PurchaseAccount = new SelectList(Accounts, "Id", "Name", Service.FirstOrDefault(e => e.Key == "PurchaseAccount")?.Value);
            ViewBag.SalesReturnAccount = new SelectList(Accounts, "Id", "Name", Service.FirstOrDefault(e => e.Key == "SalesReturnAccount")?.Value);
            ViewBag.OpeningBalanceAccount = new SelectList(Accounts, "Id", "Name", Service.FirstOrDefault(e => e.Key == "OpeningBalanceAccount")?.Value);
            ViewBag.InventoryDamageExpenseAccount = new SelectList(Accounts, "Id", "Name", Service.FirstOrDefault(e => e.Key == "InventoryDamageExpenseAccount")?.Value);
            ViewBag.SourceInventoryAccount = new SelectList(Accounts, "Id", "Name", Service.FirstOrDefault(e => e.Key == "SourceInventoryAccount")?.Value);
            ViewBag.DestinationInventoryAccount = new SelectList(Accounts, "Id", "Name", Service.FirstOrDefault(e => e.Key == "DestinationInventoryAccount")?.Value);
            ViewBag.TransitAccount = new SelectList(Accounts, "Id", "Name", Service.FirstOrDefault(e => e.Key == "TransitAccount")?.Value);

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
            ViewBag.AccountsIntegration = Service.FirstOrDefault(e => e.Key == "AccountsIntegration")?.Value == "1";
            ViewBag.AutoCreateJournalEntry = Service.FirstOrDefault(e => e.Key == "AutoCreateJournalEntry")?.Value == "1";

            selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "0", Text = "Yes" });
            selectListItems.Add(new SelectListItem { Value = "1", Text = "No" });
            ViewBag.AutoReceived = Service.FirstOrDefault(e => e.Key == "AutoReceived")?.Value == "1";
        }

        private async Task ConfigureOrderPreferences(List<PreferenceDto> Service)
        {
            ViewBag.Customers = new SelectList(await GetListApi<DealerDto>(TypeId: 1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCustomer")?.Value);

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

        private async Task ConfigureFinancialPreferences(List<PreferenceDto> Service)
        {
            ViewBag.Clients = new SelectList(await GetListApi<DealerDto>(TypeId: 1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultClient")?.Value);
            ViewBag.Suppliers = new SelectList(await GetListApi<DealerDto>(TypeId: 2), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultSupplier")?.Value);
            ViewBag.CashBoxes = new SelectList( await GetListApi<CashBoxDto>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCashBox")?.Value);
            ViewBag.PaymentTypes = new SelectList(await GetListApi<PaymentTypeDto>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultPaymentType")?.Value);
            ViewBag.Currencys = new SelectList(await GetListApi<CurrencyDto>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCurrency")?.Value);
            ViewBag.Outlays = new SelectList(await GetListApi<OutlayDto>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultOutlay")?.Value);

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

        private async Task ConfigureInventoryPreferences(List<PreferenceDto> Service)
        {
            ViewBag.Stocks = new SelectList( await GetListApi<StockDto>(TypeId: 1), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultStock")?.Value);

            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "1", Text = "Allow" });
            selectListItems.Add(new SelectListItem { Value = "2", Text = "Not allowed" });

            ViewBag.TypeSerial = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "TypeSerial")?.Value);
            ViewBag.AutoSave = Service.FirstOrDefault(e => e.Key == "AutoSave")?.Value == "1";
            ViewBag.AutoCreateAdjustment = Service.FirstOrDefault(e => e.Key == "AutoCreateAdjustment")?.Value == "1";
        }

        private async Task ConfigureJournalPreferences(List<PreferenceDto> Service)
        {
            ViewBag.Currencys = new SelectList(await GetListApi<CurrencyDto>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultCurrency")?.Value);
            ViewBag.JournalTypes = new SelectList(await GetListApi<JournalTypeDto>(), "Id", "Name", Service.FirstOrDefault(e => e.Key == "DefaultJournalType")?.Value);

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "1", Text = "Data after product" });
            selectListItems.Add(new SelectListItem { Value = "2", Text = "Product after Data" });

            ViewBag.OrderTabe = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "OrderTabe")?.Value);

            selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "1", Text = "Allow" });
            selectListItems.Add(new SelectListItem { Value = "2", Text = "Not allowed" });

            ViewBag.TypeSerial = new SelectList(selectListItems, "Value", "Text", Service.FirstOrDefault(e => e.Key == "TypeSerial")?.Value);

            ViewBag.NumberLine = Service.FirstOrDefault(e => e.Key == "NumberLine")?.Value;
            ViewBag.AutoSave = Service.FirstOrDefault(e => e.Key == "AutoSave")?.Value == "1";
            ViewBag.SaveLastStatusSetting = Service.FirstOrDefault(e => e.Key == "SaveLastStatusSetting")?.Value == "1";
        }
        
        [HttpPost]
        public async Task<JsonResult> SavePreference([FromBody] PreferenceDto list)
        {
          await base.Save(list);

          return Json("oK");
        }
    }
}
