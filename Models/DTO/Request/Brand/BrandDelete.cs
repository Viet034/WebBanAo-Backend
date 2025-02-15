namespace WebBanAoo.Models.DTO.Request.Brand;

public class BrandDelete
{
    public int Id { get; set; }

    public string Code { get; set; }

    public string BrandName { get; set; }

    public BrandDelete()
    {
    }

    public BrandDelete(int id, string code, string brandName)
    {
        Id = id;
        Code = code;
        BrandName = brandName;
    }
}
