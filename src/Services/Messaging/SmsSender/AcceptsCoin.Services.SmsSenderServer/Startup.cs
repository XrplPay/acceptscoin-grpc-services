using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Common.Auth;
using AcceptsCoin.Services.SmsSenderServer.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AcceptsCoin.Services.SmsSenderServer
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
            var PGSQL_CONNECTION_STRING = Environment.GetEnvironmentVariable("ACCEPTSCOIN_PGSQL_CONNECTION_STRING");

            if (PGSQL_CONNECTION_STRING == null)
            {
                PGSQL_CONNECTION_STRING = Configuration.GetConnectionString("ACCEPTSCOIN_PGSQL_CONNECTION_STRING");
            }

            services.AddDbContext<AcceptsCoinSmsSenderDbContext>(option =>
                option.UseNpgsql(PGSQL_CONNECTION_STRING, x => x.UseNetTopologySuite())
            );
            services.AddJwt(Configuration);
            services.AddAuthorization();
            services.AddGrpc();


            services.AddCors(options => {
                options.AddPolicy("cors", policy => {
                    policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding"); ;
                });
            });

            //services.AddScoped<ITokenRepository, TokenRepository>();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("cors");
            app.UseGrpcWeb();
            app.UseEndpoints(endpoints =>
            {
                //                endpoints.MapGrpcService<BusinessGrpcService>()
                //                .EnableGrpcWeb().RequireCors("cors");

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client");
                });
            });
        }
    }
}
