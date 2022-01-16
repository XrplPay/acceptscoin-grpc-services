using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Common.Auth;
using AcceptsCoin.Services.StorageServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace AcceptsCoin.Services.StorageServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //    var PGSQL_CONNECTION_STRING = Environment.GetEnvironmentVariable("ACCEPTSCOIN_PGSQL_CONNECTION_STRING");

            //      if (PGSQL_CONNECTION_STRING == null)
            //        {
            //              PGSQL_CONNECTION_STRING = Configuration.GetConnectionString("ACCEPTSCOIN_PGSQL_CONNECTION_STRING");
            //            }

            // services.AddDbContext<AcceptsCoinDirectoryDbContext>(option =>
            //   option.UseNpgsql(PGSQL_CONNECTION_STRING, x => x.UseNetTopologySuite())
            // );
            services.AddControllers();

            services.AddJwt(Configuration);
            services.AddAuthorization();
            services.AddGrpc();


            services.AddCors(options =>
            {
                options.AddPolicy("cors", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding"); ;
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                            Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/files")),
                RequestPath = new PathString("/app-images")
            });

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseCors("cors");
            app.UseGrpcWeb();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGrpcService<GreeterService>();

                endpoints.MapGrpcService<FileUploadGrpcService>()
                .EnableGrpcWeb().RequireCors("cors");



                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
