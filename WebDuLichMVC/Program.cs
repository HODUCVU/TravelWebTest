// using System.Text;
// using System.Web.Http;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Authentication.OpenIdConnect;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using Microsoft.Owin.Security.OAuth;
// using Microsoft.Owin.Security.OAuth.Messages;
// using NuGet.Protocol;
// using WebDuLichMVC.Models;
// using WebDuLichMVC.Services;

// //builder
// var builder = WebApplication.CreateBuilder(args);

// // HttpConfiguration config = new HttpConfiguration();
// //  OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
// //             {
// //                 AllowInsecureHttp = true,
// //                 TokenEndpointPath = new Microsoft.Owin.PathString("/token"),
// //                 AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
// //                 Provider = new SimpleAuthorizationServerProvider()
// //             };
// // Add services to the container.
// builder.Services.AddCors();
// builder.Services.AddMvc();
// builder.Services.AddScoped<IDataService<CustomerProfile>, DataService<CustomerProfile>>();
// builder.Services.AddScoped<IDataService<ProviderProfile>, DataService<ProviderProfile>>();
// builder.Services.AddScoped<IDataService<Location>, DataService<Location>>();
// builder.Services.AddScoped<IDataService<Package>, DataService<Package>>();
// builder.Services.AddScoped<IDataService<Order>, DataService<Order>>();
// builder.Services.AddScoped<IDataService<Feedback>, DataService<Feedback>>();
// builder.Services.AddControllersWithViews();


// builder.Services.AddDbContext<MyDbContext>(options =>
// {
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
// });

// builder.Services.Configure<IdentityOptions>
// (
//    config =>
//    {
//        config.Password.RequireDigit = false;
//        config.Password.RequiredLength = 3;
//        config.Password.RequireNonAlphanumeric = false;
//        config.Password.RequireUppercase = false;
//    }
// );

// builder.Services.ConfigureApplicationCookie(options => { options.AccessDeniedPath = "/Account/Denied"; });   // Trang khi User bị cấm truy cập


// //app
// var app = builder.Build();


// // Configure the HTTP request pipeline.
// // if (!app.Environment.IsDevelopment())
// // {
// //     app.UseExceptionHandler("/Home/Error");
// //     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
// //     app.UseHsts();
// // }

// // app.UseHttpsRedirection();

// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthentication();
// app.UseAuthorization();
// app.UseCors(builder=>builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

// // if(x) SeedHelper.Seed(app.ApplicationServices).Wait();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

// app.Run();

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
// using SendGrid;
// using SendGrid.Helpers.Mail;

namespace WebDuLichMVC
{
    public class Program
    {
        // public static void Main(string[] args)
        // {
        //     CreateHostBuilder(args).Build().Run();
        // }
    
        // public static IHostBuilder CreateHostBuilder(string[] args) =>
        //     Host.CreateDefaultBuilder(args)
        //         .ConfigureWebHostDefaults(webBuilder =>
        //         {
        //             webBuilder.UseStartup<Startup>();
        //         });

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();        
    }
}
