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
using RuletaPruebaMasivian.Interface.IBusiness;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RuletaPruebaMasivian.Business;
using RuletaPruebaMasivian.Context;
using RuletaPruebaMasivian.Interface.IContext;
using Microsoft.OpenApi.Models;

namespace RuletaPruebaMasivian
{
    public class Startup
    {
        public IWebHostEnvironment Env { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("RedisConnection");
                options.InstanceName = "democacheKF";
            });
            services.AddTransient<IRuletaBusiness, RuletaBusiness>();
            services.AddTransient<IRuletaContext, RuletaContext>();
            services.AddConnections(options => options.ToString());
            AddSwagger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(x => x.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader());
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Foo API V1");
            });

        }
        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Casino {groupName}",
                    Version = groupName,
                    Description = "Casino Ruletas",
                    Contact = new OpenApiContact
                    {
                        Name = "Casino Company",
                        Email = string.Empty,
                        Url = new Uri("https://casino.com/"),
                    }
                });
            });
        }
    }
}
