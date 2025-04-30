using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics.Metrics;

namespace GadevangTennisklub2025.Services
{
    public class TeamService : ITeamService
    {
        private string connectionString = Secret.ConnectionString;
        private string queryString = "SELECT  Team_Id,MembershipType,Name,Length,TimeOfDay,DayOfWeek,MinMembers,MaxMembers,Attendees,Master,Description FROM Team";
        private string updateQuery = "UPDATE Team SET MembershipType = @MembershipType, Name = @Name, Length = @Length, TimeOfDay = @TimeOfDay, DayOfWeek = @DayOfWeek, MinMembers = @MinMembers, MaxMembers = @MaxMembers, Attendees = @Attendees, Master = @Master, Description = @Description WHERE Team_Id = @Team_Id";
        private string deleteQuery = "DELETE FROM Team WHERE Name=@Name;";
        private string insertQuery = "INSERT INTO Team ( Team_Id,MembershipType,Name,Length,TimeOfDay,DayOfWeek,MinMembers,MaxMembers,Attendees,Master,Description)\r\nVALUES ( @Team_Id, @MembershipType, @Name,Length, @TimeOfDay, @DayOfWeek, @MinMembers, @MaxMembers, @Attendees, @Master, @Description);";

        private string searchNrQuery = "SELECT  Team_Id,MembershipType,Name,Length,TimeOfDay,DayOfWeek,MinMembers,MaxMembers,Attendees,Master,Description FROM Team WHERE Team_Id = @Team_Id";
        private string searchNameQuery = "SELECT  Team_Id,MembershipType,Name,Length,TimeOfDay,DayOfWeek,MinMembers,MaxMembers,Attendees,Master,Description FROM Team WHERE Name = @Name";
        private string searchMembershipTypeQuery = "SELECT  Team_Id,MembershipType,Name,Length,TimeOfDay,DayOfWeek,MinMembers,MaxMembers,Attendees,Master,Description FROM Team WHERE MembershipType = @MembershipType";
        public MemberService memberService = new MemberService();
        public List<Member> Members()
        {
            return  memberService.GetAllMembersAsync().Result;
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
        public List<Member> IdsToMembers(List<int> ids)
        {
            List<Member> members = new List<Member>();
            foreach(int id in ids)
            {
                members.Add(MemberById(id));
            }
            return members;
        }
        public List<int> ListIntFromString(string strings) 
        {
            List<int> output=new List<int>();
            List<string> outStrings=new List<string>(strings.Length);
            int k=0;
            for(int i=0;i<=strings.Length;i++){
                if(strings[i] == ',')
                {
                    k++;
                }
                else {
                    outStrings[k].Append(strings[i]);
                }

            }
            foreach(string str in outStrings)
            {
                output.Add(int.Parse(str));
            }
            return output;

        }

        public string MembersToString(List<Member> members)
        {
            string result = "";
            result += members[0].Member_Id;
            for(int i = 1; i <= members.Count; i++)
            {
                result += ","+members[i].Member_Id;
            }
            return result;
        }

        public async Task<bool> CreateTeamAsync(Team team)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(insertQuery, connection);

                    // Add parameters to prevent SQL injection

                    command.Parameters.AddWithValue("@Team_Id", team.Id);
                    command.Parameters.AddWithValue("@MembershipType", team.MembershipType);
                    command.Parameters.AddWithValue("@Name", team.Name);
                    command.Parameters.AddWithValue("@Length", team.Length);
                    command.Parameters.AddWithValue("@TimeOfDay", team.TimeOfDay);
                    command.Parameters.AddWithValue("@DayOfWeek", team.DayOfWeek);
                    command.Parameters.AddWithValue("@MinMembers", team.AttendeeRange[0]);
                    command.Parameters.AddWithValue("@MaxMembers", team.AttendeeRange[1]);
                    command.Parameters.AddWithValue("@Attendees", MembersToString(team.Attendees)); //---
                    command.Parameters.AddWithValue("@Master", team.Master.Member_Id);
                    command.Parameters.AddWithValue("@Description", team.Description);

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
                    command.Parameters.AddWithValue("@Name", GetTeamFromIdAsync(teamNr).Result.Name);


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

        public async Task<List<Team>> GetAllAttendedTeamsAsync(Member member)
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
                        Thread.Sleep(50);
                        while (await reader.ReadAsync())
                        {
                            int teamID = reader.GetInt32("Team_No");
                            string teamNavn = reader.GetString("Name");
                            int dayOfWeek = reader.GetInt32("DayOfWeek");
                            string startTime = reader.GetString("StartTime"); //ex. 14:30
                            string length = reader.GetString("Length");
                            int[] attendeeRange = { reader.GetInt32("MinMembers"), reader.GetInt32("MaxMembers") };
                            string attendeesID = reader.GetString("Attendees");//"memberID,memberID,memberID..."
                            int masterID = reader.GetInt32("Master");
                            string description = reader.GetString("Description");
                            string membershipType = reader.GetString("MembershipType");

                            Member master = MemberById(masterID);
                            List<Member> Attendees = IdsToMembers(ListIntFromString(attendeesID));

                            Team team = new Team(teamID, teamNavn, membershipType, dayOfWeek, TimeOnly.Parse(startTime), double.Parse(length), attendeeRange, Attendees, master, description);
                            if (Attendees.Contains(member))
                            {
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
                        Thread.Sleep(50);
                        while (await reader.ReadAsync())
                        {
                            int teamID = reader.GetInt32("Team_No");
                            string teamNavn = reader.GetString("Name");
                            int dayOfWeek = reader.GetInt32("DayOfWeek");
                            string startTime = reader.GetString("StartTime"); //ex. 14:30
                            string length = reader.GetString("Length");
                            int[] attendeeRange = { reader.GetInt32("MinMembers"), reader.GetInt32("MaxMembers") };
                            string attendeesID = reader.GetString("Attendees");//"memberID,memberID,memberID..."
                            int masterID = reader.GetInt32("Master");
                            string description = reader.GetString("Description");
                            string membershipType = reader.GetString("MembershipType");

                            Member master = MemberById(masterID);
                            List<Member> Attendees = IdsToMembers(ListIntFromString(attendeesID));

                            Team team = new Team(teamID, teamNavn, membershipType, dayOfWeek, TimeOnly.Parse(startTime), double.Parse(length), attendeeRange, Attendees, master, description);

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
                        Thread.Sleep(1000);
                        while (await reader.ReadAsync())
                        {
                            int teamID = reader.GetInt32("Team_No");
                            string teamNavn = reader.GetString("Name");
                            int dayOfWeek = reader.GetInt32("DayOfWeek");
                            string startTime = reader.GetString("StartTime"); //ex. 14:30
                            string length = reader.GetString("Length");
                            int[] attendeeRange = { reader.GetInt32("MinMembers"), reader.GetInt32("MaxMembers") };
                            string attendeesID = reader.GetString("Attendees");//"memberID,memberID,memberID..."
                            int masterID = reader.GetInt32("Master");
                            string description = reader.GetString("Description");
                            string membershipType = reader.GetString("MembershipType");

                            Member master = MemberById(masterID);
                            List<Member> Attendees = IdsToMembers(ListIntFromString(attendeesID));
                            if (searchID == teamID)
                            {
                                result_team = new Team(teamID, teamNavn, membershipType, dayOfWeek, TimeOnly.Parse(startTime), double.Parse(length), attendeeRange, Attendees, master, description);
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
                        Thread.Sleep(1000);
                        while (await reader.ReadAsync())
                        {

                            int teamID = reader.GetInt32("Team_No");
                            string teamNavn = reader.GetString("Name");
                            int dayOfWeek = reader.GetInt32("DayOfWeek");
                            string startTime = reader.GetString("StartTime"); //ex. 14:30
                            string length = reader.GetString("Length");
                            int[] attendeeRange = { reader.GetInt32("MinMembers"), reader.GetInt32("MaxMembers") };
                            string attendeesID = reader.GetString("Attendees");//"memberID,memberID,memberID..."
                            int masterID = reader.GetInt32("Master");
                            string description = reader.GetString("Description");
                            string membershipType = reader.GetString("MembershipType");

                            Member master = MemberById(masterID);
                            List<Member> Attendees = IdsToMembers(ListIntFromString(attendeesID));

                            if (teamNavn == name)
                            {
                                Team team = new Team(teamID, teamNavn, membershipType, dayOfWeek, TimeOnly.Parse(startTime), double.Parse(length), attendeeRange, Attendees, master, description);
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

        public async Task<bool> UpdateTeamAsync(Team team, int teamNum)
        {
            //string updateQuery = "UPDATE Hotel SET HotelName = @HotelName, HotelAddress = @HotelAddress WHERE HotelNr = @HotelNr";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(updateQuery, connection);

                    // Add parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@MembershipType", team.MembershipType);
                    command.Parameters.AddWithValue("@Name", team.Name);
                    command.Parameters.AddWithValue("@Length", team.Length);
                    command.Parameters.AddWithValue("@TimeOfDay", team.TimeOfDay);
                    command.Parameters.AddWithValue("@DayOfWeek", team.DayOfWeek);
                    command.Parameters.AddWithValue("@MinMembers", team.AttendeeRange[0]);
                    command.Parameters.AddWithValue("@MaxMembers", team.AttendeeRange[1]);
                    command.Parameters.AddWithValue("@Attendees", team.AttendeesID);
                    command.Parameters.AddWithValue("@Master", team.Master);
                    command.Parameters.AddWithValue("@Description", team.Description);

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

        public async Task<List<Team>> Search(Search SearchI)
        {
            string search = SearchI.SearchText;
            string se_type = SearchI.SearchType;
            var teams = new List<Team>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string query;

                    switch (se_type.ToLower())
                    {
                        case "number":
                            query = "SELECT TOP 5 * FROM Team WHERE Team_Id LIKE @Search";
                            break;
                        case "name":
                            query = "SELECT TOP 5 * FROM Team WHERE Name LIKE @Search";
                            break;
                        case "membershipType":
                            query = "SELECT TOP 5 * FROM Team WHERE MembershipType LIKE @Search";
                            break;
                        default:
                            throw new ArgumentException("Invalid search type");
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Search", $"%{search}%");

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int TempId = reader.GetInt32("Team_No");
                                string TempName = reader.GetString("Name");
                                string TempMembershipType = reader.GetString("MembershipType");
                                int TempDayOfWeek = reader.GetInt32("DayOfWeek");
                                TimeOnly TempTimeOfDay = TimeOnly.Parse(reader.GetString("StartTime")); //ex. 14:30
                                Double TempLength = Double.Parse(reader.GetString("Length"));
                                int[] TempAttendeeRange =[reader.GetInt32("MinMembers"), reader.GetInt32("MaxMembers")];
                                List<Member> TempAttendees = IdsToMembers(ListIntFromString(reader.GetString("Attendees")));//"memberID,memberID,memberID..."
                                Member TempMaster = MemberById(reader.GetInt32("Master"));
                                string TempDescription = reader.GetString("Description");

                                Team tem = new Team(TempId, TempName, TempMembershipType, TempDayOfWeek, TempTimeOfDay, TempLength, TempAttendeeRange, TempAttendees, TempMaster, TempDescription);
                                
                                teams.Add(tem);
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
    }
}

