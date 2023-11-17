using System.ComponentModel.DataAnnotations;

namespace MauiWithAPI.API.Services.Categories.Dto
{
    public class CreateCategoryDto
    {
        [Required]
        public string CategoryName { get; set; }
    }
}
