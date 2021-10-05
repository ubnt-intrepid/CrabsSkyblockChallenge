using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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

            AddMapEntry(new Color(255, 255, 200));
        }

        public override bool CanExplode(int i, int j) => false;

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!fail)
            {
                fail = true;

                var item = NextBlock();

                int num = Item.NewItem(i * 16, j * 16, 12, 12, item, 1, noBroadcast: false, -1);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, num, 1f);
                }
            }
        }

        static int NextBlock()
        {
            // TODO: optimize
            var drawnBlocks = new List<int> {
                ItemID.DirtBlock,
                ItemID.StoneBlock,
                ItemID.ClayBlock,
                ItemID.HardenedSand,
                ItemID.IceBlock,
            };

            if (OBCWorld.defeatBoss2)
            {
                drawnBlocks.Add(ItemID.Hellstone);
            }

            if (OBCWorld.defeatSkeletron)
            {
                drawnBlocks.AddRange(new int[] {
                    ItemID.BlueBrick,
                    ItemID.GreenBrick,
                    ItemID.PinkBrick,
                });
            }

            if (OBCWorld.defeatPlantera)
            {
                drawnBlocks.Add(ItemID.LihzahrdBrick);
            }

            var index = Main.rand.Next(0, drawnBlocks.Count);
            return drawnBlocks[index];
        }

        public override bool RightClick(int i, int j)
        {
            Main.NewText("Current sources:", Color.Magenta);
            Main.NewText("  - Basic blocks (Dirt, Stone, Clay, Hardened Sand, Ice)");
            if (OBCWorld.defeatBoss2)
            {
                Main.NewText("  - Hellstone");
            }
            if (OBCWorld.defeatSkeletron)
            {
                Main.NewText("  - Dungeon bricks");
            }
            if (OBCWorld.defeatPlantera)
            {
                Main.NewText("  - Lihzahrd brick");
            }

            return true;
        }
    }
}
