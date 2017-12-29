using DotVVM.Framework.Configuration;

namespace RFI.LazarusJokes.Web
{
    public class DotvvmStartup : IDotvvmStartup
    {
        // For more information about this class, visit https://dotvvm.com/docs/tutorials/basics-project-structure
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
#if !DEBUG
            config.Debug = false;
#endif

            ConfigureRoutes(config, applicationPath);
        }

        private void ConfigureRoutes(DotvvmConfiguration config, string applicationPath)
        {
            config.RouteTable.Add("Default", "LazarusJokes", "Views/jokes.dothtml");
            config.RouteTable.Add("Jokes", "LazarusJokes/jokes", "Views/jokes.dothtml");

            // Uncomment the following line to auto-register all dothtml files in the Views folder
            // config.RouteTable.AutoDiscoverRoutes(new DefaultRouteStrategy(config));    
        }
    }
}
