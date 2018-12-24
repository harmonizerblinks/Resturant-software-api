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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resturant.Models;
using Microsoft.EntityFrameworkCore;
using Resturant.Repository;
using Swashbuckle.AspNetCore.Swagger;
using Resturant.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Newtonsoft.Json;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Formatting.Json;
using Microsoft.AspNetCore.Http;
using Resturant.Services;
using Hangfire;

namespace Resturant
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var Date = DateTime.Now.ToString(@"yyyy-MM-dd");
            Log.Logger = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .WriteTo.File(new JsonFormatter(), $"Files/Logs/ken-{Date}.json")
              .CreateLogger();

            var log = new LoggerConfiguration()
                .WriteTo.MSSqlServer(
                    connectionString: Configuration.GetConnectionString("DefaultConnection"),
                    tableName: "Logs", columnOptions:  new ColumnOptions(),
                    autoCreateSqlTable: true ).CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));

            
            //Add Identity and Jwt
            services.AddIdentity<AppUser, IdentityRole>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequiredLength = 3;
                option.Password.RequiredUniqueChars = 0;
                option.Password.RequireLowercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;

                // Lockout settings
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                option.Lockout.MaxFailedAccessAttempts = 3;
                option.Lockout.AllowedForNewUsers = true;

                // User settings
                option.User.RequireUniqueEmail = true;
                option.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<AppDbContext>()
              /*.AddDefaultUI()*/.AddDefaultTokenProviders();
            //services.AddIdentityServer().AddAspNetIdentity<AppUser>();

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                //o.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = "http://www.acyst.tech",
                        ValidAudience = "http://localhost:53720",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("wertyuiopasdfghjklzxcvbnm123456")),
                    };
                }).AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = "662510424697-6na0e00bgn73tf5s9sn0iv89sjjja9k4.apps.googleusercontent.com";
                    googleOptions.ClientSecret = "o3P-Xb6jmpEg6uEQvWukqnWx";
                });

            services.AddDataProtection();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAny", a =>
                {
                    a.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromSeconds(20000));
                });
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("CanAssignRoles", "true"));
                options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeId"));
            });
            //services.AddAuthorization(options => options.AddPolicy("Admin", policy => policy.RequireClaim("CanAssignRoles", "true")));
            services.AddOptions();

            services.AddLogging();
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new Info { Title = "Resturant Api", Version = "v1" });
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1", Title = "Resturant Api", Description = "A Resturant Software Api", TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Harmony Alabi", Email = "Harmonizerblinks@gmail.com",
                        Url = "http://www.linkedin.com/in/harmony-alabi-93907086/"
                    },
                    License = new License
                    {
                        Name = "Acyst Technology Ltd", Url = "https://github.com/harmonizerblinks/Resturant-software-blob/master/LICENSE"
                    }
                });
            });
            services.AddSignalR();
            
            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new LowercaseContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<IActivityRepository, ActivityRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IDiscountRepository, DiscountRepository>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IFoodRepository, FoodRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<INominalRepository, NominalRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderListRepository, OrderListRepository>();
            services.AddTransient<ISalesRepository, SalesRepository>();
            services.AddTransient<ISequenceRepository, SequenceRepository>();
            services.AddTransient<ISmsApiRepository, SmsApiRepository>();
            services.AddTransient<ISmsRepository, SmsRepository>();
            services.AddTransient<IStockRepository, StockRepository>();
            services.AddTransient<IStockLogRepository, StockLogRepository>();
            services.AddTransient<ITellerRepository, TellerRepository>();
            services.AddTransient<ITransitRepository, TransitRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IAppUserRepository, AppUserRepository>();
            services.AddTransient<IEmailSender, EmailSender>();
            //services.AddTransient<IMyServices, MyServices>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }

        public class LowercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToLower();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpStatusCodeExceptionMiddleware();
            }
            else
            {
                app.UseHttpStatusCodeExceptionMiddleware();
                app.UseExceptionHandler("/Error"); 
                //app.UseExceptionHandler("/Error");
                //app.UseHsts();
            }
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //app.UseHttpStatusCodeExceptionMiddleware();
            //app.UseExceptionHandler();

            //Add our new middleware to the pipeline
            //app.UseMiddleware<LoggingMiddleware>();

            app.UseCors("AllowAny");
            app.UseAuthentication();
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            app.UseDatabaseErrorPage();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Files")), RequestPath = "/Files"
            });
            // app.UseSerilog();
            app.UseStatusCodePages();
            app.UseSignalR(o =>
            {
                o.MapHub<OrderHub>("/Orders");
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Resturant Api");
            });

            //app.UseSession();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
