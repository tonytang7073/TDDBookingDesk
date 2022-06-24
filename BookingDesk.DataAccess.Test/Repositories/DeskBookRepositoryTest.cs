using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingDesk.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace BookingDesk.DataAccess.Repositories
{
    public class DeskBookRepositoryTest
    {
        private readonly ITestOutputHelper _output;

        public DeskBookRepositoryTest(ITestOutputHelper testOutputHelper)
        {
            _output = testOutputHelper;
        }

        [Fact]
        public void ShouldSaveTheDeskBooking()
        {
            //var options = new DbContextOptionsBuilder<BookingDeskDbContext>()
            //    .UseInMemoryDatabase(databaseName: "ShouldSaveTheDeskBooking")
            //    .Options;

            var deskBooking = new DeskBook
            {
                Firstname = "Tony",
                Lastname = "Tang",
                Date = new DateTime(2023, 1, 25),
                Email = "tt@tt.com",
                DeskId = new Guid("C0843157-B797-4C8D-B063-B55E53822F02"), //sqlite will ensure a foreign key 
                Id = Guid.NewGuid(),
            };

            var connectionstringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionstringBuilder.ToString());
            var options = new DbContextOptionsBuilder<BookingDeskDbContext>()
                .UseLoggerFactory(new LoggerFactory(
                        new[] { new EFCoreLoggerProvider((log) =>
                        {
                            _output.WriteLine(log);
                        }) }))
                       .UseSqlite(connection)
                       .Options;


            using (var ctx = new BookingDeskDbContext(options))
            {
               ctx.Database.OpenConnection();
               ctx.Database.EnsureCreated(); //ensure the initial seed data created
            }

            using (var ctx = new BookingDeskDbContext(options))
            {
                var repo = new DeskBookRepository(ctx);
                repo.Save(deskBooking);
            }

            //assert
            using (var ctx = new BookingDeskDbContext(options))
            {
                var bookings = ctx.DeskBook.ToList();
                var desks = ctx.Desk.ToList();
                var storedDeskBook = bookings.Single();
                Assert.NotNull(storedDeskBook);
                Assert.Equal(deskBooking.Firstname, storedDeskBook.Firstname);
                Assert.Equal(deskBooking.Lastname, storedDeskBook.Lastname);
                Assert.Equal(deskBooking.Date, storedDeskBook.Date);
                Assert.Equal(deskBooking.Email, storedDeskBook.Email);
                Assert.Equal(deskBooking.DeskId, storedDeskBook.DeskId);
                Assert.Equal(deskBooking.Id, storedDeskBook.Id);
            }


        }
    }
}
