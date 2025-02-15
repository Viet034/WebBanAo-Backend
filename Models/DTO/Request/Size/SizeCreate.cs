using static WebBanAoo.Models.Status.Status;
using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Request.Size
{
    public class SizeCreate
    {
 

        public string Code { get; set; }

        public SizeStatus Status { get; set; } = SizeStatus.Available;
        public SizeCode SizeCode { get; set; } = SizeCode.M;

        public SizeCreate()
        {
        }

        public SizeCreate(string code, SizeStatus status, SizeCode sizeCode)
        {
            Code = code;
            Status = status;
            SizeCode = sizeCode;
        }
    }
}
