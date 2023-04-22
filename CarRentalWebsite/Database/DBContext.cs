using CarRentalWebsite.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalWebsite.Database
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        
        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<RegistrationQR> Registrations { get; set; }

        public DbSet<Complain> Complains { get; set; }
    }
}
