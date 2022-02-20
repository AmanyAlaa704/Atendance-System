using BL.AppServices;
using BL.Bases;
using BL.Interfaces;
using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtendanceAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            services.AddControllers();
            services.AddDbContext<ApplicationDBContext>(option => {
                option.UseSqlServer(Configuration.GetConnectionString("AttendanceSystem"),
                    options => options.EnableRetryOnFailure());
            });

            services.AddIdentity<ApplicationUsersIdentity, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<UserManager<ApplicationUsersIdentity>>();
            services.AddTransient<RoleManager<IdentityRole>>();
            services.AddTransient<AccountAppService>();
            services.AddTransient<RoleAppService>();
            services.AddTransient<SettingAppService>();
            services.AddTransient<DailyLogsAppServices>();

            services.AddHttpContextAccessor();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();

            services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyTestService", Version = "v1", });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
          // Adding Jwt Bearer  
          .AddJwtBearer(options =>
          {
              options.SaveToken = true;
              options.RequireHttpsMetadata = false;
              options.TokenValidationParameters = new TokenValidationParameters()
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidAudience = Configuration["JWT:ValidAudience"],
                  ValidIssuer = Configuration["JWT:ValidIssuer"],
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
              };
          });

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
           RoleAppService roleAppService, AccountAppService accountAppService)
        {            

            
            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(
                options => options
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()                );
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // create custom roles 
            roleAppService.CreateRoles().Wait();
            // add custom first admin
            accountAppService.CreateFirstAdmin().Wait();

        }

    }
}
