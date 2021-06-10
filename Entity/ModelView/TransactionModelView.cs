using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class TransactionModelView : BaseModel
    {
        public TransactionModelView()
        {

        }

        public TransactionModelView(Transaction ob)
        {
            if (ob == null)
                ob = new Transaction();
            this.Id = ob.Id;
            this.CodeNumber = ob.CodeNumber;
            this.Code = ob.Code;
            this.Date = ob.Date;
            this.DealerId = ob.DealerId;
            this.DealerName = ob.Dealer?.Name;
            this.Hide = ob.Hide;
            this.Notes = ob.Notes;
            this.OrderId = ob.OrderId;
            this.StoreId = ob.StoreId;
            this.StoreName = ob.Store?.Name;

            this.ToStoreId = ob.ToStoreId;
            this.ToStoreName = ob.ToStore?.Name;

            this.Total = ob.Total;           
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;          
            if (ob.TransactionProducts == null)
                ob.TransactionProducts = new List<TransactionProduct>();
            this.TransactionProducts = ob.TransactionProducts.Select(e => new TransactionProductModelView(e)).ToList();
        }

        public Transaction Model
        {
            get
            {
                return new Transaction
                {
                    Id = this.Id,
                    CodeNumber = this.CodeNumber,
                    Code = this.Code,
                    Date = this.Date,
                    DealerId = this.DealerId,
                    Total = this.Total,
                    Hide = this.Hide,
                    Notes = this.Notes,
                    OrderId = this.OrderId,
                    StoreId = this.StoreId,
                    ToStoreId = this.ToStoreId,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath,                  
                    TransactionProducts = this.TransactionProducts != null ? this.TransactionProducts.Select(e => e.Model).ToList() : new List<TransactionProduct>()
                };
            }
        }

        public long CodeNumber { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public long? DealerId { get; set; }

        [Display(Name = nameof(Title_Designer.Client), ResourceType = typeof(Title_Designer))]
        public string DealerName { get; set; }
     
        [Required]       
        public long StoreId { get; set; }

        [Display(Name = nameof(Title_Designer.Store), ResourceType = typeof(Title_Designer))]
        public string StoreName { get; set; }

        public long? ToStoreId { get; set; }
        public string ToStoreName { get; set; }

        public long? OrderId { get; set; }

        public decimal Total { get; set; }       

        [StringLength(500)]
        public string Notes { get; set; }
      

        public List<TransactionProductModelView> TransactionProducts { get; set; }
    }
}