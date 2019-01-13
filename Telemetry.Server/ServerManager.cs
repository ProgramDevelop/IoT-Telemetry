using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Loader;
using System.Text;
using Telemetry.Base.Interfaces;

namespace Telemetry.Server
{
    class ServerManager
    {
        private readonly List<IReceiver> _receivers;
        private string _uri;
        private readonly string _apiKey;

        public ServerManager(string apiUri, string apiKey)
        {
            _uri = apiUri;
            _apiKey = apiKey;
            _receivers = new List<IReceiver>();
        }

        public void Load(string directory)
        {
            var files = Directory.GetFiles(directory, "*.dll");

            foreach (var file in files)
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);

                var assemblyTypes = assembly.GetTypes();

                foreach (var type in assemblyTypes)
                {
                    var receiverType = type.GetInterface(nameof(IReceiver));
                    if (receiverType != null)
                    {
                        var plugin = (IReceiver)Activator.CreateInstance(type);
                        _receivers.Add(plugin);
                    }
                }
            }
        }

        public void Start()
        {
            foreach (var receiver in _receivers)
            {
                receiver.Start();
                receiver.OnMessageReceive += Receiver_OnMessageReceive;
            }
        }

        private void Receiver_OnMessageReceive(MessageEventArgs obj)
        {
        }

        private async void SendData(Guid sensorId, string name, ISensorData payload)
        {
            var bodyJson = new JObject();
            bodyJson.Add("Name", name);
            var payloadJson = JToken.FromObject(payload);
            bodyJson.Add("Payload", payloadJson);

            string body = bodyJson.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_uri);
                var request = new HttpRequestMessage(HttpMethod.Post, $"/api/sensors/{sensorId}/value/set")
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                var response = await client.SendAsync(request);
                var responseStr = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
