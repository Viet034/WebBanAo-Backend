using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Request.Brand;

public class BrandCreate
{
 
    
    public string Code { get; set; }
    
    public string BrandName { get; set; }

    public BrandCreate()
    {
    }

    public BrandCreate( string code, string brandName)
    {
        
        Code = code;
        BrandName = brandName;
    }
}
