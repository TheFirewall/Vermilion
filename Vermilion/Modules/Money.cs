using System;
using System.Collections.Generic;
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
    public class PocketMoney : IModule
    {

        private Vermilion heart { get; set; }

        public void OnEnable(PluginContext context, Vermilion heart)
        {
            this.heart = heart;
            context.PluginManager.LoadCommands(this);
            heart.GetDBManager().GetDB().Session.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS MONEYS(" +
   "Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ," +
   "User TEXT NOT NULL," +
   "PM INTEGER NOT NULL DEFAULT '0'); ");
        }

        public void OnDisable()
        {

        }

        public void updateMoney(int money, string user)
        {
            heart.GetDBManager().GetDB().Update<Money>(a => a.User == user, a => new Money { PM = money });
        }

        public void plusMoney(int money, string user)
        {
            heart.GetDBManager().GetDB().Update<Money>(a => a.User == user, a => new Money { PM = a.PM + money });
        }

        public void minusMoney(int money, string user)
        {
            heart.GetDBManager().GetDB().Update<Money>(a => a.User == user, a => new Money { PM = a.PM - money });
        }

        public int checkMoney(string user, bool create = true)
        {
            string low_name = user.ToLowerInvariant();
            Money result = heart.GetDBManager().GetDB().Query<Money>().Where(a => a.User == low_name).FirstOrDefault();
            if (result != null)
            {
                return result.PM;
            }
            else
            {
                if (create)
                {
                    Money eco = new Money { User = low_name, PM = 50 };
                    heart.GetDBManager().GetDB().Insert(eco);
                }
                return 0;
            }
        }

        public enum money2Command
        {
            pay = 1,
            set = 2,
            add = 3,
            view = 4,
            top = 5
        }

        [Command]
        public void money(Player player) => player.SendMessage("Ваш баланс@c " + this.checkMoney(player.Username) + " @wPM");

        [Command]
        public void money(Player player, money2Command команда) => money(player, команда, new Target());

        [Command]
        public void money(Player player, money2Command command, string username) => money(player, command, username, 0);

        [Command]
        public void money(Player player, money2Command command, Target target)
        {
            if(target.Players.Count() != 0) money(player, command, target.Players.FirstOrDefault().Username, 0);
        }

        [Command]
        public void money(Player player, money2Command command, Target target, int count)
        {
            if (target.Players.Count() != 0) money(player, command, target.Players.FirstOrDefault().Username, count);
        }

        [Command]
        public void money(Player player, money2Command command, string username, int count)
        {
            string low_name = player.Username.ToLowerInvariant();
            switch (command)
            {
                case money2Command.pay:
                    payCommand(player, username, count);
                    break;
                case money2Command.view:
                    viewCommand(player, username);
                    break;
                case money2Command.top:
                    topCommand(player);
                    break;
            }
        }

        public void payCommand(Player player, string username, int count)
        {
            string low_name = player.Username.ToLowerInvariant();
            if (username == null || count <= 0)
            {
                player.SendMessage("Передать игроку вот так /money pay 'ник' кол-во");
                return;
            }
            string targer_low_name = username.ToLowerInvariant();
            if (player.Username.ToLowerInvariant() == low_name)
            {
                player.SendMessage("Нельзя заплатить себе!");
                return;
            }

            Money target = heart.GetDBManager().GetDB().Query<Money>().Where(a => a.User == targer_low_name).FirstOrDefault();
            Money payer = heart.GetDBManager().GetDB().Query<Money>().Where(a => a.User == low_name).FirstOrDefault();
            if (target == null)
            {
                player.SendMessage("Игрока не существует");
                return;
            }

            if (count > payer.PM)
            {
                return;
            }

            minusMoney(count, low_name);
            plusMoney(count, targer_low_name);

            player.SendMessage("[✔] Деньги переданы");
            /*    $player2 = $this->getServer()->getPlayer($target);
            if ($player2 instanceof Player){
                    $this->Engine->sendInfo($player2, "Игрок@p $payer передал вам $amount @wPM");
                break;
            }*/
        }

        public void viewCommand(Player player, string username)
        {
            if (username == null)
            {
                player.SendMessage("Передать игроку вот так /money view 'ник'");
                return;
            }
            string targer_low_name = username.ToLowerInvariant();

            Money target = heart.GetDBManager().GetDB().Query<Money>().Where(a => a.User == targer_low_name).FirstOrDefault();
            if (target == null)
            {
                player.SendMessage("Игрока не существует");
                return;
            }

            player.SendMessage("Баланс игрока " + target.PM +" составляет@c $money @wPM");
        }

        public void topCommand(Player player)
        {
            var to = heart.GetDBManager().GetDB().Query<Money>().OrderByDesc(a => a.PM).Take(5).ToList();
            player.SendMessage("Топ-5 игроков");
            player.SendMessage("-* ======= *-");
            int i = 1;
            foreach (var t in to)
            {
                player.SendMessage("№ " + i + " | Ник '"+ t.User +"' | Денег '"+ t.PM +"'");
                i++;
            }
            player.SendMessage("-* ======= *-");
        }
    }

    [Table("MONEYS")]
    public class Money
    {
        [Column(IsPrimaryKey = true)]
        [AutoIncrement]
        public int Id { get; set; }
        public string User { get ; set; }
        public int PM { get; set; }
    }
}
