using static WebBanAoo.Models.Status.Status;
using System.ComponentModel.DataAnnotations;

namespace WebBanAoo.Models.DTO.Request.Category;

public class CategoryCreate
{
    public string Code { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public CategoryStatus Status { get; set; } = CategoryStatus.Active;

    public CategoryCreate()
    {
    }

    public CategoryCreate(string code, string categoryName, string description, CategoryStatus status)
    {
        Code = code;
        CategoryName = categoryName;
        Description = description;
        Status = status;
    }
}
