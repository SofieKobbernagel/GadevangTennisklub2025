using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GadevangTennisklub2025.Services
{
    public class CoachService : ICoachService
    {
        private string connectionString = Secret.ConnectionString;
        private string selectAllCoachesSql = "SELECT * FROM Coach";
        private string insertCoachSql = "INSERT INTO Coach (Salary, Address, ContractFilePath, Name, Tlf, Email, City, PostalCode, ProfileImagePath) VALUES ( @Salary, @Address, @ContractFilePath, @Name, @Tlf, @Email, @City, @PostalCode, @ProfileImagePath)";
        private string updateCoachSql = "UPDATE Coach SET Salary = @Salary, Address = @Address, ContractFilePath = @ContractFilePath, Name = @Name, TLF = @TLF, Email = @Email, City = @City, PostalCode = @PostalCode, ProfileImagePath = @ProfileImagePath WHERE Coach_Id = @Coach_Id";
        private string deleteCoachSql = "DELETE FROM Coach WHERE Coach_Id = @Coach_Id";
        private string selectCoachByIdSql = "SELECT * FROM Coach WHERE Coach_Id = @Coach_Id";

        public async Task<bool> CreateCoachAsync(Coach coach)
        {
            bool isCreated = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(insertCoachSql, connection);
                    command.Parameters.AddWithValue("@Coach_Id", coach.Coach_Id);
                    command.Parameters.AddWithValue("@Salary", coach.Salary);
                    command.Parameters.AddWithValue("@Address", coach.Address);
                    command.Parameters.AddWithValue("@ContractFilePath", coach.ContractFilePath);
                    command.Parameters.AddWithValue("@Tlf", coach.Phone);
                    command.Parameters.AddWithValue("@Email", coach.Email);
                    command.Parameters.AddWithValue("@City", coach.City);
                    command.Parameters.AddWithValue("@PostalCode", coach.PostalCode);
                    command.Parameters.AddWithValue("@Name", coach.Name);
                    command.Parameters.AddWithValue("@ProfileImagePath", coach.ProfileImagePath);

                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        isCreated = true;
                    }
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error: " + sqlExp.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error: " + ex.Message);
                    return false;
                }
            }
            return isCreated;
        }

        public async Task<Coach> DeleteCoachAsync(int coach_Id)
        {
            Coach? deletedCoach = await GetCoachByIdAsync(coach_Id);
            if (deletedCoach == null)
            {
                return null;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(deleteCoachSql, connection);
                    command.Parameters.AddWithValue("@Coach_Id", coach_Id);
                    await connection.OpenAsync();

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                        deletedCoach = null;
                }

                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error: " + sqlExp.Message);
                    deletedCoach = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error: " + ex.Message);
                    deletedCoach = null;
                }
                return deletedCoach;
            }
        }

        public async Task<List<Coach>> GetAllCoachesAsync()
        {
            List<Coach> coaches = new List<Coach>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectAllCoachesSql, connection);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {              
                        string name = reader.GetString("Name");
                        string city = reader.GetString("City");
                        string phone = reader.GetString("TLF");              
                        string postalcode = reader.GetString("PostalCode");
                        decimal sallary = reader.GetDecimal("Salary");
                        string contractFilePath = reader.GetString("ContractFilePath");
                        string address = reader.GetString("Address");
                        int id = reader.GetInt32("Coach_Id");
                        string email = reader.GetString("Email");        
                        string filepath = reader.IsDBNull(reader.GetOrdinal("ProfileImagePath")) ? null : reader.GetString(reader.GetOrdinal("ProfileImagePath"));

                        Coach c = new Coach(name, phone, email, id, address, filepath,contractFilePath,city,postalcode, sallary);
                  
                        coaches.Add(c);

                    }
                    reader.Close();
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("SQL ERROR: " + sqlExp.Message);
                    Console.WriteLine("Stack Trace: " + sqlExp.StackTrace);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GENERAL ERROR: " + ex.Message);
                    Console.WriteLine("Stack Trace: " + ex.StackTrace);
                }
            }
            return coaches;
        }
    
        public async Task<Coach?> GetCoachByIdAsync(int coach_Id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Coach? foundCoach = null;
                try
                {

                    SqlCommand command = new SqlCommand(selectCoachByIdSql, connection);
                    command.Parameters.AddWithValue("@Coach_Id", coach_Id);

                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        string name = reader.GetString("Name");
                        string city = reader.GetString("City");
                        string phone = reader.GetString("TLF");
                        string postalcode = reader.GetString("PostalCode");
                        decimal sallary = reader.GetDecimal("Salary");
                        string contractFilePath = reader.GetString("ContractFilePath");
                        string address = reader.GetString("Address");
                        int id = reader.GetInt32("Coach_Id");
                        string email = reader.GetString("Email");
                        string filepath = reader.IsDBNull(reader.GetOrdinal("ProfileImagePath")) ? null : reader.GetString(reader.GetOrdinal("ProfileImagePath"));

                        foundCoach = new Coach(name, phone, email, id, address, filepath, contractFilePath, city, postalcode, sallary);
                        


                    }
                    reader.Close();

                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    return null;
                }

                return foundCoach;
            }
        }

        public async Task<bool> UpdateCoachAsync(Coach coach, int coach_Id)
        {
            bool isUpdated = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(updateCoachSql, connection);
                    command.Parameters.AddWithValue("@Coach_Id", coach.Coach_Id);
                    command.Parameters.AddWithValue("@Salary", coach.Salary);
                    command.Parameters.AddWithValue("@Address", coach.Address);
                    command.Parameters.AddWithValue("@ContractFilePath", coach.ContractFilePath);
                    command.Parameters.AddWithValue("@Tlf", coach.Phone);
                    command.Parameters.AddWithValue("@Email", coach.Email);
                    command.Parameters.AddWithValue("@City", coach.City);
                    command.Parameters.AddWithValue("@PostalCode", coach.PostalCode);
                    command.Parameters.AddWithValue("@Name", coach.Name);
                    command.Parameters.AddWithValue("@ProfileImagePath", coach.ProfileImagePath);

                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        isUpdated = true;
                    }
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error: " + sqlExp.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error: " + ex.Message);
                    return false;
                }
            }
            return isUpdated;
        }

        public async Task<Coach?> GetCoachByTeamIdAsync(int teamId)
        {
            string query = @"
        SELECT c.*
        FROM Coach c
        INNER JOIN RelTeamCoach rtc ON c.Coach_Id = rtc.Coach_Id
        WHERE rtc.Team_Id = @TeamId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Coach? foundCoach = null;
                {
                    

                    try
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@TeamId", teamId);
                        await command.Connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        Thread.Sleep(1000);
                        while (await reader.ReadAsync())
                        {
                            string name = reader.GetString(reader.GetOrdinal("Name"));
                            string city = reader.GetString(reader.GetOrdinal("City"));
                            string phone = reader.GetString(reader.GetOrdinal("TLF"));
                            string postalcode = reader.GetString(reader.GetOrdinal("PostalCode"));
                            decimal salary = reader.GetDecimal(reader.GetOrdinal("Salary"));
                            string contractFilePath = reader.GetString(reader.GetOrdinal("ContractFilePath"));
                            string address = reader.GetString(reader.GetOrdinal("Address"));
                            int id = reader.GetInt32(reader.GetOrdinal("Coach_Id"));
                            string email = reader.GetString(reader.GetOrdinal("Email"));
                            string? filepath = reader.IsDBNull(reader.GetOrdinal("ProfileImagePath"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("ProfileImagePath"));

                            foundCoach = new Coach(name, phone, email, id, address, filepath, contractFilePath, city, postalcode, salary);
                        }

                        reader.Close();
                    }
                    catch (SqlException sqlExp)
                    {
                        Console.WriteLine("Database error: " + sqlExp.Message);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("General error: " + ex.Message);
                        return null;
                    }
                }
                return foundCoach;
            }
        }

    }
}
