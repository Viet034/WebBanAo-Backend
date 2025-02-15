using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.Role
{
    public class RoleUpdate
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public RoleStatus Status { get; set; } = RoleStatus.Active;

        public string Name { get; set; }

        public string Description { get; set; }

        public RoleUpdate()
        {
        }

        public RoleUpdate(int id, string code, RoleStatus status, string name, string description)
        {
            Id = id;
            Code = code;
            Status = status;
            Name = name;
            Description = description;
        }
    }
}
