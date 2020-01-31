using System;
using System.Collections.Generic;
using System.Text;
using MiNET.Entities.Projectiles;
using MiNET.Utils;
using MiNET.Worlds;
using MiNET;

namespace Vermilion.Items
{
	public class EItemSnowball : EItem
	{
		public EItemSnowball() : base(332)
		{
			MaxStackSize = 16;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			float force = 1.5f;

			var snowBall = new Snowball(player, world);
			snowBall.KnownPosition = (PlayerLocation)player.KnownPosition.Clone();
			snowBall.KnownPosition.Y += 1.62f;
			snowBall.Velocity = snowBall.KnownPosition.GetDirection().Normalize() * force;
			snowBall.SpawnEntity();
		}
	}
}
