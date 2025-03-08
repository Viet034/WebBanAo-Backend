using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.ProductDetail;
using WebBanAoo.Models.DTO.Response;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebBanAoo.Mapper.impl
{
    public class ProductDetailMapper : IProductDetailMapper
    {
        private readonly ProductDetail prod = new ProductDetail();
        public ProductDetail CreateToEntity(ProductDetailCreate create)
        {
            prod.Code = create.Code;
            prod.Name = create.Name;
            prod.Price = create.Price;
            prod.Quantity = create.Quantity;
            prod.Status = create.Status;
            prod.ProductId = create.ProductId;
            prod.ColorId = create.ColorId;
            prod.SizeId = create.SizeId;
            prod.CreatedBy = "System";
            prod.CreateDate = DateTime.Now.AddHours(7);
            prod.UpdateDate = DateTime.Now.AddHours(7);
            prod.UpdateBy = "System";
            return prod;
        }

        public ProductDetail DeleteToEntity(ProductDetailDelete delete)
        {
            prod.Id = delete.Id;
            prod.Code = delete.Code;
            prod.Name = delete.Name;
            prod.Price = delete.Price;
            prod.Quantity = delete.Quantity;
            prod.Status = delete.Status;
            prod.ProductId = delete.ProductId;
            prod.ColorId = delete.ColorId;
            prod.SizeId = delete.SizeId;
            prod.CreatedBy = "System";
            prod.CreateDate = DateTime.Now.AddHours(7);
            prod.UpdateDate = DateTime.Now.AddHours(7);
            prod.UpdateBy = "System";
            return prod;
        }

        public ProductDetailResponse EntityToResponse(ProductDetail entity)
        {
            ProductDetailResponse response = new ProductDetailResponse();
            response.Id = entity.Id;
            response.Code = entity.Code;
            response.Name = entity.Name;
            response.Price = entity.Price;
            response.Quantity = entity.Quantity;
            response.Status = entity.Status;
            response.ProductId = entity.ProductId;
            response.ColorId = entity.ColorId;
            response.SizeId = entity.SizeId;
            return response;
        }

        public IEnumerable<ProductDetailResponse> ListEntityToResponse(IEnumerable<ProductDetail> entities)
        {
            return entities.Select(x => EntityToResponse(x)).ToList();
        }

        public ProductDetail UpdateToEntity(ProductDetail prod, ProductDetailUpdate update)
        {
            prod.Code = update.Code;
            prod.Name = update.Name;
            prod.Price = update.Price != 0 ? update.Price : prod.Price;
            prod.Quantity = update.Quantity;
            prod.Status = update.Status;
            prod.ProductId = update.ProductId;
            prod.ColorId = update.ColorId;
            prod.SizeId = update.SizeId;
            prod.CreatedBy = "System";
            prod.CreateDate = DateTime.Now.AddHours(7);
            prod.UpdateDate = DateTime.Now.AddHours(7);
            prod.UpdateBy = "System";
            return prod;
        }
    }
}
