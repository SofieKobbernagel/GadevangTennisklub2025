using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Helper
{
    public class CourtNameCompare : IComparer<TennisField>
    {
        public int Compare(TennisField? x, TennisField? y)
        {
            if (x == null && y == null) { return 0; }
            else if (x == null) { return -1; }
            else if (y == null) { return 1; }
            return string.Compare(x.Name, y.Name);
        }
    }
}
