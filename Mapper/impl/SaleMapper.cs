using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Sale;
using WebBanAoo.Models.DTO.Response;
using System.Drawing;

namespace WebBanAoo.Mapper.impl
{
    public class SaleMapper : ISaleMapper
    {
        private readonly Sale sale = new Sale();
        public Sale CreateToEntity(SaleCreate create)
        {
            sale.Code = create.Code;
            sale.SaleName = create.SaleName;
            sale.Status = create.Status;
            sale.StartDate = create.StartDate;
            sale.EndDate = create.EndDate;
            sale.CreatedBy = "System";
            sale.CreateDate = DateTime.Now.AddHours(7);
            sale.UpdateDate = DateTime.Now.AddHours(7);
            sale.UpdateBy = "System";
            return sale;
        }

        public Sale DeleteToEntity(SaleDelete delete)
        {
            sale.Id = delete.Id;
            sale.Code = delete.Code;
            sale.SaleName = delete.SaleName;
            sale.Status = delete.Status;
            sale.StartDate = delete.StartDate;
            sale.EndDate = delete.EndDate;
            sale.CreatedBy = "System";
            sale.CreateDate = DateTime.Now.AddHours(7);
            sale.UpdateDate = DateTime.Now.AddHours(7);
            sale.UpdateBy = "System";
            return sale;
        }

        public SaleResponse EntityToResponse(Sale entity)
        {
            SaleResponse response = new SaleResponse();
            response.Id = entity.Id;
            response.Code = entity.Code;
            response.SaleName = entity.SaleName;
            response.Status = entity.Status;
            response.StartDate = entity.StartDate;
            response.EndDate = entity.EndDate;
            return response;
        }

        public IEnumerable<SaleResponse> ListEntityToResponse(IEnumerable<Sale> entities)
        {
            return entities.Select(x => EntityToResponse(x));
        }

        public Sale UpdateToEntity(SaleUpdate update)
        {
            sale.Code = update.Code;
            sale.SaleName = update.SaleName;
            sale.Status = update.Status;
            sale.StartDate = update.StartDate;
            sale.EndDate = update.EndDate;
            sale.CreatedBy = "System";
            sale.CreateDate = DateTime.Now.AddHours(7);
            sale.UpdateDate = DateTime.Now.AddHours(7);
            sale.UpdateBy = "System";
            return sale;
        }
    }
}
