using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.Product;
using WebBanAoo.Models.DTO.Response;
using System.Security.Cryptography;

namespace WebBanAoo.Mapper.impl
{
    public class ProductMapper : IProductMapper
    {

        public Product CreateToEntity(ProductCreate create)
        {
            Product product = new Product();
            product.Code = create.Code;
            product.ProductName = create.ProductName;
            product.Description = create.Description;
            product.Status = create.Status;
            product.CreatedBy = "System";
            product.CreateDate = DateTime.Now.AddHours(7);
            product.UpdateDate = DateTime.Now.AddHours(7);
            product.UpdateBy = "System";
            product.CategoryId = create.CategoryId;
            product.BrandId = create.BrandId;
            return product;
        }

        public Product DeleteToEntity( ProductDelete delete)
        {
            Product product = new Product();
            product.Id = delete.Id;
            product.Code = delete.Code;
            product.ProductName = delete.ProductName;
            product.Description = delete.ProductName;
            product.Status = delete.Status;
            product.CreatedBy = "System";
            product.CreateDate = DateTime.Now.AddHours(7);
            product.UpdateDate = DateTime.Now.AddHours(7);
            product.UpdateBy = "System";
            product.CategoryId = delete.CategoryId;
            product.BrandId = delete.BrandId;
            return product;
        }

        public ProductResponse EntityToResponse(Product entity)
        {
            ProductResponse response = new ProductResponse();
            response.Id = entity.Id;
            response.Code = entity.Code;
            response.ProductName = entity.ProductName;
            response.Description = entity.Description;
            response.Status = entity.Status;
            response.CreatedBy = "System";
            response.CreateDate = DateTime.Now.AddHours(7);
            response.UpdateDate = DateTime.Now.AddHours(7);
            response.UpdateBy = "System";
            response.CategoryId = entity.CategoryId;
            response.BrandId = entity.BrandId;
            return response;
        }

        public IEnumerable<ProductResponse> ListEntityToResponse(IEnumerable<Product> entities)
        {
            return entities.Select(x => EntityToResponse(x)).ToList();
        }

        public Product UpdateToEntity(ProductUpdate update)
        {
            Product product = new Product();

            product.Code = update.Code;
            product.ProductName = update.ProductName;
            product.Description = update.ProductName;
            product.Status = update.Status;
            product.CreatedBy = "System";
            product.CreateDate = DateTime.Now.AddHours(7);
            product.UpdateDate = DateTime.Now.AddHours(7);
            product.UpdateBy = "System";
            product.CategoryId = update.CategoryId;
            product.BrandId = update.BrandId;

            return product;
        }
    }
}
