using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .ConfigureWebHostDefaults(webBuilder =>
              {
                  if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                  {
                      webBuilder.ConfigureKestrel(options =>
                      {
                          // Setup a HTTP/2 endpoint without TLS.
                          //options.ListenLocalhost(5050, o => o.Protocols =
                            // HttpProtocols.Http1AndHttp2AndHttp3);
                      });
                  }
                  webBuilder.UseStartup<Startup>().UseUrls("http://*:5050");
              });
    }
}
