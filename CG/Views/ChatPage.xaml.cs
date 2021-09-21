using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CG.Providers;
using CG.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        private readonly IUserProvider userRepository;

        public ChatPage(IUserProvider userRepository)
        {
            InitializeComponent();
            this.userRepository = userRepository;
        }

        protected override void OnAppearing()
        {
            var users = userRepository.GetItems();
            new ChatPageViewModel();
            Messages.ItemsSource = ChatPageViewModel.Messages;
        }
    }
}