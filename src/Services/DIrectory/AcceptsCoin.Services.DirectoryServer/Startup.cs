using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Common.Auth;
using AcceptsCoin.Services.DirectoryServer.Core.Interfaces;
using AcceptsCoin.Services.DirectoryServer.Core.Services;
using AcceptsCoin.Services.DirectoryServer.Data.Context;
using AcceptsCoin.Services.DirectoryServer.Data.Repository;
using AcceptsCoin.Services.DirectoryServer.Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AcceptsCoin.Services.DirectoryServer
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

            services.AddDbContext<AcceptsCoinDirectoryDbContext>(option =>
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





            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IBusinessTagRepository, BusinessTagRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBusinessGalleryRepository, BusinessGalleryRepository>();
            services.AddScoped<IBusinessRepository, BusinessRepository>();
            services.AddScoped<IBusinessService, BusinessService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                endpoints.MapGrpcService<BusinessGrpcService>()
                .EnableGrpcWeb().RequireCors("cors");

                endpoints.MapGrpcService<BusinessGalleryGrpcService>()
                .EnableGrpcWeb().RequireCors("cors");

                endpoints.MapGrpcService<ReviewGrpcService>()
                 .EnableGrpcWeb().RequireCors("cors");



                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
