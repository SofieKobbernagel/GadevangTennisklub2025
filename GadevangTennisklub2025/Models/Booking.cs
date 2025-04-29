namespace GadevangTennisklub2025.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Court_Id { get; set; }
        public int Team_Id { get; set; }
        public int Event_Id { get; set; }
    }
}
