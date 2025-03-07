using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Employee;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Mapper.impl
{
    public class EmployeeMapper : IEmployeeMapper
    {
        private readonly Employee emp = new Employee();
        public Employee CreateToEntity(EmployeeCreate create)
        {
            emp.Code = create.Code;
            emp.FullName = create.FullName;
            emp.Status = create.Status;
            emp.Dob = create.Dob;
            emp.Gender = create.Gender;
            emp.Image = create.Image;
            emp.Email = create.Email;
            emp.Password = create.Password;
            emp.Phone = create.Phone;
            emp.Address = create.Address;
            emp.City = create.City;
            emp.StartDate = create.StartDate;
            emp.EndDate = create.EndDate;
            
            emp.CreatedBy = "System";
            emp.CreateDate = DateTime.Now.AddHours(7);
            emp.UpdateDate = DateTime.Now.AddHours(7);
            emp.UpdateBy = "System";
            return emp;
        }

        public Employee DeleteToEntity(EmployeeDelete delete)
        {
            emp.Id = delete.Id;
            emp.Code = delete.Code;
            emp.FullName = delete.FullName;
            emp.Status = delete.Status;
            emp.Dob = delete.Dob;
            emp.Gender = delete.Gender;
            emp.Image = delete.Image;
            emp.Email = delete.Email;
            emp.Password = delete.Password;
            emp.Phone = delete.Phone;
            emp.Address = delete.Address;
            emp.City = delete.Country;
            emp.StartDate = delete.StartDate;
            emp.EndDate = delete.EndDate;
            emp.CreatedBy = "System";
            emp.CreateDate = DateTime.Now.AddHours(7);
            emp.UpdateDate = DateTime.Now.AddHours(7);
            emp.UpdateBy = "System";
            return emp;
        }

        public EmployeeResponse EntityToResponse(Employee entity)
        {
            EmployeeResponse response = new EmployeeResponse();
            response.Id = entity.Id;
            response.Code = entity.Code;
            response.FullName = entity.FullName;
            response.Status = entity.Status;
            response.Dob = entity.Dob;
            response.Gender = entity.Gender;
            response.Image = entity.Image;
            response.Email = entity.Email;
            
            response.Phone = entity.Phone;
            response.Address = entity.Address;
            response.City = entity.City;
            response.StartDate = entity.StartDate;
            response.EndDate = entity.EndDate;
            response.RefreshToken = entity.RefreshToken;
            response.RefreshTokenExpiryTime = entity.RefreshTokenExpiryTime;
            return response;
        }

        public IEnumerable<EmployeeResponse> ListEntityToResponse(IEnumerable<Employee> entities)
        {
            return entities.Select(x => EntityToResponse(x)).ToList();
        }

        public Employee UpdateToEntity(EmployeeUpdate update)
        {
            emp.Code = update.Code;
            emp.FullName = update.FullName;
            emp.Status = update.Status;
            emp.Dob = update.Dob;
            emp.Gender = update.Gender;
            emp.Image = update.Image;
            emp.Email = update.Email;
            emp.Password = update.Password;
            emp.Phone = update.Phone;
            emp.Address = update.Address;
            emp.City = update.City;
            //emp.StartDate = update.StartDate;
            //emp.EndDate = update.EndDate;
            emp.CreatedBy = "System";
            //emp.CreateDate = DateTime.Now.AddHours(7);
            //emp.UpdateDate = DateTime.Now.AddHours(7);
            emp.UpdateBy = "System";
            return emp;
        }
    }
}
