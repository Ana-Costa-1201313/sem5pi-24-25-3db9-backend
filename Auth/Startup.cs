using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json.Serialization;

using Auth.Infrastructure;
using Auth.Infrastructure.Shared;
using Auth.Domain.Shared;
using Auth.Domain.Users;
using Auth.Domain;
using Auth.Infrastructure.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;


namespace Auth
{
    public class Startup
    {
        public string DbPath { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "Auth.db");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<BDContext>(opt =>
            //opt.UseSqlServer("Server=vsgate-s1.dei.isep.ipp.pt,10513;User Id=sa;Password=rscxDifxGw==Xa5;encrypt=true;TrustServerCertificate=True;")
            //.ReplaceService<IValueConverterSelector, StronglyEntityIdValueConverterSelector>());


            services.AddDbContext<AuthDbContext>(opt =>
                opt.UseSqlite($"Data Source={DbPath}")
                .ReplaceService<IValueConverterSelector, StronglyEntityIdValueConverterSelector>());

            ConfigureMyServices(services);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();



            services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


            // redirecionar utilizadores para a pagina de autentica��o da google  --  https://learn.microsoft.com/en-us/aspnet/web-api/overview/security/external-authentication-services
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

            })
                .AddCookie()
                .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
                {
                    options.ClientId = this.Configuration.GetSection("GoogleKeys:ClientID").Value;
                    options.ClientSecret = this.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureMyServices(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<LoginService>();

            services.AddHttpClient<ExternalApiService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["Backoffice:Uri"]);
            });
        }
    }
}



