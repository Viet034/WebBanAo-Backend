using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Color;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Ultility;

namespace WebBanAoo.Service.impl
{
    public class ColorService : IColorService
    {
        private readonly ApplicationDbContext _context;
        private IColorMapper _mapper;
        private readonly Validation<Color> _validation;

        public ColorService(ApplicationDbContext context, IColorMapper mapper, Validation<Color> validation)
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
                newCode = GenerateCode.GenerateColorCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Colors.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<ColorResponse> CreateProductColorAsync(ColorCreate create)
        {
            Color entity = _mapper.CreateToEntity(create);
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

            await _context.Colors.AddAsync(entity);

            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(entity);
            return response;
        }

        public async Task<ColorResponse> FindProductColorByIdAsync(int id)
        {
            var coId =await _context.Colors.FindAsync(id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<IEnumerable<ColorResponse>> GetAllColorProductsAsync()
        {
            var co = await _context.Colors.ToListAsync();
            if (co == null) throw new Exception($"Khong co ban ghi nao");

            var response = _mapper.ListEntityToResponse(co);

            return response;
        }

        public async Task<bool> HardDeleteProductColorAsync(int id)
        {
            var co = await _context.Colors.FindAsync( id);
            if (co == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            _context.Colors.Remove(co);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ColorResponse>> SearchColorByKeyAsync(string key)
        {
            var coKey = await _context.Colors
                .FromSqlRaw("Select * from Colors where ColorName like {0}", "%" + key + "%").ToListAsync();

            if (coKey == null) throw new Exception($"Khong co mau ten {key} nao");
            var response = _mapper.ListEntityToResponse(coKey);
            return response;
        }

        public async Task<ColorResponse> SoftDeleteProductColorAsync(int id, Status.ColorStatus newStatus)
        {
            var coId = await _context.Colors.FindAsync(id);
            if (coId == null) throw new Exception($"Khong co Id {id} ton tai");

            coId.Status = newStatus;

            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<ColorResponse> UpdateProductColorAsync(int id, ColorUpdate update)
        {
            var coId = await  _context.Colors.FindAsync(id);
            if (coId == null)
            {
                throw new Exception($" Khong co Id {id} ton tai");
            }

            coId.Code = await _validation.CheckAndUpdateAPIAsync(coId, coId.Code, update.Code, co => co.Code == update.Code);
            coId.ColorName = await _validation.CheckAndUpdateAPIAsync(coId, coId.ColorName, update.ColorName, co => co.ColorName == update.ColorName);
            
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
