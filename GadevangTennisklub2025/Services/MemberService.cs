using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Numerics;
using System.Reflection;

namespace GadevangTennisklub2025.Services
{
 

    public class MemberService : IMemberService
    {
        private string connectionString = Secret.ConnectionString;
        private string selectAllMembersSql = "select * From Members";
        private string insertSql = "Insert INTO Members (Member_Id, Name, Address, Gender, Email, PostalCode, TLF, City, " +
            "MembershipType, Birthday, OtherTLF, NewsSubscriber, Username, Password, IsAdmin, Municipality, PictureConsent)" +
            "VALUES (@Member_Id, @Name, @Address, @Gender, @Email, @Postalcode, @TLF, @City, @MembershipType, " +
            "@Birthday, @OtherTLF, @NewsSubscriber, @Username, @Password, @IsAdmin, @Municipality, @PictureConsent)";

        public async Task<bool> CreateMemberAsync(Member member)
        {
            bool isCreated = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(insertSql, connection);

                    command.Parameters.AddWithValue("@Member_Id", member.Member_Id);
                    command.Parameters.AddWithValue("@Name", member.Name);
                    command.Parameters.AddWithValue("@Address", member.Address);
                    command.Parameters.AddWithValue("@Gender", member.Gender);
                    command.Parameters.AddWithValue("@Email", member.Email);
                    command.Parameters.AddWithValue("@PostalCode", member.PostalCode);
                    command.Parameters.AddWithValue("@TLF", member.Phone);
                    command.Parameters.AddWithValue("@City", member.City);
                    command.Parameters.AddWithValue("@MembershipType", member.MemberType);
                    command.Parameters.AddWithValue("@Birthday", member.Birthday);
                    command.Parameters.AddWithValue("@OtherTLF", string.IsNullOrEmpty(member.OtherPhone) ? DBNull.Value : member.OtherPhone);
                    command.Parameters.AddWithValue("@NewsSubscriber", member.NewsSubscriber);
                    command.Parameters.AddWithValue("@Username", member.Username);
                    command.Parameters.AddWithValue("@Password", member.Password);
                    command.Parameters.AddWithValue("@IsAdmin", member.IsAdmin);
                    command.Parameters.AddWithValue("@Municipality", member.Municipality);
                    command.Parameters.AddWithValue("@PictureConsent", member.PictureConsent);
                    
                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        isCreated = true;
                    }
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error: " + sqlExp.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General error: " + ex.Message);
                    return false;
                }
            }
            return isCreated;
        }
        
        

        public async Task<List<Member>> GetAllMembersAsync()
        {
            List<Member> members = new List<Member>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectAllMembersSql, connection);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        string name = reader.GetString("Name");
                        DateOnly birthday = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("Birthday")));
                        string membertype = reader.GetString("MembershipType");
                        string city = reader.GetString("City");
                        string phone = reader.GetString("TLF");
                        string otherphone = reader.GetString("OtherTLF");
                        string postalcode = reader.GetString("PostalCode");
                        string gender = reader.GetString("Gender");
                        string address = reader.GetString("Address");
                        int id = reader.GetInt32("Member_id");
                        string email = reader.GetString("Email");
                        string password = reader.GetString("Password");
                        string username = reader.GetString("Username");
                        bool isAdmin = reader.GetBoolean("IsAdmin");
                        bool newsSubscriber = reader.GetBoolean("NewsSubscriber");
                        string municipality = reader.GetString("Municipality");
                        string consent = reader.GetString("PictureConsent");
                        Member m = new Member(username, name, birthday, membertype, city, phone, postalcode, gender, address, email, password, municipality, consent);
                        m.IsAdmin = isAdmin;
                        m.NewsSubscriber = newsSubscriber;
                        m.OtherPhone = otherphone;

                        members.Add(m);
                       
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
            return members;
        }

        public Member VerifyMember(string username, string password)
        {
            foreach (var member in GetAllMembersAsync().Result)
            {
                if (username.Equals(member.Username) && password.Equals(member.Password))
                {
                    return member;
                }
            }
            return null;
        }
    }
}
