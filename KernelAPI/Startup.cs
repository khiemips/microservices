using KernelAPI.Context;
using KernelAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KernelAPI
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
            services.AddMvc();
            services.AddSingleton(Configuration);

            if (Configuration["Storage"].ToLower() == "blob")
            {
                services.AddTransient<IStorageService, AzureStorageService>(implement => new AzureStorageService(Configuration["BlobStorage:ConnectionString"]));
            }
            else
            {
                services.AddTransient<IStorageService, AzureRedisService>(implement => new AzureRedisService(Configuration["RedisStorage:ConnectionString"]));
            }

            services.AddTransient<ICppKernelFactory, CppKernelFactory>();            
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSession();
            app.UseMvc();
        }
    }
}
