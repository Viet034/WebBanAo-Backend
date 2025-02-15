using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Customer;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Models.ultility;
using WebBanAoo.Models.Ultility;

namespace WebBanAoo.Service.impl
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        private ICustomerMapper _mapper;
        private readonly Validation<Customer> _validation;

        public CustomerService(ApplicationDbContext context, ICustomerMapper mapper, Validation<Customer> validation)
        {
            _context = context;
            _mapper = mapper;
            _validation = validation;
        }

        public async Task<string> CheckUniqueCodeAsync()
        {
            string newCode;
            bool isExist;

            do
            {
                newCode = GenerateCode.GenerateCustomerCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Customers.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<CustomerResponse> CreateCustomerAsync(CustomerCreate create)
        {
            Customer entity = _mapper.CreateToEntity(create);

            if (!string.IsNullOrEmpty(create.Code) && create.Code != "string")
            {
                entity.Code = create.Code;
            }
            else
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            while (await _context.Colors.AnyAsync(p => p.Code == entity.Code))
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            await _context.Customers.AddAsync(entity);

            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(entity);
            return response;
        }

        public async Task<CustomerResponse> FindCustomerByIdAsync(int id)
        {
            var coId = _context.Customers.FirstOrDefault(co => co.Id == id);
            if (coId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<IEnumerable<CustomerResponse>> GetAllCustomerAsync()
        {
            var co = await _context.Customers.ToListAsync();
            if (co == null) throw new Exception($"Khong co ban ghi nao");

            var response = _mapper.ListEntityToResponse(co);

            return response;
        }

        public async Task<bool> HardDeleteCustomerAsync(int id)
        {
            var co = _context.Customers.FirstOrDefault(co => co.Id == id);
            if (co == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            _context.Customers.Remove(co);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CustomerResponse>> SearchCustomerByKeyAsync(string key)
        {
            var coKey = await _context.Customers    
               .FromSqlRaw("Select * from Customers where FullName like {0}", "%" + key + "%").ToListAsync();

            if (coKey == null) throw new Exception($"Khong co Category ten {key} nao");
            var response = _mapper.ListEntityToResponse(coKey);
            return response;
        }

        public async Task<CustomerResponse> SoftDeleteCustomerAsync(int id, Status.CustomerStatus newStatus)
        {
            var coId = await _context.Customers.FindAsync(id);
            if (coId == null) throw new Exception($"Khong co Id {id} ton tai");

            coId.Status = newStatus;

            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<CustomerResponse> ChangeGenderAsync(int id, Status.Gender changeGender)
        {
            var coId = await _context.Customers.FirstOrDefaultAsync(co => co.Id == id);
            if (coId == null) throw new Exception($"Khong co Id {id} ton tai");
            coId.Gender = changeGender;
            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(int id, CustomerUpdate update)
        {
            var coId = await _context.Customers.FirstOrDefaultAsync(co => co.Id == id);
            if (coId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            coId.Code = await _validation.CheckAndUpdateAPIAsync(coId, coId.Code, update.Code, co => co.Code == update.Code);
            coId.FullName = await _validation.CheckAndUpdateAPIAsync(coId, coId.FullName, update.FullName, co => co.FullName == update.FullName);
            coId.Email = await _validation.CheckAndUpdateAPIAsync(coId, coId.Email, update.Email, co => co.Email == update.Email);
            coId.Password = await _validation.CheckAndUpdateAPIAsync(coId, coId.Password, update.Password, co => co.Password == update.Password);
            coId.Phone = await _validation.CheckAndUpdateAPIAsync(coId, coId.Phone, update.Phone, co => co.Phone == update.Phone);
            coId.Address = await _validation.CheckAndUpdateAPIAsync(coId, coId.Address, update.Address, co => co.Address == update.Address);
            coId.City = await _validation.CheckAndUpdateAPIAsync(coId, coId.City, update.City, co => co.City == update.City);
            coId.Image = await _validation.CheckAndUpdateAPIAsync(coId, coId.Image, update.Image, co => co.Image == update.Image);
            coId.Dob = await _validation.CheckAndUpdateDOBAsync(coId, coId.Dob, update.Dob);

            var result = _mapper.UpdateToEntity(update);
           
            coId.Gender = result.Gender;
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
