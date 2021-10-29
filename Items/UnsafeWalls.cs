using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace OneBlockChallenge.Items
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
            Item.useTime = 15;
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

            CreateRecipe()
                .AddIngredient(vanillaWall)
                .Register();

            Mod.CreateRecipe(vanillaWall)
                .AddIngredient(Type)
                .Register();
        }
    }

    public class UnsafeSpiderWall : UnsafeWallBase
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.SpiderWall}";

        public UnsafeSpiderWall()
            : base("Spider Wall", WallID.SpiderUnsafe, ItemID.SpiderWall, ItemID.Cobweb)
        {
        }
    }

    public class UnsafeHardenedSandWall : UnsafeWallBase
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.HardenedSandWall}";

        public UnsafeHardenedSandWall()
            : base("Hardened Sand Wall", WallID.HardenedSand, ItemID.HardenedSandWall, ItemID.HardenedSand)
        {
        }
    }

    public class UnsafeHardenedEbonsandWall : UnsafeWallBase
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.CorruptHardenedSandWall}";

        public UnsafeHardenedEbonsandWall()
            : base("Hardened Ebonsand Wall", WallID.CorruptHardenedSand, ItemID.CorruptHardenedSandWall, ItemID.CorruptHardenedSand)
        {
        }
    }

    public class UnsafeHardenedCrimsandWall : UnsafeWallBase
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.CrimsonHardenedSandWall}";

        public UnsafeHardenedCrimsandWall()
            : base("Hardened Crimsand Wall", WallID.CrimsonHardenedSand, ItemID.CrimsonHardenedSandWall, ItemID.CrimsonHardenedSand)
        {
        }
    }

    public class UnsafeHardenedPearlsandWall : UnsafeWallBase
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.HallowHardenedSandWall}";

        public UnsafeHardenedPearlsandWall()
            : base("Hardened Pearlsand Wall", WallID.HallowHardenedSand, ItemID.HallowHardenedSandWall, ItemID.HallowHardenedSand)
        {
        }
    }

    public class UnsafeSandstoneWall : UnsafeWallBase
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.SandstoneWall}";

        public UnsafeSandstoneWall()
            : base("Sandstone Wall", WallID.Sandstone, ItemID.SandstoneWall, ItemID.Sandstone)
        {
        }
    }

    public class UnsafeEbonsandstoneWall : UnsafeWallBase
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.CorruptSandstoneWall}";

        public UnsafeEbonsandstoneWall()
            : base("Ebonsandstone Wall", WallID.CorruptSandstone, ItemID.CorruptSandstoneWall, ItemID.CorruptSandstone)
        {
        }
    }

    public class UnsafeCrimsandstoneWall : UnsafeWallBase
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.CrimsonSandstoneWall}";

        public UnsafeCrimsandstoneWall()
            : base("Crimsandstone Wall", WallID.CrimsonSandstone, ItemID.CrimsonSandstoneWall, ItemID.CrimsonSandstone)
        {
        }
    }

    public class UnsafePearlsandstoneWall : UnsafeWallBase
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.HallowSandstoneWall}";

        public UnsafePearlsandstoneWall()
            : base("Pearlsandstone Wall", WallID.HallowSandstone, ItemID.HallowSandstoneWall, ItemID.HallowSandstone)
        {
        }
    }
}
