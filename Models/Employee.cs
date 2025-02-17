using WebBanAoo.Ultility;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models;

public class Employee : BaseEntity
{
    public string FullName { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Working;
    public DateTime Dob {  get; set; }
    public Gender Gender { get; set; } = Gender.Male;
    public string Image {  get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Country { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<Employee_Role> Employee_Roles { get; set; } = new List<Employee_Role>();
}
