using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Response;

public class CustomerResponse
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string FullName { get; set; }
    public DateTime Dob { get; set; }
    public Gender Gender { get; set; } = Gender.Male;
    public string Email { get; set; }
    
    public string Phone { get; set; }
    public CustomerStatus Status { get; set; } = CustomerStatus.Active;
    public string Address { get; set; }
    public string City { get; set; }
    public string Image { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }


    public CustomerResponse()
    {
    }

    public CustomerResponse(int customerId, string code, string fullName, DateTime dob, Gender gender, string email, string phone, CustomerStatus status, string address, string city, string image)
    {
        Id = customerId;
        Code = code;
        FullName = fullName;
        Dob = dob;
        Gender = gender;
        Email = email;
       
        Phone = phone;
        Status = status;
        Address = address;
        City = city;
        Image = image;
    }
}
