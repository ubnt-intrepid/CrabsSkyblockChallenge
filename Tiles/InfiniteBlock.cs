﻿using Microsoft.Xna.Framework;
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
            AddMapEntry(Color.White, name);
        }

        public override bool CanExplode(int i, int j) => false;

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!fail)
            {
                fail = true;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    var item = Main.rand.Next(5) switch
                    {
                        0 => ItemID.DirtBlock,
                        1 => ItemID.ClayBlock,
                        2 => ItemID.HardenedSand,
                        3 => ItemID.SnowBlock,
                        _ => ItemID.SiltBlock,
                    };

                    int num = Item.NewItem(new Vector2(i, j).ToWorldCoordinates(), Type: item, Stack: 1, noBroadcast: false, -1);
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, num, 1f);
                }
            }
        }
    }
}
