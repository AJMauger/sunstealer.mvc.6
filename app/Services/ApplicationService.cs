
namespace sunstealer.mvc.Services
{
    // ajm: interface
    public interface IApplication
    {
        Models.Configuration Configuration { get; }

        bool Reconfigure(Models.Configuration configuration);
    }

    // ajm: service
    public class Application : IApplication
    {
        private readonly Microsoft.Extensions.Logging.ILogger<Application> logger;

        public Models.Configuration Configuration { get; private set; }

        public Application(Microsoft.Extensions.Logging.ILogger<Application> logger) {
            this.Configuration = new Models.Configuration();
            this.logger = logger;    
        }

        public bool Reconfigure(Models.Configuration configuration)
        {
            try {
                if (configuration.Validate(configuration)) {
                    return true;
                }
            } catch(System.Exception e) {
                this.logger.LogCritical(e.ToString());
            }

            return false;
        }
    }
}