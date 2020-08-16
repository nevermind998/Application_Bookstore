using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LoboPraksa_Zadatak1.DAL;
using LoboPraksa_Zadatak1.BL;
using LoboPraksa_Zadatak1.DAL.Interfaces;
using LoboPraksa_Zadatak1.BL.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using LoboPraksa_Zadatak1.Helper;

namespace LoboPraksa_Zadatak1
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
            services.AddControllers();

            services.AddTransient<IAuthorBL, AuthorBL>();
            services.AddTransient<IAuthorDAL, AuthorDAL>();

            services.AddTransient<IBookBL, BookBL>();
            services.AddTransient<IBookDAL, BookDAL>();

            services.AddTransient<IUserBL, UserBL>();
            services.AddTransient<IUserDAL, UserDAL>();

            services.AddTransient<IGenreBL, GenreBL>();
            services.AddTransient<IGenreDAL, GenreDAL>();

            services.AddTransient<IGoogleDriveAPIHelper, GoogleDriveAPIHelper>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = Configuration["Jwt:Issuer"],
                   ValidAudience = Configuration["Jwt:Issuer"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
               };
           }
           );

            services.AddDataProtection();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IDataProtectionProvider provider, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("Logs/myapp-{Date}.txt");
            Init(provider);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ZipMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void Init(IDataProtectionProvider provider)
        {
            ProtectionHelper.Singleton.SetConfig(Configuration);

            ProtectionHelper.Singleton.SetProvider(provider, Configuration["DataProtection:Key:Value"]);
            ProtectionHelper.Singleton.GetSectionValue("DataProtection:Key");
        }
    }
}
