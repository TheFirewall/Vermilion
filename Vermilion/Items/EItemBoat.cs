using System;
using System.Collections.Generic;
using System.Text;
using MiNET.Items;
using System.Numerics;
using log4net;
using MiNET.Entities.Vehicles;
using MiNET.Utils;
using MiNET.Worlds;
using MiNET;
namespace Vermilion.Items
{
    public class EItemBoat : EItem
    {
        public EItemBoat(short metadata) : base(333, metadata)
        {

        }

		public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);

			Boat entity = new Boat(world);
			entity.KnownPosition = coordinates;
			entity.SpawnEntity();

			if (player.GameMode == GameMode.Survival)
			{
				var itemInHand = player.Inventory.GetItemInHand();
				itemInHand.Count--;
				player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
			}
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			
			base.UseItem(world, player, blockCoordinates);
		}
	}
}
