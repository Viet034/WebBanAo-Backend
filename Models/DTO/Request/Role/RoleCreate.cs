using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.Role
{
    public class RoleCreate
    {
        public string Code { get; set; }

        public RoleStatus Status { get; set; } = RoleStatus.Active;

        public string Name { get; set; }

        public string Description { get; set; }

        public RoleCreate()
        {
        }

        public RoleCreate(string code, RoleStatus status, string name, string description)
        {
            Code = code;
            Status = status;
            Name = name;
            Description = description;
        }
    }
}
