using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface IGalleryService
    {
        Task<List<Gallery>> GetAllPhotos();
        Task<bool> UploadPhotoAsync(IFormFile file, string description, IWebHostEnvironment env);
        Task<bool> DeletePhotoAsync(int id, IWebHostEnvironment env);
        Task<bool> UpdatePhotoDescriptionAsync(int photoId, string newDescription);
    }
}
