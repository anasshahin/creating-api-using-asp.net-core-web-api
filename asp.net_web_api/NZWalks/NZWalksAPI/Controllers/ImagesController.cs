using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
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
        // POST : /api/Images/Upload
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm]ImageUploadRequestDto requestDto ) 
        {
            ValidateFileUpload(requestDto);
            if (ModelState.IsValid) {
                // convert DTO to Domain model
                var imageDoaminModel = new Image {
                    File = requestDto.File,
                    FileExtension = Path.GetExtension(requestDto.File.FileName),
                    FileSizeInBytes = requestDto.File.Length,
                    FileName = requestDto.FileName,
                    FileDescription = requestDto.FileDescription, 
                };

                // User repository to upload image
              await   imageRepository.Upload(imageDoaminModel);

                return Ok(imageDoaminModel);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto requestDto)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg",".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(requestDto.File.FileName))) {
                ModelState.AddModelError("file","Unsupported file extension");
            }
            // we will git the length in byte
            // 10485760 this number in bytes is equal to 10 maga bytes
            if (requestDto.File.Length > 10485760) {
                ModelState.AddModelError("file", "File size more than 10MB please upload a smaller size file.");

            }
        }
    }
}
