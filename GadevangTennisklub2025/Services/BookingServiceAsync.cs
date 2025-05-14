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

        

        public async Task DeleteBooking(Booking b)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Booking WHERE Booking_Id=@ID", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@ID", b.Id);
                    await cmd.ExecuteNonQueryAsync();
                    con.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Database error " + e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("general error " + e.Message);
                }

            }
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
                    Console.WriteLine("Database fail " + e.Message);
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

        public async Task<List<Booking>> GetBookingsByUser(int uid)
        {
            List<Booking> BookingList = new List<Booking>();
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {

                try
                {
                    SqlCommand com = new SqlCommand("SELECT * FROM Booking INNER JOIN RelMemberBooking ON Booking.Booking_Id = RelMemberBooking.Booking_Id where Member_Id=@Id", con);
                    await com.Connection.OpenAsync();
                    com.Parameters.AddWithValue("@ID", uid);
                    SqlDataReader reader = await com.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int BookingId = reader.GetInt32("Booking_ID");
                        DateTime start = reader.GetDateTime("Start");
                        DateTime end = reader.GetDateTime("End");
                        int CourtId = reader.GetInt32("Court_Id");
                        int? TeamId = null;
                        int? EventId = null;
                        try
                        {
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

                        Booking ev = new Booking(BookingId, start, end, CourtId, TeamId, EventId);
                        BookingList.Add(ev);
                    }
                    reader.Close();

                }
                catch (SqlException e)
                {
                    Console.WriteLine("Database fail " + e.Message);
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

        public Task UpdateBooking(Booking b)
        {
            throw new NotImplementedException();
        }

        

        public async Task<List<DateTime>> TeamCreation(Team T)
        {
            List<DateTime> UnAvalibleDates = new List<DateTime>();
            List<Booking> BookingList = await GetAllBookings();
            
            foreach (Booking b in BookingList) 
            {
                if (b.Court_Id==1) 
                {
                    if ((int)b.Start.DayOfWeek == (T.DayOfWeek + 1 == 7 ? 0 : T.DayOfWeek + 1) && (T.TimeOfDay.Hour == b.Start.Hour || ((T.TimeOfDay.Hour > b.Start.Hour && T.TimeOfDay.Hour < b.End.Hour) || (T.TimeOfDay.Hour + T.Length > b.Start.Hour && T.TimeOfDay.Hour + T.Length <= b.End.Hour)))) UnAvalibleDates.Add(b.Start);
                }
            }

            if (UnAvalibleDates.Count == 0) 
            {
                DateTime temp = new DateTime(new DateOnly(DateTime.Now.Year,4,30), T.TimeOfDay);
                int Days = (T.DayOfWeek!=6? T.DayOfWeek+1:0)-(int)temp.DayOfWeek;
                temp=temp.AddDays(Days<0? 7+Days:Days);
                for (DateTime t = temp; t < new DateTime(2025, 10, 6, T.TimeOfDay.Hour, 0, 1); t=t.AddDays(7)) 
                {
                    for (int i = 0; i < T.Length; i++) 
                    {
                        await CreateTeamBooking(new Booking(0, t.AddHours(i), t.AddHours(1+i), 1,T.Id,null),T.Id);
                    }
                   
                }
            }
            return UnAvalibleDates;
        }
    }
}
