using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.Api.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage ="Code has to be a minumum 3 characters")]
        [MaxLength(4, ErrorMessage ="Code can have max of 4 characters")]
        public string Code { get; set; }

        [Required]
        [MinLength(3,ErrorMessage ="Name have min 3 characters ")]
        [MaxLength(100, ErrorMessage = "Name can have a max of 100 characters")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}