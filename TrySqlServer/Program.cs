using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TrySqlServer.Entity;

namespace TrySqlServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            UserController userController = new UserController();
            AppDBContext context = new AppDBContext();
            context.Database.EnsureCreated();
            ShowStartMenu();
            int casee = int.Parse(Console.ReadLine());

            while (casee != 4)
            {
                switch (casee)
                {
                    case 1:
                        userController.Login(context);
                        if (UserController.CurrentUser != null)
                        {
                            Console.WriteLine($"Welcome {UserController.CurrentUser.Username}");
                            userController.ShowMenu();
                            int choice = int.Parse(Console.ReadLine());
                            while (choice != 7)
                            {
                                switch (choice)
                                {
                                    case 1:
                                        userController.AddMovie(context);
                                        break;
                                    case 2:
                                        ShowMovies(context);
                                        break;
                                    case 3:
                                        userController.ShowUserMovies(context);
                                        break;
                                    case 4:
                                        userController.DeleteMovie(context);
                                        break;
                                    case 5:
                                        userController.RewriteProfile(context);
                                        break;
                                    case 6:
                                        userController.View(context);
                                        break;
                                    default:
                                        Console.WriteLine("Invalid choice, try again.");
                                        break;
                                }
                                userController.ShowMenu();
                                choice = int.Parse(Console.ReadLine());
                            }
                            userController.LoggOut();
                        }
                        else
                        {
                            Console.WriteLine("Invalid username or password.");
                        }
                        break;
                    case 2:
                        userController.Register(context);
                        break;
                    case 3:
                        userController.AddUserBySql(context);
                        break;
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }
                ShowStartMenu();
                casee = int.Parse(Console.ReadLine());
            }
        }

        public static void ShowStartMenu()
        {
            Console.WriteLine("1. Loggin");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Register BY SQL");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");
        }
        public static void ShowMovies(AppDBContext context)
        {
            var movies = context.Movie.ToList();
            if (movies.Count == 0)
            {
                Console.WriteLine("No movies found.");
            }
            else
            {
                foreach (var movie in movies)
                {
                    Console.WriteLine(movie);
                }
            }
        }
        public static void ShowUsers(AppDBContext context)
        {
            var users = context.Users.ToList();
            if (users.Count == 0)
            {
                Console.WriteLine("No users found.");
            }
            else
            {
                foreach (var user in users)
                {
                    Console.WriteLine(user);
                }
            }
        }
        public static void Exit()
        {
            Console.WriteLine("Exiting");
            Environment.Exit(0);
        }
    }

    public class UserController
    {
        public static User? CurrentUser { get; set; }

        public void AddMovie(AppDBContext context)
        {
            Movie movie = new Movie();
            Console.Write("Enter movie title: ");
            movie.Title = Console.ReadLine();

            Console.Write("Enter movie description: ");
            movie.Description = Console.ReadLine();

            Console.Write("Enter release year: ");
            movie.ReleaseYear = int.Parse(Console.ReadLine());

            movie.Users = CurrentUser;

            context.Movie.Add(movie);
            context.SaveChanges();
        }
        public void Login(AppDBContext context)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            if (context.Users.Any(u => u.Username == username))
            {
                Console.WriteLine("Enter Password");
                string password = Console.ReadLine();
                CurrentUser = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            }
        }
        public void LoggOut()
        {
            CurrentUser = null;
            Console.WriteLine("Logged out successfully.");
        }
        public void Register(AppDBContext context)
        {
            User user = new User();
            Console.Write("Enter username: ");
            user.Username = Console.ReadLine();
            Console.Write("Enter email: ");
            user.Email = Console.ReadLine();
            Console.Write("Enter password: ");
            user.Password = Console.ReadLine();
            context.Users.Add(user);
            context.SaveChanges();
            CurrentUser = user;
        }
        public void ShowUserMovies(AppDBContext context)
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("No user logged in.");
                return;
            }
            var userMovies = context.Movie.Where(m => m.Users.Id == CurrentUser.Id).ToList();
            if (userMovies.Count == 0)
            {
                Console.WriteLine("No movies found for the current user.");
            }
            else
            {
                foreach (var movie in userMovies)
                {
                    Console.WriteLine(movie);
                }
            }
        }
        public void DeleteMovie(AppDBContext context)
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("No user logged in");
                return;
            }
            Console.Write("Enter movie ID to delete: ");
            int movieId = int.Parse(Console.ReadLine());
            var movie = context.Movie.Find(movieId);
            if (movie != null && movie.Users.Id == CurrentUser.Id)
            {
                context.Movie.Remove(movie);
                context.SaveChanges();
                Console.WriteLine("Movie deleted successfully");
            }
            else
            {
                Console.WriteLine("Movie does not belong user");
            }
        }
        public void RewriteProfile(AppDBContext context)
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("No user logged in");
                return;
            }
            Console.Write("Enter new username: ");
            CurrentUser.Username = Console.ReadLine();
            Console.Write("Enter new email: ");
            CurrentUser.Email = Console.ReadLine();
            Console.Write("Enter new password: ");
            CurrentUser.Password = Console.ReadLine();
            context.SaveChanges();
            Console.WriteLine("Profile updated successfully");
        }
        public void View(AppDBContext context)
        {
            foreach (var user in context.Viever)
                Console.WriteLine($"{user.Username} --- {user.Title}");
        }
        public void AddUserBySql(AppDBContext context)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter email: ");
            string email = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            string sql = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", username),
                new SqlParameter("@Email", email),
                new SqlParameter("@Password", password)
            };
            context.Database.ExecuteSqlRaw(sql, parameters);// как на паре не получилось писало что параметры не те
            Console.WriteLine("User added successfully");
        }
        public void ShowMenu()
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Add Movie");
            Console.WriteLine("2. Show All Movies");
            Console.WriteLine("3. Show User Movies");
            Console.WriteLine("4. Delete Movie");
            Console.WriteLine("5. Rewrite profile");
            Console.WriteLine("6. Show All Movies BY VIEWER");
            Console.WriteLine("7. LoggOut");
            Console.Write("Choose an option: ");
        }
    }
}
