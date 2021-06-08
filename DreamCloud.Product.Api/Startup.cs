using System.Linq;
using DreamCloud.Product.Data.DbContext;
using DreamCloud.Product.Models.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using DreamCloud.Product.Services.Sql.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace DreamCloud.Product.Api
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
            var productSqlDbConnectionString = Configuration.GetConnectionString("ProductSqlDbConnectionString");

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddControllers()
                .AddFluentValidation(fv => fv.ImplicitlyValidateChildProperties = true); // https://fluentvalidation.net/

            var allowedHosts = Configuration.GetSection("ApplicationOptions:AllowedHosts").Get<string[]>();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins(allowedHosts)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DreamCloud.Product.Api", Version = "v1" });
            });

            services.AddDbContext<ProductContext>(opt =>
                    opt.UseSqlServer(productSqlDbConnectionString)
                        .EnableSensitiveDataLogging()
            );

            services.AddAzureSqlDatabaseServices();
            services.ApplyModelValidators();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DreamCloud.Product.Api v1"));
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    // Todo... Logging.

                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                    });
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy"); // Apply the policy globally to every request in the application
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
