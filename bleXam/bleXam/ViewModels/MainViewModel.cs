using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using bleXam.Services;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;
using bleXam.Models;
using System.Collections.Specialized;

//TODO: Only iOS was configured for accessing the bluetooth LE features
namespace bleXam.ViewModels
{
	public class MainViewModel : BaseViewModel
    {
        readonly IBleService _bleService;

        public MainViewModel(IBleService bleService)
		{
            _bleService = bleService;

            ScanDevicesCommand = new Command(ScanDevices);
            CheckBluetoothAvailabilityCommand = new Command(CheckBluetoothAvailability);

            Devices = new ObservableCollection<DeviceModel>();
        }

        public ICommand ScanDevicesCommand { get; set; }
        public ICommand CheckBluetoothAvailabilityCommand { get; set; }

        private ObservableCollection<DeviceModel> _devices;
        public ObservableCollection<DeviceModel> Devices
        {
            get => _devices;
            set => SetProperty(ref _devices, value);
        }

        private bool _isScanning;
        public bool IsScanning
        {
            get => _isScanning;
            set => SetProperty(ref _isScanning, value);
        }

        private async void ScanDevices()
        {
            if (IsScanning)
            {
                return;
            }

            if (!_bleService.BluetoothLE.IsAvailable)
            {
                await Application.Current.MainPage.DisplayAlert($"Bluetooth", $"Bluetooth is missing.", "OK");
                return;
            }

            try
            {
                if (!_bleService.BluetoothLE.IsOn)
                {
                    await Application.Current.MainPage.DisplayAlert($"Bluetooth is not on", $"Please turn Bluetooth on and try again.", "OK");
                    return;
                }

                IsScanning = true;

                List<DeviceModel> deviceCandidates = await _bleService.ScanForDevicesAsync();

                if (deviceCandidates.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert($"Unable to find nearby Bluetooth LE devices. Try again.", "Scan", "OK");
                }
                else
                {
                    Devices = new ObservableCollection<DeviceModel>(deviceCandidates.Select(x => new DeviceModel() { Id = x.Id, Name = x.Name }));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert($"Unable to get nearby Bluetooth LE devices", $"{ex.Message}.", "OK");
            }
            finally
            {
                IsScanning = false;
            }
        }

        async void CheckBluetoothAvailability()
        {
            if (IsScanning)
            {
                return;
            }

            try
            {
                if (!_bleService.BluetoothLE.IsAvailable)
                {
                    await Application.Current.MainPage.DisplayAlert($"Bluetooth", $"Bluetooth is missing.", "OK");
                    return;
                }

                if (_bleService.BluetoothLE.IsOn)
                {
                    await Application.Current.MainPage.DisplayAlert($"Bluetooth is on", $"You are good to go.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert($"Bluetooth is not on", $"Please turn Bluetooth on and try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert($"Unable to check Bluetooth availability", $"{ex.Message}.", "OK");
            }
        }
	}
}

