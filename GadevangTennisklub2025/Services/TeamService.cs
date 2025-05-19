using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace GadevangTennisklub2025.Services
{
    public class TeamService : ITeamService
    {
        private string connectionString = Secret.ConnectionString;
        private string queryString = "SELECT  Team_Id, MemberType, Name, Length, TimeOfDay, DayOfWeek, MinMembers, MaxMembers, Description FROM Team";
        private string updateQuery = "UPDATE Team SET  MemberType = @MemberType, Name = @Name, Length = @Length, TimeOfDay = @TimeOfDay, DayOfWeek = @DayOfWeek, MinMembers = @MinMembers, MaxMembers = @MaxMembers, Description = @Description WHERE Team_Id = @Team_Id";
        private string deleteQuery = "DELETE FROM Team WHERE Name=@Name;";
        private string createQuery = "INSERT INTO Team ( MemberType, Name, Length, TimeOfDay, DayOfWeek, MinMembers, MaxMembers,  Description)\r\nVALUES (  @MemberType, @Name, @Length, @TimeOfDay, @DayOfWeek, @MinMembers, @MaxMembers,  @Description);";

        private string searchNrQuery = "SELECT  Team_Id,MemberType,Name,Length,TimeOfDay,DayOfWeek,MinMembers,MaxMembers,Description FROM Team WHERE Team_Id = @Team_Id";
        private string searchNameQuery = "SELECT  Team_Id,MemberType,Name,Length,TimeOfDay,DayOfWeek,MinMembers,MaxMembers,Description FROM Team WHERE Name = @Name";
        private string searchMembershipTypeQuery = "SELECT  Team_Id,MemberType,Name,Length,TimeOfDay,DayOfWeek,MinMembers,MaxMembers,Description FROM Team WHERE MemberType = @MemberType";
        public MemberService memberService = new MemberService();
        public CoachService coachService = new CoachService();

        public Member SelectedMember { get; set; }
        public Coach Trainer { get; set; }
        public List<Models.Member> Members()
        {
            return  memberService.GetAllMembersAsync().Result;
        }
        public List<Models.Coach> Coaches()
        {
            return coachService.GetAllCoachesAsync().Result;
        }
        public Member MemberById(int id)
        {
            Member mem = new Member();
            foreach(Member me in Members())
            {
                if (me.Member_Id == id)
                {
                    return me;
                }
            }
            return mem;
        }
        
        public async Task<bool> CreateTeamAsync(Team team)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(createQuery, connection);

                    // Add parameters to prevent SQL injection

                    command.Parameters.AddWithValue("@Team_Id", team.Id);
                    command.Parameters.AddWithValue("@MemberType", team.MembershipType);                  
                    command.Parameters.AddWithValue("@Name", team.Name);
                    command.Parameters.AddWithValue("@Length", team.Length.ToString(CultureInfo.InvariantCulture));
                    Console.WriteLine("TeamService/CreateTeam/ double length = " + team.Length + " , ,  string length = " + team.Length.ToString(CultureInfo.InvariantCulture));
                    command.Parameters.AddWithValue("@TimeOfDay", team.TimeOfDay);
                    command.Parameters.AddWithValue("@DayOfWeek", team.DayOfWeek);
                    command.Parameters.AddWithValue("@MinMembers", team.AttendeeRange[0]);
                    command.Parameters.AddWithValue("@MaxMembers", team.AttendeeRange[1]);
                    command.Parameters.AddWithValue("@Description", team.Description);

                    RelationshipsServicesAsync relationshipsServices = new RelationshipsServicesAsync();
                    await relationshipsServices.TeamCoachRelation(team.Id, team.Trainer.Coach_Id);
                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync(); // Correct method for UPDATE

                    return true; // Return true if at least one row was Insert
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error: " + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error: " + ex.Message);
                }
            }
            return false; // Return false if the Insert fails
        }

        public async Task<Team> DeleteTeamAsync(int teamNr)
        {
            Team team = await GetTeamFromIdAsync(teamNr);
            //string updateQuery = "UPDATE Hotel SET HotelName = @HotelName, HotelAddress = @HotelAddress WHERE HotelNr = @HotelNr";              using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(deleteQuery, connection);

                    // Add parameters to prevent SQL injection
                    Team te = await GetTeamFromIdAsync(teamNr);
                    command.Parameters.AddWithValue("@Name", te.Name);


                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync(); // Correct method for UPDATE

                    return team; // Return true if at least one row was Deleted
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error: " + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error: " + ex.Message);
                }
            }
            return null; // Return false if the Delete fails
        }

        public async Task<List<Team>> GetAllAttendedTeamsAsync(int memberId)
        {
                List<Team> teams = new List<Team>();

                string query = @"
        SELECT t.*
        FROM RelMemberTeam r
        INNER JOIN Team t ON r.Team_Id = t.Team_Id
        WHERE r.Member_Id = @MemberId";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MemberId", memberId);
                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int[] attendeeRange = new int[]
                                {
                        reader.GetInt32(reader.GetOrdinal("MinMembers")),
                        reader.GetInt32(reader.GetOrdinal("MaxMembers"))
                                };

                                Team team = new Team
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Team_Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    DayOfWeek = reader.GetInt32(reader.GetOrdinal("DayOfWeek")),
                                    TimeOfDay = TimeOnly.Parse(reader.GetString(reader.GetOrdinal("TimeOfDay"))),
                                    Length = double.Parse(reader.GetString(reader.GetOrdinal("Length")), CultureInfo.InvariantCulture),
                                    AttendeeRange = attendeeRange,
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    MembershipType = reader.GetString(reader.GetOrdinal("MemberType")),
                                    Attendees = new List<Member>(), // Optional: populate separately if needed
                                    Trainer = await coachService.GetCoachByTeamIdAsync(reader.GetInt32(reader.GetOrdinal("Team_Id")))
                                };

                                teams.Add(team);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in GetTeamsForMemberAsync: " + ex.Message);
                }

                return teams;
            }   

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            List<Team> teams = new List<Team>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);
                        await command.Connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        //Thread.Sleep(50);
                        while (await reader.ReadAsync())
                        {
                            int teamID = reader.GetInt32("Team_Id");
                            string teamNavn = reader.GetString("Name");
                            int dayOfWeek = reader.GetInt32("DayOfWeek");
                            string startTime = reader.GetString("TimeOfDay"); //ex. 14:30
                            double length = double.Parse(reader.GetString("Length"), CultureInfo.InvariantCulture);
                            int[] attendeeRange = { reader.GetInt32("MinMembers"), reader.GetInt32("MaxMembers") };

                            
                            string description = reader.GetString("Description");
                            string membershipType = reader.GetString("MemberType");

                            List<Member> Attendees = await GetAttendeesAsync(teamID);
                            Coach trainer = await coachService.GetCoachByTeamIdAsync(teamID);
                            Team team = new Team(teamID, teamNavn, membershipType, trainer, dayOfWeek, TimeOnly.Parse(startTime), length, attendeeRange, Attendees, description);

                            teams.Add(team);
                        }
                        reader.Close();
                    }
                    catch (SqlException sqlExp)
                    {
                        Console.WriteLine("Database error" + sqlExp.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Generel fejl: " + ex.Message);
                    }
                    finally
                    {

                    }
                }
                return teams;
            }
        }

        public async Task<Team> GetTeamFromIdAsync(int searchID)
        {
            Team result_team = new Team();
            List<Team> teams = new List<Team>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);
                        await command.Connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                       
                        while (await reader.ReadAsync())
                        {
                            int teamID = reader.GetInt32("Team_Id");
                            if (searchID == teamID)
                            {
                            
                            
                            string teamNavn = reader.GetString("Name");
                            int dayOfWeek = reader.GetInt32("DayOfWeek");
                            string startTime = reader.GetString("TimeOfDay"); //ex. 14:30
                            double length = double.Parse(reader.GetString("Length"), CultureInfo.InvariantCulture);
                            int[] attendeeRange = { reader.GetInt32("MinMembers"), reader.GetInt32("MaxMembers") };
                            
                            string description = reader.GetString("Description");
                            string membershipType = reader.GetString("MemberType");
                                Coach trainer = await coachService.GetCoachByTeamIdAsync(teamID);

                                List<Member> Attendees = await GetAttendeesAsync(teamID);
                                result_team = new Team(teamID, teamNavn, membershipType, trainer, dayOfWeek, TimeOnly.Parse(startTime), length, attendeeRange, Attendees, description);
                                reader.Close();
                            }
                            

                        }
                        reader.Close();
                    }
                    catch (SqlException sqlExp)
                    {
                        Console.WriteLine("Database error" + sqlExp.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Generel fejl: " + ex.Message);
                    }
                    finally
                    {

                    }
                }
                return result_team;
            }
        }

        public async Task<List<Team>> GetTeamByNameAsync(string name)
        {


            List<Team> teams = new List<Team>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);
                        await command.Connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        //Thread.Sleep(1000);
                        while (await reader.ReadAsync())
                        {

                            int teamID = reader.GetInt32("Team_Id");
                            string teamNavn = reader.GetString("Name");
                            int dayOfWeek = reader.GetInt32("DayOfWeek");
                            string startTime = reader.GetString("TimeOfDay"); //ex. 14:30
                            double length = double.Parse(reader.GetString("Length"), CultureInfo.InvariantCulture);
                            int[] attendeeRange = { reader.GetInt32("MinMembers"), reader.GetInt32("MaxMembers") };
                            
                            string description = reader.GetString("Description");
                            string membershipType = reader.GetString("MemberType");
                            Coach trainer = await coachService.GetCoachByTeamIdAsync(teamID);
                            List<Member> Attendees =await GetAttendeesAsync(teamID);

                            if (teamNavn == name)
                            {
                                Team team = new Team(teamID, teamNavn, membershipType, trainer, dayOfWeek, TimeOnly.Parse(startTime), length, attendeeRange, Attendees, description);
                                teams.Add(team);
                            }


                        }
                        reader.Close();
                    }
                    catch (SqlException sqlExp)
                    {
                        Console.WriteLine("Database error" + sqlExp.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Generel fejl: " + ex.Message);
                    }
                    finally
                    {
                    }
                }
                return teams;
            }
        }

        public async Task<bool> UpdateTeamAsync(Team team)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(updateQuery, connection);

                    // Add parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@Team_Id", team.Id);
                    command.Parameters.AddWithValue("@MemberType", team.MembershipType);
                    command.Parameters.AddWithValue("@Name", team.Name);
                    //Console.WriteLine("TeamService/UpdateTeamAsync length = "+team.Length);
                    command.Parameters.AddWithValue("@Length", team.Length.ToString(CultureInfo.InvariantCulture));
                    command.Parameters.AddWithValue("@TimeOfDay", team.TimeOfDay);
                    command.Parameters.AddWithValue("@DayOfWeek", team.DayOfWeek);
                    command.Parameters.AddWithValue("@MinMembers", team.AttendeeRange[0]);
                    command.Parameters.AddWithValue("@MaxMembers", team.AttendeeRange[1]);
                    command.Parameters.AddWithValue("@Description", team.Description);

                    RelationshipsServicesAsync relationshipsServices = new RelationshipsServicesAsync();
                    await relationshipsServices.UpdateTeamCoachRelation(team.Id, team.Trainer.Coach_Id);
                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync(); // Correct method for UPDATE

                    return rowsAffected > 0; // Return true if at least one row was updated
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error: " + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error: " + ex.Message);
                }
            }
            return false; // Return false if the update fails
        }

        public async Task AttendTeamAsync(Team team, Member member)
        {
            
            RelationshipsServicesAsync relationshipsServices = new RelationshipsServicesAsync();
            await relationshipsServices.TeamMemberRelation(team.Id, member.Member_Id);
            
        }

        public async Task LeaveTeamAsync(Team team, Member member)
        {
            string succes = "unsuccesfully";
            List<Member> mem = await GetAttendeesAsync(team.Id);
            List<int> memint = new List<int>();
            foreach(Member m in mem)
            {
                memint.Add(m.Member_Id);
            }
            if (memint.Contains(member.Member_Id))
            {
                RelationshipsServicesAsync relationshipsServices = new RelationshipsServicesAsync();
               await relationshipsServices.RemoveTeamMemberRelation(team.Id, member.Member_Id);
                succes = "succesfully";
            }
            Console.WriteLine("TeamService/LeaveTeamAsync  just ran "+succes);
        }

        public async Task<List<Team>> Search(string SearchType, string Search)
        {
            var teams = new List<Team>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string query = SearchType.ToLower() switch
                    {
                        "id" => "SELECT TOP 5 * FROM Team WHERE Team_Id LIKE @Search",
                        "name" => "SELECT TOP 5 * FROM Team WHERE Name LIKE @Search",
                        "membershiptype" => "SELECT TOP 5 * FROM Team WHERE MembershipType LIKE @Search",
                        _ => throw new ArgumentException("Invalid search type")
                    };

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Search", $"%{Search}%");

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int tempId = Convert.ToInt32(reader["Team_Id"]);
                                string tempName = Convert.ToString(reader["Name"]);
                                string tempMembershipType = Convert.ToString(reader["MemberType"]);
                                int tempDayOfWeek = Convert.ToInt32(reader["DayOfWeek"]);
                                string timeStr = Convert.ToString(reader["TimeOfDay"]);
                                TimeOnly tempTimeOfDay = TimeOnly.Parse(timeStr);
                                double tempLength = Convert.ToDouble(reader["Length"]);
                                int[] tempAttendeeRange =
                                [
                                    Convert.ToInt32(reader["MinMembers"]),
                            Convert.ToInt32(reader["MaxMembers"])
                                ];
                                string tempDescription = Convert.ToString(reader["Description"]);

                                // Load external data
                                Coach trainer = await coachService.GetCoachByTeamIdAsync(tempId);
                                List<Member> tempAttendees = await GetAttendeesAsync(tempId);

                                var team = new Team(
                                    tempId,
                                    tempName,
                                    tempMembershipType,
                                    trainer,
                                    tempDayOfWeek,
                                    tempTimeOfDay,
                                    tempLength,
                                    tempAttendeeRange,
                                    tempAttendees,
                                    tempDescription
                                );

                                teams.Add(team);
                            }
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error: " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error: " + ex.Message);
                }
            }

            return teams;
        }


        public async Task<List<Member>> GetAttendeesAsync(int teamId)
        {
            List<Member> attendees = new List<Member>();

            string query = @"
SELECT 
    m.Member_Id,
    m.Name,
    m.Address,
    m.Gender,
    m.Email,
    m.PostalCode,
    m.TLF AS Phone,
    m.OtherTLF AS OtherPhone,
    m.City,
    m.MembershipType,
    m.birthday,
    m.NewsSubscriber,
    m.UserName AS Username,
    m.Password,
    m.IsAdmin,
    m.Municipality,
    m.PictureConsent,
    m.ProfileImagePath
FROM RelMemberTeam r
INNER JOIN Members m ON r.Member_Id = m.Member_Id
WHERE r.Team_Id = @TeamId";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TeamId", teamId);
                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Member member = new Member
                            {
                                Member_Id = reader.GetInt32(reader.GetOrdinal("Member_Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Gender = reader.GetString(reader.GetOrdinal("Gender")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                PostalCode = reader.GetString(reader.GetOrdinal("PostalCode")),
                                Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                OtherPhone = reader.IsDBNull(reader.GetOrdinal("OtherPhone")) ? null : reader.GetString(reader.GetOrdinal("OtherPhone")),
                                City = reader.GetString(reader.GetOrdinal("City")),
                                MemberType = reader.GetString(reader.GetOrdinal("MembershipType")),
                                Birthday = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("birthday"))),
                                NewsSubscriber = reader.GetBoolean(reader.GetOrdinal("NewsSubscriber")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                IsAdmin = reader.GetBoolean(reader.GetOrdinal("IsAdmin")),
                                Municipality = reader.GetString(reader.GetOrdinal("Municipality")),
                                PictureConsent = reader.GetString(reader.GetOrdinal("PictureConsent")),
                                ProfileImagePath = reader.IsDBNull(reader.GetOrdinal("ProfileImagePath")) ? null : reader.GetString(reader.GetOrdinal("ProfileImagePath"))
                            };

                            attendees.Add(member);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetAttendeesAsync: " + ex.Message);
                // Optionally log or rethrow
            }

            return attendees;
        }




    }

}


