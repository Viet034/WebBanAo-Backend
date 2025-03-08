using Microsoft.AspNetCore.Http.HttpResults;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Brand;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Mapper.impl
{
    public class BrandMapper : IBrandMapper
    {
        private readonly Brand brand = new Brand();
        public Brand CreateToEntity(BrandCreate create)
        {
            brand.Code = create.Code;
            brand.BrandName = create.BrandName;
            brand.Image = create.Image;
            brand.CreatedBy = "System";
            brand.CreateDate = DateTime.Now.AddHours(7);
            brand.UpdateDate = DateTime.Now.AddHours(7);
            brand.UpdateBy = "System";
            return brand;
        }

        public Brand DeleteToEntity(BrandDelete delete)
        {
            brand.Id = delete.Id;
            brand.Code = delete.Code;
            brand.BrandName = delete.BrandName;
            brand.Image = delete.Image;
            brand.CreatedBy = "System";
            brand.CreateDate = DateTime.Now.AddHours(7);
            brand.UpdateDate = DateTime.Now.AddHours(7);
            brand.UpdateBy = "System";
            return brand;
        }

        public BrandResponse EntityToResponse(Brand entity)
        {
            BrandResponse response = new BrandResponse();
            response.Id = entity.Id;
            response.Code = entity.Code;
            response.BrandName = entity.BrandName;
            response.Image = entity.Image;
            return response;
        }

        public IEnumerable<BrandResponse> ListEntityToResponse(IEnumerable<Brand> entities)
        {
            return entities.Select(x => EntityToResponse(x)).ToList();
        }

        public Brand UpdateToEntity(BrandUpdate update)
        {
           
            brand.Code = update.Code;
            brand.BrandName = update.BrandName;
            brand.Image = update.Image;
            brand.CreatedBy = "System";
            brand.CreateDate = DateTime.Now.AddHours(7);
            brand.UpdateDate = DateTime.Now.AddHours(7);
            brand.UpdateBy = "System";
            return brand;
        }
    }
}
