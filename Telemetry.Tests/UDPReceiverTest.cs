﻿using System;
using System.Collections.Generic;
using System.Text;
using Telemetry.Web.Services.SensorManager;
using Xunit;
using Moq;
using Telemetry.Database.Storages;
using Telemetry.Database.Models;
using System.Linq;
using Telemetry.Base;
using Telemetry.Receivers;
using System.Net.Sockets;
using Telemetry.Base.Interfaces;

namespace Telemetry.Tests
{
    public class UDPReceiverTest
    {
        [Fact]
        public void ReturnIsEventCalled()
        {
            UDPReceiver udpRec = new UDPReceiver();

            var wait = new System.Threading.AutoResetEvent(false);
            udpRec.OnMessageReceive += (MessageEventArgs) => { wait.Set(); };
            Assert.False(wait.WaitOne(TimeSpan.FromSeconds(5)));
        }

        [Fact]
        public void ReturnObjectCreated()
        {
            UDPReceiver udpRec = new UDPReceiver("rec0", "test", 2000);

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
