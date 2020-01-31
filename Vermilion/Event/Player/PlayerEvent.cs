using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermilion.Event.Player
{
    public abstract class PlayerEventArgs : MainEventArgs
    {
        protected EPlayer player { get; set; }

        public EPlayer GetPlayer()
        {
            return player;
        }
    }
}
