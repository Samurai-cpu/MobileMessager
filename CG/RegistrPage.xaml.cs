using CG.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CG.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CG.Models;
using CG.Dal;
using CG.Providers.Base;

namespace CG
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrPage : ContentPage
    {
        public RegistrPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }


        private async void BackBtnClick(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void PhoneEntry_Completed(object sender, EventArgs e)
        {
            string Phone =""+ PhoneEntry.Text;
            var PhonePattern = @"\+(9[976]\d|8[987530]\d|6[987]\d|5[90]\d|42\d|3[875]\d|2[98654321]\d|9[8543210]|8[6421]|6[6543210]|5[87654321]|4[987654310]|3[9643210]|2[70]|7|1)\d{1,14}$";
            if (Regex.IsMatch(Phone, PhonePattern))
            {
                PhoneLabel.Text = "Phone is Valid";
            }
            else
            {
                PhoneLabel.Text = "Phone is not Valid";
            }

        }

        private void EmailEntry_Completed(object sender, EventArgs e)
        {
            string Email =""+ EmailEntry.Text;
            var EmailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            if (Regex.IsMatch(Email, EmailPattern))
            {
                EmailLabel.Text = "Email is Valid";
            }
            else
            {
                EmailLabel.Text = "Email is not Valid";
            }
        }

        private void LoginEntry_Completed(object sender, EventArgs e)
        {
            string Login =""+ LoginEntry.Text;
            var LoginPattern = @"(?s)^([^a-zA-Z]*[A-Za-z]){4}.*";
            if (Regex.IsMatch(Login, LoginPattern))
            {
                LoginLabel.Text = "Login is Valid";
            }
            else
            {
                LoginLabel.Text = "Login is not Valid";
            }
        }

        private void PasswordEntry_Completed(object sender, EventArgs e)
        {
            string Password = "" + PasswordEntry.Text;
            var PassPatern = "^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*?[#?!@$%^&*-+=]).{6,20}$";
            if (Regex.IsMatch(Password, PassPatern))
            {
                PasswordLabel.Text = "Password is Valid";
            }
            else
            {
                PasswordLabel.Text = "Password is not Valid";
            }
        }

        private async void  RegBtnClick(object sender, EventArgs e)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("email", EmailEntry.Text);
            dict.Add("userName", LoginEntry.Text);
            dict.Add("password", PasswordEntry.Text);
            dict.Add("phone", PhoneEntry.Text);
            var auth = new AuthRequest();
            var  result = await auth.MakeFormHttpRequestAsync<AuthResult>("Account/signup", dict);

            User newUser = new User
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                UserName = result.UserName,
                Password = LoginEntry.Text,
                Email = EmailEntry.Text,
                Phone = PhoneEntry.Text,
            };
            if (true)
            {
                ContextProvider.Database.SaveItem(newUser);
            }
            var users= ContextProvider.Database.GetItems();
            users.ToList();
        }
    }
}