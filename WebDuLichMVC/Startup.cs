using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebDuLichMVC.Models;
using WebDuLichMVC.Services;

[assembly: OwinStartup(typeof(WebDuLichMVC.Startup))]
namespace WebDuLichMVC
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);

        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new Microsoft.Owin.PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();
            services.AddScoped<IDataService<CustomerProfile>, DataService<CustomerProfile>>();
            services.AddScoped<IDataService<ProviderProfile>, DataService<ProviderProfile>>();
            services.AddScoped<IDataService<Location>, DataService<Location>>();
            services.AddScoped<IDataService<Package>, DataService<Package>>();
            services.AddScoped<IDataService<Order>, DataService<Order>>();
            services.AddScoped<IDataService<Feedback>, DataService<Feedback>>();
            services.AddControllersWithViews();
            services.Configure<IdentityOptions>
            (
                 config =>
                   {
                       config.Password.RequireDigit = false;
                       config.Password.RequiredLength = 3;
                       config.Password.RequireNonAlphanumeric = false;
                       config.Password.RequireUppercase = false;
                   }
            );
            services.AddDbContext<MyDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<MyDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options => { options.AccessDeniedPath = "/Account/Denied"; }); 
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization ();   // Phục hồi thông tinn về quyền của User
            app.UseCors(builder=>builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseMvcWithDefaultRoute();
            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=Home}/{action=Index}/{id?}");
            // });
            
            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapControllerRoute(
            //         name: "default",
            //         pattern: "{controller=Home}/{action=Index}/{id?}");
            // });
            // SeedHelper.Seed(app.ApplicationServices).Wait();
        }
    }
}