using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Category;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models.Status;
using WebBanAoo.Ultility;

namespace WebBanAoo.Service.impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private ICategoryMapper _mapper;
        private readonly Validation<Category> _validation;

        public CategoryService(ApplicationDbContext context, ICategoryMapper mapper, Validation<Category> validation)
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
                newCode = GenerateCode.GenerateCategoryCode();
                _context.ChangeTracker.Clear();
                isExist = await _context.Categories.AnyAsync(p => p.Code == newCode);
            }
            while (isExist);

            return newCode;
        }

        public async Task<CategoryResponse> CreateCategoryAsync(CategoryCreate create)
        {
            Category entity = _mapper.CreateToEntity(create);
            if (!string.IsNullOrEmpty(create.Code) && create.Code != "string")
            {
                entity.Code = create.Code;
            }
            else
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            while (await _context.Categories.AnyAsync(p => p.Code == entity.Code))
            {
                entity.Code = await CheckUniqueCodeAsync();
            }

            await _context.Categories.AddAsync(entity);

            await _context.SaveChangesAsync();
            var response = _mapper.EntityToResponse(entity);
            return response;
        }

        public async Task<CategoryResponse> FindCategoryByIdAsync(int id)
        {
            var coId = await _context.Categories.FindAsync( id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoryAsync()
        {
            var co = await _context.Categories.ToListAsync();
            if (co == null) throw new Exception($"Khong co ban ghi nao");

            var response = _mapper.ListEntityToResponse(co);

            return response;
        }

        public async Task<bool> HardDeleteCategoryAsync(int id)
        {
            var co = await _context.Categories.FindAsync( id);
            if (co == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            _context.Categories.Remove(co);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryResponse>> SearchCategoryByKeyAsync(string key)
        {
            var coKey = await _context.Categories
                .FromSqlRaw("Select * from Categories where CategoryName like {0}", "%" + key + "%").ToListAsync();

            if (coKey == null) throw new Exception($"Khong co Category ten {key} nao");
            var response = _mapper.ListEntityToResponse(coKey);
            return response;
        }

        public async Task<CategoryResponse> SoftDeleteCategoryAsync(int id, Status.CategoryStatus newStatus)
        {
            var coId = await _context.Categories.FindAsync(id);
            if (coId == null) throw new Exception($"Khong co Id {id} ton tai");

            coId.Status = newStatus;

            await _context.SaveChangesAsync();

            var response = _mapper.EntityToResponse(coId);
            return response;
        }

        public async Task<CategoryResponse> UpdateCategoryAsync(int id, CategoryUpdate update)
        {
            var coId = await _context.Categories.FindAsync( id);
            if (coId == null)
            {
                throw new KeyNotFoundException($" Khong co Id {id} ton tai");
            }
            //check validation
            coId.Code = await _validation
                .CheckAndUpdateAPIAsync(coId, coId.Code, update.Code, cat => cat.Code == update.Code);
            
            coId.CategoryName = await _validation
                .CheckAndUpdateAPIAsync(coId, coId.CategoryName, update.CategoryName, cat => cat.CategoryName == update.CategoryName);
            
            coId.Description = await _validation
                .CheckAndUpdateAPIAsync(coId, coId.Description, update.Description, cat => cat.Description == update.Description);
             
            
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
