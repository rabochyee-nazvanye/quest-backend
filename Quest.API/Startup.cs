using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Quest.API.Services;
using Quest.DAL.Data;
using Quest.Domain.Models;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Quest.API.Helpers;
using Quest.Application;
using Quest.Application.Services.ExternalAuth;
using Quest.Application.Teams.Commands;
using Quest.Domain.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Quest.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSignalR();
            services.AddProblemDetails(setup => {

                setup.IncludeExceptionDetails = (_,__) => Environment.IsDevelopment();
            });

            services.AddControllers(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            }).AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ";
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // Get Connection string from configuration file
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<Db>(options =>
                options.UseNpgsql(connection,
                    b => b.MigrationsAssembly("Quest.DAL")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<Db>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            //Add custom services to DI container
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IExternalAuthService, ExternalAuthService>();

            services.AddHttpContextAccessor();
            services.UpgradePasswordSecurity().UseArgon2<IdentityUser>();

            //add swagger for API docs
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0", new OpenApiInfo { Title = "api", Version = "v0" });
            });

            //add MediatR for API & Application assemblies
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(CreateParticipantCommand).GetTypeInfo().Assembly);
        }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            }

            if (env.IsProduction())
            {
                var safeOrigins = Configuration.GetSection("CorsOrigins").Get<string[]>();
                app.UseCors(options => options
                    .WithOrigins(safeOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowedToAllowWildcardSubdomains());
            }

            var supportedCultures = new[] { new CultureInfo("ru-RU") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru-RU"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            
            app.UseProblemDetails();

            app.UseRouting();
            app.UseSwagger();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v0/swagger.json", "api v0");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<AttemptsHub>("/attempts");
            });
        }
    }
}
