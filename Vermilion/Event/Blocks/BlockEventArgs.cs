using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Blocks;

namespace Vermilion.Event.Blocks
{
    public abstract class BlockEventArgs : MainEventArgs
    {
        protected Block block { get; set; }

        public BlockEventArgs(Block block)
        {
            this.block = block;
        }

        public Block GetBlock()
        {
            return block;
        }
    }
}
