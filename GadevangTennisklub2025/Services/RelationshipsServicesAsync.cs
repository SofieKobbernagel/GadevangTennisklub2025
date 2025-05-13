using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;

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
                    cmd.Parameters.AddWithValue("@Event_Id", EventId);
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

        public async Task BookingMemberRelation(int MemberId, int BookingId)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO RelMemberBooking VALUES (@Member_Id,@Booking_Id);", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Member_Id", MemberId);
                    cmd.Parameters.AddWithValue("@Booking_Id", BookingId);
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

        public async Task<bool> MemberAvailible(int memberID, DateTime start, DateTime end)
        {
            List<Booking> BookingList = new List<Booking>();
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {

                try
                {
                    //SqlCommand cmd = new SqlCommand("select * from Booking where Booking_Id=(select Booking_Id from RelMemberBooking where Member_Id=@Id)", con);
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Booking INNER JOIN RelMemberBooking ON Booking.Booking_Id = RelMemberBooking.Booking_Id where Member_Id=@Id", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Id", memberID);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    Thread.Sleep(1000);
                    while (await reader.ReadAsync())
                    {
                        int BookingNr = reader.GetInt32("Booking_ID");
                        DateTime Start = reader.GetDateTime("Start");
                        DateTime End = reader.GetDateTime("End");
                        int CourtId = reader.GetInt32("Court_Id");

                        int? TeamId = null;
                        int? EventId = null;

                        Booking ev = new Booking(BookingNr, Start, End, CourtId, TeamId, EventId);
                        BookingList.Add(ev);
                    }
                    reader.Close();

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
            if (BookingList.Exists(i => start == i.Start)) return false;
            return true;

        }

        public async Task<bool> CourtAvailible(int CourtID, DateTime start, DateTime end)
        {
            List<Booking> BookingList = new List<Booking>();
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {

                try
                {
                    SqlCommand cmd = new SqlCommand("select * from Booking where Court_Id=@ID ", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Id", CourtID);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    Thread.Sleep(1000);
                    while (await reader.ReadAsync())
                    {
                        int BookingNr = reader.GetInt32("Booking_ID");
                        DateTime Start = reader.GetDateTime("Start");
                        DateTime End = reader.GetDateTime("End");
                        int CourtId = reader.GetInt32("Court_Id");
                        int? TeamId = null;
                        int? EventId = null;

                        Booking ev = new Booking(BookingNr, Start, End, CourtId, TeamId, EventId);
                        BookingList.Add(ev);
                    }
                    reader.Close();

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
            if (BookingList.Exists(i => start > i.Start && end <= start || start < end)) return false;
            return true;

        }

        public async Task TeamMemberRelation(int TeamId, int MemberId)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO RelMemberTeam VALUES (@Member_Id,@Team_Id);", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Member_Id", MemberId);
                    cmd.Parameters.AddWithValue("@Team_Id", TeamId);
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

        public async Task RemoveTeamMemberRelation(int TeamId, int MemberId)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM RelMemberTeam WHERE Member_Id = @Member_Id AND Team_Id = @Team_Id;", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Member_Id", MemberId);
                    cmd.Parameters.AddWithValue("@Team_Id", TeamId);
                    await cmd.ExecuteNonQueryAsync();
                    con.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Database error " + e.Message);
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine("General error " + e.Message);
                    throw;
                }
            }
        }

        public async Task<List<Booking>> GetBookingsByUser(int memberID)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                List<Booking> BookingList = new List<Booking>();
                try
                {

               
                   
                    SqlCommand cmd = new SqlCommand("SELECT * FROM RelMemberBooking WHERE Member_Id = @Member_Id", con);
                    await con.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int bookId = reader.GetInt32("Booking_Id");
                        int memId = reader.GetInt32("Member_Id");
                        BookingList.Add(new Booking(bookId));
                    }
                    reader.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Database error " + e.Message);
                    throw e;
 
                }
                catch (Exception e)
                {
                    Console.WriteLine("general error " + e.Message);
                    throw e;
         
                }
                return BookingList;
            }
        }
    }
}