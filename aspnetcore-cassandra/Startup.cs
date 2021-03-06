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

            string username = Environment.GetEnvironmentVariable("RESOURCECONNECTOR_TESTCASSANDRASECRETCONNECTIONSUCCEEDED_USERNAME");
            string password = Environment.GetEnvironmentVariable("RESOURCECONNECTOR_TESTCASSANDRASECRETCONNECTIONSUCCEEDED_PASSWORD");
            string contactPoints = Environment.GetEnvironmentVariable("RESOURCECONNECTOR_TESTCASSANDRASECRETCONNECTIONSUCCEEDED_CONTACTPOINT");
            int port = int.Parse(Environment.GetEnvironmentVariable("RESOURCECONNECTOR_TESTCASSANDRASECRETCONNECTIONSUCCEEDED_PORT"));
            string keyspace = Environment.GetEnvironmentVariable("RESOURCECONNECTOR_TESTCASSANDRASECRETCONNECTIONSUCCEEDED_KEYSPACE");

            CosmosDbService cosmosDbService = new CosmosDbService(
                username,
                password,
                contactPoints,
                port,
                keyspace);
            return cosmosDbService;
        }
    }
}
