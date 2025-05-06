using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GadevangTennisklub2025.Services
{
    public class BookingServiceAsync : IBookingServiceAsync
    {
        public async Task CreateBooking(Booking b)
        {
          
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Booking VALUES ( @Start, @End, @CourtId, Null,NULL );", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@ID", b.Id);
                    cmd.Parameters.AddWithValue("@Start", b.Start);
                    cmd.Parameters.AddWithValue("@End", b.End);
                    cmd.Parameters.AddWithValue("@CourtId", b.Court_Id);
                    //cmd.Parameters.AddWithValue("@TeamId", 8);
                    //cmd.Parameters.AddWithValue("@EventId", null);
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
        public async Task CreateTeamBooking(Booking b, int TeamId)
        {

            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Booking VALUES ( @Start, @End, @CourtId, @TeamId ,NULL );", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@ID", b.Id);
                    cmd.Parameters.AddWithValue("@Start", b.Start);
                    cmd.Parameters.AddWithValue("@End", b.End);
                    cmd.Parameters.AddWithValue("@CourtId", b.Court_Id);
                    cmd.Parameters.AddWithValue("@TeamId", TeamId);
                    //cmd.Parameters.AddWithValue("@EventId", null);
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
        public async Task CreateEventBooking(Booking b, int EventId)
        {

            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Booking VALUES ( @Start, @End, @CourtId, Null,@EventId );", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@ID", b.Id);
                    cmd.Parameters.AddWithValue("@Start", b.Start);
                    cmd.Parameters.AddWithValue("@End", b.End);
                    cmd.Parameters.AddWithValue("@CourtId", b.Court_Id);
                    //cmd.Parameters.AddWithValue("@TeamId", 8);
                    cmd.Parameters.AddWithValue("@EventId", EventId);
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

        

        public Task DeleteBooking(Booking b)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Booking>> GetAllBookings()
        {
            List<Booking> BookingList = new List<Booking>();
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {

                try
                {
                    SqlCommand com = new SqlCommand("Select * from Booking", con);
                    await com.Connection.OpenAsync();
                    SqlDataReader reader = await com.ExecuteReaderAsync();
                    Thread.Sleep(1000);
                    while (await reader.ReadAsync())
                    {
                        int BookingId = reader.GetInt32("Booking_ID");
                        DateTime start= reader.GetDateTime("Start");
                        DateTime end= reader.GetDateTime("End");
                        int CourtId= reader.GetInt32("Court_Id");
                        int? TeamId = null;
                        int? EventId = null;
                        try {
                            TeamId = reader.GetInt32("Team_Id");
                            EventId = reader.GetInt32("Event_Id");
                        } 
                        catch 
                        (Exception e) 
                        {
                            try
                            {
                                EventId = reader.GetInt32("Event_Id");
                            }
                            catch
                            (Exception en)
                            { }
                        }
                        
                        Booking ev = new Booking(BookingId, start, end, CourtId,TeamId,EventId);
                        BookingList.Add(ev);
                    }
                    reader.Close();

                }
                catch (SqlException e)
                {
                    Console.WriteLine("Databse fail " + e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("General fail " + e.Message);
                }
                finally
                {

                }
            }
            return BookingList;
        }

        public Task GetBookingsByUser(int uid)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBooking(Booking b)
        {
            throw new NotImplementedException();
        }
    }
}
