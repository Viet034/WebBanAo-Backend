using WebBanAoo.Ultility;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models;

public class Role : BaseEntity
{
    public RoleStatus Status { get; set; } = RoleStatus.Active;
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual ICollection<Employee_Role> Employee_Roles { get; set; } = new List<Employee_Role>();
}
