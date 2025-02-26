using Microsoft.EntityFrameworkCore;
using WebBanAoo.Data;
using WebBanAoo.Mapper;
using WebBanAoo.Models;
using WebBanAoo.Models.DTO.Request.ProductImage;
using WebBanAoo.Models.DTO.Response;
using WebBanAoo.Ultility;

namespace WebBanAoo.Service.impl;

public class ProductImageService : IProductImageService
{
    private readonly ApplicationDbContext _context;
    private IProductImageMapper _mapper;
    private readonly Validation<ProductImage> _validation;

    public ProductImageService(ApplicationDbContext context, IProductImageMapper mapper, Validation<ProductImage> validation)
    {
        _context = context;
        _mapper = mapper;
        _validation = validation;
    }

    public async Task<string> CheckUniqueCodeAsync()
    {
        string newCode;
        bool isExist;

        do
        {
            newCode = GenerateCode.GenerateProductImageCode();
            _context.ChangeTracker.Clear();
            isExist = await _context.ProductImages.AnyAsync(p => p.Code == newCode);
        }
        while (isExist);

        return newCode;
    }

    public async Task<ProductImageResponse> CreateProductImageAsync(ProductImageCreate create)
    {
        ProductImage entity = _mapper.CreateToEntity(create);

        if (string.IsNullOrEmpty(entity.Code) || entity.Code == "string")
        {
            entity.Code = await CheckUniqueCodeAsync();
        }
        while (await _context.ProductImages.AnyAsync(p => p.Code == entity.Code))
        {
            entity.Code = await CheckUniqueCodeAsync();
        }

        await _context.ProductImages.AddAsync(entity);

        await _context.SaveChangesAsync();
        var response = _mapper.EntityToResponse(entity);
        return response;
    }

    

    public async Task<IEnumerable<ProductImageResponse>> GetAllProductImageAsync()
    {
        var co = await _context.ProductImages.ToListAsync();
        if (co == null) throw new Exception($"Khong co ban ghi nao");

        var response = _mapper.ListEntityToResponse(co);

        return response;
    }

    public async Task<bool> HardDeleteProductImageAsync(int id)
    {
        var co = await _context.ProductImages.FindAsync(id);
        if (co == null)
        {
            throw new KeyNotFoundException($" Khong co Id {id} ton tai");
        }
        _context.ProductImages.Remove(co);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ProductImageResponse> UpdateProductImageAsync(int id, ProductImageUpdate update)
    {
        var coId = await _context.ProductImages.FindAsync(id);
        if (coId == null)
        {
            throw new Exception($" Khong co Id {id} ton tai");
        }
        coId.Code = await _validation.CheckAndUpdateAPIAsync(coId, coId.Code, update.Code, co => co.Code == update.Code);
        coId.Url = await _validation.CheckAndUpdateAPIAsync(coId, coId.Url, update.Url, co => co.Url == update.Url);
        coId.ProductDetailId = await _validation.CheckAndUpdateQuantityAsync(coId, coId.ProductDetailId, update.ProductDetailId, co => co.ProductDetailId == update.ProductDetailId);

        var result = _mapper.UpdateToEntity(update);

        coId.Status = result.Status;
 


        await _context.SaveChangesAsync();

        var response = _mapper.EntityToResponse(coId);
        return response;
    }

    public async Task<IEnumerable<ProductImageResponse>> FindProductImageByProductDetailIdAsync(int id)
    {
        var tId = await _context.ProductImages.Where(pr => pr.ProductDetailId == id).ToListAsync();
        if (!tId.Any())
        {
            throw new Exception($"Không có Ảnh nào thuộc id = {id}.");
        }
        var response = _mapper.ListEntityToResponse(tId);
        return response;
    }
}
