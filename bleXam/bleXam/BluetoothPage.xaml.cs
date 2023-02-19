using System;
using System.Collections.Generic;

using Xamarin.Forms;
using bleXam.ViewModels;

namespace bleXam
{
    public partial class BluetoothPage : ContentPage
    {
        public BluetoothPage(BluetoothViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}

