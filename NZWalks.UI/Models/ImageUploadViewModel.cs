using System.ComponentModel.DataAnnotations;

namespace NZWalks.UI.Models;

public class ImageUploadViewModel
{
    [Required]
    public IFormFile? File { get; set; }

    [Required]
    [StringLength(100)]
    [RegularExpression(@"^[\p{L}\p{N}_ -]+$", ErrorMessage = "Use letters, numbers, spaces, hyphens, or underscores.")]
    public string FileName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? FileDescription { get; set; }
}
