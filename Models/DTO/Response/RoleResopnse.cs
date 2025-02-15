using static WebBanAoo.Models.Status.Status;
using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Response
{
    public class RoleResopnse
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public RoleStatus Status { get; set; } = RoleStatus.Active;

        public string Name { get; set; }

        public string Description { get; set; }

        public RoleResopnse()
        {
        }

        public RoleResopnse(int id, string code, RoleStatus status, string name, string description)
        {
            Id = id;
            Code = code;
            Status = status;
            Name = name;
            Description = description;
        }
    }
}
