using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.Api.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(4)]
        public string? Code { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        public string? RegionImageUrl { get; set; }

    }
}