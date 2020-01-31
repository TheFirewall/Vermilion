using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe;
using Chloe.Annotations;
using MiNET;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Worlds;
using Vermilion.Event;
using Vermilion.Event.Player;

namespace Vermilion.Modules
{
    public class WorldGuard : IModule
    {

        private Vermilion heart { get; set; }

        private Dictionary<string, BlockCoordinates> pos1 { get; set; } = new Dictionary<string, BlockCoordinates>();
        private Dictionary<string, BlockCoordinates> pos2 { get; set; } = new Dictionary<string, BlockCoordinates>();

        public void OnEnable(PluginContext context, Vermilion heart)
        {
            this.heart = heart;
            context.PluginManager.LoadCommands(this);
            //heart.GetDBManager().GetDB().Session.ExecuteNonQuery("CREATE UNIQUE INDEX Users ON HOMES(User);");
            heart.GetContext().Server.LevelManager.LevelCreated += (sender, args) =>
            {
                Level level = args.Level;
                //level.AllowBuild = false;
                //level.AllowBreak = false;

                level.BlockBreak += OnBreakEvent;
                level.BlockPlace += OnPlaceEvent;
            };

        }

        public void OnDisable()
        {

        }

        public void OnBreakEvent(object sender, BlockBreakEventArgs e)
        {
            if (e.Player.Inventory.GetItemInHand().Id == 271) {
                pos1.Add(e.Player.Username, new BlockCoordinates(e.Block.Coordinates.X, e.Block.Coordinates.Y, e.Block.Coordinates.Z));
                //$this->Engine->sendInfo($event->getPlayer(), 'Точка 1 установлена ('.$x.', '.$y.', '.$z.')', 1);
                e.Cancel = true;
            } else {
                if (!getRegion(e.Player.Username, e.Block.Coordinates.X, e.Block.Coordinates.Y, e.Block.Coordinates.Z, e.Level.LevelName, "Break")) {
                    //$this->Engine->sendInfo($event->getPlayer(), 'Вы не можете ломать блоки на этой территории', 2);
                    e.Cancel = true;
                }
            }
        }

        public void OnPlaceEvent(object sender, BlockPlaceEventArgs e)
        {
            if (e.Player.Inventory.GetItemInHand().Id == 271)
            {
                pos1.Add(e.Player.Username, new BlockCoordinates(e.TargetBlock.Coordinates.X, e.TargetBlock.Coordinates.Y, e.TargetBlock.Coordinates.Z));
                //$this->Engine->sendInfo($event->getPlayer(), 'Точка 1 установлена ('.$x.', '.$y.', '.$z.')', 1);
                e.Cancel = true;
            }
            else
            {
                if (!getRegion(e.Player.Username, e.TargetBlock.Coordinates.X, e.TargetBlock.Coordinates.Y, e.TargetBlock.Coordinates.Z, e.Level.LevelName, "Place"))
                {
                    //$this->Engine->sendInfo($event->getPlayer(), 'Вы не можете ломать блоки на этой территории', 2);
                    e.Cancel = true;
                }
            }
        }

        [EventAttribute(Type = typeof(PlayerInteractEventArgs))]
        public void PlayerInteract(object sender, PlayerInteractEventArgs e)
        {

        }

        public bool getRegion(string username, int x, int y, int z, string level, string flag = "", bool ignor = false)
        {
            //var result = heart.GetDBManager().GetDB().Query<Region>().Where(rg => (rg.Pos1X <= x && x <= rg.Pos2X) && (rg.Pos1Y <= y && y <= rg.Pos2Y) && (rg.Pos1Z <= z && z <= rg.Pos2Z) && rg.flag ).FirstOrDefault();
             if(flag != "" && !ignor) flag = $" AND {flag} = 1";
            Region result = heart.GetDBManager().GetDB().SqlQuery<Region>($"SELECT * FROM AREAS WHERE (Pos1X <= {x} AND {x} <= Pos2X) AND (Pos1Y <= {y} AND {y} <= Pos2Y) AND (Pos1Z <= {z} AND {z} <= Pos2Z) {flag} AND Level = {level}  ORDER BY Priority DESC LIMIT 1;").FirstOrDefault();
            if (result == null) return false;
            if(getMember(result.Name, username) || result.Owner == username) return true;
            return false;
        }

        public bool getRegionY(string username, int x, int z, string level, string flag = "", bool ignor = false)
        {
            //var result = heart.GetDBManager().GetDB().Query<Region>().Where(rg => (rg.Pos1X <= x && x <= rg.Pos2X) && (rg.Pos1Y <= y && y <= rg.Pos2Y) && (rg.Pos1Z <= z && z <= rg.Pos2Z) && rg.flag ).FirstOrDefault();
            if (flag != "" && !ignor) flag = $" AND {flag} = 1";
            Region result = heart.GetDBManager().GetDB().SqlQuery<Region>($"SELECT * FROM AREAS WHERE (Pos1X <= {x} AND {x} <= Pos2X) AND (Pos1Z <= {z} AND {z} <= Pos2Z) {flag} AND Level = {level}  ORDER BY Priority DESC LIMIT 1;").FirstOrDefault();
            if (result == null) return false;
            if (getMember(result.Name, username) || result.Owner == username) return true;
            return false;
        }

        public bool getMember(string name, string username)
        {
            var result = heart.GetDBManager().GetDB().Query<Member>().Where(rg => rg.Name == name && rg.User == username).FirstOrDefault();
            if (result != null) return true;
            return false;
        }

        public bool getOwner(string name, string username)
        {
            var result = heart.GetDBManager().GetDB().Query<Member>().Where(rg => rg.Name == name && rg.User == username && rg.Owner).FirstOrDefault();
            if (result != null) return true;
            return false;
        }
    }

    [Table("AREAS")]
    public class Region
    {
        [Column(IsPrimaryKey = true)]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public int Pos1X { get; set; }
        public int Pos1Y { get; set; }
        public int Pos1Z { get; set; }
        public int Pos2X { get; set; }
        public int Pos2Y { get; set; }
        public int Pos2Z { get; set; }
        public string Level { get; set; }
        public string Mother { get; set; }
        public string MainMother { get; set; }
        public string Priority { get; set; }
        public string PvP { get; set; }
        public string Access { get; set; }
        public string Door { get; set; }
        public string Break { get; set; }
        public string Place { get; set; }
    }

    [Table("MEMBERS")]
    public class Member
    {
        [Column(IsPrimaryKey = true)]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public bool Owner { get; set; }
    }
}
