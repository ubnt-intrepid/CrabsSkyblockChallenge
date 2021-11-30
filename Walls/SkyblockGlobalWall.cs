using Terraria.ModLoader;
using Terraria.ID;

namespace CrabsSkyblockChallenge.Walls
{
    public class SkyblockGlobalWall : GlobalWall
    {
        public override bool Drop(int i, int j, int type, ref int dropType)
        {
            switch (type)
            {
                case WallID.Sandstone:
                    dropType = ModContent.ItemType<Items.UnsafeSandstoneWall>();
                    break;

                case WallID.SpiderUnsafe:
                    dropType = ModContent.ItemType<Items.UnsafeSpiderWall>();
                    break;

                default:
                    break;
            }

            return true;
        }
    }
}
