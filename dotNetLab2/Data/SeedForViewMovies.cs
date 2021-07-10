using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetLab2.Data
{
    public class SeedForViewMovies
    {
        private static Random random = new Random();

        public static void Seed(ApplicationDbContext context, int count)
        {
            context.Database.EnsureCreated();
            var usersCount = context.ApplicationUsers.Count();
            var moviesCount = context.Movies.Count();

            for (int i = 0; i < count; ++i)
            {
                var user = context.ApplicationUsers.Skip(random.Next(1, usersCount)).Take(1).First();
                var movies = context.Movies.Skip(random.Next(1, moviesCount)).Take(3).ToList();

                var favourites = new Models.ForViewMovie
                {
                    WatchDateTime = getRandomDate(),
                    ApplicationUser = user,
                    Movies = movies,

                };


                context.ForViewMovies.Add(favourites);
            }

            context.SaveChanges();
        }

        private static int generateRandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

        private static bool generateRandomBoolean()
        {
            return random.Next(0, 2) > 0;
        }

        private static DateTime getRandomDate()
        {
            int rangePastThreeYears = 3 * 365;
            DateTime randomDate = DateTime.Today.AddDays(-random.Next(rangePastThreeYears));

            return randomDate;
        }

    }
}
