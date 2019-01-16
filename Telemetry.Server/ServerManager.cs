using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Base.Interfaces;

namespace Telemetry.Server
{
    class ServerManager
    {
        private readonly List<IReceiver> _receivers;
        private readonly string _uri;
        private string _apiKey;

        public event Action<object, MessageEventArgs> OnMessageReceived;

        public ServerManager(string apiUri)
        {
            _uri = apiUri;
            _receivers = new List<IReceiver>();
        }

        public async Task<bool> Authentificate(string login, string password)
        {
            var bodyJson = new JObject();
            bodyJson.Add("email", login);
            bodyJson.Add("password", password);

            string body = bodyJson.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_uri);
                var request = new HttpRequestMessage(HttpMethod.Post, $"/api/Token/Create")
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/json"),
                };

                HttpResponseMessage response = null;
                try
                {
                    response = await client.SendAsync(request);
                }
                catch (Exception)
                {
                    return false;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();
                    _apiKey = responseStr;
                    return true;
                }
            }
            return false;
        }

        public void Load(string directory)
        {
            var fullDirectory = Path.GetFullPath(directory);
            var files = Directory.GetFiles(fullDirectory, "*.dll");

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
                        plugin.OnMessageReceive += Receiver_OnMessageReceive;
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
            }
        }

        public void Stop()
        {
            foreach (var receiver in _receivers)
            {
                receiver.Stop();
            }
        }

        private void Receiver_OnMessageReceive(MessageEventArgs obj)
        {
            SendData(obj.SensorId, obj.ValueName, obj.Payload);
            OnMessageReceived?.Invoke(this, obj);
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
