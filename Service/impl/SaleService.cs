using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Sale;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Models.ultility;
using WebBanAoo.Models.Ultility;

namespace WebBanAoo.Service.impl
{
    public class SaleService : ISaleService
    {
        private readonly ApplicationDbContext _context;
        private ISaleMapper _mapper;
        private readonly Validation<Sale> _validation;

        public SaleService(ApplicationDbContext context, ISaleMapper mapper, Validation<Sale> validation)
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
                newCode = GenerateCode.GenerateSaleCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Sales.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<SaleResponse> CreateSaleAsync(SaleCreate create)
        {
            Sale entity = _mapper.CreateToEntity(create);

            if (create.StartDate == null || create.EndDate == null)
            {
                throw new Exception("Ngày bắt đầu và ngày kết thúc không được trống");
            }
            if(create.StartDate > create.EndDate)
            {
                throw new Exception("Ngày bắt đầu không thể lớn hơn này kết thúc");
            }
            

            if (!string.IsNullOrEmpty(create.Code) && create.Code != "string")
            {
                entity.Code = create.Code;
            }
            else
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            while (await _context.Sales.AnyAsync(p => p.Code == entity.Code))
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            await _context.Sales.AddAsync(entity);

            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(entity);
            return response;
        }

        public async Task<SaleResponse> FindSaleByIdAsync(int id)
        {
            var coId = _context.Sales.FirstOrDefault(co => co.Id == id);
            if (coId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<IEnumerable<SaleResponse>> GetAllSaleAsync()
        {
            var co = await _context.Sales.ToListAsync();
            if (co == null) throw new Exception($"Khong co ban ghi nao");

            var response = _mapper.ListEntityToResponse(co);

            return response;
        }

        public async Task<bool> HardDeleteSaleAsync(int id)
        {
            var co = _context.Sales.FirstOrDefault(co => co.Id == id);
            if (co == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            _context.Sales.Remove(co);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SaleResponse>> SearchSaleByKeyAsync(string key)
        {
            var coKey = await _context.Sales
               .FromSqlRaw("Select * from Sales where SaleName like {0}", "%" + key + "%").ToListAsync();

            if (coKey == null) throw new Exception($"Khong co Code {key} nao");
            var response = _mapper.ListEntityToResponse(coKey);
            return response;
        }

        public async Task<SaleResponse> SoftDeleteSaleAsync(int id, Status.SaleStatus newStatus)
        {
            var coId = await _context.Sales.FindAsync(id);
            if (coId == null) throw new Exception($"Khong co Id {id} ton tai");

            coId.Status = newStatus;

            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<SaleResponse> UpdateSaleAsync(int id, SaleUpdate update)
        {
            var coId = await _context.Sales.FirstOrDefaultAsync(co => co.Id == id);
            if (coId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }
            coId.Code = await _validation.CheckAndUpdateAPIAsync(coId, coId.Code, update.Code, co => co.Code == update.Code);
            coId.SaleName = await _validation.CheckAndUpdateAPIAsync(coId, coId.SaleName, update.SaleName, co => co.SaleName == update.SaleName);
            coId.StartDate = await _validation.CheckAndUpdateDateAsync(coId, coId.StartDate, update.StartDate, coId.EndDate, true);
            coId.EndDate = await _validation.CheckAndUpdateDateAsync(coId, coId.EndDate, update.EndDate, coId.StartDate, false);
            
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
