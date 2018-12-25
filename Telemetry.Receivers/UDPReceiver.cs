using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telemetry.Base.Interfaces;

namespace Telemetry.Receivers
{
    public class UDPReceiver : IReceiver
    {
        #region Private Fields

        private IPAddress _ipaddress;
        private IPEndPoint _ipendpointSender;
        private Thread _rec = null;
        private UdpClient _udp;
        private bool _stopReceive = false;

        #endregion

        #region Constructors

        public UDPReceiver() : this("UdpReceiver", "", 2000)
        {
        }

        public UDPReceiver(string name, string description, int port)
        {
            Name = name;
            Description = description;
            _udp = new UdpClient(port);
        }

        #endregion

        #region Properties

        public string Name { get; }

        public string Description { get; }

        #endregion

        #region Events

        public event Action<MessageEventArgs> OnMessageReceive = e => { };

        #endregion

        public void Start()
        {
            // Запускаем отдельный поток для асинхронной работы приложения
            // во время приема сообщений
            _stopReceive = false;
            _rec = new Thread(new ThreadStart(Receive));
            _rec.Start();
        }

        public void Receive()
        {
            try
            {
                while (!_stopReceive)
                {
                    IPEndPoint ipendpoint = null;
                    byte[] message = _udp.Receive(ref ipendpoint);

                    // Анкодинг:
                    string text = Encoding.Default.GetString(message);

                    // GET_ID_1231
                    string[] textArr = text.Split('_');

                    var obj = new MessageEventArgs();
                    obj.SensorId = new Guid(textArr[0]);
                    obj.ValueName = textArr[1];
                    obj.Payload = new SensorData { DateTime = DateTime.Now, Data = textArr[2] };

                    OnMessageReceive(obj);

                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
        }

        public void Stop()
        {
            _stopReceive = true;
            if (_udp != null) _udp.Close();
            if (_rec != null) _rec.Join();
        }
    }
}
