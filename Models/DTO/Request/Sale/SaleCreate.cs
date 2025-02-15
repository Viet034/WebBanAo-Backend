using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.Sale
{
    public class SaleCreate
    {


        public string Code { get; set; }

        public string SaleName { get; set; }

        public SaleStatus Status { get; set; } = SaleStatus.Scheduled;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public SaleCreate()
        {
        }

        public SaleCreate(string code, string saleName, SaleStatus status, DateTime startDate, DateTime endDate)
        {
            Code = code;
            SaleName = saleName;
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
