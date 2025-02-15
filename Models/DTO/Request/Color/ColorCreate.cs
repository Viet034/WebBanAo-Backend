using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.Color
{
    public class ColorCreate
    {
        public string Code { get; set; }

        public string ColorName { get; set; }

        public ColorStatus Status { get; set; } = ColorStatus.Available;

        public ColorCreate()
        {
        }

        public ColorCreate(string code, string colorName, ColorStatus status)
        {
            Code = code;
            ColorName = colorName;
            Status = status;
        }
    }
}
