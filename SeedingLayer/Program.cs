using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeedingLayer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                IdentityOptions options = new IdentityOptions();
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 5;

                DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
                builder.UseSqlServer("Server=DESKTOP-IN16GJU;Database=CinemaDb;Trusted_Connection=True;");

                CinemaDbContext dbContext = new CinemaDbContext(builder.Options);
                UserManager<User> userManager = new UserManager<User>(
                    new UserStore<User>(dbContext), Options.Create(options),
                    new PasswordHasher<User>(), new List<IUserValidator<User>>() { new UserValidator<User>() },
                    new List<IPasswordValidator<User>>() { new PasswordValidator<User>() }, new UpperInvariantLookupNormalizer(),
                    new IdentityErrorDescriber(), new ServiceCollection().BuildServiceProvider(),
                    new Logger<UserManager<User>>(new LoggerFactory())
                    );

                IdentityContext identityContext = new IdentityContext(dbContext, userManager);

                dbContext.Roles.Add(new IdentityRole("Administrator") { NormalizedName = "ADMINISTRATOR" });
                dbContext.Roles.Add(new IdentityRole("User") { NormalizedName = "USER" });
                await dbContext.SaveChangesAsync();

                IdentityResultSet<User> result = await identityContext.CreateUserAsync("admin", "admin", "admin@abv.bg", 
                    "Admin", "Adminov", 20, Role.Administrator);
                
                MovieContext movieContext = new MovieContext(dbContext);
                Movie movie = new Movie("The Matrix", 200, "When a beautiful stranger leads computer hacker Neo to a forbidding underworld, he discovers the shocking truth--the life he knows is the elaborate deception of an evil cyber-intelligence.", "The Wachowskis", 8, "MatrixPoster.jpeg");
                await movieContext.CreateAsync(movie);

                HallContext hallContext = new HallContext(dbContext);
                Hall hall = new Hall(1, 48);
                await hallContext.CreateAsync(hall);

                ShowtimeContext showtimeContext = new ShowtimeContext(dbContext);
                string dateString1 = "21-4-24";
                string timeString1 = "10:10:11";
                string combinedStartDateTimeString = $"{dateString1} {timeString1}";
                DateTime startDateTime = DateTime.ParseExact(combinedStartDateTimeString, "yy-M-d H:mm:ss", null);
                string dateString2 = "21-4-24";
                string timeString2 = "12:10:11";
                string combinedEndDateTimeString = $"{dateString2} {timeString2}";
                DateTime endDateTime = DateTime.ParseExact(combinedEndDateTimeString, "yy-M-d H:mm:ss", null);
                Showtime showtime = new Showtime(movie, startDateTime, endDateTime, hall);
                await showtimeContext.CreateAsync(showtime);

                Console.WriteLine("Roles added successfully!");

                if (result.IdentityResult.Succeeded)
                {
                    Console.WriteLine("Admin account added successfully!");
                }
                else
                {
                    Console.WriteLine("Admin account failed to be created!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

            }
        }
    }
}