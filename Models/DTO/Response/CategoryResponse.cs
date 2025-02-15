using static WebBanAoo.Models.Status.Status;
using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Response;

public class CategoryResponse
{
    public int Id { get; set; }
    
    public string Code { get; set; }

    public string CategoryName { get; set; }
    public string Description { get; set; }

    public CategoryStatus Status { get; set; } = CategoryStatus.Active;

    public CategoryResponse()
    {
    }

    public CategoryResponse(int id, string code, string categoryName, string description, CategoryStatus status)
    {
        Id = id;
        Code = code;
        CategoryName = categoryName;
        Description = description;
        Status = status;
    }
}
