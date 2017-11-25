using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DemoA
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
            services.AddCap(x =>
            {
                x.UseRabbitMQ(opt =>
                {
                    opt.HostName = "192.168.0.89";
                    opt.UserName = "admin";
                    opt.Password = "admin";
                });
                x.UseSqlServer("Data Source=192.168.0.250;Initial Catalog=CapDemo;User ID=sa;Password=123123;");
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCap();
            app.UseMvc();
        }
    }
}
