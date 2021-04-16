using CG.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CG.ViewModels
{
    public class ChatPageViewModel : INotifyPropertyChanged
    {
        public static  ObservableCollection<Message> Messages { get; set; } = new ObservableCollection<Message>();
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }

        public ChatPageViewModel()
        {
            Messages.Add(new Message() { Text = "Hi",Time=new DateTime(2021,07,04,19,30,44) });
            Messages.Add(new Message() { Text = "How are you?",Time = new DateTime(2021, 07, 04, 19, 30, 46) });
            Messages.Add(new Message() { Text = "Failure", User = App.User, Time = new DateTime(2021, 07, 04, 19, 30, 48),Status=MessageStatus.MessageFailure });
            Messages.Add(new Message() { Text = "Unread", User = App.User, Time = new DateTime(2021, 07, 04, 19, 30, 48), Status = MessageStatus.Unread,ImagePath="Unread.png" });
            Messages.Add(new Message() { Text = "Read", User = App.User, Time = new DateTime(2021, 07, 04, 19, 30, 48), Status = MessageStatus.Read,ImagePath="Read.png" });

            OnSendCommand = new Command(() =>
            {
                if (!string.IsNullOrEmpty(TextToSend))
                {
                    Messages.Add(new Message() { Text = TextToSend, User = App.User });
                    TextToSend = string.Empty;
                }

            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
