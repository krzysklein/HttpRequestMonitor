using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace HttpRequestMonitor
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<HttpRequestBrokerHub>("/httpRequestBrokerHub");
            });
            app.Map("/monitor", HandleMonitorRequest);
        }

        public static void HandleMonitorRequest(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var url = context.Request.Path.ToString();
                var method = context.Request.Method;
                var headers = context.Request.Headers;
                string body;
                using (var reader = new StreamReader(context.Request.Body))
                {
                    body = reader.ReadToEnd();
                }

                var hubContext = context.RequestServices.GetRequiredService<IHubContext<HttpRequestBrokerHub>>();
                await hubContext.Clients.All.SendAsync("HttpRequestReceived", url, method, headers, body);

                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("OK");
            });
        }
    }
}
