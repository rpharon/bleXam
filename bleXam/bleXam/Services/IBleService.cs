﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using bleXam.Models;

namespace bleXam.Services
{
	public interface IBleService
	{
        IBluetoothLE BluetoothLE { get; }
        IAdapter Adapter { get; }
        IService Service { get; set; }
        ICharacteristic Characteristic { get; set; }
        IDevice Device { get; set; }
        Task<List<IDevice>> ScanForDevicesAsync();
    }
}

