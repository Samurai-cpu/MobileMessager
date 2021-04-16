using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private void SMSEntry_Unfocused(object sender, EventArgs e)
        {


        }
    }
}