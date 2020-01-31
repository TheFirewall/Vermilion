using MiNET.Blocks;
using MiNET.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermilion.Event.Blocks
{
    public class BlockBreakEventArgs : BlockEventArgs
    {
        protected EPlayer player { get; set; }

        protected Item item { get; set; }

        protected bool instaBreak { get; set; } = false;
	
	    protected List<Item> blockDrops { get; set; } = new List<Item>();
	
	    protected int xpDrops { get; set; }

        public BlockBreakEventArgs(EPlayer player, Block block, Item item, bool instaBreak, List<Item> drops, int xpDrops = 0) : base(block)
        {
            this.item = item;
            this.player = player;
            this.instaBreak = instaBreak;
            this.blockDrops = player.GameMode == MiNET.Worlds.GameMode.Survival ? block.GetDrops(item).ToList() : new List<Item>();
            this.xpDrops = xpDrops;
        }


        public EPlayer getPlayer()
        {
            return player;
        }

        public Item getItem()
        {
            return item;
        }

        public bool getInstaBreak()
        {
            return this.instaBreak;
        }

        public List<Item> getDrops()
        {
            return blockDrops;
        }

        public void setDrops(List<Item> drops)
        {
            this.blockDrops = drops;
        }

        public void setInstaBreak(bool instaBreak)
        {
            this.instaBreak = instaBreak;
        }

        public int getXpDropAmount(){
		    return xpDrops;
	    }

        public void setXpDropAmount(int amount){
		    if(amount < 0){
			    throw new InvalidCastException("Amount must be at least zero");
            }

		    xpDrops = amount;
        }
    }
}
