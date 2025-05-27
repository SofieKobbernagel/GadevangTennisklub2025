namespace GadevangTennisklub2025.Models
{
    public class Gallery
    {
        public int Id { get; set; }

        public string FilePath { get; set; } // Relative or absolute path to the image file

        public DateTime UploadDate { get; set; }

        public string Description { get; set; }

        public Gallery()
        {
            
        }
    }
}

