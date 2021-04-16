using System;
using System.Collections.Generic;
using System.Text;

namespace CG.Models
{
    public enum MessageStatus
    {
        MessageFailure,
        Unread,
        Read
    }
    public class Message
    {
        public string Text { get; set; }

        public string User { get; set; }

        public DateTime Time { get; set; }

        public MessageStatus Status { get; set; }

        public string ImagePath { get; set; }

        public Message()
        {
            if (this.Status == MessageStatus.MessageFailure)
            {
                ImagePath = "MFailure.png";
            }
            else if(this.Status == MessageStatus.Read)
            {
                ImagePath = "Read.png";
            }
            else
            {
                ImagePath = "Unread.png";
            }
        }
    }
}
