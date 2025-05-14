using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
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

        public async Task<List<Booking?>> GetBookingsByMemberId(int memberID)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                List<Booking?> BookingList = new List<Booking?>();
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT b.* FROM Booking b LEFT JOIN RelMemberBooking r ON b.Booking_Id = r.Booking_Id WHERE r.Member_Id = @Member_Id;", con);
                    cmd.Parameters.AddWithValue("@Member_Id", memberID);
                    await con.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int bookId = reader.GetInt32("Booking_Id");
                        DateTime start = reader.GetDateTime("Start");
                        DateTime end = reader.GetDateTime("End");
                        int courtId = reader.GetInt32("Court_Id");
                        int? teamId = null;
                        int? eventId = null;

                        BookingList.Add(new Booking(bookId, start, end, courtId, teamId, eventId));
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

        public async Task<TennisField> GetTennisFieldById(int court_Id)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {


                TennisField foundCourt = new TennisField();
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Court WHERE Court_Id = @Court_Id;", con);
                    cmd.Parameters.AddWithValue("@Court_Id", court_Id);
                    await con.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int Id = reader.GetInt32("Court_Id");
                        string name = reader.GetString("Name");
                        string type = reader.GetString("Type");
                        foundCourt.CourtId = Id;
                        foundCourt.Name = name;
                        foundCourt.Type = type;

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
                return foundCourt;
            }
        }

        public async Task<string> GetBookingPartnerName(int memberId, int bookingId)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                string partnerName = null;

                try
                {
                    string query = @" SELECT m.Name FROM RelMemberBooking r JOIN Members m ON r.Member_Id = m.Member_Id WHERE r.Booking_Id = @Booking_Id AND r.Member_Id != @Member_Id;";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Booking_Id", bookingId);
                    cmd.Parameters.AddWithValue("@Member_Id", memberId);

                    await con.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        partnerName = reader.GetString(0);
                    }

                    reader.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    throw;
                }

                return partnerName;
            }
        }

        public async Task<List<Member>> GetEventParticipants(int EventId) 
        {
            List<Member> members = new List<Member>();
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM RelMemberEvent where Event_Id=@EventId", con);
                    cmd.Parameters.AddWithValue("@EventID", EventId);
                    await con.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int MemberId = reader.GetInt32("Member_Id");
                        members.Add(new Member());
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
            }

            return members;
        }

    }
}

