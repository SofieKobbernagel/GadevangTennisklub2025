namespace GadevangTennisklub2025.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public Event() { }
        public Event(int id, string title, DateTime date, string desc) 
        {
            Id = id;
            Title = title;
            Date = date;
            Description = desc;
            Console.WriteLine(date.DayOfWeek);
        }
    }
}
