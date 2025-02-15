using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.Voucher
{
    public class VoucherCreate
    {


        public string Code { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public VoucherStatus Status { get; set; } = VoucherStatus.Active;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Quantity { get; set; }

        public decimal DiscountValue { get; set; } //Giá trị giảm giá

        public decimal MinimumOrderValue { get; set; } //Giá trị đơn tối thiểu

        public decimal MaxDiscount { get; set; } //Giảm tối đa

        public VoucherCreate()
        {
        }

        public VoucherCreate(string code, string name, string description, VoucherStatus status, DateTime startDate, DateTime endDate, int quantity, decimal discountValue, decimal minimumOrderValue, decimal maxDiscount)
        {
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
