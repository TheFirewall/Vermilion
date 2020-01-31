using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Plugins;
using Vermilion.Event;
using Vermilion.Event.Player;

namespace Vermilion.Modules
{
    public class TestModule : IModule
    {
        public void OnEnable(PluginContext context, Vermilion vermilion)
        {
            
        }

        public void OnDisable()
        {

        }

        [EventAttribute(Type = typeof(PlayerChatEventArgs))]
        public void PlayerChat(object sender, PlayerChatEventArgs e){
            //var ev = e as PlayerChatEventArgs;
            //e.GetPlayer().SendMessage("lol");
        }
    }
}
