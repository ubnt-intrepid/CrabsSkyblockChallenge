using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace CrabsSkyblockChallenge
{
    public class SkyblockRecipes : ModSystem
    {
        Recipe CreateRecipe(int result, int amount = 1) => Mod.CreateRecipe(result, amount);

        public override void AddRecipes()
        {
            AddCraftingStationRecipes();

            AddSurfaceChestLootRecipes();
            AddCavernChestLootRecipies();
            AddLivingWoodRecipes();
            AddUndergroundDesertRecipes();
            AddUndergroundJungleRecipes();
            AddMushroomRecipes();
            AddUnderworldRecipes();

            CreateRecipe(ItemID.SharpeningStation)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 5)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 4)
                .AddIngredient(ItemID.IronskinPotion, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.AmmoBox)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 5)
                .AddIngredient(ItemID.AmmoReservationPotion, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.DartTrap)
                .AddIngredient(ItemID.StoneBlock)
                .AddIngredient(ItemID.PoisonDart, stack: 100)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.GeyserTrap)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 2)
                .AddIngredient(ItemID.LavaBucket)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.WebCoveredChest)
                .AddIngredient(ItemID.Chest)
                .AddIngredient(ItemID.Cobweb, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.WebSlinger)
                .AddRecipeGroup(RecipeGroupID.IronBar)
                .AddIngredient(ItemID.WebRopeCoil, stack: 8)
                .AddTile(TileID.Anvils)
                .Register();
        }

        void AddCraftingStationRecipes()
        {
            CreateRecipe(ItemID.LivingLoom)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 20)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe(ItemID.HoneyDispenser)
                .AddIngredient(ItemID.HoneyBlock, stack: 10)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 5)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 3)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.SkyMill)
                .AddIngredient(ItemID.StoneBlock, stack: 20)
                .AddIngredient(ItemID.Cloud, stack: 10)
                .AddIngredient(ItemID.FallenStar)
                .AddTile(TileID.Anvils)
                .Register();

            // imported from Calamity Mod
            CreateRecipe(ItemID.IceMachine)
                .AddIngredient(ItemID.SnowBlock, stack: 25)
                .AddIngredient(ItemID.IceBlock, stack: 15)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 3)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.Hellforge)
                .AddIngredient(ItemID.Furnace)
                .AddIngredient(ItemID.Hellstone, stack: 30)
                .AddIngredient(ItemID.Obsidian, stack: 30)
                .AddTile(TileID.Anvils)
                .Register();
        }

        void AddSurfaceChestLootRecipes()
        {
            CreateRecipe(ItemID.Spear)
                .AddRecipeGroup(RecipeGroupID.IronBar)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.Blowpipe)
                .AddIngredient(ItemID.BambooBlock, stack: 5)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe(ItemID.WoodenBoomerang)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 5)
                .AddRecipeGroup(RecipeGroupID.IronBar)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.WandofSparking)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 4)
                .AddIngredient(ItemID.Torch, stack: 5)
                .AddIngredient(ItemID.ManaCrystal)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe(ItemID.PortableStool)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        void AddCavernChestLootRecipies()
        {
            CreateRecipe(ItemID.MagicMirror)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 10)
                .AddIngredient(ItemID.Glass, stack: 5)
                .AddIngredient(ItemID.RecallPotion, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.Mace)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 20)
                .AddIngredient(ItemID.Chain, stack: 5)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.FlareGun)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 10)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 2)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.Flare, amount: 25)
                .AddRecipeGroup(RecipeGroupID.IronBar)
                .AddIngredient(ItemID.Torch, stack: 25)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.HermesBoots)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 5)
                .AddIngredient(ItemID.Silk, stack: 10)
                .AddIngredient(ItemID.SwiftnessPotion, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.CloudinaBottle)
                .AddIngredient(ItemID.Bottle)
                .AddIngredient(ItemID.Cloud, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.BandofRegeneration)
                .AddIngredient(ItemID.Chain, stack: 5)
                .AddIngredient(ItemID.LifeCrystal)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.ShoeSpikes)
                .AddIngredient(ItemID.ClimbingClaws)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        void AddLivingWoodRecipes()
        {
            CreateRecipe(ItemID.LivingWoodWand)
                .AddIngredient(ItemID.Wood, stack: 8)
                .AddTile(TileID.LivingLoom)
                .Register();

            CreateRecipe(ItemID.LeafWand)
                .AddIngredient(ItemID.Wood, stack: 5)
                .AddIngredient(ItemID.GrassSeeds, stack: 10)
                .AddTile(TileID.LivingLoom)
                .Register();

            CreateRecipe(ItemID.BabyBirdStaff)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 5)
                .AddIngredient(ItemID.ManaCrystal)
                .AddTile(TileID.LivingLoom)
                .Register();

            CreateRecipe(ItemID.SunflowerMinecart)
                .AddIngredient(ItemID.Minecart)
                .AddIngredient(ItemID.Sunflower)
                .AddTile(TileID.LivingLoom)
                .Register();

            CreateRecipe(ItemID.LadybugMinecart)
                .AddIngredient(ItemID.Minecart)
                .AddIngredient(ItemID.LadyBug)
                .AddTile(TileID.LivingLoom)
                .Register();
        }

        void AddUndergroundJungleRecipes()
        {
            CreateRecipe(ItemID.LivingMahoganyLeafWand)
                .AddIngredient(ItemID.RichMahogany, stack: 5)
                .AddIngredient(ItemID.JungleGrassSeeds, stack: 10)
                .AddTile(TileID.LivingLoom)
                .Register();

            CreateRecipe(ItemID.LivingMahoganyWand)
                .AddIngredient(ItemID.RichMahogany, stack: 5)
                .AddTile(TileID.LivingLoom)
                .Register();

            CreateRecipe(ItemID.BeeMinecart)
                .AddIngredient(ItemID.Minecart)
                .AddIngredient(ItemID.Hive, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        void AddUndergroundDesertRecipes()
        {
            CreateRecipe(ItemID.HardenedSand)
                .AddIngredient(ItemID.SandBlock, stack: 2)
                .AddTile(TileID.Furnaces)
                .Register();

            CreateRecipe(ItemID.DesertMinecart)
                .AddIngredient(ItemID.Minecart)
                .AddIngredient(ItemID.Sandstone, stack: 10)
                .AddIngredient(ItemID.Amber)
                .AddTile(TileID.Anvils)
                .Register();
        }

        void AddMushroomRecipes()
        {
            CreateRecipe(ItemID.ShroomMinecart)
                .AddIngredient(ItemID.Minecart)
                .AddIngredient(ItemID.GlowingMushroom, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.MushroomHat)
                .AddIngredient(ItemID.Silk, stack: 15)
                .AddIngredient(ItemID.MushroomGrassSeeds, stack: 3)
                .AddTile(TileID.Loom)
                .Register();

            CreateRecipe(ItemID.MushroomVest)
                .AddIngredient(ItemID.Silk, stack: 20)
                .AddIngredient(ItemID.MushroomGrassSeeds, stack: 3)
                .AddTile(TileID.Loom)
                .Register();

            CreateRecipe(ItemID.MushroomPants)
                .AddIngredient(ItemID.Silk, stack: 20)
                .AddIngredient(ItemID.MushroomGrassSeeds, stack: 3)
                .AddTile(TileID.Loom)
                .Register();
        }

        void AddUnderworldRecipes()
        {
            CreateRecipe(ItemID.HellMinecart)
                .AddIngredient(ItemID.Minecart)
                .AddIngredient(ItemID.HellstoneBar, stack: 5)
                .AddIngredient(ItemID.LavaCharm)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
