namespace GadevangTennisklub2025.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MembershipType { get; set; }
        public double Length {  get; set; }
        public TimeOnly TimeOfDay { get; set; }
        public int DayOfWeek { get; set; }  //1=monday  2=tuesday ...
        public int[] AttendeeRange { get; set; }
        //new int[2];
        public List<Member> Attendees { get; set; }
        public string AttendeesID { get { string Out = " "; foreach (Member me in Attendees) { Out.Concat(me.Member_Id + ","); } return Out; } }
        public Member Master { get; set; }
        public string Description { get; set; }


        private string[] week = { "Monday", "Tuesday", "Wednesday","Thursday","Friday","Saturday","Sunday" };

        public Team()
        {
        }

        public Team(int id, string name, string membershipType , int dayOfWeek,TimeOnly startTime,double length, int[] attendeeRange, List<Member> attendees, Member master, string description)
        {
            Id = id;
            MembershipType = membershipType;
            Name = name;
            DayOfWeek = dayOfWeek;
            TimeOfDay = startTime;
            Length = length;
            AttendeeRange = attendeeRange;
            Attendees = attendees;
            Master = master;
            Description = description;
        }
        public override string ToString()
        {
            return $"id: {Id} \n MembershipType required: {MembershipType} \n Name: {Name} \n  timeslot   from: TimeSlot[0], to:TimeSlot[1] \n time of day start: {TimeOfDay}, end:  {TimeOfDay.AddHours(Length)}   Repeated every {week[DayOfWeek]}\n Creator: {Master.Name} \n  Minimum Number of Attendees: {AttendeeRange[0]} ,  Max Number of Attendees: {AttendeeRange[1]}  \n  places left: {((Attendees.Count < AttendeeRange[1]) ? $"der er {AttendeeRange[1] - Attendees.Count} pladser tilbage" : $"der er ikke flere pladser {Attendees.Count} ud af {AttendeeRange[1]} \n  description: {Description}" + $"")}";
        }

        public void AttendTeam(Member SelectedMember)
        {
            Attendees.Add(SelectedMember);
            Console.WriteLine($"\n added  {SelectedMember} to {Name},   ");
            foreach (Member Member in Attendees)
            {
                Console.WriteLine(Member.Name);
            }
            Console.WriteLine();
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
