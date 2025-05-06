namespace GadevangTennisklub2025.Models
{
    public class TennisField : IComparable<TennisField>
    {
        public int CourtId { get; set; }
        public string Type { get; set; }
        public TennisField()
        {
            
        }
        public TennisField(int courtId, string type)
        {
            CourtId = courtId;
            Type = type;
        }

        public int CompareTo(TennisField? other)
        {
            return Type.CompareTo(other.Type);
        }
    }
}
