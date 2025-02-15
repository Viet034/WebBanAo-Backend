using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Response
{
    public class ColorResponse
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string ColorName { get; set; }

        public ColorStatus Status { get; set; } = ColorStatus.Available;

        public ColorResponse()
        {
        }

        public ColorResponse(int id, string code, string colorName, ColorStatus status)
        {
            Id = id;
            Code = code;
            ColorName = colorName;
            Status = status;
        }
    }
}
