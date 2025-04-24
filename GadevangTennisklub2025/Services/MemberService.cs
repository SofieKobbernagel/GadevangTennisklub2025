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
                        string postalcode = reader.GetString("PostalCode");
                        string gender = reader.GetString("Gender");
                        string address = reader.GetString("Address");
                        int id = reader.GetInt32("Member_id");
                        string email = reader.GetString("Email");
                        string password = reader.GetString("Password");
                        string username = reader.GetString("Username");
                        bool isAdmin = reader.GetBoolean("IsAdmin");
                        bool newsSubscriber = reader.GetBoolean("NewsSubscriber");
                        Member m = new Member(username, name, birthday, membertype, city, phone, postalcode, gender, address, email, password);
                        m.IsAdmin = isAdmin;
                        m.NewsSubscriber = newsSubscriber;

                        members.Add(m);
                       
                    }
                    reader.Close();
                }
                catch (SqlException sqlExp)
                {
                    Console.WriteLine("Database error" + sqlExp.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl: " + ex.Message);
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
