using CG.Dal;
using CG.Models;
using CG.Providers.Base;
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
    public partial class TestPage1 : ContentPage
    {
        public TestPage1()
        {
            InitializeComponent();
            usersList.HasUnevenRows = true;
        }
        protected override void OnAppearing()
        {
            usersList.ItemsSource = ContextProvider.Database.GetItems();
            base.OnAppearing();
        }
        // обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            User selectedFriend = (User)e.SelectedItem;
            TestPage2 friendPage = new TestPage2();
            friendPage.BindingContext = selectedFriend;
            await Navigation.PushAsync(friendPage);
        }
        // обработка нажатия кнопки добавления
        private async void CreateUser(object sender, EventArgs e)
        {
            User friend = new User();
            TestPage2 friendPage = new TestPage2();
            friendPage.BindingContext = friend;
            await Navigation.PushAsync(friendPage);
        }
    }
}