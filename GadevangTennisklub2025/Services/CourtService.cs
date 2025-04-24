using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GadevangTennisklub2025.Services
{
    public class CourtService : ICourtService
    {
        private string connectionString = Secret.ConnectionString;

        private string queryString = "SELECT Court_Id, Type FROM Court";
        private string findCourtByIDSql = "select Court_Id, Type FROM Court WHERE Court_Id = @ID";
        private string insertSql = "Insert INTO Court Values(@ID, @Type)";
        private string updateSql = "UPDATE Court SET Type = @Type WHERE Court_Id = @ID";
        private string deleteSql = "DELETE FROM Court WHERE Court_Id = @ID";

        public async Task<bool> CreateCourtAsync(TennisField tennisField)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(insertSql, connection);

                try
                {
                    command.Parameters.AddWithValue("@ID", tennisField.CourtId);
                    command.Parameters.AddWithValue("@Type", tennisField.Type);

                    await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();


                    return true;
                }
                catch (SqlException sqlEx)
                {
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return false;
        }

        public async Task<TennisField> DeleteCourtAsync(int courtId)
        {
            TennisField? court = GetCourtFromIdAsync(courtId).Result;
            if (court == null)
                return null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(deleteSql, connection);
                    cmd.Parameters.AddWithValue("@ID", courtId);
                    await connection.OpenAsync();
                    int noOfRows = cmd.ExecuteNonQueryAsync().Result;
                    if (noOfRows > 0)
                        return court;
                    else
                        return null;
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine(sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
                return court;
            }
        }

        public async Task<List<TennisField>> GetAllCourtsAsync()
        {
            List<TennisField> courts = new List<TennisField>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    Thread.Sleep(1000);
                    while (await reader.ReadAsync())
                    {
                        int courtId = reader.GetInt32("Court_Id");
                        string courtType = reader.GetString("Type");
                        TennisField court = new TennisField(courtId, courtType);
                        courts.Add(court);
                    }
                    reader.Close();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error" + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    throw ex;
                }
                finally
                {

                }
            }
            return courts;
        }

        public async Task<TennisField> GetCourtFromIdAsync(int courtId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                TennisField court = null;
                try
                {
                    SqlCommand command = new SqlCommand(findCourtByIDSql, connection);
                    command.Parameters.AddWithValue("@ID", courtId);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        int cNr = reader.GetInt32("Court_Id");
                        string courtType = reader.GetString("Type");
                        court = new TennisField(cNr, courtType);
                    }
                    reader.Close();
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error" + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    throw ex;
                }
                finally
                {

                }
                return court;
            }
        }

        public async Task<bool> UpdateHotelAsync(TennisField tennisField)
        {
            bool temp = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(updateSql, connection);
                    command.Parameters.AddWithValue("@ID", tennisField.CourtId);
                    command.Parameters.AddWithValue("@Type", tennisField.Type);
                    command.Connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    reader.Close();
                    temp = true;
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error" + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    throw ex;
                }
                finally
                {

                }
            }
            return temp;
        }
    }
}
