using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Customer;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Ultility;

namespace WebBanAoo.Service.impl
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        private ICustomerMapper _mapper;
        private readonly Validation<Customer> _validation;
        private readonly ICustomPasswordHasher _passwordHasher;

        public CustomerService(ApplicationDbContext context, ICustomerMapper mapper, Validation<Customer> validation, ICustomPasswordHasher passwordHasher)
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
                newCode = GenerateCode.GenerateCustomerCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Customers.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<CustomerResponse> CreateCustomerAsync(CustomerCreate create)
        {
            if (!await _validation.IsValidEmail(create.Email))
            {
                throw new Exception("Email không hợp lệ, Vui lòng nhập đúng định dạng");
            }
            if (!await _validation.IsValidPassword(create.Password))
            {
                throw new Exception("Password không hợp lệ, Vui lòng nhập đúng định dạng");
            }
            if (!await _validation.IsValidPhone(create.Phone))
            {
                throw new Exception("Phone không hợp lệ, Vui lòng nhập đúng định dạng tối thiểu 10 số");
            }

            Customer entity = _mapper.CreateToEntity(create);
            entity.Password = _passwordHasher.HashPassword(create.Password);

            if (!string.IsNullOrEmpty(create.Code) && create.Code != "string")
            {
                entity.Code = create.Code;
            }
            else
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            while (await _context.Customers.AnyAsync(p => p.Code == entity.Code))
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
            var coId = await _context.Customers.FindAsync(id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<IEnumerable<CustomerResponse>> GetAllCustomerAsync()
        {
            var co = await _context.Customers.OrderByDescending(x => x.CreateDate).ToListAsync();
            if (co == null) throw new Exception($"Khong co ban ghi nao");

            var response = _mapper.ListEntityToResponse(co);

            return response;
        }

        public async Task<bool> HardDeleteCustomerAsync(int id)
        {
            var co = await _context.Customers.FindAsync(id);
            if (co == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            _context.Customers.Remove(co);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CustomerResponse>> SearchCustomerByKeyAsync(string key)
        {
            var coKey = await _context.Customers    
               .FromSqlRaw("Select * from Customers where FullName like {0}", "%" + key + "%").ToListAsync();

            if (coKey == null) throw new Exception($"Khong co Customer ten {key} nao");
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
            var coId = await _context.Customers.FindAsync( id);
            if (coId == null) throw new KeyNotFoundException($"Khong co Id {id} ton tai");
            coId.Gender = changeGender;
            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<CustomerResponse> UpdateCustomerAsync(int id, CustomerUpdate update)
        {
            var coId = await _context.Customers.FindAsync( id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            coId.Code = await _validation.CheckAndUpdateAPIAsync(coId, coId.Code, update.Code, co => co.Code == update.Code);
            //coId.FullName = await _validation.CheckAndUpdateAPIAsync(coId, coId.FullName, update.FullName, co => co.FullName == update.FullName);
            coId.FullName = update.FullName;
            coId.Phone = await _validation.ValidateAndUpdateAsync(coId, coId.Phone, update.Phone, co => co.Phone == update.Phone, isPhone: true);
            coId.Email = await _validation.ValidateAndUpdateAsync(coId, coId.Email, update.Email, co => co.Email == update.Email, isEmail: true);
            //coId.Password = await _validation.ValidateAndUpdateAsync(coId, coId.Password, update.Password, co => co.Password == update.Password, isPassword: true);
            coId.Address = await _validation.CheckAndUpdateAPIAsync(coId, coId.Address, update.Address, co => co.Address == update.Address);
            coId.City = await _validation.CheckAndUpdateAPIAsync(coId, coId.City, update.City, co => co.City == update.City);
            coId.Image = await _validation.CheckAndUpdateAPIAsync(coId, coId.Image, update.Image, co => co.Image == update.Image);
            coId.Dob = await _validation.CheckAndUpdateDOBGeneralAsync(coId, coId.Dob, update.Dob);
            coId.Gender = update.Gender;
            var result = _mapper.UpdateToEntity(update);

            //coId.Gender = result.Gender;
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
