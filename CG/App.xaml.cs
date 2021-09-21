using CG.Providers;
using CG.Services;
using CG.Views;
using System;
using System.IO;
using System.Security.Cryptography;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CG
{
    public partial class App : Application
    {

        public static string User = "Ilya";
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new PhoneConfirmPage());
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
