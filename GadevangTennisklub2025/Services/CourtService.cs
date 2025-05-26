using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GadevangTennisklub2025.Services
{
    /// <summary>
    /// Indeholder funktioner omhandlende baner
    /// </summary>
    public class CourtService : ICourtService
    {
        private string connectionString = Secret.ConnectionString;

        private string queryString = "SELECT Court_Id, Name, Type FROM Court";
        private string findCourtByIDSql = "select Court_Id, Name, Type FROM Court WHERE Court_Id = @ID";
        private string insertSql = "Insert INTO Court Values(@Name, @Type)";
        private string updateSql = "UPDATE Court SET Type = @Type, Name = @Name WHERE Court_Id = @ID";
        private string deleteSql = "DELETE FROM Court WHERE Court_Id = @ID";

        /// <summary>
        /// Create a court asynchronously
        /// </summary>
        /// <param name="tennisField">Takes a TennisField object</param>
        /// <returns>Adds an entry in the court database</returns>
        public async Task<bool> CreateCourtAsync(TennisField tennisField)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(insertSql, connection);

                try
                {
                    //command.Parameters.AddWithValue("@ID", tennisField.CourtId);
                    command.Parameters.AddWithValue("@Name", tennisField.Name);
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

        /// <summary>
        /// Deletes a court asynchronously
        /// </summary>
        /// <param name="courtId">Takes the ID of a court</param>
        /// <returns>Deletes an entry from the court database</returns>
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
                        string courtName = reader.GetString("Name");
                        string courtType = reader.GetString("Type");
                        TennisField court = new TennisField(courtId, courtType);
                        court.Name = courtName;
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
                        string courtName = reader.GetString("Name");
                        string courtType = reader.GetString("Type");
                        court = new TennisField(cNr, courtType);
                        court.Name = courtName;
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

        public async Task<List<TennisField>> GetCourtFromTypeAsync(string type)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<TennisField> courts = new List<TennisField>();
                try
                {
                    SqlCommand command = new SqlCommand(queryString + " where Type like @Search", connection);
                    command.Parameters.AddWithValue("@Search", "%" + type + "%");
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync()) // reads from data not from console
                    {
                        int courtId = reader.GetInt32("Court_Id");
                        string courtName = reader.GetString("Name");
                        string cType = reader.GetString("Type");
                        TennisField court = new TennisField(courtId, cType);
                        court.Name = courtName;
                        courts.Add(court);
                    }
                    reader.Close();
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                    throw sqlExp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    throw ex;
                }
                finally { }
                return courts;
            }
        }

        public async Task<List<TennisField>> GetCourtFromNameAsync(string name)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<TennisField> courts = new List<TennisField>();
                try
                {
                    SqlCommand command = new SqlCommand(queryString + " where Name like @Search", connection);
                    command.Parameters.AddWithValue("@Search", "%" + name + "%");
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync()) // reads from data not from console
                    {
                        int courtId = reader.GetInt32("Court_Id");
                        string courtName = reader.GetString("Name");
                        string cType = reader.GetString("Type");
                        TennisField court = new TennisField(courtId, cType);
                        court.Name = courtName;
                        courts.Add(court);
                    }
                    reader.Close();
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                    throw sqlExp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                    throw ex;
                }
                finally { }
                return courts;
            }
        }

        public async Task<bool> UpdateCourtAsync(TennisField tennisField)
        {
            bool temp = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(updateSql, connection);
                    command.Parameters.AddWithValue("@ID", tennisField.CourtId);
                    command.Parameters.AddWithValue("@Name", tennisField.Name);
                    command.Parameters.AddWithValue("@Type", tennisField.Type);
                    
                    await command.Connection.OpenAsync();
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
