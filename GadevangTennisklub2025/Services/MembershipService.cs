using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GadevangTennisklub2025.Services
{
    public class MembershipService : IMembershipService
    {
        private string connectionString = Secret.ConnectionString;
        private string selectAllMembershipsSql = "select * From Membership";


        public async Task<List<Membership>> GetAllMembershipsAsync()
        {
            List<Membership> memberships = new List<Membership>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectAllMembershipsSql, connection);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        string type = reader.GetString("MembershipType");
                        decimal price = reader.GetDecimal("Price");
                        string rights = reader.GetString("Rights");

                        Membership m = new Membership(type,price,rights);

                        memberships.Add(m);

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
            return memberships;
        }
    }
}
