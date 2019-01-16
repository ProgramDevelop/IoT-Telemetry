using System;
using System.Text;
using Xunit;
using Telemetry.Receivers;
using System.Net.Sockets;
using Telemetry.Base.Interfaces;
using System.Net;

namespace Telemetry.Tests
{
    public class UDPReceiverTest
    {
        [Fact]
        public void ReturnIsEventCalled()
        {
            IReceiver udpRec = new UDPReceiver("UDP", "", 2020);
            var wait = new System.Threading.AutoResetEvent(true);
            udpRec.OnMessageReceive += (MessageEventArgs) => { wait.Set(); };
            udpRec.Start();

            var ipaddress = IPAddress.Parse("127.0.0.1");
            var ipendpoint = new IPEndPoint(ipaddress, 2020);
            var udp = new UdpClient(2001);
            var id = new Guid("0DAC21AC-67A2-4639-9C6E-30E993C288CC");
            string msg = id.ToString() + "_test_000";
            byte[] bytesArr = Encoding.ASCII.GetBytes(msg);
            udp.Send(bytesArr, bytesArr.Length, ipendpoint);

            Assert.True(wait.WaitOne(TimeSpan.FromSeconds(5)));
        }

        [Fact]
        public void ReturnObjectCreated()
        {
            var udpRec = new UDPReceiver("rec0", "test", 2000);

            Assert.NotNull(udpRec);
            Assert.Equal("rec0", udpRec.Name);
            Assert.Equal("test", udpRec.Description);
        }

        [Fact]
        public void ReturnCorrectParsing()
        {
            var origObj = new MessageEventArgs();
            origObj.SensorId = new Guid("0DAC21AC-67A2-4639-9C6E-30E993C288CC");
            origObj.ValueName = "12345";
            origObj.Payload = new SensorData { DateTime = DateTime.Today, Data = "SUNDAY" };

            string text = "0DAC21AC-67A2-4639-9C6E-30E993C288CC_12345_SUNDAY";
            string[] textArr = text.Split('_');

            var obj = new MessageEventArgs();
            obj.SensorId = new Guid(textArr[0]);
            obj.ValueName = textArr[1];
            obj.Payload = new SensorData { DateTime = DateTime.Today, Data = textArr[2] };

            Assert.Equal(origObj.SensorId, obj.SensorId);
            Assert.Equal(origObj.ValueName, obj.ValueName);
            Assert.Equal(origObj.Payload.Data, obj.Payload.Data);
            Assert.Equal(origObj.Payload.DateTime, obj.Payload.DateTime);
        }
    }
}
