using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Size;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Ultility;

namespace WebBanAoo.Service.impl
{
    public class SizeService : ISizeService
    {
        private readonly ApplicationDbContext _context;
        private ISizeMapper _mapper;
        private readonly Validation<Size> _validation;

        public SizeService(ApplicationDbContext context, ISizeMapper mapper, Validation<Size> validation)
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
                newCode = GenerateCode.GenerateSizeCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Sizes.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<SizeResponse> CreateSizeAsync(SizeCreate create)
        {
            Size entity = _mapper.CreateToEntity(create);

            if (!string.IsNullOrEmpty(create.Code) && create.Code != "string")
            {
                entity.Code = create.Code;
            }
            else
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            while (await _context.Sizes.AnyAsync(p => p.Code == entity.Code))
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            await _context.Sizes.AddAsync(entity);

            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(entity);
            return response;
        }

        public async Task<SizeResponse> FindSizeByIdAsync(int id)
        {
            var coId = await _context.Sizes.FindAsync( id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<IEnumerable<SizeResponse>> GetAllSizeAsync()
        {
            var co = await _context.Sizes.ToListAsync();
            if (co == null) throw new Exception($"Khong co ban ghi nao");

            var response = _mapper.ListEntityToResponse(co);

            return response;
        }

        public async Task<bool> HardDeleteSizeAsync(int id)
        {
            var co = await _context.Sizes.FindAsync( id);
            if (co == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            _context.Sizes.Remove(co);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SizeResponse>> SearchSizeByKeyAsync(string key)
        {
            var coKey = await _context.Sizes
               .FromSqlRaw("Select * from Sizes where SizeCode like {0}", "%" + key + "%").ToListAsync();

            if (coKey == null) throw new Exception($"Khong co Code {key} nao");
            var response = _mapper.ListEntityToResponse(coKey);
            return response;
        }

        public async Task<SizeResponse> SoftDeleteSizeAsync(int id, Status.SizeStatus newStatus)
        {
            var coId = await _context.Sizes.FindAsync(id);
            if (coId == null) throw new KeyNotFoundException($"Khong co Id {id} ton tai");

            coId.Status = newStatus;

            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<SizeResponse> UpdateSizeAsync(int id, SizeUpdate update)
        {
            var coId = await _context.Sizes.FindAsync( id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            coId.Code = await _validation.CheckAndUpdateAPIAsync(coId, coId.Code, update.Code, co => co.Code == update.Code);
           
            var result = _mapper.UpdateToEntity(update);
            
            coId.Status = result.Status;
            coId.SizeCode = result.SizeCode;
            
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
