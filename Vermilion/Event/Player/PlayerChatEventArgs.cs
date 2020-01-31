using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermilion.Event.Player
{
    public class PlayerChatEventArgs : PlayerEventArgs
    {

        //public static EventPublisher eventPublisher;
        public static event EventHandler<PlayerChatEventArgs> Event;

        public override bool OnCallEvent()
        {
            //return eventPublisher.OnEvent(this);
            Event?.Invoke(this, this);
            return !this.Cancel;
        }

        private string message { get; set; }
        private string format { get; set; }

        public PlayerChatEventArgs(EPlayer ePlayer, string message, string format = "chat.type.text")
        {
            this.player = ePlayer;
            this.message = message;
            this.format = format;
        }

        public string GetMessage()
        {
            return this.message;
        }

        public string GetFormat()
        {
            return this.format;
        }
    }
}
