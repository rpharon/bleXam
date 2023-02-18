using System;
using Autofac;
using bleXam.Services;
using bleXam.ViewModels;
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
            builder.RegisterType<BleService>().As<IBleService>();
            builder.RegisterType<MainViewModel>();
            builder.RegisterType<MainPage>();

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
    }
}

