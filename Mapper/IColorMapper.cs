using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Color;

namespace WebBanAoo.Mapper
{
    public interface IColorMapper
    {
        // request => Entity(DTO)
        Color CreateToEntity(ColorCreate create);
        Color UpdateToEntity(ColorUpdate update);
        Color DeleteToEntity(ColorDelete delete);

        // Entity(DTO) => Response
        ColorResponse EntityToResponse(Color entity);
        IEnumerable<ColorResponse> ListEntityToResponse(IEnumerable<Color> entities);
    }
}
