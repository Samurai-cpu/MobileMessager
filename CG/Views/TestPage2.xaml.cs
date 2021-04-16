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
    public partial class TestPage2 : ContentPage
    {
        public TestPage2()
        {
            InitializeComponent();
        }

        private void SaveUser(object sender, EventArgs e)
        {
            var user = (User)BindingContext;
            if (!String.IsNullOrEmpty(user.UserName))
            {
                ContextProvider.Database.SaveItem(user);
            }
            this.Navigation.PopAsync();
        }
        private void DeleteUser(object sender, EventArgs e)
        {
            var user = (User)BindingContext;
            ContextProvider.Database.DeleteItem(user.Id);
            this.Navigation.PopAsync();
        }
        private void Cancel(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }
    }
}