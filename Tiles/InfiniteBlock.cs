using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OneBlockChallenge.Tiles
{
    public class InfiniteBlock : ModTile
    {
        public override string Texture => $"Terraria/Images/Tiles_{TileID.Cloud}";

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = false;
            Main.tileNoSunLight[Type] = false;

            DustType = DustID.Cloud;
            AdjTiles = new int[] { TileID.DemonAltar };

            TileID.Sets.DoesntGetReplacedWithTileReplacement[Type] = true;

            var name = CreateMapEntryName();
            name.SetDefault("Infinite Block");
            AddMapEntry(new Color(255, 255, 200), name);
        }

        public override bool CanExplode(int i, int j) => false;

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!fail)
            {
                fail = true;

                var item = Main.rand.Next(420) switch
                {
                    // w=50
                    (>= 0) and (< 50) => ItemID.DirtBlock,
                    (>= 50) and (< 100) => ItemID.StoneBlock,
                    (>= 100) and (< 150) => ItemID.SandBlock,
                    (>= 150) and (< 200) => ItemID.SnowBlock,
                    (>= 200) and (< 250) => ItemID.IceBlock,

                    // w=30
                    (>= 250) and (< 280) => ItemID.ClayBlock,
                    (>= 280) and (< 310) => ItemID.HardenedSand,
                    (>= 310) and (< 340) => ItemID.SiltBlock,
                    (>= 340) and (< 370) => ItemID.SlushBlock,
                    (>= 370) and (< 400) => ItemID.DesertFossil,

                    // w=10
                    (>= 400) and (< 410) => ItemID.Cloud,
                    (>= 410) and (< 420) => ItemID.Cobweb,

                    _ => 0,
                };

                int num = Item.NewItem(i * 16, j * 16, 12, 12, item, 1, noBroadcast: false, -1);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, num, 1f);
                }
            }
        }
    }
}
