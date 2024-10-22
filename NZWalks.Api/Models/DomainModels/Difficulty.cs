using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace NZWalks.Api.Models.DomainModels
{
    public class Difficulty
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}