using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.UI.Models
{
    public class AddRegionViewModel
    {
        [Required]
        [StringLength(4, MinimumLength = 3)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Url]
        public string? RegionImageUrl { get; set; }
    }
}