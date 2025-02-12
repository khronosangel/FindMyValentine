using Microsoft.EntityFrameworkCore;

namespace FindMyValentine.Models
{
    public partial class MainDBContext : DbContext
    {
        public MainDBContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray();

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("Con1"));
        }
    }
}
