using System;
using System.Collections.Generic;
using System.Text;
using MiNET.Entities.Projectiles;
using MiNET.Utils;
using MiNET.Worlds;
using MiNET;

namespace Vermilion.Items
{
	public class EItemEgg : EItem
	{
		public EItemEgg() : base(344)
		{
			MaxStackSize = 16;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			float force = 1.5f;

			var egg = new Egg(player, world);
			egg.KnownPosition = (PlayerLocation)player.KnownPosition.Clone();
			egg.KnownPosition.Y += 1.62f;
			egg.Velocity = egg.KnownPosition.GetDirection().Normalize() * (force);
			egg.SpawnEntity();
		}
	}
}