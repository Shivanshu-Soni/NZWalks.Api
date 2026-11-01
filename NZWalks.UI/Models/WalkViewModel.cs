using System.ComponentModel.DataAnnotations;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Models;

public class WalkViewModel
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Range(0.1, 50)]
    public double LenghtInkm { get; set; }

    [Url]
    public string? WalkImageUrl { get; set; }

    [Required]
    public Guid DifficultyId { get; set; }

    [Required]
    public Guid RegionId { get; set; }

    public List<RegionDTO> Regions { get; set; } = [];
    public List<DifficultyDTO> Difficulties { get; set; } =
    [
        new() { Id = Guid.Parse("1ad5baba-b6e0-4bba-8c26-1b87ebb681dd"), Name = "Easy" },
        new() { Id = Guid.Parse("3830149f-ec80-49fd-a483-9a7b4ac9dad8"), Name = "Medium" },
        new() { Id = Guid.Parse("efafab3a-cf95-4d84-b92d-4fea45c16825"), Name = "Hard" }
    ];
}
