using static WebBanAoo.Models.Status.Status;
using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Request.OrderDetail
{
    public class OrderDetailCreate
    {

        public string Code { get; set; }

        public OrderDetailStatus Status { get; set; } = OrderDetailStatus.Processed;
     
        public int Quantity { get; set; }
    
        public decimal UnitPrice { get; set; } // Giá mỗi đơn vị sản phẩm
  
        public decimal Discount { get; set; } // Chiết khấu (nếu có)

        public decimal TotalAmount { get; set; } // Tổng tiền cho sản phẩm (sau chiết khấu)
        public string? Note { get; set; }
  
        public int OrderId { get; set; }
     
        public int ProductDetailId { get; set; }

        public OrderDetailCreate()
        {
        }

        public OrderDetailCreate(string code, OrderDetailStatus status, int quantity, decimal unitPrice, decimal discount, decimal totalAmount, string? note, int orderId, int productDetailId)
        {
            Code = code;
            Status = status;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discount;
            TotalAmount = totalAmount;
            Note = note;
            OrderId = orderId;
            ProductDetailId = productDetailId;
        }
    }
}
