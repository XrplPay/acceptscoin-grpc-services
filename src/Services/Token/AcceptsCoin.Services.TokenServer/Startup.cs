using System;
using AcceptsCoin.Services.TokenServer.Core.Interfaces;
using AcceptsCoin.Services.TokenServer.Core.Services;
using AcceptsCoin.Services.TokenServer.Data.Context;
using AcceptsCoin.Services.TokenServer.Data.Repository;
using AcceptsCoin.Services.TokenServer.Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AcceptsCoin.Services.TokenServer
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

            services.AddDbContext<AcceptsCoinTokenDbContext>(option =>
                option.UseNpgsql(PGSQL_CONNECTION_STRING, x => x.UseNetTopologySuite())
            );


            services.AddGrpc();


            services.AddCors(options => {
                options.AddPolicy("cors", policy => {
                    policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding"); ;
                });
            });




            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<ITokenService, TokenService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseCors("cors");
            app.UseGrpcWeb();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGrpcService<TokenGrpcService>().EnableGrpcWeb()
                                                .RequireCors("cors");

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
