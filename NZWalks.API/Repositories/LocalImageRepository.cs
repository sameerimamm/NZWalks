using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;


namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository


    {
        private readonly IWebHostEnvironment webHostEnvioronment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvioronment, 
            IHttpContextAccessor httpContextAccessor, NZWalksDbContext dbContext)
        {
            this.webHostEnvioronment = webHostEnvioronment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }
        public async Task<Image> Upload(Image image)

        {
            string FileNameWithExtension = $"{image.FileName}{image.FileExtension}";
            var localImagePath = Path.Combine(webHostEnvioronment.ContentRootPath, "Images",FileNameWithExtension);

            //upload image to local path
            using var stream = new FileStream(localImagePath, FileMode.Create);
            await image.File.CopyToAsync(stream);
            // create url -> https://localhost:1234/

            var httpC= httpContextAccessor.HttpContext.Request;

            //var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            var urlFilePath = $"{httpC.Scheme}://{httpC.Host}{httpC.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;
            //Add image to Images table

            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();
            return image;


        }
    }
}
