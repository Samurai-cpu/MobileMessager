using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CG
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        

        private async void RegistrBtnClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistrPage());
        }

        private void PhoneValidate(object sender, EventArgs e)
        {
            string Phone =""+ PhoneEntry.Text;
            var PhonePattern = @"\+(9[976]\d|8[987530]\d|6[987]\d|5[90]\d|42\d|3[875]\d|2[98654321]\d|9[8543210]|8[6421]|6[6543210]|5[87654321]|4[987654310]|3[9643210]|2[70]|7|1)\d{1,14}$";
            if (Regex.IsMatch(Phone, PhonePattern))
            {
                ErrorLabel.Text = "Phone is Valid";
            }
            else
            {
                ErrorLabel.Text = "Phone is not Valid";
            }
        }
    }
}
