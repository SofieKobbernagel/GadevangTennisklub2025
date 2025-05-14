using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GadevangTennisklub2025.Services
{
    public class EventServicesAsync : IEventServiceAsync
    {
        public async Task CreateEventAsync(Event ev)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Event VALUES (@Title, @Date,@DESCRIPTION,@Max);", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@ID", ev.Id);
                    cmd.Parameters.AddWithValue("@Title", ev.Title);
                    cmd.Parameters.AddWithValue("@Date", ev.Date);
                    cmd.Parameters.AddWithValue("@DESCRIPTION", ev.Description);
                    cmd.Parameters.AddWithValue("@Max", ev.Maximum);
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

       

        public async Task<List<Event>> GetEventsAsync()
        {
            List<Event> eventList = new List<Event>();
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {

                try
                {
                    SqlCommand com = new SqlCommand("Select * from Event", con);
                    await com.Connection.OpenAsync();
                    SqlDataReader reader = await com.ExecuteReaderAsync();
                    Thread.Sleep(1000);
                    while (await reader.ReadAsync())
                    {
                        int eventNr = reader.GetInt32("EVENT_ID");
                        string Title = reader.GetString("Title");
                        DateTime EventDate = reader.GetDateTime("Date");
                        string desc= reader.GetString("DESCRIPTION");
                        int MaxMember = reader.GetInt32("Maxmimum");
                        Event ev = new Event(eventNr, Title, EventDate,desc, MaxMember);
                        eventList.Add(ev);
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
            return eventList;
        }
    
        

        public async Task<Event> returnEventAsync(int eid)
        {
            List<Event> t=await GetEventsAsync();
            return t.Find(i=>i.Id==eid);
        }  

        public async Task DeleteEventAsync(Event ev)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Event WHERE EVENT_Id=@ID", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@ID", ev.Id);
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

       

        public async Task UpdateEventAsync(Event ev)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {

                try
                {
                    SqlCommand com = new SqlCommand("UPDATE Event set Title= @Title,Date=@Date,DESCRIPTION=@Desc, Maxmimum=@Max WHERE EVENT_Id=@ID;", con);
                    await com.Connection.OpenAsync();
                    com.Parameters.AddWithValue("@ID", ev.Id);
                    com.Parameters.AddWithValue("@Title",ev.Title);
                    com.Parameters.AddWithValue("@Date", ev.Date);
                    com.Parameters.AddWithValue("@Desc", ev.Description);
                    com.Parameters.AddWithValue("@Max", ev.Maximum);
                    await com.ExecuteNonQueryAsync();

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
        }

        public async Task<List<Event>> GetEventspermonthInYearAsync (int month, int year)
        {
            
            List<Event> eventsFM= new List<Event>();
            return eventsFM.FindAll(e => e.Date.Month == month && e.Date.Year==year);
        }
    }
}
