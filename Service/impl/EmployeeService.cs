using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Employee;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Models.ultility;

namespace WebBanAoo.Service.impl
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private IEmployeeMapper _mapper;

        public EmployeeService(ApplicationDbContext context, IEmployeeMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            Employee entity = _mapper.CreateToEntity(create);

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
            var coId = _context.Employees.FirstOrDefault(co => co.Id == id);
            if (coId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<IEnumerable<EmployeeResponse>> GetAllEmployeeAsync()
        {
            var co = await _context.Employees.ToListAsync();
            if (co == null) throw new Exception($"Khong co ban ghi nao");

            var response = _mapper.ListEntityToResponse(co);

            return response;
        }

        public async Task<bool> HardDeleteEmployeeAsync(int id)
        {
            var co = _context.Employees.FirstOrDefault(co => co.Id == id);
            if (co == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
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
            if (coId == null) throw new Exception($"Khong co Id {id} ton tai");

            coId.Status = newStatus;

            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }
        public async Task<EmployeeResponse> ChangeGenderAsync(int id, Status.Gender changeGender)
        {
            var coId = await _context.Employees.FirstOrDefaultAsync(co => co.Id == id);
            if (coId == null) throw new Exception($"Khong co Id {id} ton tai");
            coId.Gender = changeGender;
            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(coId);
            return response;
        }
        public async Task<EmployeeResponse> UpdateEmployeeAsync(int id, EmployeeUpdate update)
        {
            var coId = await _context.Employees.FirstOrDefaultAsync(co => co.Id == id);
            if (coId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            if (!string.IsNullOrEmpty(update.Code) && update.Code != "string" && update.Code != coId.Code)
            {
                bool isExist = await _context.Employees.AnyAsync(p => p.Code == update.Code);
                if (isExist)
                {
                    throw new Exception();
                }
                coId.Code = update.Code;
            }
            else
            {
                update.Code = coId.Code;
            }
            var result = _mapper.UpdateToEntity(update);
            
            coId.FullName = result.FullName;
            coId.Dob = result.Dob;
            coId.Gender = result.Gender;
            coId.Email = result.Email;
            coId.Password = result.Password;
            coId.Phone = result.Phone;
            coId.Address = result.Address;
            coId.Country = result.Country;
            coId.Image = result.Image;
            coId.Status = result.Status;
            coId.StartDate = result.StartDate;
            coId.EndDate = result.EndDate;
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
