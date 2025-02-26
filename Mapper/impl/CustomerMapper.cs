using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Customer;
using WebBanAoo.Models.DTO.Response;
using System.Drawing;

namespace WebBanAoo.Mapper.impl
{
    public class CustomerMapper : ICustomerMapper
    {
        private readonly Customer cus = new Customer();
        public Customer CreateToEntity(CustomerCreate create)
        {
            cus.Code = create.Code;
            cus.FullName = create.FullName;
            cus.Dob = create.Dob;
            cus.Status = create.Status;
            cus.Gender = create.Gender;
            cus.Email = create.Email;
            cus.Password = create.Password;
            cus.Phone = create.Phone;
            cus.Address = create.Address;
            cus.City = create.City;
            cus.Image = create.Image;
            cus.CreatedBy = "System";
            cus.CreateDate = DateTime.Now.AddHours(7);
            cus.UpdateDate = DateTime.Now.AddHours(7);
            cus.UpdateBy = "System";
            return cus;


        }

        public Customer DeleteToEntity(CustomerDelete delete)
        {
            cus.Id = delete.Id;
            cus.Code = delete.Code;
            cus.FullName = delete.FullName;
            cus.Dob = delete.Dob;
            cus.Status = delete.Status;
            cus.Gender = delete.Gender;
            cus.Email = delete.Email;
            cus.Password = delete.Password;
            cus.Phone = delete.Phone;
            cus.Address = delete.Address;
            cus.City = delete.City;
            cus.Image = delete.Image;
            cus.CreatedBy = "System";
            cus.CreateDate = DateTime.Now.AddHours(7);
            cus.UpdateDate = DateTime.Now.AddHours(7);
            cus.UpdateBy = "System";
            return cus;
        }

        public CustomerResponse EntityToResponse(Customer entity)
        {
            CustomerResponse response = new CustomerResponse();
            response.Id = entity.Id;
            response.Code = entity.Code;
            response.FullName = entity.FullName;
            response.Dob = entity.Dob;
            response.Status = entity.Status;
            response.Gender = entity.Gender;
            response.Email = entity.Email;
            
            response.Phone = entity.Phone;
            response.Address = entity.Address;
            response.City = entity.City;
            response.Image = entity.Image;
            response.RefreshToken = entity.RefreshToken;
            response.RefreshTokenExpiryTime = entity.RefreshTokenExpiryTime;
            return response;
        }

        public IEnumerable<CustomerResponse> ListEntityToResponse(IEnumerable<Customer> entities)
        {
            return entities.Select(x => EntityToResponse(x)).ToList();
        }

        public Customer UpdateToEntity(CustomerUpdate update)
        {
            cus.Code = update.Code;
            cus.FullName = update.FullName;
            cus.Dob = update.Dob;
            cus.Status = update.Status;
            cus.Gender = update.Gender;
            cus.Email = update.Email;
            cus.Password = update.Password;
            cus.Phone = update.Phone;
            cus.Address = update.Address;
            cus.City = update.City;
            cus.Image = update.Image;
            cus.CreatedBy = "System";
            cus.CreateDate = DateTime.Now.AddHours(7);
            cus.UpdateDate = DateTime.Now.AddHours(7);
            cus.UpdateBy = "System";
            return cus;
        }
    }
}
