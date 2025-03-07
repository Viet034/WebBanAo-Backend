using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Response
{
    public class CustomerVoucherResponse
    {
        public int Id { get; set; }
        public CustomerVoucherStatus Status { get; set; }
        public int CustomerId { get; set; }
        public int VoucherId { get; set; }
        public string CustomerName { get; set; }
        public string VoucherCode { get; set; }
        public string VoucherName { get; set; }
        public int DiscountValue { get; set; }
        public decimal MinimumOrderValue { get; set; }
        public decimal MaxDiscount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CustomerVoucherResponse()
        {
        }

        public CustomerVoucherResponse(int id, CustomerVoucherStatus status, int customerId, int voucherId, 
            string customerName, string voucherCode, string voucherName, int discountValue, 
            decimal minimumOrderValue, decimal maxDiscount, DateTime startDate, DateTime endDate)
        {
            Id = id;
            Status = status;
            CustomerId = customerId;
            VoucherId = voucherId;
            CustomerName = customerName;
            VoucherCode = voucherCode;
            VoucherName = voucherName;
            DiscountValue = discountValue;
            MinimumOrderValue = minimumOrderValue;
            MaxDiscount = maxDiscount;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
} 