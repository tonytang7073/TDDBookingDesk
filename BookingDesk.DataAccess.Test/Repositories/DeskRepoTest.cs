﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingDesk.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookingDesk.DataAccess.Repositories
{
    public class DeskRepoTest
    {
        [Fact]
        public void ShouldReturnAll()
        {
            var options = new DbContextOptionsBuilder<BookingDeskDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldReturnAll")
                .Options;




            using (var ctx = new BookingDeskDbContext(options))
            {
                ctx.Desk.AddRange(
               new Desk { Id = new Guid("C0843157-B797-4C8D-B063-B55E53822F02"), Description = "Desk #1" },
               new Desk { Id = new Guid("3B1CCD31-C286-45FD-AD73-5B2229916606"), Description = "Desk #2" },
               new Desk { Id = new Guid("7BC3C939-FE90-47FB-A410-DAA8B3A95A8B"), Description = "Desk #11" }
               );
                ctx.SaveChanges();

                //act
                var repo = new DeskRepository(ctx);
                var alldesks = repo.GetAllDesks().ToList();

                //assert
                Assert.Equal(3, alldesks.Count);
            }

        }

        [Fact]
        public void ShouldReturnTheAvailabeDesk()
        {
            //arrange
            var date = new DateTime(2023, 1, 25);

            var options = new DbContextOptionsBuilder<BookingDeskDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldReturnTheAvailabeDesk")
                .Options;

            using (var ctx = new BookingDeskDbContext(options))
            {
                ctx.Desk.AddRange(
              new Desk { Id = new Guid("C0843157-B797-4C8D-B063-B55E53822F02"), Description = "Desk #1" },
              new Desk { Id = new Guid("3B1CCD31-C286-45FD-AD73-5B2229916606"), Description = "Desk #2" },
              new Desk { Id = new Guid("7BC3C939-FE90-47FB-A410-DAA8B3A95A8B"), Description = "Desk #11" }
              );

                ctx.DeskBook.Add(new Domain.DeskBook { Date = date, DeskId = new Guid("C0843157-B797-4C8D-B063-B55E53822F02"), Id = Guid.NewGuid(), Email="tt.tt.com", Firstname="tt", Lastname="ja" });
                ctx.DeskBook.Add(new Domain.DeskBook { Date = date.AddDays(1), DeskId = new Guid("C0843157-B797-4C8D-B063-B55E53822F02"), Id = Guid.NewGuid(), Email = "tt.tt.com", Firstname = "tt", Lastname = "ja" });
                ctx.SaveChanges();
            }


            //act
            using (var ctx = new BookingDeskDbContext(options))
            {
                var repo = new DeskRepository(ctx);
                var availableDesks = repo.GetDesksByDate(date).ToList();

                //assert
                Assert.Equal(2, availableDesks.Count);

                Assert.Contains(availableDesks, d => d.Id == new Guid("3B1CCD31-C286-45FD-AD73-5B2229916606"));
                Assert.Contains(availableDesks, d => d.Id == new Guid("7BC3C939-FE90-47FB-A410-DAA8B3A95A8B"));
                Assert.DoesNotContain(availableDesks, d => d.Id == new Guid("C0843157-B797-4C8D-B063-B55E53822F02"));


            }




        }
    }
}
