using DreamCloud.Product.Data.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using DreamCloud.Product.Services.Sql.DependencyInjection;
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DreamCloud.Product.Api", Version = "v1" });
            });

            //services.AddDbContext<ProductContext>(opt => opt.UseInMemoryDatabase("ProductDb")); // Hardcode inmemory database name for demo only.
            //services.AddScoped<ProductContext>();

            services.AddDbContext<ProductContext>(opt =>
                    opt.UseSqlServer(productSqlDbConnectionString)
                        .EnableSensitiveDataLogging()
            );

            services.AddAzureSqlDatabaseServices(productSqlDbConnectionString);

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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
