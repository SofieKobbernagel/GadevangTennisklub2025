﻿using GadevangTennisklub2025.Interfaces;
using GadevangTennisklub2025.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Numerics;
using System.Reflection;

namespace GadevangTennisklub2025.Services
{

    /// <summary>
    /// Indeholder funktionerne for members.
    /// </summary>
    public class MemberService : IMemberService
    {
        // Connection string til databasen
        private string connectionString = Secret.ConnectionString;

        // SQL-kommandoer til forskellige operationer
        private string selectAllMembersSql = "select * From Members";
        private string insertSql = "Insert INTO Members (Name, Address, Gender, Email, PostalCode, TLF, City, " +
            "MembershipType, Birthday, OtherTLF, NewsSubscriber, Username, Password, IsAdmin, Municipality, PictureConsent, ProfileImagePath)" +
            "VALUES (@Name, @Address, @Gender, @Email, @Postalcode, @TLF, @City, @MembershipType, " +
            "@Birthday, @OtherTLF, @NewsSubscriber, @Username, @Password, @IsAdmin, @Municipality, @PictureConsent, @ProfileImagePath)";
        private string getByIdSql = @"SELECT * FROM Members WHERE Member_Id = @Member_Id";
        private string deleteSql = "DELETE FROM Members WHERE Member_Id = @Member_Id;";
        private string updateSql = @"Update Members Set Name = @Name,Address = @Address, Gender = @Gender, Email = @Email, PostalCode = @Postalcode, TLF= @TLF, City = @City, MembershipType = @MembershipType, Birthday = @Birthday, OtherTLF = @OtherTLF, NewsSubscriber = @NewsSubscriber, Username = @Username,Password = @Password, IsAdmin = @IsAdmin,Municipality =  @Municipality, PictureConsent = @PictureConsent, ProfileImagePath = @ProfileImagePath WHERE Member_Id = @Member_Id";


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
                    command.Parameters.AddWithValue("@ProfileImagePath", string.IsNullOrEmpty(member.ProfileImagePath) ? DBNull.Value : member.ProfileImagePath);

                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    // Hvis mindst en række er påvirket, blev oprettelsen succesfuld
                    if (rowsAffected > 0)
                    {
                        isCreated = true;
                    }
                }
                catch (SqlException sqlEx)
                {
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    throw ex;
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
                        string otherphone = reader.IsDBNull(reader.GetOrdinal("OtherTLF")) ? null : reader.GetString(reader.GetOrdinal("OtherTLF"));
                        string postalcode = reader.GetString("PostalCode");
                        string gender = reader.GetString("Gender");
                        string address = reader.GetString("Address");
                        int id = reader.GetInt32("Member_Id");
                        string email = reader.GetString("Email");
                        string password = reader.GetString("Password");
                        string username = reader.GetString("Username");
                        bool isAdmin = reader.GetBoolean("IsAdmin");
                        bool newsSubscriber = reader.GetBoolean("NewsSubscriber");
                        string municipality = reader.GetString("Municipality");
                        string consent = reader.GetString("PictureConsent");
                        string filepath = reader.IsDBNull(reader.GetOrdinal("ProfileImagePath")) ? null : reader.GetString(reader.GetOrdinal("ProfileImagePath"));

                        Member m = new Member(username, name, birthday, membertype, city, phone, postalcode, gender, address, email, password, municipality, consent, id);
                        m.IsAdmin = isAdmin;
                        m.NewsSubscriber = newsSubscriber;
                        m.OtherPhone = otherphone;
                        m.ProfileImagePath = filepath;

                        members.Add(m);

                    }
                    reader.Close();
                }
                catch (SqlException sqlEx)
                {
                    throw new ApplicationException("Databasefejl opstod.", sqlEx);
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            return members;
        }

        public async Task<bool> UpdateMemberAsync(Member member, int member_Id)
        {
            bool isUpdated = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(updateSql, connection);
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
                    command.Parameters.AddWithValue("@ProfileImagePath", string.IsNullOrEmpty(member.ProfileImagePath) ? DBNull.Value : member.ProfileImagePath);

                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        isUpdated = true;
                    }
                }
                catch (SqlException sqlEx)
                {
                    throw new ApplicationException("Databasefejl opstod.", sqlEx);
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            return isUpdated;
        }

        public async Task<Member> VerifyMember(string username, string password)
        {
            // Hent alle medlemmer og sammenlign brugernavn og kodeord
            foreach (var member in GetAllMembersAsync().Result)
            {
                if (username.Equals(member.Username) && password.Equals(member.Password))
                {
                    return member;
                }
            }
            return null;
        }

        public async Task<Member?> GetMemberById(int member_id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Member? foundMember = null;
                try
                {

                    SqlCommand command = new SqlCommand(getByIdSql, connection);
                    command.Parameters.AddWithValue("@Member_Id", member_id);

                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        string name = reader.GetString("Name");
                        DateOnly birthday = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("Birthday")));
                        string membertype = reader.GetString("MembershipType");
                        string city = reader.GetString("City");
                        string phone = reader.GetString("TLF");
                        string otherphone = reader.IsDBNull(reader.GetOrdinal("OtherTLF")) ? null : reader.GetString(reader.GetOrdinal("OtherTLF"));
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
                        string filepath = reader.IsDBNull(reader.GetOrdinal("ProfileImagePath")) ? null : reader.GetString(reader.GetOrdinal("ProfileImagePath"));

                        foundMember = new Member(username, name, birthday, membertype, city, phone, postalcode, gender, address, email, password, municipality, consent, member_id);
                        foundMember.IsAdmin = isAdmin;
                        foundMember.NewsSubscriber = newsSubscriber;
                        foundMember.OtherPhone = otherphone;
                        foundMember.ProfileImagePath = filepath;



                    }
                    reader.Close();

                }
                catch (SqlException sqlEx)
                {
                    throw new ApplicationException("Databasefejl opstod.", sqlEx);
                }
                catch (Exception ex)
                {
                    throw;
                }


                return foundMember;
            }
        }

        public async Task<Member?> DeleteMemberAsync(int member_Id)
        {
            Member? deletedMember = await GetMemberById(member_Id);
            if (deletedMember == null)
            {
                return null;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(deleteSql, connection);
                    command.Parameters.AddWithValue("@Member_Id", member_Id);
                    await connection.OpenAsync();

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                        deletedMember = null;
                }

                catch (SqlException sqlEx)
                {
                    throw new ApplicationException("Databasefejl opstod.", sqlEx);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return deletedMember;
            }
        }

        public async Task<bool> IsUsernameUnique(string username)
        {
            bool isuniqe = true;
         
            List<Member> members = await GetAllMembersAsync();
            foreach (Member m in members)
            {
                if (m.Username == username)
                {
                    isuniqe = false;
                    break;
                }       
            }
            return isuniqe;
        }

        public async Task SubtrackHour(int id)
        {
            return;
        }
    }
}
