using GadevangTennisklub2025.Models;
using Microsoft.EntityFrameworkCore;

namespace GadevangTennisklub2025.Data
{
    public class AppDataContext : DbContext
    {
        private string connectionString = Secret.ConnectionString;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {       
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<Member> Members { get; set; }
    }
}
