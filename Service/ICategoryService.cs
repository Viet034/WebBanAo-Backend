using WebBanAoo.Models.DTO.Response;
using static WebBanAoo.Models.Status.Status;
using WebBanAoo.Models.DTO.Request.Category;

namespace WebBanAoo.Service
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryResponse>> GetAllCategoryAsync();
        public Task<IEnumerable<CategoryResponse>> SearchCategoryByKeyAsync(string key);
        public Task<CategoryResponse> UpdateCategoryAsync(int id, CategoryUpdate update);
        public Task<CategoryResponse> CreateCategoryAsync(CategoryCreate create);
        public Task<bool> HardDeleteCategoryAsync(int id);
        public Task<CategoryResponse> SoftDeleteCategoryAsync(int id, CategoryStatus newStatus);
        public Task<CategoryResponse> FindCategoryByIdAsync(int id);
        public Task<string> CheckUniqueCodeAsync();
    }
}
