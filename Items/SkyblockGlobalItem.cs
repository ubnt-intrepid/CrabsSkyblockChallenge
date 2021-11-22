using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrabsSkyblockChallenge.Items
{
    public class SkyblockGlobalItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Hive)
            {
                item.useStyle = ItemUseStyleID.Swing;
                item.useTurn = true;
                item.useAnimation = 15;
                item.useTime = 15;
                item.autoReuse = true;
                item.consumable = true;
                item.createTile = TileID.Hive;
            }
        }
    }
}
