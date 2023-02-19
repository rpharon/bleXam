using System;
using bleXam.Services;
using Xamarin.Forms;
using bleXam.Models;
using System.Windows.Input;

namespace bleXam.ViewModels
{
	public class BluetoothViewModel : BaseViewModel
	{
        private readonly IBleService _bleService;

        public BluetoothViewModel(IBleService bleService)
		{
            _bleService = bleService;

            SendDataCommand = new Command(SendData);
        }

        public ICommand SendDataCommand { get; set; }

        private DeviceModel _device;
        public DeviceModel Device
        {
            get => _device;
            set => SetProperty(ref _device, value);
        }

        private string _data;
        public string Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        private async void SendData()
        {
            await Application.Current.MainPage.DisplayAlert("Send Data", string.Empty, "OK");
        }
	}
}


