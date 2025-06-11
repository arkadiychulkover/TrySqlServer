using NUnit.Framework;
using System.Linq;
using TrySqlServer;
using TrySqlServer.Entity;
namespace TestProject1
{
    public class Tests
    {
        [Test]
        public void AddUserAndMovie()
        {
            // Очистити базу
            using (AppDBContext context = new AppDBContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // Додати користувача
                User user = new User
                {
                    Username = "testuser",
                    Email = "test@test.com",
                    Password = "password123"
                };
                context.Users.Add(user);

                // Додати фільм
                Movie movie = new Movie
                {
                    Title = "Test Movie",
                    Description = "This is a test movie.",
                    ReleaseYear = 2023,
                    Users = user
                };
                context.Movie.Add(movie);
                context.SaveChanges();
            }

            // Перевірити дані
            using (AppDBContext context = new AppDBContext())
            {
                string username = context.Users.FirstOrDefault().Username;
                string title = context.Movie.FirstOrDefault().Title;

                Assert.That(username, Is.EqualTo("testuser"));
                Assert.That(title, Is.EqualTo("Test Movie"));
            }
        }
    }
}
