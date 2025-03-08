using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Response;

public class ProductDetailResponse
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string ProductCode { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public ProductDetailStatus Status { get; set; } = ProductDetailStatus.Available;
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public int SizeId { get; set; }

    public ProductDetailResponse()
    {
    }

    public ProductDetailResponse(int id, string code, string name, string productCode, decimal price, ProductDetailStatus status, int productId, int colorId, int sizeId, int quantity)
    {
        Id = id;
        Code = code;
        Name = name;
        ProductCode = productCode;
        Price = price;
        Status = status;
        ProductId = productId;
        ColorId = colorId;
        SizeId = sizeId;
        Quantity = quantity;
    }
}
