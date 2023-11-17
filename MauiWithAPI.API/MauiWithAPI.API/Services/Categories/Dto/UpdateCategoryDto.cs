using System.ComponentModel.DataAnnotations;

namespace MauiWithAPI.API.Services.Categories.Dto
{
    public class UpdateCategoryDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
