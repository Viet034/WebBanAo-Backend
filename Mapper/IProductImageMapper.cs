using WebBanAoo.Models.DTO.Request.ProductImage;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models;

namespace WebBanAoo.Mapper;

public interface IProductImageMapper
{
    // request => Entity(DTO)
    ProductImage CreateToEntity(ProductImageCreate create);
    ProductImage UpdateToEntity(ProductImageUpdate update);
    ProductImage DeleteToEntity(ProductImageDelete delete);

    // Entity(DTO) => Response
    ProductImageResponse EntityToResponse(ProductImage entity);
    IEnumerable<ProductImageResponse> ListEntityToResponse(IEnumerable<ProductImage> entities);
}
