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
            //TODO: Create background task
            var directory = AppDomain.CurrentDomain.BaseDirectory;

            var serverManager = new ServerManager("http://localhost:5000", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoicGF2ZWwua3MxOTk2QGdtYWlsLmNvbSIsInN1YiI6InBhdmVsLmtzMTk5NkBnbWFpbC5jb20iLCJuYmYiOjE1NDc0MTgxNDAsImV4cCI6MTU1MDAxMDE0MCwiaXNzIjoiU01UVS1EZXYiLCJhdWQiOiJTTVRVLURldi1TZW5zb3JTZXJ2ZXIifQ.UdPpvSUG1vEEKsbHCRPZH2H59w_wUKIIadyMnGQFIRs");
            serverManager.Load(directory);

            serverManager.Start();
        }
    }
}
