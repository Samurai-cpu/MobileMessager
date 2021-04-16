using CG.Models;
using CG.ViewModels;
using System;
using CG.Views;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CG.Views.Partials
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatInputBarView : ContentView
    {
        public ChatInputBarView()
        {
            InitializeComponent();
        }
        public void Handle_Completed(object sender, EventArgs e)
        {
            if (chatTextInput.Text != "")
            {
                Message msg = new Message
                {
                    User = App.User,
                    Text = chatTextInput.Text,
                    Time = DateTime.Now
                };
                ChatPageViewModel.Messages.Add(msg);
                chatTextInput.Text = "";
            }

        }

        public void UnFocusEntry()
        {
            chatTextInput?.Unfocus();
        }

        public void IncomingMsg(object sender, EventArgs e)
        {
            if (chatTextInput.Text != "")
            {
                Message msg = new Message
                {
                    User = "Xyi s gori",
                    Text = chatTextInput.Text,
                    Time = DateTime.Now
                };
                ChatPageViewModel.Messages.Add(msg);
                chatTextInput.Text = "";
            }

        }
    }
}