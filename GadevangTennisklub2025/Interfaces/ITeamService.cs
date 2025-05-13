using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface ITeamService
    {
        public List<Models.Member> Members();
        public Task<List<Models.Team>> GetAllTeamsAsync();
        public Task<List<Models.Team>> GetAllAttendedTeamsAsync(int memberId);
        public Task<List<Member>> GetAttendeesAsync(int teamId);
        public Task<Team> GetTeamFromIdAsync(int Id);
        public Task<List<Team>> GetTeamByNameAsync(string name);
        public Task<bool> CreateTeamAsync(Team team);
        public Task<bool> UpdateTeamAsync(Team team, int teamNum);
        public Task<Team> DeleteTeamAsync(int teamNr);

        public Task AttendTeamAsync(Team team, Member member);
        public Task LeaveTeamAsync(Team team, Member member);
        
        public Member MemberById(int id);

    }
}
