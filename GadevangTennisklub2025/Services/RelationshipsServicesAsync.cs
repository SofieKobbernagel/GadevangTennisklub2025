using GadevangTennisklub2025.Interfaces;
using Microsoft.Data.SqlClient;

namespace GadevangTennisklub2025.Services
{
    public class RelationshipsServicesAsync : IRelationshipsServicesAsync
    {
        public async Task EventMemberRelation(int EventId, int MemberId)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO RelMemberEvent VALUES (@Member_Id,@Event_Id);", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Member_Id", MemberId);
                    cmd.Parameters.AddWithValue("@Event_Id",EventId );
                    await cmd.ExecuteNonQueryAsync();
                    con.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Database error " + e.Message);
                    throw e;
                    //return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine("general error " + e.Message);
                    throw e;
                    //return false;
                }

            }
        }
    }
}
