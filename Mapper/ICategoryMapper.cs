using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Category;

namespace WebBanAoo.Mapper;

public interface ICategoryMapper
{
    // request => Entity(DTO)
    Category CreateToEntity(CategoryCreate create);
    Category UpdateToEntity(CategoryUpdate update);
    Category DeleteToEntity(CategoryDelete delete);

    // Entity(DTO) => Response
    CategoryResponse EntityToResponse(Category entity);
    IEnumerable<CategoryResponse> ListEntityToResponse(IEnumerable<Category> entities);
}
