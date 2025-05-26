using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface ICoachService
    {
        /// <summary>
        /// Tilføjer asynkront en ny træner til databasen.
        /// </summary>
        /// <param name="coach">Den træner som skal oprettes.</param>
        /// <returns> true hvis træneren er oprettet succesfuldt og false hvis der går noget galt.</returns>
        Task<bool> CreateCoachAsync(Coach coach);
       
        /// <summary>
        /// Opdaterer et træner objekt.
        /// </summary>
        /// <param name="coach">Træneren som skal opdateres med de nye værdier</param>
        /// <param name="coach_Id">Trænerens id</param>
        /// <returns> true hvis træneren er opdateret succesfuldt og false hvis der går noget galt.</returns>
        Task<bool> UpdateCoachAsync(Coach coach, int coach_Id);
        
        /// <summary>
        /// Sletter en træner fra databasen
        /// </summary>
        /// <param name="coach_Id">Id på den træner som skal slettes</param>
        /// <returns>Returnerer det slettede træner object hvis det er fundet og ellers null</returns>
        Task<Coach> DeleteCoachAsync(int coach_Id);

        /// <summary>
        /// Henter et bestemt træner objekt fra databasen.
        /// </summary>
        /// <param name="coach_Id">Id'et på den træner man ønsker at finde</param>
        /// <returns>Returnerer træner objektet hvis det er fundet og ellers null</returns>
        Task<Coach?> GetCoachByIdAsync(int coach_Id);

        /// <summary>
        /// Finder alle informationer om alle træner objekter i databasen.
        /// </summary>
        /// <returns>En liste af alle trænere</returns>
        Task<List<Coach>> GetAllCoachesAsync();

        /// <summary>
        /// Finder en træner ud fra et hold id (den træner som er knyttet til holdet)
        /// </summary>
        /// <param name="teamId">Id'et på det hold man ønsker at finde træneren til</param>
        /// <returns>Hvis fundet returnerer den træneren tilknyttet holdet og ellers null</returns>
        Task<Coach?> GetCoachByTeamIdAsync(int teamId);
    }
}
