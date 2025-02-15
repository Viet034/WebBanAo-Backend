using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Color;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Mapper.impl
{
    public class ColorMapper : IColorMapper
    {
        private readonly Color color = new Color();
        public Color CreateToEntity(ColorCreate create)
        {
            
            color.Code = create.Code;
            color.Status = create.Status;
            color.ColorName = create.ColorName;
            color.CreatedBy = "System";
            color.CreateDate = DateTime.Now.AddHours(7);
            color.UpdateDate = DateTime.Now.AddHours(7);
            color.UpdateBy = "System";
            return color;
        }

        public Color DeleteToEntity(ColorDelete delete)
        {
            color.Id = delete.Id;
            color.Code = delete.Code;
            color.Status = delete.Status;
            color.ColorName = delete.ColorName;
            color.CreatedBy = "System";
            color.CreateDate = DateTime.Now.AddHours(7);
            color.UpdateDate = DateTime.Now.AddHours(7);
            color.UpdateBy = "System";
            return color;
        }

        public ColorResponse EntityToResponse(Color entity)
        {
            ColorResponse response = new ColorResponse();
            response.Id = entity.Id;
            response.Code = entity.Code;
            response.Status = entity.Status;
            response.ColorName = entity.ColorName;
            return response;
        }

        public IEnumerable<ColorResponse> ListEntityToResponse(IEnumerable<Color> entities)
        {
            return entities.Select(x => EntityToResponse(x)).ToList();
        }

        public Color UpdateToEntity(ColorUpdate update)
        {
            color.Code = update.Code;
            color.Status = update.Status;
            color.ColorName = update.ColorName;
            color.CreatedBy = "System";
            color.CreateDate = DateTime.Now.AddHours(7);
            color.UpdateDate = DateTime.Now.AddHours(7);
            color.UpdateBy = "System";
            return color;
        }
    }
}
