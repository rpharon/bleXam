using System;
using Autofac;
using bleXam.Services;
using bleXam.ViewModels;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bleXam
{
    public partial class App : Application
    {
        private static IContainer Container { get; set; }

        public App ()
        {
            InitializeComponent();
        }

        protected override void OnStart ()
        {
            var builder = new ContainerBuilder();

            RegisterServices(builder);
            RegisterViewModels(builder);
            RegisterPages(builder);

            Container = builder.Build();

            var mainPage = Container.Resolve<MainPage>();

            MainPage = new NavigationPage(mainPage);
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }

        private void RegisterServices(ContainerBuilder container)
        {
            container.RegisterType<BleService>().As<IBleService>();
        }

        private void RegisterViewModels(ContainerBuilder container)
        {
            container.RegisterType<MainViewModel>();
            container.RegisterType<BluetoothViewModel>();
        }

        private void RegisterPages(ContainerBuilder container)
        {
            container.RegisterType<MainPage>();
            container.RegisterType<BluetoothPage>();
        }
    }
}

