namespace WebBanAoo.Models.DTO.Response;

public class BrandResponse
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string BrandName { get; set; }
    public string? Image { get; set; }

    public BrandResponse()
    {
    }

    public BrandResponse(int id, string code, string brandName, string? image)
    {
        Id = id;
        Code = code;
        BrandName = brandName;
        Image = image;
    }
}
