﻿using System;
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

            // Dungeon and Jungle Temple are the only early structures in the world
            // (except the initial spawn point.)
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
        }

        //
        //             :e: :e: :e: :a: :a: :a:     :c: :c:
        //             :e: :e: :e: :a: (a) :a:     (c) :c:
        // (-) [x] [x] :e: (e) :e: [x] [x] [x] :w: [x] [x]
        // y=0 [x] [x] [x] [x] [x] [x] [x] [x] [x] [x] [x]
        // (+)         [x] [x] [x] [x] [x] [x] [x]
        //                     [x] [x] :t:
        //                     (-) x=0 (+)
        static readonly Tuple<int, int>[] SpawnIslandOffsets = new Tuple<int, int>[] {
            new(-5,-1), new(-4,-1),                                     new(0,-1), new(1,-1), new(2,-1),            new(4,-1), new(5,-1),  
            new(-5, 0), new(-4, 0), new(-3, 0), new(-2, 0), new(-1, 0), new(0, 0), new(1, 0), new(2, 0), new(3, 0), new(4, 0), new(5, 0),
                                    new(-3, 1), new(-2, 1), new(-1, 1), new(0, 1), new(1, 1), new(2, 1), new(3, 1),
                                                            new(-1, 2), new(0, 2),
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

            WorldGen.PlaceTile(x - 2, y - 1, TileID.Extractinator);
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

                chest.item[nextSlot++] = new Item(ItemID.Acorn, stack: 10);
                chest.item[nextSlot++] = new Item(ItemID.JungleGrassSeeds);
                chest.item[nextSlot++] = new Item(ItemID.MushroomGrassSeeds);
                chest.item[nextSlot++] = new Item(ItemID.Cobweb, stack: 10);
                chest.item[nextSlot++] = new Item(ItemID.Marble, stack: 25);
                chest.item[nextSlot++] = new Item(ItemID.Granite, stack: 25);
                chest.item[nextSlot++] = new Item(ItemID.Sandstone, stack: 25);
                chest.item[nextSlot++] = new Item(ItemID.HiveWand);
            }

            var liquidType = Main.getGoodWorld ? LiquidID.Lava : LiquidID.Water;
            WorldGen.PlaceLiquid(x + 3, y - 1, (byte)liquidType, amount: 255);

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
    }
}
