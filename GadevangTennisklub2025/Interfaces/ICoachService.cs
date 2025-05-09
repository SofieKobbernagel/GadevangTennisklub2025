using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface ICoachService
    {
        Task<bool> CreateCoachAsync(Coach coach);
        Task<bool> UpdateCoachAsync(Coach coach, int coach_Id);
        Task<Coach> DeleteCoachAsync(int coach_Id);
        Task<Coach?> GetCoachByIdAsync(int coach_Id);
        Task<List<Coach>> GetAllCoachesAsync();
    }
}
