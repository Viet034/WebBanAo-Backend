using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Size;

namespace WebBanAoo.Mapper
{
    public interface ISizeMapper
    {
        // request => Entity(DTO)
        Size CreateToEntity(SizeCreate create);
        Size UpdateToEntity(SizeUpdate update);
        Size DeleteToEntity(SizeDelete delete);

        // Entity(DTO) => Response
        SizeResponse EntityToResponse(Size entity);
        IEnumerable<SizeResponse> ListEntityToResponse(IEnumerable<Size> entities);
    }
}
