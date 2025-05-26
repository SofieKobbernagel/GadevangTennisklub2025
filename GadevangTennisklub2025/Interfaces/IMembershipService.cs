using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface IMembershipService
    {
        /// <summary>
        /// Henter en liste af alle medlemsskaber i databasen
        /// </summary>
        /// <returns>En liste af medlemsskaber (medlemsskabstyper og deres pris og rettigheder)</returns>
        Task<List<Membership>> GetAllMembershipsAsync();
    }
}
