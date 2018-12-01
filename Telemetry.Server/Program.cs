using System;
using System.IO;
using System.Runtime.Loader;
using Telemetry.Base.Interfaces;

namespace Telemetry.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO: Create load function
            //TODO: Create background task
            var directory = AppDomain.CurrentDomain.BaseDirectory;

            var files = Directory.GetFiles(directory, "*.dll");

            foreach (var file in files)
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);

                var assemblyTypes = assembly.GetTypes();

                foreach (var type in assemblyTypes)
                {
                    var iface = type.GetInterface(nameof(IReceiver));
                    if (iface != null)
                    {
                        var plugin = (IReceiver)Activator.CreateInstance(type);
                    }
                }
            }
        }
    }
}
