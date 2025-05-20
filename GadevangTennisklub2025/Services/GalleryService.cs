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
        private string deleteSql = "DELETE FROM Photo WHERE Id = @Id";
        private string getFilePathSql = "SELECT FilePath FROM Photo WHERE Id = @Id";



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
        public async Task<bool> DeletePhotoAsync(int id, IWebHostEnvironment env)
        {
            string filePath = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                // First find the file path for particular image
                using (SqlCommand getCmd = new SqlCommand(getFilePathSql, conn))
                {
                    getCmd.Parameters.AddWithValue("@Id", id);
                    var result = await getCmd.ExecuteScalarAsync();
                    if (result != null)
                    {
                        filePath = result.ToString();
                    }
                    else
                    {
                        return false; // Photo not found
                    }
                }

                // This is for deleting the image from the database
                using (SqlCommand delCmd = new SqlCommand(deleteSql, conn))
                {
                    delCmd.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = await delCmd.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        return false; // No row deleted
                    }
                }
            }

            // This will delete the image from the computer. This is so that the image won't stay and clog the folder when there's no database entry for it anymore.
            if (!string.IsNullOrEmpty(filePath))
            {
                string physicalPath = Path.Combine(env.WebRootPath, filePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                }
            }

            return true;
        }


    }
}
