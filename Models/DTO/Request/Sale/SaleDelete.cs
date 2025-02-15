using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.Sale
{
    public class SaleDelete
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string SaleName { get; set; }

        public SaleStatus Status { get; set; } = SaleStatus.Scheduled;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public SaleDelete()
        {
        }

        public SaleDelete(int id, string code, string saleName, SaleStatus status, DateTime startDate, DateTime endDate)
        {
            Id = id;
            Code = code;
            SaleName = saleName;
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
