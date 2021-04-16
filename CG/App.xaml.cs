using CG.Providers;
using CG.Services;
using CG.Views;
using System;
using System.IO;
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
            MainPage = new NavigationPage( new TestPage1());
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
