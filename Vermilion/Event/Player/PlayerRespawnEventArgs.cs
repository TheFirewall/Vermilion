using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Utils;

namespace Vermilion.Event.Player
{
    public class PlayerRespawnEventArgs : PlayerEventArgs
    {

        //public static EventPublisher eventPublisher;
        public static event EventHandler<PlayerRespawnEventArgs> Event;

        public override bool OnCallEvent()
        {
            //return eventPublisher.OnEvent(this);
            Event?.Invoke(this, this);
            return !this.Cancel;
        }

        private PlayerLocation location  { get; set; }

        public PlayerRespawnEventArgs(EPlayer ePlayer, PlayerLocation location)
        {
            this.player = ePlayer;
            this.location = location;
        }

        public PlayerLocation GetRespawnLocation()
        {
            return this.location;
        }

        public PlayerLocation SetRespawnLocation(PlayerLocation location)
        {
            return this.location = location;
        }
    }
}
