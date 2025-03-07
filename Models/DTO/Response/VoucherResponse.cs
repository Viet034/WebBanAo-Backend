using static WebBanAoo.Models.Status.Status;
using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Response
{
    public class VoucherResponse
    {
        public int Id { get; set; }
   
        public string Code { get; set; }
   
        public string Name { get; set; }
        public string Description { get; set; }

        public VoucherStatus Status { get; set; } = VoucherStatus.Active;
 
        public DateTime StartDate { get; set; }
 
        public DateTime EndDate { get; set; }
   
        public int Quantity { get; set; }
     
        public int DiscountValue { get; set; } //Giá trị giảm giá
     
        public decimal MinimumOrderValue { get; set; } //Giá trị đơn tối thiểu
     
        public decimal MaxDiscount { get; set; } //Giảm tối đa

        public VoucherResponse()
        {
        }

        public VoucherResponse(int id, string code, string name, string description, VoucherStatus status, DateTime startDate, DateTime endDate, int quantity, int discountValue, decimal minimumOrderValue, decimal maxDiscount)
        {
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
            Quantity = quantity;
            DiscountValue = discountValue;
            MinimumOrderValue = minimumOrderValue;
            MaxDiscount = maxDiscount;
        }
    }
}
