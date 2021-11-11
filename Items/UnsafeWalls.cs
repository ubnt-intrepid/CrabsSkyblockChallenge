using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace CrabsSkyblockChallenge.Items
{
    public abstract class UnsafeWallBase : ModItem
    {
        readonly string name;
        readonly int createWall;
        readonly int vanillaWall;
        readonly int ingredientTile;

        internal UnsafeWallBase(string name, int createWall, int vanillaWall, int ingredientTile)
        {
            this.name = name;
            this.createWall = createWall;
            this.vanillaWall = vanillaWall;
            this.ingredientTile = ingredientTile;
        }

        public override string Texture => $"Terraria/Images/Item_{vanillaWall}";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault($"Unsafe {name}");
            Tooltip.SetDefault($"Place naturally-generating {name}, that does not prevent enemy spawns");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 14;
            Item.useAnimation = 15;
            Item.useTime = 8;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.maxStack = 999;

            Item.createWall = createWall;
        }

        public override void AddRecipes()
        {
            CreateRecipe(amount: 4)
                .AddIngredient(ingredientTile)
                .AddTile(TileID.WorkBenches)
                .AddCondition(Recipe.Condition.InGraveyardBiome)
                .Register();

            Mod.CreateRecipe(ingredientTile)
                .AddIngredient(Type, stack: 4)
                .AddTile(TileID.WorkBenches)
                .AddCondition(Recipe.Condition.InGraveyardBiome)
                .Register();
        }
    }

    public class UnsafeSpiderWall : UnsafeWallBase
    {
        public UnsafeSpiderWall()
            : base("Spider Wall", WallID.SpiderUnsafe, ItemID.SpiderEcho, ItemID.Cobweb)
        {
        }
    }

    public class UnsafeSandstoneWall : UnsafeWallBase
    {
        public UnsafeSandstoneWall()
            : base("Sandstone Wall", WallID.Sandstone, ItemID.SandstoneWall, ItemID.Sandstone)
        {
        }
    }
}
