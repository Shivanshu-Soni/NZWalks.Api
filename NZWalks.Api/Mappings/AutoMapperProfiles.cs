using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NZWalks.Api.Models.DomainModels;
using NZWalks.Api.Models.DTO;

namespace NZWalks.Api.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //inside ctor we create mapping

            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<RegionDto, Region>().ReverseMap();

            CreateMap<AddWalksRequestDto, Walk>().ReverseMap();
            CreateMap<Walk, WalksDTO>().ReverseMap();
            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();
            CreateMap<UpdateWalkRequiestDto, Walk>().ReverseMap();

        }
    }


}