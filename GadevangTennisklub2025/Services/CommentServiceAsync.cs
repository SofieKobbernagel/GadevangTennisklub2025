using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GadevangTennisklub2025.Services
{
    public class CommentServiceAsync : ICommentServiceAsync
    {
        public async Task CreateComment(Comment Co)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Comment VALUES ( @CommentContent, @MemberId, @BlogId);", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@CommentContent", Co.CommentContent);
                    cmd.Parameters.AddWithValue("@MemberId", Co.MemberId);
                    cmd.Parameters.AddWithValue("@BlogId", Co.BlogId);
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

        public async Task DeleteComment(Comment Co)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Comment WHERE CommentId=@ID", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@ID", Co.Id);
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

        public async Task<List<Comment>> GetAllComments()
        {
            List<Comment> CommentList = new List<Comment>();
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {

                try
                {
                    SqlCommand com = new SqlCommand("Select * from Comment", con);
                    await com.Connection.OpenAsync();
                    SqlDataReader reader = await com.ExecuteReaderAsync();
                    Thread.Sleep(1000);
                    while (await reader.ReadAsync())
                    {
                        int CommentId = reader.GetInt32("CommentId");
                        string Content = reader.GetString("CommentContent");
                        int MemberId = reader.GetInt32("MemberId");
                        int BlogId = reader.GetInt32("BlogId");

                        Comment ev = new Comment(CommentId, BlogId, MemberId, Content);
                        CommentList.Add(ev);
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
            return CommentList;
        }

        public async Task<Comment> GetComment(int id)
        {
            List<Comment> t =await GetAllComments();
            return t.Find(I=> I.Id==id);
        }

        public async Task<List<Comment>> GetCommentsForPost(int Blogid)
        {
            List<Comment> CommentList = new List<Comment>();
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {

                try
                {
                    SqlCommand com = new SqlCommand("Select * from Comment Where BlogId=@Blog", con);
                    await com.Connection.OpenAsync();
                    com.Parameters.AddWithValue("@Blog", Blogid);
                    SqlDataReader reader = await com.ExecuteReaderAsync();
                    Thread.Sleep(1000);
                    while (await reader.ReadAsync())
                    {
                        int CommentId = reader.GetInt32("CommentId");
                        string Content = reader.GetString("CommentContent");
                        int MemberId = reader.GetInt32("MemberId");
                        int BlogId = reader.GetInt32("BlogId");

                        Comment ev = new Comment(CommentId, BlogId, MemberId, Content);
                        CommentList.Add(ev);
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
            return CommentList;
        }

        public async Task UpdateComment(Comment Co)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {

                try
                {
                    SqlCommand com = new SqlCommand("UPDATE Comment set CommentContent= @Content WHERE CommentId=@ID;", con);
                    await com.Connection.OpenAsync();
                    com.Parameters.AddWithValue("@Content", Co.CommentContent);
                    com.Parameters.AddWithValue("@ID", Co.Id);
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

       
    }
}
