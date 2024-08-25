using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using PetBooK.BL.UOW;
using PetBooK.DAL.Models;
using Microsoft.Extensions.Logging;
using PetBooK.BL.Config;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PetBooK.DAL.Services;
using Microsoft.Extensions.FileProviders;
using PetBooK.PL.Hubs;

namespace PetBooK.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Configure logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //--------------------- Register the DbContext ---------------------//
            builder.Services.AddDbContext<PetBookContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("pet")));

            //--------------------- Register UnitOfWork ---------------------//
            builder.Services.AddScoped<UnitOfWork>();


            //------------------------validate token------------------------//

            builder.Services.AddAuthentication(option => option.DefaultAuthenticateScheme = "myscheme")
                .AddJwtBearer("myscheme",
                //validate token
                op =>
                {
                    #region secret key
                    string key = "welcome to my secret key PetBook Alex";
                    var secertkey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                    #endregion
                    op.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = secertkey,
                        ValidateIssuer = false,
                        ValidateAudience = false

                    };
                }
                );

            //------------------------ Inject File Service ------------------------//
            builder.Services.AddTransient<IFileService, FileService>();

            //--------------------- Define CORS policy name ---------------------//
            string corsPolicyName = "AllowAll";

           
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicyName, builder =>
                {
                    builder.SetIsOriginAllowed(origin => true) // Allow any origin
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            /// For Auto Mapper:
            builder.Services.AddAutoMapper(typeof(AutoMapConfig).Assembly);

            ///////////////////// Register SignalR: /////////////////////
            builder.Services.AddSignalR();

            

            var app = builder.Build();

            app.UseRouting();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
                RequestPath = "/Resources"
            });


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();


            app.UseAuthorization();

            //--------------------- Use CORS policy ---------------------//
            app.UseCors(corsPolicyName);

            ///////////////////// To create an End point for each Hub class (server configuration): /////////////////////
            app.MapHub<PetHub>("/PetHub");

            app.MapControllers();

            app.Run();
        }
    }
}