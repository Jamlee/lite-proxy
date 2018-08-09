using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using LiteSocks;
using Serilog;

namespace Web
{
    public class Program
    {

        // 启动代理进程
        public static void ProxyProcess()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            Log.Logger.Information("Start the Proxy Server");

            Proxy proxy = new Proxy("0.0.0.0", 8000);
            proxy.Start();
        }

        public static void Main(string[] args)
        {
            var task = Task.Factory.StartNew(ProxyProcess, TaskCreationOptions.LongRunning);

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
