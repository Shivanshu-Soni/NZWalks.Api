using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.Api.Models.DomainModels
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LenghtInkm { get; set; }  
        public string? WalkImageUrl { get; set; }   

        public Guid DifficultyId    { get; set; }
        public Guid regionId    { get; set; }
        //Navigation prop
        public Difficulty difficulty{ get; set; }
        public Region region{ get; set; }
    }
}