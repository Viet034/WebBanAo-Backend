using static WebBanAoo.Models.Status.Status;

 namespace WebBanAoo.Models.DTO.Request.Customer;

public class CustomerUpdate
{
    public int Id { get; set; } 
    public string Code { get; set; }
    public string FullName { get; set; }
    public DateTime Dob { get; set; }
    public Gender Gender { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public CustomerStatus Status { get; set; } = CustomerStatus.Active;
    public string Address { get; set; }
    public string City { get; set; }
    public string Image { get; set; }

    public CustomerUpdate(int id, string code, string fullName, DateTime dob, Gender gender, string email, string password, string phone, CustomerStatus status, string address, string city, string image)
    {
        Id = id;
        Code = code;
        FullName = fullName;
        Dob = dob;
        Gender = gender;
        Email = email;
        Password = password;
        Phone = phone;
        Status = status;
        Address = address;
        City = city;
        Image = image;
    }
}
