using Utility;
using Entity.Model;
using Utility.Resource;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class InventoryModelView : BaseModel
    {
        public InventoryModelView()
        {

        }

        public InventoryModelView(Inventory ob)
        {
            if (ob == null)
                ob = new Inventory();
            this.Id = ob.Id;
            this.CodeNumber = ob.CodeNumber;
            this.Code = ob.Code;
            this.Date = ob.Date;
            this.Hide = ob.Hide;
            this.Notes = ob.Notes;
            this.Status = ob.Status;
            this.ParentId = ob.ParentId;
            this.TypeId = ob.TypeId;
            this.ImgPath = ob.ImgPath;
            if (ob.InventoryProducts == null)
                ob.InventoryProducts = new List<InventoryProduct>();
            this.InventoryProducts = ob.InventoryProducts.Select(e => new InventoryProductModelView(e)).ToList();
        }

        public Inventory Model
        {
            get
            {
                return new Inventory
                {
                    Id = this.Id,
                    CodeNumber = this.CodeNumber,
                    Code = this.Code,
                    Date = this.Date,
                    Hide = this.Hide,
                    Notes = this.Notes,
                    Status = this.Status,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    ImgPath = this.ImgPath,
                    InventoryProducts = this.InventoryProducts != null ? this.InventoryProducts.Select(e => e.Model).ToList() : new List<InventoryProduct>()
                };
            }
        }

        public long CodeNumber { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime Date { get; set; }
        [Required]       
       

        [StringLength(500)]
        public string Notes { get; set; }
     
        public List<InventoryProductModelView> InventoryProducts { get; set; }
    }
}