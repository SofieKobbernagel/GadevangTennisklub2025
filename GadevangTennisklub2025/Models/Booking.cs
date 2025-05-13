namespace GadevangTennisklub2025.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Court_Id { get; set; }
        public int? Team_Id { get; set; }
        public int? Event_Id { get; set; }
        public Booking(int id, DateTime start, DateTime end, int courtId, int? teamId, int? eventId) 
        {
            Id = id;
            Start = start;
            End = end;
            Court_Id = courtId;
            if (teamId != null) Team_Id=teamId;
            if (eventId != null) Event_Id=eventId;
        }

        public Booking() { }

    }
}
