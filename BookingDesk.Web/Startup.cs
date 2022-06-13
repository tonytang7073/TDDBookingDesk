using BookingDesk.DataAccess;
using BookingDesk.DataAccess.Repositories;
using BookingDesk.Datainterface;
using BookingDesk.Processor;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BookingDesk.Web
{
    public class Startup
    {
        public Startup(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }
        public IConfigurationRoot Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            services.AddRazorPages();


            var connectionString = "DataSource=:memory:";
            var connection = new SqliteConnection(connectionString);
            connection.Open();

            services.AddDbContext<BookingDeskDbContext>(options =>
                options.UseSqlite(connection));

            EnsureDatabaseExists(connection);

            services.AddTransient<IDeskRepo, DeskRepository>();
            services.AddTransient<IDeskBookRepo, DeskBookRepository>();
            services.AddTransient<IDeskBookingRequestProcess, DeskBookingRequestProcess>();


        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(x => x.MapControllers());
        }


        private static void EnsureDatabaseExists(SqliteConnection connection)
        {
            var builder = new DbContextOptionsBuilder<BookingDeskDbContext>();
            builder.UseSqlite(connection);

            using (var context = new BookingDeskDbContext(builder.Options))
            {
                context.Database.EnsureCreated();
            }
        }

    }
}
