using Microsoft.EntityFrameworkCore;
using MovieBest.MVC.Core.Services.Implementations;
using MovieBest.BLL.Services.Interfaces;
using MovieBest.BLL.Services.Implementations;
using MovieBest.BLL.UnitOfWork;
using MovieBest.BLL.SharedRepository;
using MovieBest.DAL.Entities;
using MovieBest.DAL.DbContext;
using MovieBest.DAL.SeedData;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Features;

namespace MovieBest.MVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<MovieBestContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnString"));
            });

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedEmail = true)
                .AddEntityFrameworkStores<MovieBestContext>()
                .AddDefaultTokenProviders();

            builder.WebHost.UseKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 524288000;
                options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
                options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 524288000;
            });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, unitOfWork>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();


            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<IFileService, FileService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    await AppSeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred seeding the database: {ex.Message}");
                }
            }
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");



            await app.RunAsync();
        }
    }
}
