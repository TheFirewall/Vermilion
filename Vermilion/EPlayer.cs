using MiNET;
using MiNET.Net;
using MiNET.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vermilion.Event;
using Vermilion.Event.Player;

namespace Vermilion
{
    public class EPlayer : Player
    {

        public Vermilion heart;

        public EPlayer(MiNetServer server, IPEndPoint endPoint, Vermilion vermilion ) : base(server, endPoint)
        {
            heart = vermilion;
        }


        //[EventAttribute(Type = typeof(EventHandler<PlayerChatEventArgs>))]
        //[EventAttribute(Type = typeof(PlayerChatEventArgs))]
        public override void HandleMcpeText(McpeText message)
        {
            //base.HandleMcpeText(message);
            Stopwatch watch = new Stopwatch();
            watch.Start();
            if (message.type == (byte)McpeText.ChatTypes.Chat)
            {
                PlayerChatEventArgs chatEvent = new PlayerChatEventArgs(this, message.message, null);
                if (chatEvent.OnCallEvent())
                {
                    //Level.BroadcastMessage(chatEvent.GetMessage());
                    base.HandleMcpeText(message);
                }
            }
            watch.Stop();
            Console.WriteLine(message.GetType() + " в Тиках :" + watch.ElapsedTicks + "\r\n");
            Console.WriteLine(message.GetType() + " в MS :" + watch.ElapsedMilliseconds + "\r\n");
        }

        public override void HandleMcpeRespawn(McpeRespawn mcpeRespawn)
        {
            PlayerLocation oldSpawnPosition = SpawnPosition.Clone() as PlayerLocation;

            PlayerRespawnEventArgs respawnEvent = new PlayerRespawnEventArgs(this, null);
            respawnEvent.OnCallEvent();

            SpawnPosition = respawnEvent.GetRespawnLocation() ?? SpawnPosition;

            base.HandleMcpeRespawn(mcpeRespawn);

            SpawnPosition = oldSpawnPosition;
        }
    }
}
