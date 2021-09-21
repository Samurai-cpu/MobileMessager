using CG.Providers.Base;
using CG.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CG
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhoneConfirmPage : ContentPage
    {
        public PhoneConfirmPage()
        {
            InitializeComponent();
        }



        private async void SMSEntry_Unfocused(object sender, FocusEventArgs e)
        {

            var auth = new AuthRequest();
            await auth.MakeAuthTokenAsync();
            await auth.CreateSessionAsync();
            var strongKey = ProviderFactory.StrongKeyProvider.GetItem();
        }
    }
}