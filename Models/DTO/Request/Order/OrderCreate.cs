using static WebBanAoo.Models.Status.Status;
using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Request.Order
{
    public class OrderCreate
    {


        public string Code { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    
        public decimal InitialTotalAmount { get; set; } // tổng tiền ban đầu
       
        public decimal TotalAmount { get; set; }
        public string? Note { get; set; }
        
        public DateTime OrderDate { get; set; }
       
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public OrderCreate()
        {
        }

        public OrderCreate(string code, OrderStatus status, decimal initialTotalAmount, decimal totalAmount, string? note, DateTime orderDate, DateTime createDate)
        {
            Code = code;
            Status = status;
            InitialTotalAmount = initialTotalAmount;
            TotalAmount = totalAmount;
            Note = note;
            OrderDate = orderDate;
            CreateDate = createDate;
        }
    }
}
