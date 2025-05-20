using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.Data.SqlClient;

namespace GadevangTennisklub2025.Services
{
    public class GalleryService : IGalleryService
    {
        private string connectionString = Secret.ConnectionString;

        private string GetPhotoSql = "SELECT Id, FilePath, UploadDate, Description FROM Photo ORDER BY UploadDate DESC";
        private string insertSql = "INSERT INTO Photo (FilePath, UploadDate, Description) VALUES (@FilePath, @UploadDate, @Description)";

        public async Task<List<Gallery>> GetAllPhotos()
        {
            var photos = new List<Gallery>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(GetPhotoSql, conn);
                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        photos.Add(new Gallery
                        {
                            Id = (int)reader["Id"],
                            FilePath = reader["FilePath"].ToString(),
                            UploadDate = (DateTime)reader["UploadDate"],
                            Description = reader["Description"].ToString()
                        });
                    }
                }
            }

            return photos;
        }
        public async Task<bool> UploadPhotoAsync(IFormFile file, string description, IWebHostEnvironment env)
        {
            if (file == null || file.Length == 0)
                return false;

            string uploadsFolder = Path.Combine(env.WebRootPath, "Images", "Gallery");
            Directory.CreateDirectory(uploadsFolder); // Ensure folder exists

            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Save to database
            string dbPath = $"/Images/Gallery/{uniqueFileName}";
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(insertSql, conn);
                cmd.Parameters.AddWithValue("@FilePath", dbPath);
                cmd.Parameters.AddWithValue("@UploadDate", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@Description", description);

                await conn.OpenAsync();
                int rows = await cmd.ExecuteNonQueryAsync();
                return rows > 0;
            }
        }
    }
}
