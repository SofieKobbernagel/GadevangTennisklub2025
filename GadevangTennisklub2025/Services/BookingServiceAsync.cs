using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.Data.SqlClient;

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

        public Task DeleteBooking(Booking b)
        {
            throw new NotImplementedException();
        }

        public Task<List<Booking>> GetAllBookings()
        {
            throw new NotImplementedException();
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
