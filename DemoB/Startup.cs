using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DemoB
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCap(x=> {
                x.UseRabbitMQ(opt=> {
                    opt.HostName = "192.168.0.89";
                    opt.UserName = "admin";
                    opt.Password = "admin";
                });
                x.UseSqlServer("Data Source=192.168.0.250;Initial Catalog=cap_trans_test;User ID=sa;Password=123123;");
                x.PollingDelay = 1;
            });
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseCap();
        }
    }
}
