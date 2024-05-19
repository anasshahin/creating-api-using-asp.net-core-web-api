using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalksAPI.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }
        [NotMapped] // becuase we don't want to stor File in database

        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
        public string FileExtension{ get; set; }
        public long FileSizeInBytes{ get; set; }
        public string FilePath { get; set; }

    }
}
