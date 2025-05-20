using GadevangTennisklub2025.Services;

namespace GadevangTennisklub2025.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MembershipType { get; set; }
        public Coach Trainer { get; set; }
        public double Length {  get; set; }
        public TimeOnly TimeOfDay { get; set; }
        public int DayOfWeek { get; set; }  //0=monday  1=tuesday ...
        public int[] AttendeeRange { get; set; }
        //new int[2];
        public List<Member> Attendees { get; set; }
        //public string AttendeesID { get { string Out = " "; if(Attendees!=null){ foreach (Member me in Attendees) { Out.Concat(me.Member_Id + ","); } } return Out; } }
        public string Description { get; set; }


        public string[] week = { "Monday", "Tuesday", "Wednesday","Thursday","Friday","Saturday","Sunday" };

        public Team()
        {
        }

        public Team(int id, string name, string membershipType, Coach trainer, int dayOfWeek,TimeOnly startTime, double length, int[] attendeeRange, List<Member> Attendees, string description)
        {
            Id = id;
            MembershipType = membershipType;
            Name = name;
            Trainer = trainer;
            DayOfWeek = dayOfWeek;
            TimeOfDay = startTime;
            Length = length;
            AttendeeRange = attendeeRange;
            Description = description;

            //Console.WriteLine("Team.cs/ length is: " + Length);
        }
        public override string ToString()
        {
            return $"id: {Id} \n MembershipType required: {MembershipType} \n Name: {Name} \n Trainer: {Trainer.Name} \n timeslot   from: TimeSlot[0], to:TimeSlot[1] \n time of day start: {TimeOfDay}, end:  {TimeOfDay.AddHours(Length)}   Repeated every {week[DayOfWeek]} \n  Minimum Number of Attendees: {AttendeeRange[0]} ,  Max Number of Attendees: {AttendeeRange[1]}  \n  description: {Description} ";
        }

       


       
    }
    public class Search
    {

        public String SearchText { get; set; }
        public String SearchType { get; set; }

        public Search()
        {
        }

        public Search(string searchText, string searchType)
        {

            SearchText = searchText;
            SearchType = searchType;
        }

        public override string ToString()
        {
            return $" {nameof(SearchText)}: {SearchText}, {nameof(SearchType)}: {SearchType}";
        }
    }
}
