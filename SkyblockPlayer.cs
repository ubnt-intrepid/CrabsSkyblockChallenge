using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CrabsSkyblockChallenge
{
    public class SkyblockPlayer : ModPlayer
    {
        const string ReceiveStarterItemsName = "ReceiveStarterItems";
        public bool ReceiveStarterItems = false;

        public override void LoadData(TagCompound tag)
        {
            ReceiveStarterItems = tag.ContainsKey(ReceiveStarterItemsName);
        }

        public override void SaveData(TagCompound tag)
        {
            if (ReceiveStarterItems)
            {
                tag[ReceiveStarterItemsName] = true;
            }
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            if (mediumCoreDeath)
            {
                return Enumerable.Empty<Item>();
            }

            return new[] {
                new Item(ItemID.CopperHammer),
            };
        }
    }
}
