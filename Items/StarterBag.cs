using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrabsSkyblockChallenge.Items
{
    public class StarterBag : ModItem
    {
        // TODO: update texture

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starter Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.expert = true;
            Item.rare = ItemRarityID.Purple;
        }

        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
            player.QuickSpawnItem(ItemID.SilverBroadsword);
            player.QuickSpawnItem(ItemID.SilverPickaxe);
            player.QuickSpawnItem(ItemID.SilverAxe);
            player.QuickSpawnItem(ItemID.SilverHammer);
            player.QuickSpawnItem(ItemID.BugNet);
            player.QuickSpawnItem(ItemID.HermesBoots);
            player.QuickSpawnItem(ItemID.CloudinaBottle);
        }
    }
}
