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
    public partial class ProfileSettingsPage : ContentPage
    {
        public ProfileSettingsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        protected override void OnAppearing()
        {
            usersList.ItemsSource = new List<Test>
            {
                new Test() { Title ="Email",Content="your.email@mail.ru"},
                new Test() { Title = "Phone", Content = "+380732166847" },
                new Test() { Title ="Login",Content="Username"},
                new Test() { Title ="Bio",Content="23 года дизайнер из Санкт-Петербурга"}
            };

            base.OnAppearing();
        }

    }

    public class Test
    {
        public string Title { get; set; }

        public string Content { get; set; }

    }
}