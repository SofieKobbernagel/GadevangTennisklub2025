using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface IMembershipService
    {
        Task<List<Membership>> GetAllMembershipsAsync();
    }
}
