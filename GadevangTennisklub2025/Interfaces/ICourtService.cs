using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface ICourtService
    {
        Task<List<TennisField>> GetAllCourtsAsync();

        Task<TennisField> GetCourtFromIdAsync(int courtId);

        Task<List<TennisField>> GetCourtFromTypeAsync(string type);

        Task<bool> CreateCourtAsync(TennisField tennisField);

        Task<bool> UpdateCourtAsync(TennisField tennisField);

        Task<TennisField> DeleteCourtAsync(int courtId);
    }
}