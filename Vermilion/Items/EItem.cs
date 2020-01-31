using System;
using System.Collections.Generic;
using System.Text;
using MiNET.Items;

using MiNET.Utils;
using MiNET.Worlds;
using Vermilion.Event.Player;
using MiNET;
namespace Vermilion.Items
{
    public class EItem : Item
    {
		protected internal EItem(short id, short metadata = 0, int count = 1): base(id, metadata, count)
		{

		}
		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			base.UseItem(world, player, blockCoordinates);
			PlayerInteractEventArgs interactEvent = new PlayerInteractEventArgs((EPlayer)player, player.Inventory.GetItemInHand(), blockCoordinates, (BlockFace)0);
			if (interactEvent.OnCallEvent())
			{
				base.UseItem(world, player, blockCoordinates);
			}
		}
	}
	
}
