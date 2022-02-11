using Entity.ModelView;
using Microsoft.AspNetCore.Mvc;
using OrgSys.Controllers;
using Service;
using System;
using System.Collections.Generic;

namespace OrgSys.Areas.Transaction.Controllers
{
    [Area("Transactions")]
    public class TransactionController : BaseController<TransactionModelView>
    {
        public override void LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {
            var type = new TransactionTypeService(User.GetSchema()).Get(TypeId);
            ViewBag.TransactionsType = type.Name;            
            ViewBag.TransactionsIcon = type.Icon;
            base.LoadViewBagIndex();
        }

        public override void LoadViewBag(TransactionModelView model)
        {
            var type = new TransactionTypeService(User.GetSchema()).Get(model.TypeId);
            ViewBag.TransactionsType = type.Name;
            ViewBag.TransactionsType = type.Icon;
        }

        public override TransactionModelView InitializeData(TransactionModelView ob)
        {
            var setting = new PreferenceService(User.GetSchema());
            var StoreId = long.Parse("0" + setting.GetByKey("DefaultStore", "Transaction", ob.TypeId, 0)?.Value);

            long DealerId = 0;
            if (ob.TypeId == 1)
                DealerId = long.Parse("0" + setting.GetByKey("DefaultSupplier", "Transaction", ob.TypeId, 0)?.Value);
            else if (ob.TypeId == 2)
                DealerId = long.Parse("0" + setting.GetByKey("DefaultCustomer", "Transaction", ob.TypeId, 0)?.Value);

            ViewBag.NumberLine = int.Parse("0" + setting.GetByKey("NumberLine", "Transaction", ob.TypeId, 0)?.Value);
            ViewBag.OrderTabe = int.Parse("0" + setting.GetByKey("OrderTabe", "Transaction", ob.TypeId, 0)?.Value);
            ViewBag.AutoSave = int.Parse("0" + setting.GetByKey("AutoSave", "Transaction", ob.TypeId, 0)?.Value);
            var TypeCode = int.Parse("0" + setting.GetByKey("TypeSerial", "Transaction", ob.TypeId, 0)?.Value);
            ViewBag.TypeSerial = TypeCode;
            ViewBag.AllowRepeated = int.Parse("0" + setting.GetByKey("AllowRepeated", "Transaction", ob.TypeId, 0)?.Value);

            if (ob == null)
                ob = new TransactionModelView();

            if (ob.Id == 0)
            {
                ob.CodeNumber = new TransactionService(User.GetSchema()).GetMaxCode(ob.TypeId);
                ob.Code = "" + ob.CodeNumber;
                ob.StoreId = StoreId;
                ob.DealerId = DealerId;
                ob.Date = DateTime.Now;
                ob.TransactionProductList = new List<TransactionProductModelView>();
            }
            ob.StoreName = new StoreService(User.GetSchema()).Get(ob.StoreId??0).Name;
            ob.ToStoreName = new StoreService(User.GetSchema()).Get(ob.ToStoreId??0)?.Name;
            ob.DealerName = new DealerService(User.GetSchema()).Get(ob.DealerId??0)?.Name;
            return ob;
        }
    }
}