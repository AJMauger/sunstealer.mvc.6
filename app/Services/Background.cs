
namespace sunstealer.mvc.Services {

    // ajm: interface
    public interface IBackground
    {
        void ConsoleWriteMessage(string message);
    }

    // ajm: service
    class Background : Microsoft.Extensions.Hosting.BackgroundService 
    {
        public Background() {
        }

        protected override async System.Threading.Tasks.Task<bool> ExecuteAsync(System.Threading.CancellationToken cancellationToken) {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, cancellationToken);
                }

                return true;
            } catch (System.Exception)
            {
                return false;
            }
        }
    }
}