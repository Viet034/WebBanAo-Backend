using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Brand;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Ultility;

namespace WebBanAoo.Service.impl
{
    public class BrandService : IBrandService
    {
        private readonly ApplicationDbContext _context;
        private  IBrandMapper _mapper;
        private readonly Validation<Brand> _validation;

        public BrandService(ApplicationDbContext context, IBrandMapper mapper, Validation<Brand> validation)
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
                newCode = GenerateCode.GenerateBrandCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Brands.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<BrandResponse> CreateBrandAsync(BrandCreate create)
        {
            Brand entity = _mapper.CreateToEntity(create);
            if (!string.IsNullOrEmpty(create.Code) && create.Code != "string")
            {
                entity.Code = create.Code;
            }
            else
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            while (await _context.Brands.AnyAsync(p => p.Code == entity.Code))
            {
                entity.Code = await CheckUniqueCodeAsync();
            }


            await _context.Brands.AddAsync(entity);

            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(entity);
            return response;
        }

        public async Task<BrandResponse> FindBrandByIdAsync(int id)
        {
            var coId = await  _context.Brands.FindAsync(id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<IEnumerable<BrandResponse>> GetAllBrandsAsync()
        {
            var co = await _context.Brands.ToListAsync();
            if (co == null) throw new Exception($"Khong co ban ghi nao");

            var response = _mapper.ListEntityToResponse(co);

            return response;
        }

        public async Task<bool> HardDeleteBrandAsync(int id)
        {
            var co = await _context.Brands.FindAsync( id);
            if (co == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            _context.Brands.Remove(co);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BrandResponse>> SearchBrandByKeyAsync(string key)
        {
            var coKey = await _context.Brands
                .FromSqlRaw("Select * from Brands where BrandName like {0}", "%" + key + "%").ToListAsync();

            if (coKey == null) throw new Exception($"Khong co brand ten {key} nao");
            var response = _mapper.ListEntityToResponse(coKey);
            return response;
        }

        public async Task<BrandResponse> UpdateBrandAsync(int id, BrandUpdate update)
        {
            var coId = await _context.Brands.FindAsync( id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            //check validation
            coId.Code = await _validation.CheckAndUpdateAPIAsync(coId, coId.Code, update.Code, b => b.Code == update.Code);
            coId.BrandName = await _validation.CheckAndUpdateAPIAsync(coId, coId.BrandName, update.BrandName, b => b.BrandName == update.BrandName);
            
            var result = _mapper.UpdateToEntity(update);
            
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
