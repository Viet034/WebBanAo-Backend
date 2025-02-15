using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Brand;
using WebBanAoo.Models.DTO.Response;
namespace WebBanAoo.Mapper;

public interface IBrandMapper
{
    Brand CreateToEntity(BrandCreate create);
    Brand UpdateToEntity(BrandUpdate update);
    Brand DeleteToEntity(BrandDelete delete);

    BrandResponse EntityToResponse(Brand entity);
    IEnumerable<BrandResponse> ListEntityToResponse(IEnumerable<Brand> entities);
}
