using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetcore_cassandra.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace aspnetcore_cassandra
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
            services.AddControllersWithViews();
            CosmosDbService sev = InitializeCosmosClientInstance();
            services.AddSingleton<ICosmosDbService>(sev);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=MyItem}/{action=Index}/{id?}");
            });
        }

        private static CosmosDbService InitializeCosmosClientInstance()
        {
            string username = "servicelinker-cassandra-cosmos";
            string password = "NdwOj9g7Avq9ZOeJ9vmM2NrHWKv4KuOzmzKyhL4ziXrWw1pwNjAhnaNHu3viURrB2LpwcE9wwDaAYaL69idKYQ==";
            string contactPoints = "servicelinker-cassandra-cosmos.cassandra.cosmos.azure.com";
            int port = 10350;

            //string username = Environment.GetEnvironmentVariable("RESOURCECONNECTOR_MYCONN_USERNAME");
            //string password = Environment.GetEnvironmentVariable("RESOURCECONNECTOR_MYCONN_PASSWORD");
            //string contactPoints = Environment.GetEnvironmentVariable("RESOURCECONNECTOR_MYCONN_CONTACTPOINTS");
            //int port = int.Parse(Environment.GetEnvironmentVariable("RESOURCECONNECTOR_MYCONN_PORT"));

            CosmosDbService cosmosDbService = new CosmosDbService(
                username,
                password,
                contactPoints,
                port);
            return cosmosDbService;
        }
    }
}