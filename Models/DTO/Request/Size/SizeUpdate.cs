using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.Size
{
    public class SizeUpdate
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public SizeStatus Status { get; set; } = SizeStatus.Available;
        public SizeCode SizeCode { get; set; } = SizeCode.M;

        public SizeUpdate()
        {
        }

        public SizeUpdate(int id, string code, SizeStatus status, SizeCode sizeCode)
        {
            Id = id;
            Code = code;
            Status = status;
            SizeCode = sizeCode;
        }
    }
}
