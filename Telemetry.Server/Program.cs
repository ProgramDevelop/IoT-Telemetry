using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Telemetry.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var credentials = configuration.GetSection("Credentials");
            var login = credentials["email"];
            var password = credentials["password"];
            var server = configuration["server"];
            var directory = configuration["directory"];

            var serverManager = new ServerManager(server);

            serverManager.OnMessageReceived += ServerManager_OnMessageReceived;

            ToLog("Start authentification");
            var result = serverManager.Authentificate(login, password).GetAwaiter().GetResult();
            if (result)
            {
                ToLog("Authentification success");
                serverManager.Load(directory);
                ToLog("Plugins loaded");
                serverManager.Start();
                ToLog("Plugins started");
            }
            else
            {
                ToLog("Authentification failed");
            }
            Console.ReadLine();
            ToLog("Stopping receivers");
            serverManager.Stop();
            ToLog("All receivers stopped");
            Console.ReadLine();
        }

        private static void ServerManager_OnMessageReceived(object sendler, Base.Interfaces.MessageEventArgs e) =>
            ToLog($"To sensor {e.SensorId} sended value {e.ValueName} with data {e.Payload?.Data} in {e.Payload.DateTime}");

        public static void ToLog(string message) => Console.WriteLine($"{DateTime.Now}: {message}");
    }
}
