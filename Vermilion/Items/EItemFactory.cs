using System;
using System.Collections.Generic;
using System.Text;
using MiNET.Items;
namespace Vermilion.Items
{
    public class EItemFactory : ICustomItemFactory
    {
		public Item GetItem(short id, short metadata = 0, int count = 1)
		{
			Item item = null;

			if (id == 280) item = new ItemStick();
			else if (id == 332) item = new ItemSnowball();
			else if (id == 333) item = new ItemBoat(metadata);
			else if (id == 340) item = new ItemBook();
			else if (id == 344) item = new ItemEgg();
			
			else if (id == 401) item = new ItemFireworks();
			
			item.Metadata = metadata;
			item.Count = (byte)count;

			return item;
		}
	}
}
