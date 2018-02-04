using System;
using System.Net.Sockets;
using Serilog;

namespace LiteSocks
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            Log.Logger.Information("Start the Proxy Server");
            Proxy proxy = new Proxy("0.0.0.0", 8000);
            proxy.Start();
        }
    }
}
