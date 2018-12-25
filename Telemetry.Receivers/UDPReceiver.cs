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
        private IPAddress ipaddress;
        private IPEndPoint ipendpoint_sender;
        private Thread rec = null;
        private UdpClient udp;

        private bool stopReceive = false;

        #region Constructors

        public UDPReceiver() : this("UdpReceiver", "", 15000)
        {
        }

        public UDPReceiver(string name, string description, int port)
        {
            Name = name;
            Description = description;
            udp = new UdpClient(port);
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
            stopReceive = false;
            rec = new Thread(new ThreadStart(Receive));
            rec.Start();
        }

        public void Receive()
        {
            try
            {
                while (true)
                {
                    IPEndPoint ipendpoint = null;
                    byte[] message = udp.Receive(ref ipendpoint);

                    // Анкодинг:
                    string text = Encoding.Default.GetString(message);

                    // Если дана команда остановить поток, останавливаем бесконечный цикл.
                    if (stopReceive == true) break;
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
        }

        public void Stop()
        {
            stopReceive = true;
            if (udp != null) udp.Close();
            if (rec != null) rec.Join();
        }

        private void SendMessage(string text)
        {
            UdpClient udp = new UdpClient();

            // Формирование отправляемого сообщения и его отправка:
            byte[] message = Encoding.Default.GetBytes(text);
            int sended = udp.Send(message, message.Length, ipendpoint_sender);

            // Закрытие потока:
            udp.Close();
        }
    }
}
