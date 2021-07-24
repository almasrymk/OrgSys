using Entity.Model;
using System.Collections.Generic;
using System.Linq;

namespace Entity.ModelView
{
    public class OrderModelView : MovementModel
    {
        public OrderModelView()
        {

        }

        public OrderModelView(Order ob)
        {
            if (ob == null)
                ob = new Order();

            this.DealerId = ob.DealerId;

            this.DealerName = ob.Dealer?.Name;

            this.InvoiceId = ob.InvoiceId;

            this.Invoice = ob.Invoice;

            this.Notes = ob.Notes;

            this.TableId = ob.TableId;

            this.TableName = ob.Table?.Name;
          
            this.CloseTable = ob.CloseTable;

            this.Total = ob.Total;

            this.Service = ob.Service;

            this.ServiceType = ob.ServiceType;

            this.Tax = ob.Tax;

            this.TaxType = ob.TaxType;

            this.Discount = ob.Discount;

            this.DiscountType = ob.DiscountType;

            this.Net = ob.Net;

            this.BranchId = ob.BranchId;

            this.BranchName = ob.Branch?.Name;

            this.CreateUserId = ob.CreateUserId;

            this.CreateUserName = ob.CreateUser?.Name;

            this.ModifyUserId = ob.ModifyUserId;

            this.ModifyUserName = ob.ModifyUser?.Name;

            this.ShiftId = ob.ShiftId;

            this.ShiftName = ob.Shift?.Name;

            this.Date = ob.Date;

            this.CreateDate = ob.CreateDate;

            this.ModifyDate = ob.ModifyDate;

            this.Id = ob.Id;

            this.CodeNumber = ob.CodeNumber;

            this.Code = ob.Code;

            this.MaskText = ob.MaskText;

            this.ParentId = ob.ParentId;

            this.TypeId = ob.TypeId;

            this.Hide = ob.Hide;

            this.ImgPath = ob.ImgPath;

            this.Status = ob.Status;

            if (ob.OrderProducts == null)
                ob.OrderProducts = new List<OrderProduct>();

            this.OrderProducts = ob.OrderProducts.Select(e => new OrderProductModelView(e)).ToList();
        }

        public Order Model
        {
            get
            {
                return new Order
                {
                    DealerId = this.DealerId,
                    InvoiceId = this.InvoiceId,
                    Total = this.Total,                   
                    Net  = this.Net,
                    Notes = this.Notes,
                    TableId = this.TableId,
                    CloseTable = this.CloseTable,
                    Service = this.Service,
                    ServiceType = this.ServiceType,
                    Tax = this.Tax,
                    TaxType = this.TaxType,
                    Discount = this.Discount,
                    DiscountType = this.DiscountType,
                    BranchId = this.BranchId,
                    CreateUserId = this.CreateUserId,
                    ModifyUserId = this.ModifyUserId,
                    ShiftId = this.ShiftId,
                    Date = this.Date,
                    CreateDate = this.CreateDate,
                    ModifyDate = this.ModifyDate,
                    Id = this.Id,
                    CodeNumber = this.CodeNumber,
                    Code = this.Code,
                    MaskText = this.MaskText,
                    ParentId = this.ParentId,
                    TypeId = this.TypeId,
                    Hide = this.Hide,
                    Status = this.Status,
                    ImgPath = this.ImgPath,
                    OrderProducts = this.OrderProducts != null ? this.OrderProducts.Select(e => e.Model).ToList() : new List<OrderProduct>()
                };
            }
        }

        public long? TableId { get; set; }

        public long? InvoiceId { get; set; }

        public string TableName { get; set; }

        public bool CloseTable { get; set; }
                
        public long? DealerId { get; set; }

        public string DealerName { get; set; }       

        public decimal Total { get; set; }

        public decimal Discount { get; set; }

        public int DiscountType { get; set; }

        public decimal Tax { get; set; }

        public int TaxType { get; set; }

        public decimal Service { get; set; }

        public int ServiceType { get; set; }

        public decimal Net { get; set; }

        public string Notes { get; set; }

        public string BranchName { get; set; }

        public string CreateUserName { get; set; }

        public string ModifyUserName { get; set; }

        public string ShiftName { get; set; }

         public Invoice Invoice { get; set; }

        public List<OrderProductModelView> OrderProducts { get; set; }
    }
}