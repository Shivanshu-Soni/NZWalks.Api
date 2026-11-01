using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.UI.Models.DTO
{
    public class RegionDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? RegionImageUrl { get; set; }
        
    }
}