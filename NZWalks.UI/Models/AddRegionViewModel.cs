using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NZWalks.UI.Models
{
    public class AddRegionViewModel
    {
        public Guid Code { get; set; }
        public string Name { get; set; }
        public string RegionImageUrl { get; set; }
    }
}