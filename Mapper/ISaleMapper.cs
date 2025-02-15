using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Sale;

namespace WebBanAoo.Mapper
{
    public interface ISaleMapper
    {
        // request => Entity(DTO)
        Sale CreateToEntity(SaleCreate create);
        Sale UpdateToEntity(SaleUpdate update);
        Sale DeleteToEntity(SaleDelete delete);

        // Entity(DTO) => Response
        SaleResponse EntityToResponse(Sale entity);
        IEnumerable<SaleResponse> ListEntityToResponse(IEnumerable<Sale> entities);
    }
}
