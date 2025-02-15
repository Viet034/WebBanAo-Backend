using System.ComponentModel.DataAnnotations;
using static WebBanAoo.Models.Status.Status;

namespace WebBanAoo.Models.DTO;

public class CategoryDTO
{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [Required(ErrorMessage = "Category Name is Missing")]
    public string CategoryName { get; set; }
    public string Description { get; set; }
    [EnumDataType(typeof(CategoryStatus))]
    public CategoryStatus Status { get; set; } = CategoryStatus.Active;
}
