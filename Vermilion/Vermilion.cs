using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermilion.Event;
using Vermilion.Modules;
using Chloe.SQLite;
using MiNET;
using System.Net;
using MiNET.Items;
using MiNET.Plugins.Attributes;
using Vermilion.Items;

namespace Vermilion
{
    [Plugin(PluginName = "Macross Core", PluginVersion = "0.29")]
    public class Vermilion : Plugin
    {
        private EventManager eventManager { get; set; }

        public List<IModule> Modules { get; set; }

        private DBManager dBManager { get; set; }


        protected override void OnEnable()
        {

            Context.Server.PlayerFactory = new xPlayerFactory();
            ItemFactory.CustomItemFactory = new EItemFactory();
            ((xPlayerFactory)Context.Server.PlayerFactory).heart = this;
            eventManager = new EventManager();

            dBManager = new DBManager();
            
            dBManager.SetDB(new SQLiteContext(new SQLiteConnectionFactory("Data Source=Vermilion.db;")));

            Modules = new List<IModule>();
            Modules.Add(new TestModule());
            Modules.Add(new PocketHome());
            Modules.Add(new PocketMoney());
            foreach (var m in Modules)
            {
                m.OnEnable(Context, this);
                eventManager.LoadModuleEvents(m);
            }
        }

        public PluginContext GetContext()
        {
            return Context;
        }

        /*private void LevelOnBlockBreak(object sender, BlockBreakEventArgs e)
        {

            BlockBreakEventArgs chatEvent = new BlockBreakEventArgs(e.Player, e.Block, );
            if (!chatEvent.OnCallEvent())
            {
                e.Cancel = true;
            }

        }

        private void LevelOnBlockPlace(object sender, BlockPlaceEventArgs e)
        {
            if (e.ExistingBlock.Coordinates.DistanceTo((BlockCoordinates)e.Player.SpawnPosition) < 15)
            {
                e.Cancel = e.Player.GameMode != GameMode.Creative;
            }
        }*/

        public DBManager GetDBManager()
        {
            return dBManager;
        }
    }

    public class xPlayerFactory : PlayerFactory
    {

        public Vermilion heart { get; set; }

        public override Player CreatePlayer(MiNetServer server, IPEndPoint endPoint, PlayerInfo pd)
        {
            var player = new EPlayer(server, endPoint, heart);
            player.MaxViewDistance = 7;
            player.UseCreativeInventory = false;
            return player;
        }
    }
}
