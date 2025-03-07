using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.ProductDetail;

namespace WebBanAoo.Mapper;

public interface IProductDetailMapper
{
    // request => Entity(DTO)
    ProductDetail CreateToEntity(ProductDetailCreate create);
    ProductDetail UpdateToEntity(ProductDetail prod, ProductDetailUpdate update);
    ProductDetail DeleteToEntity(ProductDetailDelete delete);

    // Entity(DTO) => Response
    ProductDetailResponse EntityToResponse(ProductDetail entity);
    IEnumerable<ProductDetailResponse> ListEntityToResponse(IEnumerable<ProductDetail> entities);
}
