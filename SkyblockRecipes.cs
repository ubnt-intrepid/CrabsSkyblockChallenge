using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrabsSkyblockChallenge
{
    public class SkyblockRecipes : ModSystem
    {
        public override void AddRecipes()
        {
            Mod.CreateRecipe(ItemID.HardenedSand)
                .AddIngredient(ItemID.SandBlock)
                .AddTile(TileID.Furnaces)
                .Register();

            Mod.CreateRecipe(ItemID.Hellforge)
                .AddIngredient(ItemID.Furnace)
                .AddIngredient(ItemID.Hellstone, stack: 30)
                .AddIngredient(ItemID.Obsidian, stack: 30)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
