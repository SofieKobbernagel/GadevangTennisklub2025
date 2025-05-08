using GadevangTennisklub2025.Models;

namespace GadevangTennisklub2025.Interfaces
{
    public interface ITeamService
    {
        public Task<List<Models.Team>> GetAllTeamsAsync();
        public Task<List<Models.Team>> GetAllAttendedTeamsAsync(Member member);
        public Task<Team> GetTeamFromIdAsync(int Id);
        public Task<List<Team>> GetTeamByNameAsync(string name);
        public Task<bool> CreateTeamAsync(Team team);
        public Task<bool> UpdateTeamAsync(Team team, int teamNum);
        public Task<Team> DeleteTeamAsync(int teamNr);

        public Task<bool> AttendTeamAsync(Team team, Member member);
        // Member? SelectedMember { get; set; }
        public Member MemberById(int id);

    }
}
