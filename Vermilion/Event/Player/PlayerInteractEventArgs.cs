using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MiNET;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Utils;

namespace Vermilion.Event.Player
{
    public class PlayerInteractEventArgs : PlayerEventArgs
    {

        public enum Action
        {
            LEFT_CLICK_BLOCK,
            RIGHT_CLICK_BLOCK,
            LEFT_CLICK_AIR,
            RIGHT_CLICK_AIR,
            PHYSICAL
        }

        //public static EventPublisher eventPublisher;
        public static event EventHandler<PlayerInteractEventArgs> Event;

        public override bool OnCallEvent()
        {
            //return eventPublisher.OnEvent(this);
            Event?.Invoke(this, this);
            return !this.Cancel;
        }

        private Block blockTouched;

        private BlockCoordinates touchVector;

        private BlockFace blockFace;

        private Item item;

        private Action action;

        public PlayerInteractEventArgs(EPlayer ePlayer, Item item, object block, BlockFace face, Action action = Action.RIGHT_CLICK_BLOCK)
        {
            if (block is Block) {
                this.blockTouched = (Block)block;
                this.touchVector = new Vector3(0, 0, 0);
            } else {
                this.touchVector = (BlockCoordinates)block;
                this.blockTouched = BlockFactory.GetBlockById(0);
                this.blockTouched.Coordinates = new BlockCoordinates(0, 0, 0);
            }

            this.player = player;
            this.item = item;
            this.blockFace = face;
            this.action = action;
        }

        public Action getAction()
        {
            return action;
        }

        public Item getItem()
        {
            return item;
        }

        public Block getBlock()
        {
            return blockTouched;
        }

        public BlockCoordinates getTouchVector()
        {
            return touchVector;
        }

        public BlockFace getFace()
        {
            return blockFace;
        }
    }
}
