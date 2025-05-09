﻿using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Employee;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Ultility;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace WebBanAoo.Service.impl
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private IEmployeeMapper _mapper;
        private readonly Validation<Employee> _validation;
        private readonly ICustomPasswordHasher _passwordHasher;

        public EmployeeService(ApplicationDbContext context, IEmployeeMapper mapper, Validation<Employee> validation, ICustomPasswordHasher passwordHasher)
        {
            _context = context;
            _mapper = mapper;
            _validation = validation;
            _passwordHasher = passwordHasher;
        }

        

        public async Task<string> CheckUniqueCodeAsync()
        {
            string newCode;
            bool isExist;

            do
            {
                newCode = GenerateCode.GenerateEmployeeCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Employees.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<EmployeeResponse> CreateEmployeeAsync(EmployeeCreate create)
        {
            //Check Email, Password
            if (!await _validation.IsValidEmail(create.Email))
            {
                throw new Exception("Email không hợp lệ, Vui lòng nhập đúng định dạng 'vidu@gmail.com' ");
            }
            if (!await _validation.IsValidPassword(create.Password))
            {
                throw new Exception("Password không hợp lệ, Password cần chứa ít nhất 1 chữ cái viết hoa, 1 kí tự đặc biệt, tối thiểu chứa 6 kí tự");
            }
            if(!await _validation.IsValidPhone(create.Phone))
            {
                throw new Exception("Phone không hợp lệ, Vui lòng nhập đúng định dạng tối thiểu 10 số");
            }
            // Kiểm tra email đã tồn tại
            if (await _context.Employees.AnyAsync(x => x.Email == create.Email))
            {
                throw new Exception("Email đã tồn tại");
            }
            if (await _context.Employees.AnyAsync(x => x.Phone == create.Phone))
            {
                throw new Exception("Số điện thoại đã được sử dụng");
            }
            
            if (string.IsNullOrEmpty(create.FullName))
            {
                throw new Exception("Không được để trống tên");
            }
            create.FullName = Regex.Replace(create.FullName.Trim(), @"\s+", " ");
            if (!Regex.IsMatch(create.FullName, @"^[a-zA-ZÀ-Ỹà-ỹ\s]+$"))
            {
                throw new Exception("Tên không được chứa kí tự đặc biệt");
            }
            var today = DateTime.Today;
            var age = today.Year - create.Dob.Year;
            if (create.Dob.Date > today.AddYears(-age))
            {
                age--;
            }

            if (age < 18)
            {
                throw new Exception("Nhân viên phải từ 18 tuổi trở lên");
            }
            
            Employee entity = _mapper.CreateToEntity(create);
            entity.Password = _passwordHasher.HashPassword(create.Password);
            
            //Check trùng Code
            if (string.IsNullOrEmpty(entity.Code) || entity.Code == "string")
            {
                entity.Code = await CheckUniqueCodeAsync();
            }
            while (await _context.Employees.AnyAsync(p => p.Code == entity.Code))
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            await _context.Employees.AddAsync(entity);

            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(entity);
            return response;
        }

        public async Task<EmployeeResponse> FindEmployeeByIdAsync(int id)
        {
            var coId = await _context.Employees.FindAsync(id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<IEnumerable<EmployeeResponse>> GetAllEmployeeAsync()
        {
            var co = await _context.Employees.OrderByDescending(x => x.CreateDate).ToListAsync();
            if (co == null) throw new Exception($"Khong co ban ghi nao");

            var response = _mapper.ListEntityToResponse(co);

            return response;
        }

        public async Task<bool> HardDeleteEmployeeAsync(int id)
        {
            var co = await _context.Employees.FindAsync(id);
            if (co == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            _context.Employees.Remove(co);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EmployeeResponse>> SearchEmployeeByKeyAsync(string key)
        {
            var coKey = await _context.Employees
               .FromSqlRaw("Select * from Employees where FullName like {0}", "%" + key + "%").ToListAsync();

            if (coKey == null) throw new Exception($"Khong co Employee ten {key} nao");
            var response = _mapper.ListEntityToResponse(coKey);
            return response;
        }

        public async Task<EmployeeResponse> SoftDeleteEmployeeAsync(int id, Status.EmployeeStatus newStatus)
        {
            var coId = await _context.Employees.FindAsync(id);
            if (coId == null) throw new KeyNotFoundException($"Khong co Id {id} ton tai");

            coId.Status = newStatus;

            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }
        public async Task<EmployeeResponse> ChangeGenderAsync(int id, Status.Gender changeGender)
        {
            var coId = await _context.Employees.FindAsync(id);
            if (coId == null) throw new KeyNotFoundException($"Khong co Id {id} ton tai");
            coId.Gender = changeGender;
            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(coId);
            return response;
        }
        public async Task<EmployeeResponse> UpdateEmployeeAsync(int id, EmployeeUpdate update)
        {
            var coId = await _context.Employees.FindAsync(id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }

            coId.Code = await _validation.CheckAndUpdateAPIAsync(coId, coId.Code, update.Code, co => co.Code == update.Code);
            //coId.FullName = await _validation.CheckAndUpdateAPIAsync(coId, coId.FullName, update.FullName, co => co.FullName == update.FullName);
            coId.FullName = update.FullName;
            coId.Address = await _validation.CheckAndUpdateAPIAsync(coId, coId.Address, update.Address, co => co.Address == update.Address);
            coId.City = await _validation.CheckAndUpdateAPIAsync(coId, coId.City, update.City, co => co.City == update.City);
            //coId.StartDate = await _validation.CheckAndUpdateDateGeneralAsync(coId, coId.StartDate, update.StartDate, coId.StartDate, true);
            //coId.EndDate = await _validation.CheckAndUpdateDateEmployeeAsync(coId, coId.EndDate, update.EndDate, coId.EndDate, false);
            coId.Dob = await _validation.CheckAndUpdateDOBGeneralAsync(coId, coId.Dob, update.Dob);
            coId.Phone = await _validation.ValidateAndUpdateAsync(coId, coId.Phone, update.Phone, co => co.Phone == update.Phone, isPhone: true);
            coId.Email = await _validation.ValidateAndUpdateAsync(coId, coId.Email, update.Email, co => co.Email == update.Email, isEmail: true);
            //coId.Password = await _validation.ValidateAndUpdateAsync(coId, coId.Password, update.Password, co => co.Password == update.Password, isPassword: true);
            coId.Image = await _validation.CheckAndUpdateAPIAsync(coId, coId.Image, update.Image, co => co.Image == update.Image);
            coId.Gender = update.Gender;

            var result = _mapper.UpdateToEntity(update);


            
            
            
            coId.Status = result.Status;
            
            coId.CreateDate = result.CreateDate;
            coId.UpdateDate = result.UpdateDate;
            coId.CreatedBy = result.CreatedBy;
            coId.UpdateBy = result.UpdateBy;


            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }
    }
}
