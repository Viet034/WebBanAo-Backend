﻿using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.Employee;

public class EmployeeDelete
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string FullName { get; set; }
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Working;
    public DateTime Dob { get; set; }
    public Gender Gender { get; set; }
    public string? Image { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Country { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public EmployeeDelete()
    {
    }

    public EmployeeDelete(int id, string code, string fullName, EmployeeStatus status, DateTime dob, Gender gender, string image, string email, string password, string phone, string address, string country, DateTime startDate, DateTime? endDate)
    {
        Id = id;
        Code = code;
        FullName = fullName;
        Status = status;
        Dob = dob;
        Gender = gender;
        Image = image;
        Email = email;
        Password = password;
        Phone = phone;
        Address = address;
        Country = country;
        StartDate = startDate;
        EndDate = endDate;
    }
}
