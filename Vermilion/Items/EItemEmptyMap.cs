using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using MiNET.Entities.World;
using MiNET.Utils;
using MiNET.Worlds;
using MiNET;
using MiNET.Items;

namespace Vermilion.Items
{
	public class EItemEmptyMap : EItem
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(EItemEmptyMap));

		public EItemEmptyMap(short metadata = 0, byte count = 1) : base(395, metadata, count)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			MapEntity mapEntity = new MapEntity(world);
			mapEntity.SpawnEntity();

			// Initialize a new map and add it.
			ItemMap itemMap = new ItemMap(mapEntity.EntityId);
			player.Inventory.SetFirstEmptySlot(itemMap, true);
		}
	}
}
