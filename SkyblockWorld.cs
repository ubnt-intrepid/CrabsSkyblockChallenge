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

            var snowIslandX = dungeonX + dungeonDirection * Main.rand.Next(250, 350);
            PlaceSnowIsland(snowIslandX, Main.spawnTileY);

            var altarIslandX = templeX + Main.rand.Next(-100, 100);
            PlaceAltarIsland(altarIslandX, Main.spawnTileY);

            var sandstoneIslandX = (int)((Main.maxTilesX * 0.5 + dungeonDirection * 200 + dungeonX) * 0.5) + Main.rand.Next(-50, 50);
            PlaceSandstoneIsland(sandstoneIslandX, Main.spawnTileY);

            var oceanIslandX = Main.rand.Next(2) == 0 ? Main.rand.Next(150, 250) : Main.maxTilesX - Main.rand.Next(150, 200);
            PlaceOceanIsland(oceanIslandX, Main.spawnTileY);


            var skyIslandX = (int)(Main.maxTilesX * 0.5) + Main.rand.Next(-400, 400);
            var skyIslandY = (int)(Main.worldSurface * 0.5) + Main.rand.Next(-50, 50);
            PlaceSkyIsland(skyIslandX, skyIslandY);

            var graniteIslandX = Main.rand.Next(150, 250);
            var graniteIslandY = (int)(Main.rockLayer + Main.rand.Next(150, 200));
            PlaceGraniteIsland(graniteIslandX, graniteIslandY);

            var marbleIslandX = Main.maxTilesX - Main.rand.Next(150, 250);
            var marbleIslandY = (int)(Main.rockLayer + Main.rand.Next(150, 200));
            PlaceMarbleIsland(marbleIslandX, marbleIslandY);

            var cavernIslandX = (int)(Main.maxTilesX * 0.5) + Main.rand.Next(-100, 100);
            var cavernIslandY = Main.UnderworldLayer - Main.rand.Next(150, 250);
            PlaceCavernIsland(cavernIslandX, cavernIslandY);
        }

        //
        //         s s s   c c
        //         s s s   c c
        // -   x x s s s x x x x   x
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
            new( 3, -1),
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

            int chestIndex = WorldGen.PlaceChest(x + 1, y - 2, type: chestType, style: chestStyle);
            if (chestIndex != -1)
            {
                var chest = Main.chest[chestIndex];
                int nextSlot = 0;

                chest.item[nextSlot++] = new Item(WorldGen.crimson ? ItemID.FleshBlock : ItemID.LesionBlock, stack: 25);
                chest.item[nextSlot++] = new Item(WorldGen.SavedOreTiers.Iron == TileID.Lead ? ItemID.LeadOre : ItemID.IronOre, stack: 9);
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
                chest.item[nextSlot++] = new Item(ItemID.HiveWand);
                chest.item[nextSlot++] = new Item(ItemID.BugNet);
                chest.item[nextSlot++] = new Item(ItemID.LifeCrystal, stack: 3);
            }
        }


        //
        //         a a a
        // - d x x a a a   x x
        // 0 x x x x x x x x
        // +     x x x x   x
        //           x
        //         - 0 +
        static readonly Tuple<int, int>[] AltarIslandOffsets = new Tuple<int, int>[] {
            new(-3, -1),
            new(-2, -1),
            new( 3, -1),
            new( 4, -1),

            new(-4, 0),
            new(-3, 0),
            new(-2, 0),
            new(-1, 0),
            new( 0, 0),
            new( 1, 0),
            new( 2, 0),
            new( 3, 0),

            new(-2, 1),
            new(-1, 1),
            new( 0, 1),
            new( 1, 1),
            new( 3, 1),

            new(0, 2),
        };

        static void PlaceAltarIsland(int x, int y)
        {
            foreach ((var i, var j) in AltarIslandOffsets)
            {
                WorldGen.PlaceTile(x + i, y + j, WorldGen.crimson ? TileID.Crimstone : TileID.Ebonstone);
            }
            WorldGen.PlaceTile(x - 4, y - 1, TileID.Dirt);
            WorldGen.PlaceTile(x - 4, y - 1, WorldGen.crimson ? TileID.CrimsonGrass : TileID.CorruptGrass);

            WorldGen.PlaceTile(x, y - 1, TileID.DemonAltar, style: WorldGen.crimson ? 1 : 0);
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

                chest.item[nextSlot++] = new Item(ItemID.HermesBoots);
                chest.item[nextSlot++] = new Item(ItemID.CloudinaBottle);
                chest.item[nextSlot++] = new Item(ItemID.LifeCrystal, stack: 3);
                chest.item[nextSlot++] = new Item(ItemID.SlimeCrown, stack: 10);
            }
        }

        //         l                   l
        //         l                   l
        //         l   m m m s s c c   l
        // -   x x x   m m m s s c c   x x x
        // 0 x x x x x m m m s s x x x x x x x
        // +     x x x x x x x x x x x x x
        //           x x x x x x x x x
        //           x x x x x
        //             - 0 +
        static readonly Tuple<int, int>[] SnowIslandOffsets = new Tuple<int, int>[]
        {
            new(-5, -1),
            new(-4, -1),
            new(-3, -1),
            new( 7, -1),
            new( 8, -1),
            new( 9, -1),

            new(-6, 0),
            new(-5, 0),
            new(-4, 0),
            new(-3, 0),
            new(-2, 0),
            new( 4, 0),
            new( 5, 0),
            new( 6, 0),
            new( 7, 0),
            new( 8, 0),
            new( 9, 0),
            new(10, 0),

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
            new( 6, 1),
            new( 7, 1),
            new( 8, 1),

            new(-2, 2),
            new(-1, 2),
            new( 0, 2),
            new( 1, 2),
            new( 2, 2),
            new( 3, 2),
            new( 4, 2),
            new( 5, 2),
            new( 6, 2),

            new(-2, 3),
            new(-1, 3),
            new( 0, 3),
            new( 1, 3),
            new( 2, 3),
        };

        static void PlaceSnowIsland(int x, int y)
        {
            foreach ((var i, var j) in SnowIslandOffsets)
            {
                WorldGen.PlaceTile(x + i, y + j, TileID.SnowBlock);
            }

            WorldGen.PlaceTile(x + 2, y, TileID.Statues, style:68); // Undead Viking Statue
            WorldGen.PlaceTile(x, y, TileID.IceMachine);

            WorldGen.PlaceTile(x - 3, y - 2, TileID.Lamps, style: 20); // Boreal Wood Lamp
            WorldGen.PlaceTile(x + 7, y - 2, TileID.Lamps, style: 20);

            int chestIndex = WorldGen.PlaceChest(x + 4, y - 1, type: TileID.Containers, style: 11); // Frozen Chest
            if (chestIndex != -1)
            {
                var chest = Main.chest[chestIndex];
                int nextSlot = 0;

                chest.item[nextSlot++] = new Item(ItemID.IceSkates);
            }
        }

        //                       l                   l
        //                       l       s s m m m   l
        //                       l   c c s s m m m   l
        // -                   x x x c c s s m m m x x x
        // 0 d d d     d d d x x x x x x x x x x x x x x x d d d d d d
        // +   d d d d d d d d d x x x x x x x x x x x d d d d d d d d d d
        //         d d d d d d d d d d d d x x d d d d d d d d d d d d d d d
        //           d d d d d d d d d d d d d d d d d d d d d d d
        //                   d d d d d d d d d d d d d d d d
        //                             d d d d d d d d d 
        //                                 - 0 +
        static readonly Tuple<int, int, int>[] SkyIslandOffsets = new Tuple<int, int, int>[]
        {
            new(-7, -1, TileID.Sunplate),
            new(-6, -1, TileID.Sunplate),
            new(-5, -1, TileID.Sunplate),
            new( 3, -1, TileID.Sunplate),
            new( 4, -1, TileID.Sunplate),
            new( 5, -1, TileID.Sunplate),

            new(-16, 0, TileID.Cloud),
            new(-15, 0, TileID.Cloud),
            new(-14, 0, TileID.Cloud),
            new(-11, 0, TileID.Cloud),
            new(-10, 0, TileID.Cloud),
            new( -9, 0, TileID.Cloud),
            new( -8, 0, TileID.Sunplate),
            new( -7, 0, TileID.Sunplate),
            new( -6, 0, TileID.Sunplate),
            new( -5, 0, TileID.Sunplate),
            new( -4, 0, TileID.Sunplate),
            new( -3, 0, TileID.Sunplate),
            new( -2, 0, TileID.Sunplate),
            new( -1, 0, TileID.Sunplate),
            new(  0, 0, TileID.Sunplate),
            new(  1, 0, TileID.Sunplate),
            new(  2, 0, TileID.Sunplate),
            new(  3, 0, TileID.Sunplate),
            new(  4, 0, TileID.Sunplate),
            new(  5, 0, TileID.Sunplate),
            new(  6, 0, TileID.Sunplate),
            new(  7, 0, TileID.Cloud),
            new(  8, 0, TileID.Cloud),
            new(  9, 0, TileID.Cloud),
            new( 10, 0, TileID.Cloud),
            new( 11, 0, TileID.Cloud),
            new( 12, 0, TileID.Cloud),

            new(-15, 1, TileID.Cloud),
            new(-14, 1, TileID.Cloud),
            new(-13, 1, TileID.Cloud),
            new(-12, 1, TileID.Cloud),
            new(-11, 1, TileID.Cloud),
            new(-10, 1, TileID.Cloud),
            new( -9, 1, TileID.Cloud),
            new( -8, 1, TileID.Cloud),
            new( -7, 1, TileID.Cloud),
            new( -6, 1, TileID.Sunplate),
            new( -5, 1, TileID.Sunplate),
            new( -4, 1, TileID.Sunplate),
            new( -3, 1, TileID.Sunplate),
            new( -2, 1, TileID.Sunplate),
            new( -1, 1, TileID.Sunplate),
            new(  0, 1, TileID.Sunplate),
            new(  1, 1, TileID.Sunplate),
            new(  2, 1, TileID.Sunplate),
            new(  3, 1, TileID.Sunplate),
            new(  4, 1, TileID.Sunplate),
            new(  5, 1, TileID.Cloud),
            new(  6, 1, TileID.Cloud),
            new(  7, 1, TileID.Cloud),
            new(  8, 1, TileID.Cloud),
            new(  9, 1, TileID.Cloud),
            new( 10, 1, TileID.Cloud),
            new( 11, 1, TileID.Cloud),
            new( 12, 1, TileID.Cloud),
            new( 13, 1, TileID.Cloud),
            new( 14, 1, TileID.Cloud),

            new(-13, 2, TileID.Cloud),
            new(-12, 2, TileID.Cloud),
            new(-11, 2, TileID.Cloud),
            new(-10, 2, TileID.Cloud),
            new( -9, 2, TileID.Cloud),
            new( -8, 2, TileID.Cloud),
            new( -7, 2, TileID.Cloud),
            new( -6, 2, TileID.Cloud),
            new( -5, 2, TileID.Cloud),
            new( -4, 2, TileID.Cloud),
            new( -3, 2, TileID.Cloud),
            new( -2, 2, TileID.Cloud),
            new( -1, 2, TileID.Sunplate),
            new(  0, 2, TileID.Sunplate),
            new(  1, 2, TileID.Cloud),
            new(  2, 2, TileID.Cloud),
            new(  3, 2, TileID.Cloud),
            new(  4, 2, TileID.Cloud),
            new(  5, 2, TileID.Cloud),
            new(  6, 2, TileID.Cloud),
            new(  7, 2, TileID.Cloud),
            new(  8, 2, TileID.Cloud),
            new(  9, 2, TileID.Cloud),
            new( 10, 2, TileID.Cloud),
            new( 11, 2, TileID.Cloud),
            new( 12, 2, TileID.Cloud),
            new( 13, 2, TileID.Cloud),
            new( 14, 2, TileID.Cloud),
            new( 15, 2, TileID.Cloud),

            new(-12, 3, TileID.Cloud),
            new(-11, 3, TileID.Cloud),
            new(-10, 3, TileID.Cloud),
            new( -9, 3, TileID.Cloud),
            new( -8, 3, TileID.Cloud),
            new( -7, 3, TileID.Cloud),
            new( -6, 3, TileID.Cloud),
            new( -5, 3, TileID.Cloud),
            new( -4, 3, TileID.Cloud),
            new( -3, 3, TileID.Cloud),
            new( -2, 3, TileID.Cloud),
            new( -1, 3, TileID.Cloud),
            new(  0, 3, TileID.Cloud),
            new(  1, 3, TileID.Cloud),
            new(  2, 3, TileID.Cloud),
            new(  3, 3, TileID.Cloud),
            new(  4, 3, TileID.Cloud),
            new(  5, 3, TileID.Cloud),
            new(  6, 3, TileID.Cloud),
            new(  7, 3, TileID.Cloud),
            new(  8, 3, TileID.Cloud),
            new(  9, 3, TileID.Cloud),
            new( 10, 3, TileID.Cloud),

            new( -8, 4, TileID.Cloud),
            new( -7, 4, TileID.Cloud),
            new( -6, 4, TileID.Cloud),
            new( -5, 4, TileID.Cloud),
            new( -4, 4, TileID.Cloud),
            new( -3, 4, TileID.Cloud),
            new( -2, 4, TileID.Cloud),
            new( -1, 4, TileID.Cloud),
            new(  0, 4, TileID.Cloud),
            new(  1, 4, TileID.Cloud),
            new(  2, 4, TileID.Cloud),
            new(  3, 4, TileID.Cloud),
            new(  4, 4, TileID.Cloud),
            new(  5, 4, TileID.Cloud),
            new(  6, 4, TileID.Cloud),
            new(  7, 4, TileID.Cloud),

            new( -3, 5, TileID.Cloud),
            new( -2, 5, TileID.Cloud),
            new( -1, 5, TileID.Cloud),
            new(  0, 5, TileID.Cloud),
            new(  1, 5, TileID.Cloud),
            new(  2, 5, TileID.Cloud),
            new(  3, 5, TileID.Cloud),
            new(  4, 5, TileID.Cloud),
            new(  5, 5, TileID.Cloud),
        };
        
        static void PlaceSkyIsland(int x, int y)
        {
            foreach ((var i, var j, var type) in SkyIslandOffsets)
            {
                WorldGen.PlaceTile(x + i, y + j, type);
            }

            WorldGen.PlaceTile(x - 2, y - 1, TileID.Statues, style: 70); // Harpy Statue
            WorldGen.PlaceTile(x + 1, y - 1, TileID.SkyMill);

            WorldGen.PlaceTile(x - 6, y - 2, TileID.Lamps, style: 9); // Skyware Lamp
            WorldGen.PlaceTile(x + 4, y - 2, TileID.Lamps, style: 9);

            int chestIndex = WorldGen.PlaceChest(x - 4, y - 1, type: TileID.Containers, style: 13); // Skyware Chest
            if (chestIndex != -1)
            {
                var chest = Main.chest[chestIndex];
                int nextSlot = 0;

                chest.item[nextSlot++] = new Item(ItemID.CreativeWings);
                chest.item[nextSlot++] = new Item(ItemID.ShinyRedBalloon);
                chest.item[nextSlot++] = new Item(ItemID.Starfury);
            }
        }

        //             s s
        //     t       s s c c         t
        // - x x x x x s s c c x x x x x x
        // 0 x x x x x x x x x x x x x x h
        // + h h x x x x x x x x x x h h h
        //     h h h h x x x x x h h h
        //           h h h h h h h
        //             - 0 +
        static readonly Tuple<int, int, int>[] OceanIslandOffsets = new Tuple<int, int, int>[]
        {
            new(-6, -1, TileID.Sand),
            new(-5, -1, TileID.Sand),
            new(-4, -1, TileID.Sand),
            new(-3, -1, TileID.Sand),
            new(-2, -1, TileID.Sand),
            new( 3, -1, TileID.Sand),
            new( 4, -1, TileID.Sand),
            new( 5, -1, TileID.Sand),
            new( 6, -1, TileID.Sand),
            new( 7, -1, TileID.Sand),
            new( 8, -1, TileID.Sand),

            new(-6, 0, TileID.Sand),
            new(-5, 0, TileID.Sand),
            new(-4, 0, TileID.Sand),
            new(-3, 0, TileID.Sand),
            new(-2, 0, TileID.Sand),
            new(-1, 0, TileID.Sand),
            new( 0, 0, TileID.Sand),
            new( 1, 0, TileID.Sand),
            new( 2, 0, TileID.Sand),
            new( 3, 0, TileID.Sand),
            new( 4, 0, TileID.Sand),
            new( 5, 0, TileID.Sand),
            new( 6, 0, TileID.Sand),
            new( 7, 0, TileID.Sand),
            new( 8, 0, TileID.HardenedSand),

            new(-6, 1, TileID.HardenedSand),
            new(-5, 1, TileID.HardenedSand),
            new(-4, 1, TileID.Sand),
            new(-3, 1, TileID.Sand),
            new(-2, 1, TileID.Sand),
            new(-1, 1, TileID.Sand),
            new( 0, 1, TileID.Sand),
            new( 1, 1, TileID.Sand),
            new( 2, 1, TileID.Sand),
            new( 3, 1, TileID.Sand),
            new( 4, 1, TileID.Sand),
            new( 5, 1, TileID.Sand),
            new( 6, 1, TileID.HardenedSand),
            new( 7, 1, TileID.HardenedSand),
            new( 8, 1, TileID.HardenedSand),

            new(-5, 2, TileID.HardenedSand),
            new(-4, 2, TileID.HardenedSand),
            new(-3, 2, TileID.HardenedSand),
            new(-2, 2, TileID.HardenedSand),
            new(-1, 2, TileID.Sand),
            new( 0, 2, TileID.Sand),
            new( 1, 2, TileID.Sand),
            new( 2, 2, TileID.Sand),
            new( 3, 2, TileID.Sand),
            new( 4, 2, TileID.HardenedSand),
            new( 5, 2, TileID.HardenedSand),
            new( 6, 2, TileID.HardenedSand),

            new(-2, 3, TileID.HardenedSand),
            new(-1, 3, TileID.HardenedSand),
            new( 0, 3, TileID.HardenedSand),
            new( 1, 3, TileID.HardenedSand),
            new( 2, 3, TileID.HardenedSand),
            new( 3, 3, TileID.HardenedSand),
            new( 4, 3, TileID.HardenedSand),

        };

        static void PlaceOceanIsland(int x, int y)
        {
            foreach ((var i, var j, var type) in OceanIslandOffsets)
            {
                WorldGen.PlaceTile(x + i, y + j, type);
            }

            WorldGen.PlaceTile(x - 1, y - 1, TileID.Statues, style: 5); // Goblin Statue
            WorldGen.PlaceTile(x - 5, y - 2, TileID.Torches, style: 17); // Coral Torch
            WorldGen.PlaceTile(x + 7, y - 2, TileID.Torches, style: 17);

            int chestIndex = WorldGen.PlaceChest(x + 1, y - 1, type: TileID.Containers, style: 17); // Water Chest
            if (chestIndex != -1)
            {
                var chest = Main.chest[chestIndex];
                int nextSlot = 0;

                chest.item[nextSlot++] = new Item(ItemID.WaterWalkingBoots);
                chest.item[nextSlot++] = new Item(ItemID.GoldenFishingRod);
                chest.item[nextSlot++] = new Item(ItemID.JourneymanBait, stack: 20);
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

                chest.item[nextSlot++] = new Item(ItemID.FlareGun);
                chest.item[nextSlot++] = new Item(ItemID.Flare, stack: Main.rand.Next(25, 50));
                chest.item[nextSlot++] = new Item(ItemID.LifeCrystal, stack: 3);
                chest.item[nextSlot++] = new Item(ItemID.SnowGlobe, stack: 10);
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

                chest.item[nextSlot++] = new Item(ItemID.ShoeSpikes);
                chest.item[nextSlot++] = new Item(ItemID.Mace);
                chest.item[nextSlot++] = new Item(ItemID.LifeCrystal, stack: 3);
                chest.item[nextSlot++] = new Item(ItemID.BloodMoonStarter, stack: 10);
            }
        }

        //           w w
        //         w w w w w
        //       w w s s w w w w
        // -     l w s s c c w l
        // 0     l w s s c c w l w
        // +   w l w x x x x x l x 
        //   g x x x x       x x x
        //   x x x               x
        //             - 0 +
        static readonly Tuple<int, int>[] CavernIslandOffsets = new Tuple<int, int>[] {
            new(-2, 1),
            new(-1, 1),
            new( 0, 1),
            new( 1, 1),
            new( 2, 1),

            new(-5, 2),
            new(-4, 2),
            new(-3, 2),
            new(-2, 2),
            new( 2, 2),
            new( 3, 2),
            new( 4, 2),

            new(-6, 3),
            new(-5, 3),
            new(-4, 3),
            new( 4, 3),
        };
        static readonly Tuple<int, int>[] CavernIslandWallOffsets = new Tuple<int, int>[] {
            new(-2, -4),
            new(-1, -4),

            new(-3, -3),
            new(-2, -3),
            new(-1, -3),
            new( 0, -3),
            new( 1, -3),

            new(-4, -2),
            new(-3, -2),
            new(-2, -2),
            new(-1, -2),
            new( 0, -2),
            new( 1, -2),
            new( 2, -2),
            new( 3, -2),

            new(-4, -1),
            new(-3, -1),
            new(-2, -1),
            new(-1, -1),
            new( 0, -1),
            new( 1, -1),
            new( 2, -1),
            new( 3, -1),

            new(-4, 0),
            new(-3, 0),
            new(-2, 0),
            new(-1, 0),
            new( 0, 0),
            new( 1, 0),
            new( 2, 0),
            new( 3, 0),
            new( 4, 0),

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
        };

        static void PlaceCavernIsland(int x, int y)
        {
            foreach ((var i, var j) in CavernIslandOffsets)
            {
                WorldGen.PlaceTile(x + i, y + j, TileID.Stone);
            }
            foreach ((var i, var j) in CavernIslandWallOffsets)
            {
                WorldGen.PlaceWall(x + i, y + j, WallID.SpiderUnsafe);
            }
            WorldGen.PlaceTile(x - 6, y + 2, TileID.Mud);
            WorldGen.PlaceTile(x - 6, y + 2, TileID.MushroomGrass);

            WorldGen.PlaceTile(x - 2, y, TileID.Statues, style: 37); // Heart Statue
            WorldGen.PlaceTile(x - 4, y + 1, TileID.Lamps, style: 32); // Spider Lamp
            WorldGen.PlaceTile(x + 3, y + 1, TileID.Lamps, style: 32);

            int chestIndex = WorldGen.PlaceChest(x, y, type: TileID.Containers2, style: 2); // Spider Chest
            if (chestIndex != -1)
            {
                var chest = Main.chest[chestIndex];
                int nextSlot = 0;

                chest.item[nextSlot++] = new Item(ItemID.BandofRegeneration);
                chest.item[nextSlot++] = new Item(ItemID.MagicMirror);
                chest.item[nextSlot++] = new Item(ItemID.LifeCrystal, stack: 3);
                chest.item[nextSlot++] = new Item(ItemID.SuspiciousLookingEye, stack: 10);
            }
        }
    }
}
