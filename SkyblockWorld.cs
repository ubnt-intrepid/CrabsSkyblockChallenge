using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;
using System.Collections.Generic;
using Terraria.IO;
using System;

namespace CrabsSkyblockChallenge
{
    public class OBCWorld : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            Mod.Logger.Info("Switch to OBC world generation");

            var resetTask = tasks.Find(pass => pass.Name.Contains("Reset"));

            tasks.Clear();
            tasks.Add(resetTask);
            tasks.Add(new OBCWorldGenPass());
        }
    }

    class OBCWorldGenPass : GenPass
    {
        readonly int dungeonDirection;

        public OBCWorldGenPass() : base("OBC World Generation", 0f)
        {
            dungeonDirection = Main.rand.NextBool() ? 1 : -1;
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generate One Block Challenge World";

            WorldGen.clearWorld();

            Main.worldSurface = Main.maxTilesY * 0.3;
            Main.rockLayer = Main.maxTilesY * 0.5;

            Main.spawnTileX = (int)(Main.maxTilesX * 0.5);
            Main.spawnTileY = (int)Main.worldSurface;

            PlaceSpawnIsland(Main.spawnTileX, Main.spawnTileY);

            // Dungeon and Jungle Temple are the only early structures in the world
            // (except the initial spawn point.)
            var dungeonX = (int)(Main.maxTilesX * (0.5 + dungeonDirection * 0.3));
            var dungeonY = (int)((Main.spawnTileY + Main.rockLayer) / 2.0) + Main.rand.Next(-200, 200);
            WorldGen.MakeDungeon(dungeonX, dungeonY);

            var templeX = (int)(Main.maxTilesX * (0.5 - dungeonDirection * 0.3));
            var templeY = Main.rand.Next((int)Main.rockLayer, Main.UnderworldLayer - 200);
            WorldGen.makeTemple(templeX, templeY);
            WorldGen.templePart2();
        }

        //      :e: :e: :e:
        //      :e: :e: :e:     :c: :c:
        //  -   :e: (e) :e:     (c) :c:     :t:
        // y=0  [s] [s] [s] (o) [s] [s] :w: [s]
        //  +   [s] [s] [s] [s] [s] [s] [s] [s]
        //               -  x=0  +
        static readonly Tuple<int, int>[] SpawnIslandOffsets = new[] {
            new Tuple<int, int>(-3, 0),
            new Tuple<int, int>(-2, 0),
            new Tuple<int, int>(-1, 0),
            new Tuple<int, int>( 1, 0),
            new Tuple<int, int>( 2, 0),
            new Tuple<int, int>( 4, 0),
            new Tuple<int, int>(-3, 1),
            new Tuple<int, int>(-2, 1),
            new Tuple<int, int>(-1, 1),
            new Tuple<int, int>( 0, 1),
            new Tuple<int, int>( 1, 1),
            new Tuple<int, int>( 2, 1),
            new Tuple<int, int>( 3, 1),
            new Tuple<int, int>( 4, 1),
        };

        static void PlaceSpawnIsland(int x, int y)
        {
            WorldGen.PlaceTile(x, y, ModContent.TileType<Tiles.InfiniteBlock>());

            foreach (var offset in SpawnIslandOffsets)
            {
                WorldGen.PlaceTile(x + offset.Item1, y + offset.Item2, TileID.Stone);
            }

            WorldGen.PlaceTile(x - 2, y - 1, TileID.Extractinator);
            WorldGen.PlaceTile(x + 4, y - 1, TileID.Torches, style: TorchID.Torch);

            int chestIndex = WorldGen.PlaceChest(x + 1, y - 1);
            if (chestIndex != -1)
            {
                var chest = Main.chest[chestIndex];
                int nextSlot = 0;

                chest.item[nextSlot++] = new Item(ItemID.JungleGrassSeeds);
                chest.item[nextSlot++] = new Item(ItemID.MushroomGrassSeeds);
                chest.item[nextSlot++] = new Item(ItemID.Acorn, stack: 5);
                chest.item[nextSlot++] = new Item(ItemID.Cobweb, stack: 10);
                chest.item[nextSlot++] = new Item(ItemID.Marble, stack: 25);
                chest.item[nextSlot++] = new Item(ItemID.Granite, stack: 25);

                if (Main.expertMode || Main.masterMode)
                {
                    chest.item[nextSlot++] = new Item(ItemID.SlimeCrown, stack: 10);
                    chest.item[nextSlot++] = new Item(ItemID.SuspiciousLookingEye, stack: 10);
                    chest.item[nextSlot++] = new Item(ItemID.BloodMoonStarter, stack: 10);
                    chest.item[nextSlot++] = new Item(ItemID.SnowGlobe, stack: 10);
                }
            }

            WorldGen.PlaceLiquid(x + 3, y, LiquidID.Water, amount: 180);

            int guideIndex = NPC.NewNPC(x * 16, y * 16, NPCID.Guide);
            Main.npc[guideIndex].homeless = true;
            Main.npc[guideIndex].homeTileX = x;
            Main.npc[guideIndex].homeTileY = y;
            Main.npc[guideIndex].direction = 1;
        }
    }
}
