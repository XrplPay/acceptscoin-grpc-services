using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcceptsCoin.Common.Auth;
using AcceptsCoin.Services.CoreServer.Core.Interfaces;
using AcceptsCoin.Services.CoreServer.Core.Services;
using AcceptsCoin.Services.CoreServer.Data.Context;
using AcceptsCoin.Services.CoreServer.Data.Repository;
using AcceptsCoin.Services.CoreServer.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AcceptsCoin.Services.CoreServer
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

            services.AddDbContext<AcceptsCoinCoreDbContext>(option =>
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


            


            services.AddScoped<IPartnerCategoryRepository, PartnerCategoryRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<ILanguageService, LanguageService>();

            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagService, TagService>();


            services.AddScoped<IPartnerRepository, PartnerRepository>();
            services.AddScoped<IPartnerService, PartnerService>();

            services.AddScoped<IPartnerRepository, PartnerRepository>();
            services.AddScoped<IPartnerService, PartnerService>();
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
                endpoints.MapGrpcService<CategoryService>()
                .EnableGrpcWeb().RequireCors("cors");
                
                endpoints.MapGrpcService<TagGrpcService>()
                .EnableGrpcWeb().RequireCors("cors");
                
                endpoints.MapGrpcService<PartnerGrpcService>()
                .EnableGrpcWeb()
                .RequireCors("cors");

                endpoints.MapGrpcService<LanguageGrpcService>()
                .EnableGrpcWeb().RequireCors("cors");

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
