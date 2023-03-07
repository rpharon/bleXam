using System;
using bleXam.Services;
using Xamarin.Forms;
using bleXam.Models;
using System.Windows.Input;
using Plugin.BLE.Abstractions.Contracts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bleXam.ViewModels
{
	public class BluetoothViewModel : BaseViewModel
	{
        private readonly IBleService _bleService;

        public BluetoothViewModel(IBleService bleService)
		{
            _bleService = bleService;

            SendDataCommand = new Command(SendData);
            SendDoneCommand = new Command(SendDone);
            SendTestCommand = new Command(SendTest);
            SendErrorCommand = new Command(SendError);
        }

        public ICommand SendDataCommand { get; set; }
        public ICommand SendDoneCommand { get; set; }
        public ICommand SendTestCommand { get; set; }
        public ICommand SendErrorCommand { get; set; }

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

        private async Task Write(string data)
        {
            try
            {
                if (_bleService.Device.State == Plugin.BLE.Abstractions.DeviceState.Connected)
                {
                    var guid = new Guid(_bleService.Device.Id.ToString());

                    _bleService.Service = await _bleService.Device.GetServiceAsync(new Guid("0783b03e-8535-b5a0-7140-a304d2495cb7")); //This Id is specific from the rs232 BLE of Gilgen
                    _bleService.Characteristic = await _bleService.Service.GetCharacteristicAsync(new Guid("0783b03e-8535-b5a0-7140-a304d2495cba")); //This Id is specific from the rs232 BLE of Gilgen

                    await _bleService.Characteristic.WriteAsync(Encoding.ASCII.GetBytes(data));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Send Data", "You are not connected.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Send Data", ex.Message, "OK");
            }
        }

        private async void SendData()
        {
            await Write(Data);
        }

        private async void SendDone()
        {
            await Write("Done_M1U");
        }

        private async void SendTest()
        {
            await Write("Test");
        }

        private async void SendError()
        {
            await Write("Error_M1U");
        }
	}
}


