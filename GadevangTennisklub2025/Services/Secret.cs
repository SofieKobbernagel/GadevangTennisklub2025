namespace GadevangTennisklub2025.Services
{
    public class Secret
    {
       private static string connectionString = @"Data Source=mssql6.unoeuro.com ;Initial Catalog=sneakyturtle_dk_db_sofie_database;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
       public static string ConnectionString
       {
           get { return connectionString; }
       }
        }
    }
