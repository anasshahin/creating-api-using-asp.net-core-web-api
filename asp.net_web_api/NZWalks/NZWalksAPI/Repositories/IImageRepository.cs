using NZWalksAPI.Models.Domain;
using System.Net;

namespace NZWalksAPI.Repositories
{
    public interface IImageRepository
    {
       Task<Image> Upload(Image image);
    }
}
