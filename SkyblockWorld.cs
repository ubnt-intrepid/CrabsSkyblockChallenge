using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.GameContent.Generation;
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
            tasks.Add(new PassLegacy("Skyblock World Generation", GenerateSkyblockWorld));
        }

        private static void GenerateSkyblockWorld(GenerationProgress progress, GameConfiguration config)
        {
            progress.Message = "Generate Skyblock World";

            var dungeonDirection = Main.rand.NextBool() ? 1 : -1;

            Main.worldSurface = Main.maxTilesY * 0.3;
            Main.rockLayer = Main.maxTilesY * 0.5;

            Main.spawnTileX = (int)(Main.maxTilesX * 0.5);
            Main.spawnTileY = (int)Main.worldSurface;

            if (Main.tenthAnniversaryWorld)
            {
                // The beach side opposite the Dungeon entrance.
                Main.spawnTileX = (int)(Main.maxTilesX * 0.5 * (1 - dungeonDirection)) + dungeonDirection * Main.rand.Next(150, 250);
            }

            #region Setup Dungeon and Jungle Temple

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

            #endregion

            #region Setup floating islands

            // TODO: detect collision

            var spawn = new SpawnIsland
            {
                X = Main.spawnTileX,
                Y = Main.spawnTileY
            };
            spawn.PlaceTiles();

            var jungle = new JungleIsland
            {
                X = (int)(Main.maxTilesX * 0.5 - dungeonDirection * Main.rand.Next(250, 350)),
                Y = Main.spawnTileY
            };
            jungle.PlaceTiles();

            var snow = new SnowIsland
            {
                X = (int)(Main.maxTilesX * 0.5 + dungeonDirection * Main.rand.Next(250, 350)),
                Y = Main.spawnTileY
            };
            snow.PlaceTiles();

            var sandstone = new SandstoneIsland
            {
                X = (Main.rand.Next(2) == 0 ? jungle.X : snow.X) + Main.rand.Next(-50, 50),
                Y = (int)Main.rockLayer - Main.rand.Next(50, 150)
            };
            sandstone.PlaceTiles();

            var ocean = new OceanIsland
            {
                // The same direction as Dungeon, and opposite the spawn point on celebrationmk10 world.
                X = (int)(Main.maxTilesX * 0.5 * (1 + dungeonDirection)) - dungeonDirection * Main.rand.Next(150, 250),
                Y = Main.spawnTileY
            };
            ocean.PlaceTiles();

            var cavern = new CavernIsland
            {
                X = (int)(Main.maxTilesX * 0.5) + Main.rand.Next(-100, 100),
                Y = Main.UnderworldLayer - Main.rand.Next(50, 150)
            };
            cavern.PlaceTiles();

            var sky = new SkyIsland
            {
                X = (int)(Main.maxTilesX * (Main.rand.Next(2) == 0 ? 0.2 : 0.8)) + Main.rand.Next(-100, 100),
                Y = (int)(Main.worldSurface * 0.5) + Main.rand.Next(-50, 50)
            };
            sky.PlaceTiles();

            var altar = new AltarIsland
            {
                X = templeX + Main.rand.Next(-100, 100),
                Y = Main.spawnTileY
            };
            altar.PlaceTiles();

            #endregion

            #region Setup chest loots

            jungle.AddChestItem(ItemID.StaffofRegrowth);
            jungle.AddChestItem(ItemID.HiveWand);
            jungle.AddChestItem(ItemID.BugNet);

            snow.AddChestItem(ItemID.IceSkates);
            snow.AddChestItem(ItemID.SnowGlobe, stack: 10);

            sandstone.AddChestItem(ItemID.MagicConch);
            sandstone.AddChestItem(ItemID.CatBast);

            ocean.AddChestItem(ItemID.WaterWalkingBoots);
            ocean.AddChestItem(ItemID.GoldenFishingRod);
            ocean.AddChestItem(ItemID.JourneymanBait, stack: 20);
            ocean.AddChestItem(ItemID.LifeCrystal, stack: 5);

            cavern.AddChestItem(ItemID.BandofRegeneration);
            cavern.AddChestItem(ItemID.LuckyHorseshoe);
            cavern.AddChestItem(ItemID.ShoeSpikes);
            cavern.AddChestItem(ItemID.Mace);
            cavern.AddChestItem(ItemID.FlareGun);
            cavern.AddChestItem(ItemID.Flare, stack: Main.rand.Next(25, 50));
            cavern.AddChestItem(ItemID.SuspiciousLookingEye, stack: 10);
            cavern.AddChestItem(ItemID.LifeCrystal, stack: 5);

            sky.AddChestItem(ItemID.CreativeWings);
            sky.AddChestItem(ItemID.ShinyRedBalloon);
            sky.AddChestItem(ItemID.Starfury);
            sky.AddChestItem(ItemID.LifeCrystal, stack: 5);

            // Boots and double-jump accessory
            switch (Main.rand.Next(3))
            {
                case 0:
                    jungle.AddChestItem(ItemID.HermesBoots);
                    jungle.AddChestItem(ItemID.CloudinaBottle);
                    break;

                case 1:
                    snow.AddChestItem(ItemID.FlurryBoots);
                    snow.AddChestItem(ItemID.BlizzardinaBottle);
                    break;

                case 2:
                    sandstone.AddChestItem(ItemID.SandBoots);
                    sandstone.AddChestItem(ItemID.SandstorminaBottle);
                    break;

                default:
                    break;
            }

            // Magic mirror
            switch (Main.rand.Next(2))
            {
                case 0:
                    cavern.AddChestItem(ItemID.MagicMirror);
                    break;

                case 1:
                    snow.AddChestItem(ItemID.IceMirror);
                    break;

                default:
                    break;
            }

            #endregion
        }
    }

    abstract class FloatingIsland
    {
        public int X { get; set; }
        public int Y { get; set; }

        private int chestIndex = -1;
        private int chestNextSlot = -1;

        protected FloatingIsland()
        {
            X = 0;
            Y = 0;
        }

        protected void PlaceTile(int xoffset, int yoffset, int type, int style = 0) => PlaceTile(new[] { xoffset }, new[] { yoffset }, type, style);
        protected void PlaceTile(int[] xoffsets, int yoffset, int type, int style = 0) => PlaceTile(xoffsets, new[] { yoffset }, type, style);
        protected void PlaceTile(int xoffset, int[] yoffsets, int type, int style = 0) => PlaceTile(new[] { xoffset }, yoffsets, type, style);
        protected void PlaceTile(int[] xoffsets, int[] yoffsets, int type, int style = 0)
        {
            foreach (var i in xoffsets)
            {
                foreach (var j in yoffsets)
                {
                    WorldGen.PlaceTile(X + i, Y + j, Type: type, style: style);
                }
            }
        }

        protected void PlaceWall(int xoffset, int yoffset, int type) => PlaceWall(new[] { xoffset }, new[] { yoffset }, type);
        protected void PlaceWall(int[] xoffsets, int yoffset, int type) => PlaceWall(xoffsets, new[] { yoffset }, type);
        protected void PlaceWall(int xoffset, int[] yoffsets, int type) => PlaceWall(new[] { xoffset }, yoffsets, type);
        protected void PlaceWall(int[] xoffsets, int[] yoffsets, int type)
        {
            foreach (var i in xoffsets)
            {
                foreach (var j in yoffsets)
                {
                    WorldGen.PlaceWall(X + i, Y + j, type);
                }
            }
        }

        protected void PlaceLiquid(int i, int j, byte type = LiquidID.Water, byte amount = 255)
        {
            WorldGen.PlaceLiquid(X + i, Y + j, liquidType: type, amount: amount);
        }

        protected void PlaceChest(int i, int j, ushort type = TileID.Containers, int style = 0)
        {
            int num = WorldGen.PlaceChest(X + i, Y + j, type: type, style: style);
            if (num != -1)
            {
                chestIndex = num;
                chestNextSlot = 0;
            }
        }

        public void AddChestItem(int type, int stack = 1, int prefix = 0)
        {
            var chest = Main.chest[chestIndex];
            chest.item[chestNextSlot] = new Item(type, stack, prefix);
            chestNextSlot++;
        }
    }

    sealed class SpawnIsland : FloatingIsland
    {
        //
        //         s s s c c p p
        //         s s s c c p p   t
        // -   x x s s s x x x x   x
        // 0   * x x x x x x x x x x * *
        // +   * * x x x x x x x * * *
        //       * * * x x i i i * *
        //         * * * i i i i *
        //           * * * i i * *
        //               * * *
        //
        //             - 0 +

        public void PlaceTiles()
        {
            PlaceTile(new[] { -5, -4,             0, 1, 2, 3,    5 }, -1, TileID.Dirt);
            PlaceTile(new[] {     -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 },  0, TileID.Dirt);
            PlaceTile(new[] {         -3, -2, -1, 0, 1, 2, 3       },  1, TileID.Dirt);
            PlaceTile(new[] {                 -1, 0                },  2, TileID.Dirt);

            var baseBlockType = WorldGen.crimson ? TileID.FleshBlock : TileID.LesionBlock;
            PlaceTile(new[] { -5,                                   6, 7 }, 0, baseBlockType);
            PlaceTile(new[] { -5, -4,                         4, 5, 6    }, 1, baseBlockType);
            PlaceTile(new[] {     -4, -3, -2,                 4, 5       }, 2, baseBlockType);
            PlaceTile(new[] {         -3, -2, -1,             4          }, 3, baseBlockType);
            PlaceTile(new[] {             -2, -1, 0,       3, 4          }, 4, baseBlockType);
            PlaceTile(new[] {                     0, 1, 2                }, 5, baseBlockType);

            PlaceTile(new[] {    1, 2, 3 }, 2, WorldGen.SavedOreTiers.Iron);
            PlaceTile(new[] { 0, 1, 2, 3 }, 3, WorldGen.SavedOreTiers.Iron);
            PlaceTile(new[] {    1, 2    }, 4, WorldGen.SavedOreTiers.Iron);

            PlaceTile(-2, -1, TileID.Solidifier);
            PlaceTile( 3, -2, TileID.CookingPots);
            PlaceTile( 5, -2, TileID.Torches, style: TorchID.Torch);

            #region Place chest

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

            PlaceChest(0, -2, chestType, chestStyle);

            if (WorldGen.dontStarveWorldGen)
            {
                AddChestItem(ItemID.Teacup, stack: 5);
            }

            #endregion

            if (Main.tenthAnniversaryWorld)
            {
                BirthdayParty.GenuineParty = true;
                BirthdayParty.PartyDaysOnCooldown = 5;

                var andrew = NPC.NewNPC(X * 16, Y * 16, NPCID.Guide);
                Main.npc[andrew].GivenName = Language.GetTextValue("GuideNames.Andrew");
                Main.npc[andrew].homeless = true;
                Main.npc[andrew].homeTileX = X;
                Main.npc[andrew].homeTileY = Y;
                Main.npc[andrew].direction = 1;
                BirthdayParty.CelebratingNPCs.Add(andrew);

                var whitney = NPC.NewNPC(X * 16, Y * 16, NPCID.Steampunker);
                Main.npc[whitney].GivenName = Language.GetTextValue("SteampunkerNames.Whitney");
                Main.npc[whitney].homeless = true;
                Main.npc[whitney].homeTileX = X;
                Main.npc[whitney].homeTileY = Y;
                Main.npc[whitney].direction = 1;
                BirthdayParty.CelebratingNPCs.Add(whitney);

                var yorai = NPC.NewNPC(X * 16, Y * 16, NPCID.Princess);
                Main.npc[yorai].GivenName = Language.GetTextValue("PrincessNames.Yorai");
                Main.npc[yorai].homeless = true;
                Main.npc[yorai].homeTileX = X;
                Main.npc[yorai].homeTileY = Y;
                Main.npc[yorai].direction = 1;
                BirthdayParty.CelebratingNPCs.Add(yorai);

                var organizer = NPC.NewNPC(X * 16, Y * 16, NPCID.PartyGirl);
                Main.npc[organizer].homeless = true;
                Main.npc[organizer].homeTileX = X;
                Main.npc[organizer].homeTileY = Y;
                Main.npc[organizer].direction = 1;
                BirthdayParty.CelebratingNPCs.Add(organizer);

                var bunny = NPC.NewNPC(X * 16, Y * 16, NPCID.TownBunny);
                Main.npc[bunny].homeless = true;
                Main.npc[bunny].homeTileX = X;
                Main.npc[bunny].homeTileY = Y;
                Main.npc[bunny].direction = 1;
                Main.npc[bunny].townNpcVariationIndex = 1;
                NPC.boughtBunny = true;
            }
            else if (Main.getGoodWorld)
            {
                var guide = NPC.NewNPC(X * 16, Y * 16, NPCID.Demolitionist);
                Main.npc[guide].homeless = true;
                Main.npc[guide].homeTileX = X;
                Main.npc[guide].homeTileY = Y;
                Main.npc[guide].direction = 1;
            }
            else if (Main.drunkWorld)
            {
                var guide = NPC.NewNPC(X * 16, Y * 16, NPCID.PartyGirl);
                Main.npc[guide].homeless = true;
                Main.npc[guide].homeTileX = X;
                Main.npc[guide].homeTileY = Y;
                Main.npc[guide].direction = 1;
            }
            else if (Main.notTheBeesWorld)
            {
                var guide = NPC.NewNPC(X * 16, Y * 16, NPCID.Merchant);
                Main.npc[guide].homeless = true;
                Main.npc[guide].homeTileX = X;
                Main.npc[guide].homeTileY = Y;
                Main.npc[guide].direction = 1;
            }
            else
            {
                var guide = NPC.NewNPC(X * 16, Y * 16, NPCID.Guide);
                Main.npc[guide].homeless = true;
                Main.npc[guide].homeTileX = X;
                Main.npc[guide].homeTileY = Y;
                Main.npc[guide].direction = 1;
            }
        }
    }

    sealed class JungleIsland : FloatingIsland
    {
        //             l                 l
        //             l   s s           l
        //       x x x l   s s c c       l x x
        // 0 x x x x x x x s s c c     x x x x x x
        //   x x x x x x x x x x x w x x x x x x x
        //           x x x x x x x x x x x x x x
        //               x x x x x x x x   x
        //                   0

        public void PlaceTiles()
        {
            PlaceTile(new[] {         -6, -5, -4,                                  7, 8        }, -1, TileID.Mud);
            PlaceTile(new[] { -8, -7, -6, -5, -4, -3, -2,                    5, 6, 7, 8, 9, 10 },  0, TileID.Mud);
            PlaceTile(new[] { -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2,    4, 5, 6, 7, 8, 9, 10 },  1, TileID.Mud);
            PlaceTile(new[] {                 -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9     },  2, TileID.Mud);
            PlaceTile(new[] {                         -2, -1, 0, 1, 2, 3, 4, 5,    7           },  3, TileID.Mud);

            PlaceTile(new[] {         -6, -5, -4,                                  7, 8        }, -1, TileID.JungleGrass);
            PlaceTile(new[] { -8, -7, -6,     -4, -3, -2,                    5, 6, 7, 8, 9, 10 },  0, TileID.JungleGrass);
            PlaceTile(new[] { -8, -7, -6, -5, -4,     -2, -1, 0, 1, 2,    4, 5,          9, 10 },  1, TileID.JungleGrass);
            PlaceTile(new[] {                 -4, -3, -2,           2, 3, 4, 5, 6, 7, 8, 9     },  2, TileID.JungleGrass);
            PlaceTile(new[] {                         -2, -1, 0, 1, 2, 3, 4, 5,    7           },  3, TileID.JungleGrass);

            PlaceLiquid(3, 1, WorldGen.getGoodWorldGen ? (byte)LiquidID.Lava : WorldGen.notTheBees ? (byte)LiquidID.Honey : (byte)LiquidID.Water);

            // Hornet Statue
            PlaceTile(-1, 0, TileID.Statues, style: 16);

            // Rich Mahogany Lamp
            PlaceTile(-3, -1, TileID.Lamps, style: 6);
            PlaceTile( 6, -1, TileID.Lamps, style: 6);

            // Ivy Chest
            PlaceChest(1, 0, type: TileID.Containers, style: 10);
        }
    }

    sealed class SnowIsland : FloatingIsland
    {
        //         l               l
        //         l               l
        //         l   s s         l
        // -   x x x   s s c c     x x
        // 0 x x x x x s s c c x x x x x x
        // +     x x x x x x x x x x x x
        //           x x x x x x x x
        //           x x x x x
        //             - 0 +

        public void PlaceTiles()
        {
            PlaceTile(new[] {     -5, -4, -3,                        5, 6        }, -1, TileID.SnowBlock);
            PlaceTile(new[] { -6, -5, -4, -3, -2,              3, 4, 5, 6, 7, 8  },  0, TileID.SnowBlock);
            PlaceTile(new[] {         -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7     },  1, TileID.SnowBlock);
            PlaceTile(new[] {                 -2, -1, 0, 1, 2, 3, 4, 5           },  2, TileID.SnowBlock);
            PlaceTile(new[] {                 -2, -1, 0, 1, 2                    },  3, TileID.SnowBlock);

            // Undead Viking Statue
            PlaceTile(-1, 0, TileID.Statues, style:68);

            // Boreal Wood Lamp
            PlaceTile(-3, -2, TileID.Lamps, style: 20);
            PlaceTile( 5, -2, TileID.Lamps, style: 20);

            // Frozen Chest
            PlaceChest(1, 0, type: TileID.Containers, style: 11);
        }
    }

    sealed class AltarIsland : FloatingIsland
    {
        //
        //         a a a
        // - d x x a a a   x x
        // 0 x x x x x x x x
        // +     x x x x   x
        //           x
        //         - 0 +

        public void PlaceTiles()
        {
            var type = WorldGen.crimson ? TileID.Crimstone : TileID.Ebonstone;
            PlaceTile(new[] {     -3, -2,              3, 4 }, -1, type);
            PlaceTile(new[] { -4, -3, -2, -1, 0, 1, 2, 3    },  0, type);
            PlaceTile(new[] {         -2, -1, 0, 1,    3    },  1, type);
            PlaceTile(new[] {                 0             },  2, type);

            PlaceTile(-4, -1, TileID.Dirt);
            PlaceTile(-4, -1, WorldGen.crimson ? TileID.CrimsonGrass : TileID.CorruptGrass);

            PlaceTile(0, -1, TileID.DemonAltar, style: WorldGen.crimson ? 1 : 0);
        }
    }

    sealed class SandstoneIsland : FloatingIsland
    {
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

        public void PlaceTiles()
        {
            PlaceTile(new[] {                                       6, 7 }, -1, TileID.Sandstone);
            PlaceTile(new[] {         -3, -2,                 4, 5, 6, 7 },  0, TileID.Sandstone);
            PlaceTile(new[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5       },  1, TileID.Sandstone);
            PlaceTile(new[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4          },  2, TileID.Sandstone);
            PlaceTile(new[] {     -4, -3, -2, -1, 0, 1, 2, 3             },  3, TileID.Sandstone);
            PlaceTile(new[] {         -3,     -1, 0, 1, 2                },  4, TileID.Sandstone);
            PlaceTile(new[] {                 -1, 0, 1                   },  5, TileID.Sandstone);
            PlaceTile(new[] {                 -1,    1                   },  6, TileID.Sandstone);

            PlaceTile(-2, -1, TileID.Lamps, style: 38); // Sandstone Lamp
            PlaceTile(4, -1, TileID.Lamps, style: 38);

            PlaceTile(2, 0, TileID.Extractinator);

            PlaceChest(-1, 0, type: TileID.Containers2, style: 10); // Sandstone Chest
        }
    }

    sealed class OceanIsland : FloatingIsland
    {
        //             s s
        //     t       s s c c         t
        // - x x x x x s s c c x x x x x x
        // 0 x x x x x x x x x x x x x x h
        // + h h x x x x x x x x x x h h h
        //     h h h h x x x x x h h h
        //           h h h h h h h
        //             - 0 +

        public void PlaceTiles()
        {
            PlaceTile(new[] { -6, -5, -4, -3, -2,              3, 4, 5, 6, 7, 8 }, -1, TileID.Sand);
            PlaceTile(new[] { -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7    },  0, TileID.Sand);
            PlaceTile(new[] {         -4, -3, -2, -1, 0, 1, 2, 3, 4, 5          },  1, TileID.Sand);
            PlaceTile(new[] {                     -1, 0, 1, 2, 3                },  2, TileID.Sand);

            PlaceTile(new[] {                                                 8 }, 0, TileID.HardenedSand);
            PlaceTile(new[] { -6, -5,                                   6, 7, 8 }, 1, TileID.HardenedSand);
            PlaceTile(new[] {     -5, -4, -3, -2,                 4, 5, 6       }, 2, TileID.HardenedSand);
            PlaceTile(new[] {                 -2, -1, 0, 1, 2, 3, 4             }, 3, TileID.HardenedSand);

            PlaceTile(-1, -1, TileID.Statues, style: 5); // Goblin Statue

            PlaceTile(-5, -2, TileID.Torches, style: 17); // Coral Torch
            PlaceTile(7, -2, TileID.Torches, style: 17);

            PlaceChest(1, -1, type: TileID.Containers, style: 17); // Water Chest
        }
    }

    sealed class CavernIsland : FloatingIsland
    {
        //           s s
        // -     l   s s c c   l
        // 0     l   s s c c   l
        // +     l   x x x x x l x 
        //   M x x x x g m m x x x m
        //   x x x g g g     m m x m m
        //   g g g g g         m m m
        //         g
        //             - 0 +

        public void PlaceTiles()
        {
            PlaceWall(new[] {             -2, -1                }, -4, WallID.SpiderUnsafe);
            PlaceWall(new[] {         -3, -2, -1, 0, 1          }, -3, WallID.SpiderUnsafe);
            PlaceWall(new[] {     -4, -3, -2, -1, 0, 1, 2, 3    }, -2, WallID.SpiderUnsafe);
            PlaceWall(new[] {     -4, -3, -2, -1, 0, 1, 2, 3    }, -1, WallID.SpiderUnsafe);
            PlaceWall(new[] {     -4, -3, -2, -1, 0, 1, 2, 3, 4 },  0, WallID.SpiderUnsafe);
            PlaceWall(new[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4 },  1, WallID.SpiderUnsafe);
            PlaceWall(new[] {     -4, -3,               2,      },  2, WallID.SpiderUnsafe);

            var type = WorldGen.notTheBees ? TileID.CrispyHoneyBlock : TileID.Stone;
            PlaceTile(new[] {                 -2, -1, 0, 1, 2       }, 1, type);
            PlaceTile(new[] {     -5, -4, -3, -2,           2, 3, 4 }, 2, type);
            PlaceTile(new[] { -6, -5, -4,                         4 }, 3, type);

            PlaceTile(new[] {                     -1 }, 2, TileID.Granite);
            PlaceTile(new[] {             -3, -2, -1 }, 3, TileID.Granite);
            PlaceTile(new[] { -6, -5, -4, -3, -2     }, 4, TileID.Granite);
            PlaceTile(new[] {             -3         }, 5, TileID.Granite);

            PlaceTile(new[] { 0, 1                }, 2, TileID.Marble);
            PlaceTile(new[] {    1, 2, 3,    5, 6 }, 3, TileID.Marble);
            PlaceTile(new[] {          3, 4, 5    }, 4, TileID.Marble);

            PlaceTile(-6, 2, TileID.Mud);
            PlaceTile(-6, 2, TileID.MushroomGrass);

            // Heart Statue
            PlaceTile(-2, 0, TileID.Statues, style: 37);

            // Spider Lamp
            PlaceTile(-4, 1, TileID.Lamps, style: 32);
            PlaceTile( 3, 1, TileID.Lamps, style: 32);

            // Spider Chest
            PlaceChest(0, 0, type: TileID.Containers2, style: 2);
        }
    }

    sealed class SkyIsland : FloatingIsland
    {
        //                         l                 l
        //                         l     s s         l
        //                         l     s s c c     l
        // -                     x x x   s s c c   x x x
        // 0 d d d     d d d x x x x x x x x x x x x x x x d d d d d d
        // +   d d d d d d d d d x x x x x x x x x x x d d d d d d d d d d
        //         d d d d d d d d d d d d x x d d d d d d d d d d d d d d d
        //           d d d d d d d d d d d d d d d d d d d d d d d
        //                   d d d d d d d d d d d d d d d d
        //                             d d d d d d d d d 
        //                                 - 0 +
        
        public void PlaceTiles()
        {
            PlaceTile(new[] {         -6, -5, -4,                      3, 4, 5    }, -1, TileID.Sunplate);
            PlaceTile(new[] { -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6 },  0, TileID.Sunplate);
            PlaceTile(new[] {         -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4       },  1, TileID.Sunplate);
            PlaceTile(new[] {                             -1, 0                   },  2, TileID.Sunplate);

            PlaceTile(new[] { -16, -15, -14,           -11, -10, -9,                                                      7, 8, 9, 10, 11, 12             }, 0, TileID.Cloud);
            PlaceTile(new[] {      -15, -14, -13, -12, -11, -10, -9, -8, -7,                                        5, 6, 7, 8, 9, 10, 11, 12, 13, 14     }, 1, TileID.Cloud);
            PlaceTile(new[] {                -13, -12, -11, -10, -9, -8, -7, -6, -5, -4, -3, -2,        1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 2, TileID.Cloud);
            PlaceTile(new[] {                     -12, -11, -10, -9, -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10                     }, 3, TileID.Cloud);
            PlaceTile(new[] {                                        -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7                               }, 4, TileID.Cloud);
            PlaceTile(new[] {                                                            -3, -2, -1, 0, 1, 2, 3, 4, 5                                     }, 5, TileID.Cloud);

            // Harpy Statue
            PlaceTile(-2, -1, TileID.Statues, style: 70);

            // Skyware Lamp
            PlaceTile(-5, -2, TileID.Lamps, style: 9);
            PlaceTile( 4, -2, TileID.Lamps, style: 9);

            // Skyware Chest
            PlaceChest(0, -1, type: TileID.Containers, style: 13);
        }
    }
}
