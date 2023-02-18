using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;

namespace bleXam.Models
{
    public class DeviceModel
    {
        public Guid Id { get; internal set; }
        public string Name { get; internal set; }
    }
}

