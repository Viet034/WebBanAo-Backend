using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Product;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Mapper;

public interface IProductMapper
{
    // request => Entity(DTO)
    Product CreateToEntity(ProductCreate create);
    Product UpdateToEntity(ProductUpdate update);
    Product DeleteToEntity(ProductDelete delete);

    // Entity(DTO) => Response
    ProductResponse EntityToResponse(Product entity);
    IEnumerable<ProductResponse> ListEntityToResponse(IEnumerable<Product> entities);   
}
