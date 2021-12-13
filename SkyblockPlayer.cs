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
        public bool ReceiveStarterItems = false;
        public Dictionary<short, int> CrateFishingCounts = new();

        public override void LoadData(TagCompound tag)
        {
            ReceiveStarterItems = tag.ContainsKey("ReceiveStarterItems");

            if (tag.ContainsKey("CrateFishingCounts"))
            {
                foreach ((var rawType, var rawCount) in tag.GetCompound("CrateFishingCounts"))
                {
                    var type = short.Parse(rawType);
                    var count = TagIO.Deserialize<int>(rawCount);
                    CrateFishingCounts[type] = count;
                }
            }
        }

        public override void SaveData(TagCompound tag)
        {
            if (ReceiveStarterItems)
            {
                tag.Set("ReceiveStarterItems", true);
            }

            if (CrateFishingCounts.Count > 0)
            {
                var subTag = new TagCompound();
                foreach ((var type, var count) in CrateFishingCounts)
                {
                    subTag.Set(type.ToString(), count);
                }
                tag.Set("CrateFishingCounts", subTag);
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
