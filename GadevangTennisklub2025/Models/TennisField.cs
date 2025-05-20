namespace GadevangTennisklub2025.Models
{
    public class TennisField : IComparable<TennisField>
    {
        public int CourtId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public TennisField()
        {
            
        }
        public TennisField(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public TennisField(int courtId, string type)
        {
            CourtId = courtId;
            Type = type;
        }

        public TennisField(int courtId, string name, string type)
        {
            CourtId = courtId;
            Name = name;
            Type = type;
        }

        public int CompareTo(TennisField? other)
        {
            return Type.CompareTo(other.Type);
        }
    }
}
