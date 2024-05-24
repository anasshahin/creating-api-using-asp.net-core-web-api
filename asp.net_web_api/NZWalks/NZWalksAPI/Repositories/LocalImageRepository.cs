using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalkDbContext nZWalkDb;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor  httpContextAccessor,
            NZWalkDbContext nZWalkDb)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.nZWalkDb = nZWalkDb;
        }
        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath,"Images",
                $"{image.FileName}{image.FileExtension}");
            // upload image to local path
            using var stream = new FileStream(localFilePath,FileMode.Create);
            await image.File.CopyToAsync(stream);
            // https://localhost:1234/images/image.jpg
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.Path}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;
            await nZWalkDb.Images.AddAsync(image);
            await nZWalkDb.SaveChangesAsync();

            return image;
        }
    }
}
