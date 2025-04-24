using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface IMemberService
    {
        public Task<List<Member>> GetAllMembersAsync();

        public Member VerifyMember(string username, string password);
    }
}
