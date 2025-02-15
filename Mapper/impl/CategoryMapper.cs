using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Category;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Mapper.impl
{
    public class CategoryMapper : ICategoryMapper
    {
        private readonly Category cat = new Category();
        public Category CreateToEntity(CategoryCreate create)
        {
            cat.Code = create.Code;
            cat.CategoryName = create.CategoryName;
            cat.Description = create.Description;
            cat.Status = create.Status;
            cat.CreatedBy = "System";
            cat.CreateDate = DateTime.Now.AddHours(7);
            cat.UpdateDate = DateTime.Now.AddHours(7);
            cat.UpdateBy = "System";
            return cat;
        }

        public Category DeleteToEntity(CategoryDelete delete)
        {
            cat.Id = delete.Id;
            cat.Code = delete.Code;
            cat.CategoryName = delete.CategoryName;
            cat.Description = delete.Description;
            cat.Status = delete.Status;
            cat.CreatedBy = "System";
            cat.CreateDate = DateTime.Now.AddHours(7);
            cat.UpdateDate = DateTime.Now.AddHours(7);
            cat.UpdateBy = "System";
            return cat;
        }

        public CategoryResponse EntityToResponse(Category entity)
        {
            CategoryResponse response = new CategoryResponse();
            response.Id = entity.Id;
            response.Code = entity.Code;
            response.CategoryName = entity.CategoryName;
            response.Description = entity.Description;
            response.Status = entity.Status;
            return response;
        }

        public IEnumerable<CategoryResponse> ListEntityToResponse(IEnumerable<Category> entities)
        {
            return entities.Select(x => EntityToResponse(x)).ToList();
        }

        public Category UpdateToEntity(CategoryUpdate update)
        {
            cat.Code = update.Code;
            cat.CategoryName = update.CategoryName;
            cat.Description = update.Description;
            cat.Status = update.Status;
            cat.CreatedBy = "System";
            cat.CreateDate = DateTime.Now.AddHours(7);
            cat.UpdateDate = DateTime.Now.AddHours(7);
            cat.UpdateBy = "System";
            return cat;
        }
    }
}
