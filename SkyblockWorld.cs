using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace CrabsSkyblockChallenge
{
    public class SkyblockWorld : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            Mod.Logger.Info("Switch to Skyblock world generation");

            var resetTask = tasks.Find(pass => pass.Name.Contains("Reset"));

            tasks.Clear();
            tasks.Add(resetTask);
            tasks.Add(new SkyblockWorldGenPass());
        }
    }

    class SkyblockWorldGenPass : GenPass
    {
        readonly int dungeonDirection;

        public SkyblockWorldGenPass() : base("Skyblock World Generation", 0f)
        {
            dungeonDirection = Main.rand.NextBool() ? 1 : -1;
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generate Skyblock World";

            Main.worldSurface = Main.maxTilesY * 0.3;
            Main.rockLayer = Main.maxTilesY * 0.5;

            Main.spawnTileX = (int)(Main.maxTilesX * 0.5);
            Main.spawnTileY = (int)Main.worldSurface;

            if (Main.tenthAnniversaryWorld)
            {
                Main.spawnTileX = Utils.Clamp((int)(Main.maxTilesX * (0.5 - dungeonDirection * 0.45)), 100, Main.maxTilesX - 100);
            }

            PlaceSpawnIsland(Main.spawnTileX, Main.spawnTileY);

            var dungeonX = (int)(Main.maxTilesX * (0.5 + dungeonDirection * 0.3));
            var dungeonY = (int)((Main.spawnTileY + Main.rockLayer) / 2.0) + Main.rand.Next(-200, 200);
            if (WorldGen.drunkWorldGen)
            {
                dungeonY = Math.Max((int)(Main.rockLayer + Main.rand.Next(-100, 100)), (int)(Main.worldSurface + 100));
            }
            WorldGen.MakeDungeon(dungeonX, dungeonY);

            var templeX = (int)(Main.maxTilesX * (0.5 - dungeonDirection * 0.3));
            var templeY = Main.rand.Next((int)Main.rockLayer, Main.UnderworldLayer - 200);
            WorldGen.makeTemple(templeX, templeY);
            WorldGen.templePart2();

            var jungleIslandX = (int)(Main.maxTilesX * 0.5 - dungeonDirection * Main.rand.Next(150, 250));
            PlaceJungleIsland(jungleIslandX, Main.spawnTileY);

            var sandstoneIslandX = (int)((Main.maxTilesX * 0.5 + dungeonDirection * 200 + dungeonX) * 0.5) + Main.rand.Next(-50, 50);
            PlaceSandstoneIsland(sandstoneIslandX, Main.spawnTileY);

            var graniteIslandX = Main.rand.Next(150, 250);
            var graniteIslandY = (int)(Main.rockLayer + Main.rand.Next(150, 200));
            PlaceGraniteIsland(graniteIslandX, graniteIslandY);

            var marbleIslandX = Main.maxTilesX - Main.rand.Next(150, 250);
            var marbleIslandY = (int)(Main.rockLayer + Main.rand.Next(150, 200));
            PlaceMarbleIsland(marbleIslandX, marbleIslandY);
        }

        //
        //         s s s a a a   c c
        //         s s s a a a   c c
        // -   x x s s s x x x w x x
        // 0   x x x x x x x x x x x
        // +       x x x x x x x
        //             x x t
        //
        //             - 0 +
        static readonly Tuple<int, int>[] SpawnIslandOffsets = new Tuple<int, int>[] {
            new(-5, -1),
            new(-4, -1),
            new( 0, -1),
            new( 1, -1),
            new( 2, -1),
            new( 4, -1),
            new( 5, -1),  

            new(-5, 0),
            new(-4, 0),
            new(-3, 0),
            new(-2, 0),
            new(-1, 0),
            new( 0, 0),
            new( 1, 0),
            new( 2, 0),
            new( 3, 0),
            new( 4, 0),
            new( 5, 0),

            new(-3, 1),
            new(-2, 1),
            new(-1, 1),
            new( 0, 1),
            new( 1, 1),
            new( 2, 1),
            new( 3, 1),

            new(-1, 2),
            new( 0, 2),
        };

        static void PlaceSpawnIsland(int x, int y)
        {
            foreach ((var i, var j) in SpawnIslandOffsets)
            {
                WorldGen.PlaceTile(x + i, y + j, TileID.Dirt);
                if (WorldGen.tenthAnniversaryWorldGen)
                {
                    WorldGen.paintTile(x + i, y + j, PaintID.DeepPinkPaint);
                }
            }

            WorldGen.PlaceTile(x - 2, y - 1, TileID.Solidifier);
            WorldGen.PlaceTile(x + 1, y - 2, TileID.DemonAltar, style: WorldGen.crimson ? 1 : 0);
            WorldGen.PlaceTile(x + 1, y + 2, TileID.Torches, style: TorchID.Torch);

            ushort chestType = TileID.Containers;
            int chestStyle = 0;

            if (WorldGen.tenthAnniversaryWorldGen)
            {
                // Palm Wood Chest
                chestType = TileID.Containers;
                chestStyle = 31;
            }
            else if (WorldGen.getGoodWorldGen)
            {
                // Obsidian Chest
                chestType = TileID.Containers;
                chestStyle = 44;
            }
            else if (WorldGen.drunkWorldGen)
            {
                if (WorldGen.crimson)
                {
                    // Lesion Chest
                    chestType = TileID.Containers2;
                    chestStyle = 3;
                }
                else
                {
                    // Flesh Chest
                    chestType = TileID.Containers;
                    chestStyle = 43;
                }
            }
            else if (WorldGen.notTheBees)
            {
                // Honey Chest
                chestType = TileID.Containers;
                chestStyle = 29;
            }

            int chestIndex = WorldGen.PlaceChest(x + 4, y - 2, type: chestType, style: chestStyle);
            if (chestIndex != -1)
            {
                var chest = Main.chest[chestIndex];
                int nextSlot = 0;

                chest.item[nextSlot++] = new Item(ItemID.MushroomGrassSeeds);
                chest.item[nextSlot++] = new Item(ItemID.Cobweb, stack: 10);
                chest.item[nextSlot++] = new Item(ItemID.DynastyWood, stack: 25);
            }

            if (Main.tenthAnniversaryWorld)
            {
                BirthdayParty.GenuineParty = true;
                BirthdayParty.PartyDaysOnCooldown = 5;

                var andrew = NPC.NewNPC(x * 16, y * 16, NPCID.Guide);
                Main.npc[andrew].GivenName = Language.GetTextValue("GuideNames.Andrew");
                Main.npc[andrew].homeless = true;
                Main.npc[andrew].homeTileX = x;
                Main.npc[andrew].homeTileY = y;
                Main.npc[andrew].direction = 1;
                BirthdayParty.CelebratingNPCs.Add(andrew);

                var whitney = NPC.NewNPC(x * 16, y * 16, NPCID.Steampunker);
                Main.npc[whitney].GivenName = Language.GetTextValue("SteampunkerNames.Whitney");
                Main.npc[whitney].homeless = true;
                Main.npc[whitney].homeTileX = x;
                Main.npc[whitney].homeTileY = y;
                Main.npc[whitney].direction = 1;
                BirthdayParty.CelebratingNPCs.Add(whitney);

                var yorai = NPC.NewNPC(x * 16, y * 16, NPCID.Princess);
                Main.npc[yorai].GivenName = Language.GetTextValue("PrincessNames.Yorai");
                Main.npc[yorai].homeless = true;
                Main.npc[yorai].homeTileX = x;
                Main.npc[yorai].homeTileY = y;
                Main.npc[yorai].direction = 1;
                BirthdayParty.CelebratingNPCs.Add(yorai);

                var organizer = NPC.NewNPC(x * 16, y * 16, NPCID.PartyGirl);
                Main.npc[organizer].homeless = true;
                Main.npc[organizer].homeTileX = x;
                Main.npc[organizer].homeTileY = y;
                Main.npc[organizer].direction = 1;
                BirthdayParty.CelebratingNPCs.Add(organizer);

                var bunny = NPC.NewNPC(x * 16, y * 16, NPCID.TownBunny);
                Main.npc[bunny].homeless = true;
                Main.npc[bunny].homeTileX = x;
                Main.npc[bunny].homeTileY = y;
                Main.npc[bunny].direction = 1;
                Main.npc[bunny].townNpcVariationIndex = 1;
                NPC.boughtBunny = true;
            }
            else if (Main.getGoodWorld)
            {
                var guide = NPC.NewNPC(x * 16, y * 16, NPCID.Demolitionist);
                Main.npc[guide].homeless = true;
                Main.npc[guide].homeTileX = x;
                Main.npc[guide].homeTileY = y;
                Main.npc[guide].direction = 1;
            }
            else if (Main.drunkWorld)
            {
                var guide = NPC.NewNPC(x * 16, y * 16, NPCID.PartyGirl);
                Main.npc[guide].homeless = true;
                Main.npc[guide].homeTileX = x;
                Main.npc[guide].homeTileY = y;
                Main.npc[guide].direction = 1;
            }
            else if (Main.notTheBeesWorld)
            {
                var guide = NPC.NewNPC(x * 16, y * 16, NPCID.Merchant);
                Main.npc[guide].homeless = true;
                Main.npc[guide].homeTileX = x;
                Main.npc[guide].homeTileY = y;
                Main.npc[guide].direction = 1;
            }
            else
            {
                var guide = NPC.NewNPC(x * 16, y * 16, NPCID.Guide);
                Main.npc[guide].homeless = true;
                Main.npc[guide].homeTileX = x;
                Main.npc[guide].homeTileY = y;
                Main.npc[guide].direction = 1;
            }
        }


        //             l                 l
        //             l   s s           l
        //       x x x l   s s c c       l x x
        // 0 x x x x x x x s s c c     x x x x x x
        //   x x x x x x x x x x x w x x x x x x x
        //           x x x x x x x x x x x x x x
        //               x x x x x x x x   x
        //                   0
        static readonly Tuple<int, int, bool>[] JungleIslandOffsets = new Tuple<int, int, bool>[] {
            new(-6, -1, true),
            new(-5, -1, true),
            new(-4, -1, true),
            new( 7, -1, true),
            new( 8, -1, true),

            new(-8, 0, true),
            new(-7, 0, true),
            new(-6, 0, false),
            new(-5, 0, false),
            new(-4, 0, false),
            new(-3, 0, false),
            new(-2, 0, true),
            new( 5, 0, true),
            new( 6, 0, true),
            new( 7, 0, false),
            new( 8, 0, false),
            new( 9, 0, true),
            new(10, 0, true),

            new(-8, 1, true),
            new(-7, 1, true),
            new(-6, 1, true),
            new(-5, 1, true),
            new(-4, 1, false),
            new(-3, 1, false),
            new(-2, 1, false),
            new(-1, 1, false),
            new( 0, 1, false),
            new( 1, 1, false),
            new( 2, 1, false),
            new( 4, 1, false),
            new( 5, 1, false),
            new( 6, 1, false),
            new( 7, 1, false),
            new( 8, 1, false),
            new( 9, 1, false),
            new(10, 1, true),

            new(-4, 2, true),
            new(-3, 2, true),
            new(-2, 2, false),
            new(-1, 2, false),
            new( 0, 2, false),
            new( 1, 2, false),
            new( 2, 2, false),
            new( 3, 2, false),
            new( 4, 2, false),
            new( 5, 2, false),
            new( 6, 2, false),
            new( 7, 2, false),
            new( 8, 2, true),
            new( 9, 2, true),

            new(-2, 3, true),
            new(-1, 3, true),
            new( 0, 3, true),
            new( 1, 3, true),
            new( 2, 3, true),
            new( 3, 3, true),
            new( 4, 3, true),
            new( 5, 3, true),
            new( 7, 3, true),
        };

        static void PlaceJungleIsland(int x, int y)
        {
            foreach ((var i, var j, var grass) in JungleIslandOffsets)
            {
                
                WorldGen.PlaceTile(x + i, y + j, TileID.Mud);
                if (grass)
                {
                    WorldGen.PlaceTile(x + i, y + j, TileID.JungleGrass);
                }
            }

            var liquidType = Main.getGoodWorld ? LiquidID.Lava : LiquidID.Water;
            WorldGen.PlaceLiquid(x + 3, y + 1, (byte)liquidType, amount: 255);

            WorldGen.PlaceTile(x - 1, y, TileID.Statues, style: 16); // Hornet Statue
            WorldGen.PlaceTile(x - 3, y - 1, TileID.Lamps, style: 6); // Rich Mahogany Lamp
            WorldGen.PlaceTile(x + 6, y - 1, TileID.Lamps, style: 6);

            int chestIndex = WorldGen.PlaceChest(x + 1, y, type: TileID.Containers, style: 10); // Ivy Chest
            if (chestIndex != -1)
            {
                var chest = Main.chest[chestIndex];
                int nextSlot = 0;

                chest.item[nextSlot++] = new Item(ItemID.StaffofRegrowth);
                chest.item[nextSlot++] = new Item(ItemID.Extractinator);
                chest.item[nextSlot++] = new Item(ItemID.HiveWand);
            }
        }

        //         l           l
        //         l     e e e l
        // -       l c c e e e l   x x
        // 0     x x c c e e e x x x x
        // + x x x x x x x x x x x
        //   x x x x x x x x x x
        //     x x x x x x x x
        //       x   x x x x
        //           x x x
        //           x   x
        //           - 0 +
        static readonly Tuple<int, int>[] SandstoneIslandOffsets = new Tuple<int, int>[] {
            new(6, -1),
            new(7, -1),

            new(-3, 0),
            new(-2, 0),
            new( 4, 0),
            new( 5, 0),
            new( 6, 0),
            new( 7, 0),

            new(-5, 1),
            new(-4, 1),
            new(-3, 1),
            new(-2, 1),
            new(-1, 1),
            new( 0, 1),
            new( 1, 1),
            new( 2, 1),
            new( 3, 1),
            new( 4, 1),
            new( 5, 1),

            new(-5, 2),
            new(-4, 2),
            new(-3, 2),
            new(-2, 2),
            new(-1, 2),
            new( 0, 2),
            new( 1, 2),
            new( 2, 2),
            new( 3, 2),
            new( 4, 2),

            new(-4, 3),
            new(-3, 3),
            new(-2, 3),
            new(-1, 3),
            new( 0, 3),
            new( 1, 3),
            new( 2, 3),
            new( 3, 3),

            new(-3, 4),
            new(-1, 4),
            new( 0, 4),
            new( 1, 4),
            new( 2, 4),

            new(-1, 5),
            new( 0, 5),
            new( 1, 5),

            new(-1, 6),
            new( 1, 6),
        };

        static void PlaceSandstoneIsland(int x, int y)
        {
            foreach ((var i, var j) in SandstoneIslandOffsets)
            {
                WorldGen.PlaceTile(x + i, y + j, TileID.Sandstone);
            }

            WorldGen.PlaceTile(x - 2, y - 1, TileID.Lamps, style: 38); // Sandstone Lamp
            WorldGen.PlaceTile(x + 4, y - 1, TileID.Lamps, style: 38);

            WorldGen.PlaceTile(x + 2, y, TileID.Extractinator);

            int chestIndex = WorldGen.PlaceChest(x - 1, y, type: TileID.Containers2, style: 10); // Sandstone Chest
            if (chestIndex != -1)
            {
                var chest = Main.chest[chestIndex];
                int nextSlot = 0;

                chest.item[nextSlot++] = new Item(ItemID.LifeCrystal, stack: 5);
            }
        }

        //       l             l
        //       l   s s       l
        // - x x l   s s c c   l x x
        // 0 x x x x s s c c x x x x
        // +     x x x x x x x x
        //           x x x x
        //           - 0 +
        static readonly Tuple<int, int>[] GraniteIslandOffsets = new Tuple<int, int>[] {
            new(-5, -1),
            new(-4, -1),
            new( 5, -1),
            new( 6, -1),

            new(-5, 0),
            new(-4, 0),
            new(-3, 0),
            new(-2, 0),
            new( 3, 0),
            new( 4, 0),
            new( 5, 0),
            new( 6, 0),

            new(-3, 1),
            new(-2, 1),
            new(-1, 1),
            new( 0, 1),
            new( 1, 1),
            new( 2, 1),
            new( 3, 1),
            new( 4, 1),

            new(-1, 2),
            new( 0, 2),
            new( 1, 2),
            new( 2, 2),
        };

        static void PlaceGraniteIsland(int x, int y)
        {
            foreach ((var i, var j) in GraniteIslandOffsets)
            {
                WorldGen.PlaceTile(x + i, y + j, TileID.Granite);
            }

            WorldGen.PlaceTile(x - 1, y, TileID.Statues, style: 73); // Granite Golem Statue
            WorldGen.PlaceTile(x - 3, y - 1, TileID.Lamps, style: 29); // Granite Lamp
            WorldGen.PlaceTile(x + 4, y - 1, TileID.Lamps, style: 29);

            int chestIndex = WorldGen.PlaceChest(x + 1, y, type: TileID.Containers, style: 50); // Granite Chest
            if (chestIndex != -1)
            {
                var chest = Main.chest[chestIndex];
                int nextSlot = 0;

                chest.item[nextSlot++] = new Item(ItemID.LifeCrystal, stack: 5);
            }
        }


        //       l             l
        //       l   s s       l
        // - x x l   s s c c   l x x
        // 0 x x x x s s c c x x x x
        // +     x x x x x x x x
        //           x x x x
        //           - 0 +
        static readonly Tuple<int, int>[] MarbleIslandOffsets = new Tuple<int, int>[] {
            new(-5, -1),
            new(-4, -1),
            new( 5, -1),
            new( 6, -1),

            new(-5, 0),
            new(-4, 0),
            new(-3, 0),
            new(-2, 0),
            new( 3, 0),
            new( 4, 0),
            new( 5, 0),
            new( 6, 0),

            new(-3, 1),
            new(-2, 1),
            new(-1, 1),
            new( 0, 1),
            new( 1, 1),
            new( 2, 1),
            new( 3, 1),
            new( 4, 1),

            new(-1, 2),
            new( 0, 2),
            new( 1, 2),
            new( 2, 2),
        };

        static void PlaceMarbleIsland(int x, int y)
        {
            foreach ((var i, var j) in MarbleIslandOffsets)
            {
                WorldGen.PlaceTile(x + i, y + j, TileID.Marble);
            }

            WorldGen.PlaceTile(x - 1, y, TileID.Statues, style: 72); // Hoplite Statue
            WorldGen.PlaceTile(x - 3, y - 1, TileID.Lamps, style: 30); // Marble Lamp
            WorldGen.PlaceTile(x + 4, y - 1, TileID.Lamps, style: 30);

            int chestIndex = WorldGen.PlaceChest(x + 1, y, type: TileID.Containers, style: 51); // Marble Chest
            if (chestIndex != -1)
            {
                var chest = Main.chest[chestIndex];
                int nextSlot = 0;

                chest.item[nextSlot++] = new Item(ItemID.LifeCrystal, stack: 5);
            }
        }
    }
}
