using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GadevangTennisklub2025.Services
{
    public class BlogPostService : IBlogPostServicesAsync
    {
        public async Task CreateBlogPost(BlogPost bp)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO BlogPost VALUES ( @Title, @Content, @Member_Id);", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Title", bp.Title);
                    cmd.Parameters.AddWithValue("@Content", bp.Content);
                    cmd.Parameters.AddWithValue("@Member_Id", bp.MemberId);
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

        public async Task DeleteBlogPost(BlogPost bp)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM BlogPost WHERE BlogId=@ID", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@ID", bp.Id);
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

        public async Task<List<BlogPost>> GetAllBlogPost()
        {
            List<BlogPost> BlogList = new List<BlogPost>();
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {

                try
                {
                    SqlCommand com = new SqlCommand("Select * from BlogPost", con);
                    await com.Connection.OpenAsync();
                    SqlDataReader reader = await com.ExecuteReaderAsync();
                    Thread.Sleep(1000);
                    while (await reader.ReadAsync())
                    {
                        int BlogId = reader.GetInt32("BlogId");
                        string Title= reader.GetString("Title");
                        string Content =reader.GetString("Content");
                        int MemberId = reader.GetInt32("Member_Id");
                        
                        BlogPost ev = new BlogPost(BlogId,Title,Content,MemberId);
                        BlogList.Add(ev);
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
            return BlogList;
        }

        public async Task<BlogPost> GetBlogPost(int id)
        {
            List<BlogPost> t = await GetAllBlogPost();
            return t.Find(i=> i.Id==id);
        }

        public async Task UpdateBlogPost(BlogPost bp)
        {
            using (SqlConnection con = new SqlConnection(Secret.ConnectionString))
            {

                try
                {
                    SqlCommand com = new SqlCommand("UPDATE BlogPost set Title= @Title,Content=@Content,Member_Id=@MemberId WHERE BlogId=@ID;", con);
                    await com.Connection.OpenAsync();
                    com.Parameters.AddWithValue("@ID", bp.Id);
                    com.Parameters.AddWithValue("@Title", bp.Title);
                    com.Parameters.AddWithValue("@Content", bp.Content);
                    com.Parameters.AddWithValue("@MemberId", bp.MemberId);
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
