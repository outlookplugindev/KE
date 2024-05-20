using Microsoft.AspNetCore.Builder;

namespace KE_Service
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            // Enable CORS
            //app.UseCors("AllowAll");
            app.UseCors("AllowOrigin");
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: "MyAllowSpecificOrigins",
            //                      policy =>
            //                      {
            //                          policy.WithOrigins("http://example.com",
            //                                              "http://www.contoso.com");
            //                      });
            //});
            //services.AddControllers();
        }
    }
}
