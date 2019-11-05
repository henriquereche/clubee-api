using Clubee.API.Infrastructure.Authorization;
using Clubee.API.Infrastructure.DI;
using Clubee.API.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Clubee.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Routing settings.
            services.AddRouting(config =>
            {
                config.LowercaseQueryStrings = true;
                config.LowercaseUrls = true;
            });

            // Enable Jwt authorization.
            services.AddJwtAuthorizarion();

            // Swagger document gereneration options.
            services.AddSwaggerGen(config =>
            {
                config.EnableAnnotations();
                config.SwaggerDoc("v1", new Info { Title = "Clubee API", Version = "v1" });
            });

            // Register application services.
            services.RegisterApplicationServices();

            // Enble reponse compression services.
            services.AddResponseCompression();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Clubee API V1");
            });

            // Enable response compression middleware.
            app.UseResponseCompression();

            // Register middleware for exception handling.
            app.UseMiddleware<ApplicationExceptionHandlerMiddleware>();

            // Register CORS settings.
            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
                options.AllowCredentials();
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
