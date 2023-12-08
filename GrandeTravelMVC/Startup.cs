using GrandeTravelMVC.Services;
using Microsoft.AspNetCore.Identity;
using GrandeTravelMVC.Models;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager; // Add this for ConfigurationManager


[assembly: OwinStartup(typeof(GrandeTravelMVC.Startup))]
namespace GrandeTravelMVC
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
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
                    // Thiết lập về Password
                        // options.Password.RequireDigit = false; // Không bắt phải có số
                        // options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                        // options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                        // options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                        // options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                        // options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                        // // Cấu hình Lockout - khóa user
                        // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
                        // options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
                        // options.Lockout.AllowedForNewUsers = true;

                        // // Cấu hình về User.
                        // options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                        //     "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                        // options.User.RequireUniqueEmail = true; // Email là duy nhất

                        // // Cấu hình đăng nhập.
                        // options.SignIn.RequireConfirmedEmail = true; // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                        // options.SignIn.RequireConfirmedPhoneNumber = false; // Xác thực số điện thoại
                    }
                );
                // ).AddEntityFrameworkStores<MyDbContext>(); 

        services.AddDbContext<MyDbContext>();
        //  services.AddDbContext<MyDbContext>(
        //     options => {
        //         string connectionString = ConfigurationManager.ConnectionStrings["AuthContext"].ConnectionString;
        //         options.UseSqlServer(connectionString);
        //     }
        // );
        services.ConfigureApplicationCookie(options => { options.AccessDeniedPath = "/Account/Denied"; });       
    }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            //SeedHelper.Seed(app.ApplicationServices).Wait();
        }
    }
}
