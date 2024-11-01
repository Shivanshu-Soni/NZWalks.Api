using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NZWalks.Api.Models.DomainModels;

namespace NZWalks.Api.Models.DTO
{
    public class WalksDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double LenghtInkm { get; set; }
        public string? WalkImageUrl { get; set; }

        public RegionDto region { get; set; }
        public DifficultyDTO difficulty { get; set; }

    }
}