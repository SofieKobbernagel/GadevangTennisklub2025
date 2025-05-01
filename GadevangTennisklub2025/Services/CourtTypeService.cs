using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GadevangTennisklub2025.Services
{
    public class CourtTypeService : ICourtTypeService
    {
        private string connectionString = Secret.ConnectionString;
        private string selectAllCourtTypesSql = "select * From CourtType";


        public async Task<List<CourtTypes>> GetAllCourtsAsync()
        {
            List<CourtTypes> courtTypes = new List<CourtTypes>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectAllCourtTypesSql, connection);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        string type = reader.GetString("Type");

                        CourtTypes c = new CourtTypes(type);

                        courtTypes.Add(c);

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
            return courtTypes;
        }
    }
}
