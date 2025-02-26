using Microsoft.AspNetCore.Http.HttpResults;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.ProductImage;
using WebBanAoo.Models.DTO.Response;

namespace WebBanAoo.Mapper.impl;

public class ProductImageMapper : IProductImageMapper
{
    public ProductImage CreateToEntity(ProductImageCreate create)
    {
        ProductImage pro = new ProductImage();
        pro.Url = create.Url;
        pro.Code = create.Code;
        pro.Status = create.Status;
        pro.ProductDetailId = create.ProductDetailId;
        return pro;

    }

    public ProductImage DeleteToEntity(ProductImageDelete delete)
    {
        ProductImage pro = new ProductImage();
        pro.Id = delete.Id;
        pro.Url = delete.Url;
        pro.Code = delete.Code;
        pro.Status = delete.Status;
        pro.ProductDetailId = delete.ProductDetailId;
        return pro;
    }

    public ProductImageResponse EntityToResponse(ProductImage entity)
    {
        ProductImageResponse response = new ProductImageResponse();
        response.Id = entity.Id;
        response.Code = entity.Code;
        response.ProductDetailId = entity.ProductDetailId;
        response.Status = entity.Status;
        response.Url = entity.Url;
        return response;
    }

    public IEnumerable<ProductImageResponse> ListEntityToResponse(IEnumerable<ProductImage> entities)
    {
        return entities.Select(x => EntityToResponse(x)).ToList();
    }

    public ProductImage UpdateToEntity(ProductImageUpdate update)
    {
        ProductImage pro = new ProductImage();
        pro.Id = update.Id;
        pro.Url = update.Url;
        pro.Code = update.Code;
        pro.Status = update.Status;
        pro.ProductDetailId = update.ProductDetailId;
        return pro;
    }
}
