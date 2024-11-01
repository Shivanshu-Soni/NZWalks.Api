using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.Api.Models.DTO
{
    public class AddWalksRequestDto
    {
         public string Name { get; set; }
        public string Description { get; set; }
        public double LenghtInkm { get; set; }  
        public string? WalkImageUrl { get; set; }   

        public Guid DifficultyId    { get; set; }
        public Guid RegionId    { get; set; }
    }
}