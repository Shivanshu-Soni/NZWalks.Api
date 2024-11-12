using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.Api.Models;
using NZWalks.Api.Models.DTO;
using NZWalks.Api.Repository;

namespace NZWalks.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        //POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequestDto)
        {
            ValidateFileUpload(imageUploadRequestDto);

            if(ModelState.IsValid)
            {
                //convert dto to model
                var ImageDomainModel = new Image
                {
                    File = imageUploadRequestDto.File,
                    FileExtension = Path.GetExtension(imageUploadRequestDto.File.FileName),
                    FileSizeInBytes = imageUploadRequestDto.File.Length,
                    FileName = imageUploadRequestDto.FileName,
                    FileDescription = imageUploadRequestDto.FileDescripstion

                };
                    //user repository to upload file
                    await imageRepository.Upload(ImageDomainModel);
                    return Ok(ImageDomainModel);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto requestDto)
        {
            var allowedExtension = new string[] { ".jpg", ".jpeg", ".ong", };
            if (!allowedExtension.Contains(Path.GetExtension(requestDto.File.FileName)))
            {
                ModelState.AddModelError("file", "unsupported file Extension");
            }
            if (requestDto.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size is more than 10 mb , please add file  less than 10 mb");
            }
        }
    }



}

