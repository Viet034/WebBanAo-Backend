using static WebBanAoo.Models.Status.Status;
using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Request.Role
{
    public class RoleDelete
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public RoleStatus Status { get; set; } = RoleStatus.Active;

        public string Name { get; set; }

        public string Description { get; set; }

        public RoleDelete()
        {
        }

        public RoleDelete(int id, string code, RoleStatus status, string name, string description)
        {
            Id = id;
            Code = code;
            Status = status;
            Name = name;
            Description = description;
        }
    }
}
