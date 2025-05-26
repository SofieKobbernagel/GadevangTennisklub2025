using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface IMemberService
    {
        /// <summary>
        /// Henter en liste af alle medlemmer i databasen
        /// </summary>
        /// <returns>Liste af alle medlemmer</returns>
        Task<List<Member>> GetAllMembersAsync();
      
        /// <summary>
        /// Opretter et medlem til databasen
        /// </summary>
        /// <param name="member">Det medlem man ønsker at tilføje</param>
        /// <returns>Hvis det lykkedet at tilføje medlemmet til databasen returneres true og ellers false</returns>
        Task<bool> CreateMemberAsync(Member member);

        /// <summary>
        /// Tjekker om en bruger med de indtastede værdier for brugernavn og kodeord eksisterer i databasen
        /// </summary>
        /// <param name="username">Det indtastede brugernavn</param>
        /// <param name="password">Det indtastede kodeord</param>
        /// <returns>Hvis medlemmet bliver fundet returneres det og ellers returneres null</returns>
        Task<Member?> VerifyMember(string username, string password);

        /// <summary>
        /// Finder et bestemt medlem fra databasen udfra dette medlems id
        /// </summary>
        /// <param name="id">id'et på det medlem man ønsker at finde</param>
        /// <returns>Returnerer det medlem som er fundet eller null hvis ingen medlemmer matcher id'et</returns>
        Task<Member?> GetMemberById(int id);

        /// <summary>
        /// Opdaterer et medlem i databasen med de nye værdier som er givet i member objektet
        /// </summary>
        /// <param name="member">medlem objektet med de nye værdier</param>
        /// <param name="member_Id">id'et på det medlem man ønsker at opdaterer</param>
        /// <returns>returnerer true hvis medlemmet er blevet opdateret og ellers false</returns>
        Task<bool> UpdateMemberAsync(Member member, int member_Id);

        /// <summary>
        /// Sletter et medlem fra databasen udfra dets id
        /// </summary>
        /// <param name="member_Id">id'et på det medlem man ønsker at slette</param>
        /// <returns>hvis medlemmet er stettes returneres det medlem og ellers null</returns>
        Task<Member?> DeleteMemberAsync(int member_Id);

        /// <summary>
        /// Tjekker om et brugernavn allerede eksisterer i databasen
        /// </summary>
        /// <param name="username">Det indtastede username</param>
        /// <returns>true hvis brugernavnet er unikt (ikke taget) og ellers false</returns>
        Task<bool> IsUsernameUnique(string username);

        Task SubtrackHour(int id);
     
    }
}
