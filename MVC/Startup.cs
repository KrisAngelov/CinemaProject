﻿using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using ServiceLayer;

namespace MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped<MovieManager, MovieManager>();
            services.AddScoped<MovieContext, MovieContext>();

            services.AddScoped<IdentityManager, IdentityManager>();
            services.AddScoped<IdentityContext, IdentityContext>();

            services.AddScoped<HallManager, HallManager>();
            services.AddScoped<HallContext, HallContext>();

            services.AddScoped<ReviewManager, ReviewManager>();
            services.AddScoped<ReviewContext, ReviewContext>();

            services.AddScoped<SeatManager, SeatManager>();
            services.AddScoped<SeatContext, SeatContext>();

            services.AddScoped<ShowtimeManager, ShowtimeManager>();
            services.AddScoped<ShowtimeContext, ShowtimeContext>();

            services.AddScoped<TicketManager, TicketManager>();
            services.AddScoped<TicketContext, TicketContext>();

            services.AddScoped<CinemaDbContext, CinemaDbContext>();

            services.AddIdentity<User, IdentityRole>(iop =>
            {
                iop.Password.RequiredLength = 5;
                iop.Password.RequireNonAlphanumeric = false;
                iop.Password.RequiredUniqueChars = 0;
                iop.Password.RequireUppercase = false;
                iop.Password.RequireLowercase = false;
                iop.Password.RequireDigit = false;

                iop.User.RequireUniqueEmail = false;

                iop.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<CinemaDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
