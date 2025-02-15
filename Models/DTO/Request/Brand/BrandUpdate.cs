namespace WebBanAoo.Models.DTO.Request.Brand;

public class BrandUpdate
{
    public int Id { get; set; }

    public string Code { get; set; }

    public string BrandName { get; set; }

    public BrandUpdate()
    {
    }

    public BrandUpdate(int id, string code, string brandName)
    {
        Id = id;
        Code = code;
        BrandName = brandName;
    }
}
