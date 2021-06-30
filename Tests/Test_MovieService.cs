using dotNetLab2.Data;
using dotNetLab2.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;

namespace Tests
{
    public class Test_MovieService

    {
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            Console.WriteLine("In setup.");
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Movie_lab2.2").Options;

            _context = new ApplicationDbContext(options, new OperationalStoreOptionsForTests());
            _context.Movies.Add(new dotNetLab2.Models.Movie { Title = "movie title 1", Description = "bla bla bla aaa dsdnakdnak", Rating = 8 });
            _context.Movies.Add(new dotNetLab2.Models.Movie { Title = "movie title 1", Description = "mdksldm ksjnd ksjdn sdn", Rating = 7 });
            _context.SaveChanges();
        }

        [TearDown]

        public void TearDown()
        {
            Console.WriteLine("In teardown");
        }
        public void TestGetMinRating()
        {
            var service = new MoviesService(_context);
            Assert.AreEqual(2, service.GetAllAboveRating(8).Count);
            Assert.AreEqual(2, service.GetAllAboveRating(2).Count);
            Assert.AreEqual(2, service.GetAllAboveRating(14).Count);
        }
    }

}