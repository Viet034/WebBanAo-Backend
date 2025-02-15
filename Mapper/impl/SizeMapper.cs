using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Size;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Mapper.impl
{
    public class SizeMapper : ISizeMapper
    {
        private readonly Size size = new Size();
        public Size CreateToEntity(SizeCreate create)
        {
            size.Code = create.Code;
            size.Status = create.Status;
            size.SizeCode = create.SizeCode;
            size.CreatedBy = "System";
            size.CreateDate = DateTime.Now.AddHours(7);
            size.UpdateDate = DateTime.Now.AddHours(7);
            size.UpdateBy = "System";
            return size;
        }

        public Size DeleteToEntity(SizeDelete delete)
        {
            size.Id = delete.Id;
            size.Code = delete.Code;
            size.Status = delete.Status;
            size.SizeCode = delete.SizeCode;
            size.CreatedBy = "System";
            size.CreateDate = DateTime.Now.AddHours(7);
            size.UpdateDate = DateTime.Now.AddHours(7);
            size.UpdateBy = "System";
            return size;
        }

        public SizeResponse EntityToResponse(Size entity)
        {
            SizeResponse response = new SizeResponse();
            response.Id = entity.Id;
            response.Code = entity.Code;
            response.Status = entity.Status;
            response.SizeCode = entity.SizeCode;
            return response;
        }

        public IEnumerable<SizeResponse> ListEntityToResponse(IEnumerable<Size> entities)
        {
            return entities.Select(x => EntityToResponse(x)).ToList();
        }

        public Size UpdateToEntity(SizeUpdate update)
        {
            size.Code = update.Code;
            size.Status = update.Status;
            size.SizeCode = update.SizeCode;
            size.CreatedBy = "System";
            size.CreateDate = DateTime.Now.AddHours(7);
            size.UpdateDate = DateTime.Now.AddHours(7);
            size.UpdateBy = "System";
            return size;
        }
    }
}
