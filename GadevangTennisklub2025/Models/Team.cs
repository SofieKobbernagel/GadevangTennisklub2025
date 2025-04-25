namespace GadevangTennisklub2025.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime[] TimeSlot = new DateTime[2];
        public int[] AttendeeRange { get; set; }
        //new int[2];
        public List<Member> Attendees { get; set; }
        public Member Master { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }


        public Team()
        {
        }

        public Team(int id, string name, DateTime startDateTime, DateTime endDateTime, int[] attendeeRange, List<Member> attendees, Member master, string summary, string description)
        {
            Id = id;
            Name = name;
            TimeSlot[0] = startDateTime;
            TimeSlot[1] = endDateTime;
            AttendeeRange = attendeeRange;
            Attendees = attendees;
            Master = master;
            Summary = summary;
            Description = description;
        }
        public override string ToString()
        {
            return $"id: {Id} \n Name: {Name} \n  timeslot   from: {TimeSlot[0]}, to:{TimeSlot[1]} \n  Creator: {Master.Name} \n  Minimum Number of Attendees: {AttendeeRange[0]} ,  Max Number of Attendees: {AttendeeRange[1]}  \n  places left: {((Attendees.Count < AttendeeRange[1]) ? $"der er {AttendeeRange[1] - Attendees.Count} pladser tilbage" : $"der er ikke flere pladser {Attendees.Count} ud af {AttendeeRange[1]} \n short description: {Summary}  \n  long description: {Description}" + $"")}";
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
}
