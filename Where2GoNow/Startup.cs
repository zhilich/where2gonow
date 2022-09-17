using Microsoft.AspNetCore.Mvc;
using Where2GoNow.DataAccess.Repositories;

namespace Where2GoNow
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Startup.Configuration = configuration;

        public static IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            MvcServiceCollectionExtensions.AddMvc(services, options => {
                options.EnableEndpointRouting = false;
            });
            ServiceCollectionServiceExtensions.AddSingleton<AttractionRepository>(services);
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            DefaultFilesExtensions.UseDefaultFiles(app);
            StaticFileExtensions.UseStaticFiles(app);
            MvcApplicationBuilderExtensions.UseMvc(app);
        }
    }
}