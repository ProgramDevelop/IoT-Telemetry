using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Telemetry.Receivers
{
    public class UDPReceiver : IReceiver
    {
        private IPAddress ipaddress;
        private IPEndPoint ipendpoint_sender;
        private Thread rec = null;
        private UdpClient udp = new UdpClient(15000);

        private bool stopReceive = false;

        private string m_name;
        private string m_description;

        private UDPReceiver() { }

        public UDPReceiver(string name, string description)
        {
            m_name = name;
            m_description = description;
        }

        public string Name
        {
            get { return m_name; }
        }

        public string Description
        {
            get { return m_description; }
        }

        public Task StartAsync(CancellationToken token)
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

        public Task StopAsync()
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
