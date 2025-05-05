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
        private string updateCoachSql = "UPDATE Coach SET Name = @Name, ProfileImagePath = @ProfileImagePath WHERE Coach_Id = @Coach_Id";
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

        public Task<bool> DeleteCoachAsync(int coachId)
        {
            throw new NotImplementedException();
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

                        Coach c = new Coach(name, phone, email, id, address, filepath,contractFilePath,city,postalcode);
                  
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
    

        public Task<Coach?> GetCoachByIdAsync(int coachId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateCoachAsync(Coach coach)
        {
            throw new NotImplementedException();
        }
    }
}
