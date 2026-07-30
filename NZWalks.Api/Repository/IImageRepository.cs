using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NZWalks.Api.Models;

namespace NZWalks.Api.Repository
{
    public interface IImageRepository
    {
      Task<Image>  Upload(Image image);
    }
}