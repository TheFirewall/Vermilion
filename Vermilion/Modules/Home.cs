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
using Vermilion.Event;
using Vermilion.Event.Player;

namespace Vermilion.Modules
{
    public class PocketHome : IModule
    {

        private Vermilion heart { get; set; }

        public void OnEnable(PluginContext context, Vermilion heart)
        {
            this.heart = heart;
            context.PluginManager.LoadCommands(this);
            //heart.GetDBManager().GetDB().Session.ExecuteNonQuery("CREATE UNIQUE INDEX Users ON HOMES(User);");
        }

        public void OnDisable()
        {
           
        }

        [Command(Description = "Установка точка дома")]
        public void sethome(Player player)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            string low_name = player.Username.ToLowerInvariant();
            var coord = player.KnownPosition.GetCoordinates3D();

            //Home result = heart.GetDBManager().GetDB().Query<Home>().Where(a => a.User == low_name).FirstOrDefault();
            Home result = heart.GetDBManager().GetDB().SqlQuery<Home>("SELECT HomeSpawn FROM HOMES WHERE User = '" + low_name + "';").FirstOrDefault();
            if (result != null)
            {
                //home.HomeSpawn = result.HomeSpawn;
                //heart.GetDBManager().GetDB().Update(home);
                //heart.GetDBManager().Update("UPDATE HOMES SET PosX = '"+ coord.X + "', PosY = '"+ coord.Y + "', PosZ = '"+ coord.Z +"', Level = '"+ player.Level.LevelId + "' WHERE User = '"+ low_name +"';", true);
                heart.GetDBManager().GetDB().Session.ExecuteNonQuery("UPDATE HOMES SET PosX = '" + coord.X + "', PosY = '" + coord.Y + "', PosZ = '" + coord.Z + "', Level = '" + player.Level.LevelId + "' WHERE User = '" + low_name + "';");
                player.SendMessage("Точка дома установлена1");
            }
            else
            {
                Home home = new Home()
                {
                    User = low_name,
                    PosX = coord.X,
                    PosY = coord.Y,
                    PosZ = coord.Z,
                    HomeSpawn = true,
                    Level = player.Level.LevelId
                };
                heart.GetDBManager().GetDB().Insert(home);
                player.SendMessage("Точка дома установлена2");
            }
           // player.SendMessage("Точка дома установлена");
            watch.Stop();
            Console.WriteLine(" в Тиках :" + watch.ElapsedTicks + "\r\n");
            Console.WriteLine(" в MS :" + watch.ElapsedMilliseconds + "\r\n");
        }

        [Command(Description = "Удаление точка дома")]
        public void delhome(Player player)
        {
            string low_name = player.Username.ToLowerInvariant();
            //Home result = heart.GetDBManager().GetDB().Query<Home>().Where(a => a.User == low_name).FirstOrDefault();
            Home result = heart.GetDBManager().GetDB().SqlQuery<Home>("SELECT User FROM HOMES WHERE User = '" + low_name + "';").FirstOrDefault();
            if (result != null)
            {
                heart.GetDBManager().GetDB().Delete(result);
                player.SendMessage("Вы удалили точку дома");
            }
            else
            {
                player.SendMessage("У вас нет дома");
            }
        }

        [Command(Description = "Телепорт на точку дома")]
        public void home(Player player)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            string low_name = player.Username.ToLowerInvariant();
            Home result = heart.GetDBManager().GetDB().SqlQuery<Home>("SELECT * FROM HOMES WHERE User = '" + low_name + "';").FirstOrDefault();
            //Home result = heart.GetDBManager().GetDB().Query<Home>().Where(a => a.User == low_name).FirstOrDefault();
            //var v = heart.GetDBManager().Query("SELECT * FROM HOMES WHERE User = '"+ low_name + "';");

            //Home result = null;
            if (result != null)
            {
                player.Teleport(new PlayerLocation(result.PosX, result.PosY, result.PosZ));
                player.SendMessage("Вы телепортировались домой");
            }
            else
            {
                player.SendMessage("У вас нет дома");
            }
            watch.Stop();
            Console.WriteLine( " в Тиках :" + watch.ElapsedTicks + "\r\n");
            Console.WriteLine( " в MS :" + watch.ElapsedMilliseconds + "\r\n");
        }

        [Command(Description = "Админский телепорт")]
        public void tphome(Player player, string username)
        {
            //добавить чек пермов
            string low_name = username.ToLowerInvariant();
            Home result = heart.GetDBManager().GetDB().Query<Home>().Where(a => a.User == low_name).FirstOrDefault();
            if (result != null)
            {
                player.Teleport(new PlayerLocation(result.PosX, result.PosY, result.PosZ));
                player.SendMessage("Вы телепортировались домой к игроку "+ username);
            }
            else
            {
                player.SendMessage("У вас нет дома");
            }
        }

        [Command]
        public void homespawn(Player player)
        {
            string low_name = player.Username.ToLowerInvariant();
            Home result = heart.GetDBManager().GetDB().Query<Home>().Where(a => a.User == low_name).FirstOrDefault();
            if (result != null)
            {
                if (result.HomeSpawn)
                {
                    heart.GetDBManager().GetDB().Update<Home>(a => a.User == low_name, a => new Home() { HomeSpawn = false });
                    player.SendMessage("Теперь при входе и смерти вы будете оставаться на месте!");
                }
                else
                {
                    heart.GetDBManager().GetDB().Update<Home>(a => a.User == low_name, a => new Home() { HomeSpawn = true });
                    player.SendMessage("Теперь при входе и смерти вы будете появлятся дома!");
                }
            }
            else
            {
                player.SendMessage("У вас нет дома");
            }
        }

        [EventAttribute(Type = typeof(PlayerRespawnEventArgs))]
        public void PlayerRespawn(object sender, PlayerRespawnEventArgs e){
            var ev = e as PlayerRespawnEventArgs;
            string low_name = ev.GetPlayer().Username.ToLowerInvariant();
            Home result = heart.GetDBManager().GetDB().Query<Home>().Where(a => a.User == low_name).FirstOrDefault();
            if (result != null){
                if (result.HomeSpawn){
				    ev.SetRespawnLocation(new PlayerLocation(result.PosX, result.PosY, result.PosZ));
			    }
            }
        }

        [EventAttribute(Type = typeof(PlayerChatEventArgs))]
        public void PlayerChat(object sender, PlayerChatEventArgs e)
        {
            var ev = e as MainEventArgs;
            e.GetPlayer().SendMessage("lol");
        }
    }

    [Table("HOMES")]
    public class Home
    {
        [Column(IsPrimaryKey = true)]
        public string User { get ; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int PosZ { get; set; }
        public string Level { get; set; }
        public bool HomeSpawn { get; set; }
    }
}
