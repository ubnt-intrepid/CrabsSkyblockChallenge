using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace OneBlockChallenge.Items
{
    public class UnsafeSpiderWall : ModItem
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.SpiderWall}";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Unsafe Spider Wall");
            Tooltip.SetDefault("Place naturally-generating Spider Wall, that does not prevent enemy spawns");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 14;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 999;

            Item.createWall = WallID.SpiderUnsafe;
        }

        public override void AddRecipes()
        {
            CreateRecipe(amount: 4)
                .AddIngredient(ItemID.Cobweb)
                .AddTile(TileID.WorkBenches)
                .AddCondition(Recipe.Condition.InGraveyardBiome)
                .Register();

            Mod.CreateRecipe(ItemID.Cobweb)
                .AddIngredient(Type, stack: 4)
                .AddTile(TileID.WorkBenches)
                .AddCondition(Recipe.Condition.InGraveyardBiome)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.SpiderEcho)
                .Register();

            Mod.CreateRecipe(ItemID.SpiderEcho)
                .AddIngredient(Type)
                .Register();
        }
    }
}
