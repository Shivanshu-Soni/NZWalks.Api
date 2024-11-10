using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NZWalks.Api.Models;

namespace NZWalks.Api.Repository
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<Image> Upload(Image image)
        {
           var loacalFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", 
           image.FileName, image.FileExtension);

            //upload images to localpath
           using var stream = new FileStream(loacalFilePath, FileMode.Create);
           await image.File.CopyToAsync(stream);

           //https//loaclhost:1234/images/image.jpg
           

        }
    }
}