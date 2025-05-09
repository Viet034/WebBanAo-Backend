﻿using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.Product;

public class ProductDelete
{
    public int Id { get; set; }
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
}
