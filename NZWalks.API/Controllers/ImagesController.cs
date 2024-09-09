using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //POST /api/Images/upload


        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageRequest)
        {
            ValidateFileUpload(imageRequest);
            if (ModelState.IsValid) 
            {
                var image = new Image
                {
                    File = imageRequest.File,
                    FileName = imageRequest.FileName,
                    Description = imageRequest.Description,
                    FileExtension = Path.GetExtension(imageRequest.File.FileName),
                    FileSizeInBytes = imageRequest.File.Length,
                };

                var uploadedImage = await imageRepository.Upload(image);

                var uploadedimagedto = new UploadedImageDto
                {
                    FileName = uploadedImage.FileName,
                    Description = uploadedImage.Description,
                    FilePath = uploadedImage.FilePath
                };

                return Ok(uploadedimagedto);



            }

            return BadRequest(ModelState);

        }

        private void ValidateFileUpload(ImageUploadRequestDto imageRequest)
        {
            var allowedExtension = new string[] { ".jpg", ".png", ".jpeg" ,".jfif"};

            if (!allowedExtension.Contains(Path.GetExtension(imageRequest.File.FileName))) 
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }
            if (imageRequest.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File is more than 10mb, please upload a smaller file");
            }
        }


    }
}
