using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface ICourtTypeService
    {
        Task<List<CourtTypes>> GetAllCourtsAsync();
    }
}
