using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.Product;

public class ProductCreate
{
    public string Code { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateDate { get; set; }
    public string CreatedBy { get; set; }
    public string? UpdateBy { get; set; }
    public ProductStatus Status { get; set; } = ProductStatus.Available;
    public int CategoryId { get; set; }
    public int BrandId { get; set; }

    public ProductCreate(string code, string productName, string description, DateTime createDate, DateTime? updateDate, string createdBy, string? updateBy, ProductStatus status, int categoryId, int brandId)
    {
        Code = code;
        ProductName = productName;
        Description = description;
        CreateDate = createDate;
        UpdateDate = updateDate;
        CreatedBy = createdBy;
        UpdateBy = updateBy;
        Status = status;
        CategoryId = categoryId;
        BrandId = brandId;
    }
}
