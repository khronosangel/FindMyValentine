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
            //optionsBuilder.UseSqlServer("Data Source=SQL1002.site4now.net;Initial Catalog=db_ab2aa2_fmv;User ID=db_ab2aa2_fmv_admin;Password=pass@123;");
        }
    }
}
