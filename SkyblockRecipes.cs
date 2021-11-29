using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrabsSkyblockChallenge
{
    public class SkyblockRecipes : ModSystem
    {
        Recipe CreateRecipe(int result, int amount = 1) => Mod.CreateRecipe(result, amount);

        public override void AddRecipes()
        {
            CreateRecipe(ItemID.HardenedSand)
                .AddIngredient(ItemID.SandBlock)
                .AddTile(TileID.Furnaces)
                .Register();

            CreateRecipe(ItemID.Hellforge)
                .AddIngredient(ItemID.Furnace)
                .AddIngredient(ItemID.Hellstone, stack: 30)
                .AddIngredient(ItemID.Obsidian, stack: 30)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.DartTrap)
                .AddIngredient(ItemID.StoneBlock)
                .AddIngredient(ItemID.PoisonDart, stack: 100)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
