using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Models;

namespace NZWalks.Api.Repository
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext nZWalksDbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, NZWalksDbContext nZWalksDbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            var loacalFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",
            $"{image.FileName}{image.FileExtension}");

            //upload images to localpath
            using var stream = new FileStream(loacalFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            //https//loaclhost:1234/images/image.jpg
            var urlFilepath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilepath;

            //Add Image to the image table
            await nZWalksDbContext.Images.AddAsync(image);
            await nZWalksDbContext.SaveChangesAsync();
            return image;
        }
    }
}