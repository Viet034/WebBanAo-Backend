using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO.Request.Category;

public class CategoryUpdate
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public CategoryStatus Status { get; set; } = CategoryStatus.Active;

    public CategoryUpdate()
    {
    }

    public CategoryUpdate(int id, string code, string categoryName, string description, CategoryStatus status)
    {
        Id = id;
        Code = code;
        CategoryName = categoryName;
        Description = description;
        Status = status;
    }
}
