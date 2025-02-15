using System.ComponentModel.DataAnnotations;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO;

public class RoleDTO
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [EnumDataType(typeof(RoleStatus))]
    public RoleStatus Status { get; set; } = RoleStatus.Active;
    [Required(ErrorMessage = "Role Name is Missing")]
    public string Name { get; set; }

    public string Description { get; set; }
}
