using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface ICoachService
    {
        Task<bool> CreateCoachAsync(Coach coach);
        Task<bool> UpdateCoachAsync(Coach coach);
        Task<bool> DeleteCoachAsync(int coachId);
        Task<Coach?> GetCoachByIdAsync(int coachId);
        Task<List<Coach>> GetAllCoachesAsync();
    }
}
