using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Xamarin.Essentials;
using Xamarin.Forms;
using bleXam.Models;

namespace bleXam.Services
{
    public class BleService : IBleService
    {
        public List<IDevice> Devices { get; private set; }

        public BleService()
        {
            Adapter.ScanTimeout = 4000;
            Adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
            Adapter.DeviceConnected += Adapter_DeviceConnected;
            Adapter.DeviceDisconnected += Adapter_DeviceDisconnected;
            Adapter.DeviceConnectionLost += Adapter_DeviceConnectionLost;

            BluetoothLE.StateChanged += BluetoothLE_StateChanged;

            Devices = new List<IDevice>();
        }

        public IBluetoothLE BluetoothLE => CrossBluetoothLE.Current;
        public IAdapter Adapter => CrossBluetoothLE.Current.Adapter;
        public IService Service { get; set; }
        public ICharacteristic Characteristic { get; set; }
        public IDevice Device { get; set; }

        public async Task<List<IDevice>> ScanForDevicesAsync()
        {
            try
            {
                Devices.Clear();

                await Adapter.StartScanningForDevicesAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert($"Unable to scan nearby Bluetooth LE devices", $"{ex.Message}.", "OK");
            }

            return Devices;
        }

        private async void Adapter_DeviceDiscovered(object sender, DeviceEventArgs e)
        {
            try
            {
                Devices.Add(e.Device);
                Debug.WriteLine($"Found {e.Device.State.ToString().ToLower()} {e.Device.Name}.");
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert($"{ex.Message}", $"{e.Device}.", "OK");
            }
        }

        private void Adapter_DeviceConnectionLost(object sender, DeviceErrorEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await Application.Current.MainPage.DisplayAlert($"{e.Device.Name} connection is lost.", $"{e.Device}.", "OK");
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert($"Device connection is lost.", $"{e.Device}.", "OK");
                }
            });
        }

        private void Adapter_DeviceConnected(object sender, DeviceEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await Application.Current.MainPage.DisplayAlert($"{e.Device.Name} is connected.", $"{e.Device}.", "OK");
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert($"Device is connected.", $"{e.Device}.", "OK");
                }
            });
        }

        private void Adapter_DeviceDisconnected(object sender, DeviceEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await Application.Current.MainPage.DisplayAlert($"{e.Device.Name} is disconnected.", $"{e.Device}.", "OK");
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert($"Device is disconnected.", $"{e.Device}.", "OK");
                }
            });
        }

        private void BluetoothLE_StateChanged(object sender, BluetoothStateChangedArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await Application.Current.MainPage.DisplayAlert($"Bluetooth state is {e.NewState}.", $"{e.NewState}.", "OK");
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert($"Bluetooth state has changed.", $"{e.NewState}.", "OK");
                }
            });
        }
    }
}

