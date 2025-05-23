using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface IMemberService
    {
        Task<List<Member>> GetAllMembersAsync();
        /// <summary>
        /// Laver en medlem
        /// </summary>
        /// <param name="member">Tager en medlem</param>
        /// <returns>Returnerer en medlem til databasen</returns>
        Task<bool> CreateMemberAsync(Member member);

        Member VerifyMember(string username, string password);

        Task<Member> GetMemberById(int id);

        Task<bool> UpdateMemberAsync(Member member, int member_Id);

        Task<Member> DeleteMemberAsync(int member_Id);

        Task<bool> IsUsernameUnique(string username);

        Task SubtrackHour(int id);
     
    }
}
