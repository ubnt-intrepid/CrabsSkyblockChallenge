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

            var spawn = new SpawnIsland(
                x: Main.spawnTileX,
                y: Main.spawnTileY
            );
            spawn.PlaceTiles();

            var jungle = new JungleIsland(
                x: (int)(Main.maxTilesX * 0.5 - dungeonDirection * Main.rand.Next(250, 350)),
                y: Main.spawnTileY
            );
            jungle.PlaceTiles();

            var snow = new SnowIsland(
                x: (int)(Main.maxTilesX * 0.5 + dungeonDirection * Main.rand.Next(250, 350)),
                y: Main.spawnTileY
            );
            snow.PlaceTiles();

            var altar = new AltarIsland(
                x: templeX + Main.rand.Next(-100, 100),
                y: Main.spawnTileY
            );
            altar.PlaceTiles();

            var sandstone = new SandstoneIsland(
                x: (Main.rand.Next(2) == 0 ? jungle.X : snow.X) + Main.rand.Next(-50, 50),
                y: (int)Main.rockLayer - Main.rand.Next(50, 150)
            );
            sandstone.PlaceTiles();

            var ocean = new OceanIsland(
                x: Main.rand.Next(2) == 0 ? Main.rand.Next(150, 250) : Main.maxTilesX - Main.rand.Next(150, 200),
                y: Main.spawnTileY
            );
            ocean.PlaceTiles();

            var cavern = new CavernIsland(
                x: (int)(Main.maxTilesX * 0.5) + Main.rand.Next(-100, 100),
                y: Main.UnderworldLayer - Main.rand.Next(50, 150)
            );
            cavern.PlaceTiles();

            var granite = new GraniteIsland(
                x: (int)(Main.maxTilesX * 0.5) - Main.rand.Next(350, 450),
                y: (int)(Main.rockLayer + Main.rand.Next(150, 200))
            );
            granite.PlaceTiles();

            var marble = new MarbleIsland(
                x: (int)(Main.maxTilesX * 0.5) + Main.rand.Next(350, 450),
                y: (int)(Main.rockLayer + Main.rand.Next(150, 200))
            );
            marble.PlaceTiles();

            var sky = new SkyIsland(
                x: (int)(Main.maxTilesX * (Main.rand.Next(2) == 0 ? 0.2 : 0.8)) + Main.rand.Next(-100, 100),
                y: (int)(Main.worldSurface * 0.5) + Main.rand.Next(-50, 50)
            );
            sky.PlaceTiles();
        }
    }

    abstract class FloatingIsland
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        protected FloatingIsland(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        protected void PlaceTile(int i, int j, int type, int style = 0, int paintColor = 0)
        {
            if (type is (TileID.JungleGrass or TileID.MushroomGrass))
            {
                WorldGen.PlaceTile(X + i, Y + j, Type: TileID.Mud);
            }
            else if (type is (TileID.Grass or TileID.CorruptGrass or TileID.CrimsonGrass or TileID.HallowedGrass))
            {
                WorldGen.PlaceTile(X + i, Y + j, Type: TileID.Dirt);
            }
            WorldGen.PlaceTile(X + i, Y + j, Type: type, style: style);

            if (paintColor != 0)
            {
                WorldGen.paintColor(paintColor);
            }
        }

        protected void PlaceWall(int i, int j, int type)
        {
            WorldGen.PlaceWall(X + i, Y + j, type);
        }

        protected void PlaceLiquid(int i, int j, byte type = LiquidID.Water, byte amount = 255)
        {
            WorldGen.PlaceLiquid(X + i, Y + j, liquidType: type, amount: amount);
        }

        protected Chest PlaceChest(int i, int j, ushort type = 21, int style = 0)
        {
            int num = WorldGen.PlaceChest(X + i, Y + j, type: type, style: style);
            return num != 0 ? Main.chest[num] : null;
        }
    }

    sealed class SpawnIsland : FloatingIsland
    {

        public SpawnIsland(int x, int y) : base(x, y)
        {
        }

        public void PlaceTiles()
        {
            //
            //         s s s   c c
            //         s s s   c c
            // -   x x s s s x x x x   x
            // 0   x x x x x x x x x x x
            // +       x x x x x x x
            //             x x t
            //
            //             - 0 +

            var paintColor = WorldGen.tenthAnniversaryWorldGen ? PaintID.DeepPinkPaint : 0;

            PlaceTile(-5, -1, TileID.Dirt, paintColor: paintColor);
            PlaceTile(-4, -1, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 0, -1, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 1, -1, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 2, -1, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 3, -1, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 5, -1, TileID.Dirt, paintColor: paintColor);  

            PlaceTile(-5, 0, TileID.Dirt, paintColor: paintColor);
            PlaceTile(-4, 0, TileID.Dirt, paintColor: paintColor);
            PlaceTile(-3, 0, TileID.Dirt, paintColor: paintColor);
            PlaceTile(-2, 0, TileID.Dirt, paintColor: paintColor);
            PlaceTile(-1, 0, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 0, 0, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 1, 0, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 2, 0, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 3, 0, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 4, 0, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 5, 0, TileID.Dirt, paintColor: paintColor);

            PlaceTile(-3, 1, TileID.Dirt, paintColor: paintColor);
            PlaceTile(-2, 1, TileID.Dirt, paintColor: paintColor);
            PlaceTile(-1, 1, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 0, 1, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 1, 1, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 2, 1, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 3, 1, TileID.Dirt, paintColor: paintColor);

            PlaceTile(-1, 2, TileID.Dirt, paintColor: paintColor);
            PlaceTile( 0, 2, TileID.Dirt, paintColor: paintColor);

            PlaceTile(2, 1, TileID.Solidifier);
            PlaceTile(1, 2, TileID.Torches, style: TorchID.Torch);

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

            var chest = PlaceChest(1, -2, chestType, chestStyle);
            if (chest != null)
            {
                int nextSlot = 0;
                chest.item[nextSlot++] = new Item(WorldGen.crimson ? ItemID.FleshBlock : ItemID.LesionBlock, stack: 25);
                chest.item[nextSlot++] = new Item(WorldGen.SavedOreTiers.Iron == TileID.Lead ? ItemID.LeadOre : ItemID.IronOre, stack: 9);
            }

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

        public JungleIsland(int x, int y) : base(x, y)
        {
        }

        public void PlaceTiles()
        {
            PlaceTile(-6, -1, TileID.JungleGrass);
            PlaceTile(-5, -1, TileID.JungleGrass);
            PlaceTile(-4, -1, TileID.JungleGrass);
            PlaceTile( 7, -1, TileID.JungleGrass);
            PlaceTile( 8, -1, TileID.JungleGrass);

            PlaceTile(-8, 0, TileID.JungleGrass);
            PlaceTile(-7, 0, TileID.JungleGrass);
            PlaceTile(-6, 0, TileID.Mud);
            PlaceTile(-5, 0, TileID.Mud);
            PlaceTile(-4, 0, TileID.Mud);
            PlaceTile(-3, 0, TileID.Mud);
            PlaceTile(-2, 0, TileID.JungleGrass);
            PlaceTile( 5, 0, TileID.JungleGrass);
            PlaceTile( 6, 0, TileID.JungleGrass);
            PlaceTile( 7, 0, TileID.Mud);
            PlaceTile( 8, 0, TileID.Mud);
            PlaceTile( 9, 0, TileID.JungleGrass);
            PlaceTile(10, 0, TileID.JungleGrass);

            PlaceTile(-8, 1, TileID.JungleGrass);
            PlaceTile(-7, 1, TileID.JungleGrass);
            PlaceTile(-6, 1, TileID.JungleGrass);
            PlaceTile(-5, 1, TileID.JungleGrass);
            PlaceTile(-4, 1, TileID.Mud);
            PlaceTile(-3, 1, TileID.Mud);
            PlaceTile(-2, 1, TileID.Mud);
            PlaceTile(-1, 1, TileID.Mud);
            PlaceTile( 0, 1, TileID.Mud);
            PlaceTile( 1, 1, TileID.Mud);
            PlaceTile( 2, 1, TileID.Mud);
            PlaceTile( 4, 1, TileID.Mud);
            PlaceTile( 5, 1, TileID.Mud);
            PlaceTile( 6, 1, TileID.Mud);
            PlaceTile( 7, 1, TileID.Mud);
            PlaceTile( 8, 1, TileID.Mud);
            PlaceTile( 9, 1, TileID.Mud);
            PlaceTile(10, 1, TileID.JungleGrass);

            PlaceTile(-4, 2, TileID.JungleGrass);
            PlaceTile(-3, 2, TileID.JungleGrass);
            PlaceTile(-2, 2, TileID.Mud);
            PlaceTile(-1, 2, TileID.Mud);
            PlaceTile( 0, 2, TileID.Mud);
            PlaceTile( 1, 2, TileID.Mud);
            PlaceTile( 2, 2, TileID.Mud);
            PlaceTile( 3, 2, TileID.Mud);
            PlaceTile( 4, 2, TileID.Mud);
            PlaceTile( 5, 2, TileID.Mud);
            PlaceTile( 6, 2, TileID.Mud);
            PlaceTile( 7, 2, TileID.Mud);
            PlaceTile( 8, 2, TileID.JungleGrass);
            PlaceTile( 9, 2, TileID.JungleGrass);

            PlaceTile(-2, 3, TileID.JungleGrass);
            PlaceTile(-1, 3, TileID.JungleGrass);
            PlaceTile( 0, 3, TileID.JungleGrass);
            PlaceTile( 1, 3, TileID.JungleGrass);
            PlaceTile( 2, 3, TileID.JungleGrass);
            PlaceTile( 3, 3, TileID.JungleGrass);
            PlaceTile( 4, 3, TileID.JungleGrass);
            PlaceTile( 5, 3, TileID.JungleGrass);
            PlaceTile( 7, 3, TileID.JungleGrass);

            PlaceLiquid(3, 1, (byte)(Main.getGoodWorld ? LiquidID.Lava : LiquidID.Water));

            PlaceTile(1, 0, TileID.Statues, style: 16); // Hornet Statue
            PlaceTile(3, -1, TileID.Lamps, style: 6); // Rich Mahogany Lamp
            PlaceTile(6, -1, TileID.Lamps, style: 6);

            var chest = PlaceChest(1, 0, type: TileID.Containers, style: 10); // Ivy Chest
            if (chest != null)
            {
                int nextSlot = 0;
                chest.item[nextSlot++] = new Item(ItemID.StaffofRegrowth);
                chest.item[nextSlot++] = new Item(ItemID.HiveWand);
                chest.item[nextSlot++] = new Item(ItemID.BugNet);
                chest.item[nextSlot++] = new Item(ItemID.LifeCrystal, stack: 3);
            }
        }
    }

    sealed class SnowIsland : FloatingIsland
    {
        //         l                   l
        //         l                   l
        //         l   m m m s s c c   l
        // -   x x x   m m m s s c c   x x x
        // 0 x x x x x m m m s s x x x x x x x
        // +     x x x x x x x x x x x x x
        //           x x x x x x x x x
        //           x x x x x
        //             - 0 +

        public SnowIsland(int x, int y) : base(x, y)
        {
        }

        public void PlaceTiles()
        {
            PlaceTile(-5, -1, TileID.SnowBlock);
            PlaceTile(-4, -1, TileID.SnowBlock);
            PlaceTile(-3, -1, TileID.SnowBlock);
            PlaceTile( 7, -1, TileID.SnowBlock);
            PlaceTile( 8, -1, TileID.SnowBlock);
            PlaceTile( 9, -1, TileID.SnowBlock);

            PlaceTile(-6, 0, TileID.SnowBlock);
            PlaceTile(-5, 0, TileID.SnowBlock);
            PlaceTile(-4, 0, TileID.SnowBlock);
            PlaceTile(-3, 0, TileID.SnowBlock);
            PlaceTile(-2, 0, TileID.SnowBlock);
            PlaceTile( 4, 0, TileID.SnowBlock);
            PlaceTile( 5, 0, TileID.SnowBlock);
            PlaceTile( 6, 0, TileID.SnowBlock);
            PlaceTile( 7, 0, TileID.SnowBlock);
            PlaceTile( 8, 0, TileID.SnowBlock);
            PlaceTile( 9, 0, TileID.SnowBlock);
            PlaceTile(10, 0, TileID.SnowBlock);

            PlaceTile(-4, 1, TileID.SnowBlock);
            PlaceTile(-3, 1, TileID.SnowBlock);
            PlaceTile(-2, 1, TileID.SnowBlock);
            PlaceTile(-1, 1, TileID.SnowBlock);
            PlaceTile( 0, 1, TileID.SnowBlock);
            PlaceTile( 1, 1, TileID.SnowBlock);
            PlaceTile( 2, 1, TileID.SnowBlock);
            PlaceTile( 3, 1, TileID.SnowBlock);
            PlaceTile( 4, 1, TileID.SnowBlock);
            PlaceTile( 5, 1, TileID.SnowBlock);
            PlaceTile( 6, 1, TileID.SnowBlock);
            PlaceTile( 7, 1, TileID.SnowBlock);
            PlaceTile( 8, 1, TileID.SnowBlock);

            PlaceTile(-2, 2, TileID.SnowBlock);
            PlaceTile(-1, 2, TileID.SnowBlock);
            PlaceTile( 0, 2, TileID.SnowBlock);
            PlaceTile( 1, 2, TileID.SnowBlock);
            PlaceTile( 2, 2, TileID.SnowBlock);
            PlaceTile( 3, 2, TileID.SnowBlock);
            PlaceTile( 4, 2, TileID.SnowBlock);
            PlaceTile( 5, 2, TileID.SnowBlock);
            PlaceTile( 6, 2, TileID.SnowBlock);

            PlaceTile(-2, 3, TileID.SnowBlock);
            PlaceTile(-1, 3, TileID.SnowBlock);
            PlaceTile( 0, 3, TileID.SnowBlock);
            PlaceTile( 1, 3, TileID.SnowBlock);
            PlaceTile( 2, 3, TileID.SnowBlock);

            PlaceTile(2, 0, TileID.Statues, style:68); // Undead Viking Statue
            PlaceTile(0, 0, TileID.IceMachine);

            PlaceTile(-3, -2, TileID.Lamps, style: 20); // Boreal Wood Lamp
            PlaceTile(7, -2, TileID.Lamps, style: 20);

            var chest = PlaceChest(4, -1, type: TileID.Containers, style: 11); // Frozen Chest
            if (chest != null)
            {
                int nextSlot = 0;
                chest.item[nextSlot++] = new Item(ItemID.IceSkates);
            }
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

        public AltarIsland(int x, int y) : base(x, y)
        {
        }

        public void PlaceTiles()
        {
            var type = WorldGen.crimson ? TileID.Crimstone : TileID.Ebonstone;
            var grassType = WorldGen.crimson ? TileID.CrimsonGrass : TileID.CorruptGrass;

            PlaceTile(-3, -1, type);
            PlaceTile(-2, -1, type);
            PlaceTile( 3, -1, type);
            PlaceTile( 4, -1, type);

            PlaceTile(-4, 0, type);
            PlaceTile(-3, 0, type);
            PlaceTile(-2, 0, type);
            PlaceTile(-1, 0, type);
            PlaceTile( 0, 0, type);
            PlaceTile( 1, 0, type);
            PlaceTile( 2, 0, type);
            PlaceTile( 3, 0, type);

            PlaceTile(-2, 1, type);
            PlaceTile(-1, 1, type);
            PlaceTile( 0, 1, type);
            PlaceTile( 1, 1, type);
            PlaceTile( 3, 1, type);

            PlaceTile(0, 2, type);

            PlaceTile(-4, -1, grassType);

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

        public SandstoneIsland(int x, int y) : base(x, y)
        {
        }

        public void PlaceTiles()
        {
            PlaceTile(6, -1, TileID.Sandstone);
            PlaceTile(7, -1, TileID.Sandstone);

            PlaceTile(-3, 0, TileID.Sandstone);
            PlaceTile(-2, 0, TileID.Sandstone);
            PlaceTile( 4, 0, TileID.Sandstone);
            PlaceTile( 5, 0, TileID.Sandstone);
            PlaceTile( 6, 0, TileID.Sandstone);
            PlaceTile( 7, 0, TileID.Sandstone);

            PlaceTile(-5, 1, TileID.Sandstone);
            PlaceTile(-4, 1, TileID.Sandstone);
            PlaceTile(-3, 1, TileID.Sandstone);
            PlaceTile(-2, 1, TileID.Sandstone);
            PlaceTile(-1, 1, TileID.Sandstone);
            PlaceTile( 0, 1, TileID.Sandstone);
            PlaceTile( 1, 1, TileID.Sandstone);
            PlaceTile( 2, 1, TileID.Sandstone);
            PlaceTile( 3, 1, TileID.Sandstone);
            PlaceTile( 4, 1, TileID.Sandstone);
            PlaceTile( 5, 1, TileID.Sandstone);

            PlaceTile(-5, 2, TileID.Sandstone);
            PlaceTile(-4, 2, TileID.Sandstone);
            PlaceTile(-3, 2, TileID.Sandstone);
            PlaceTile(-2, 2, TileID.Sandstone);
            PlaceTile(-1, 2, TileID.Sandstone);
            PlaceTile( 0, 2, TileID.Sandstone);
            PlaceTile( 1, 2, TileID.Sandstone);
            PlaceTile( 2, 2, TileID.Sandstone);
            PlaceTile( 3, 2, TileID.Sandstone);
            PlaceTile( 4, 2, TileID.Sandstone);

            PlaceTile(-4, 3, TileID.Sandstone);
            PlaceTile(-3, 3, TileID.Sandstone);
            PlaceTile(-2, 3, TileID.Sandstone);
            PlaceTile(-1, 3, TileID.Sandstone);
            PlaceTile( 0, 3, TileID.Sandstone);
            PlaceTile( 1, 3, TileID.Sandstone);
            PlaceTile( 2, 3, TileID.Sandstone);
            PlaceTile( 3, 3, TileID.Sandstone);

            PlaceTile(-3, 4, TileID.Sandstone);
            PlaceTile(-1, 4, TileID.Sandstone);
            PlaceTile( 0, 4, TileID.Sandstone);
            PlaceTile( 1, 4, TileID.Sandstone);
            PlaceTile( 2, 4, TileID.Sandstone);

            PlaceTile(-1, 5, TileID.Sandstone);
            PlaceTile( 0, 5, TileID.Sandstone);
            PlaceTile( 1, 5, TileID.Sandstone);

            PlaceTile(-1, 6, TileID.Sandstone);
            PlaceTile( 1, 6, TileID.Sandstone);

            PlaceTile(-2, -1, TileID.Lamps, style: 38); // Sandstone Lamp
            PlaceTile(4, -1, TileID.Lamps, style: 38);

            PlaceTile(2, 0, TileID.Extractinator);

            var chest = PlaceChest(-1, 0, type: TileID.Containers2, style: 10); // Sandstone Chest
            if (chest != null)
            {
                int nextSlot = 0;
                chest.item[nextSlot++] = new Item(ItemID.HermesBoots);
                chest.item[nextSlot++] = new Item(ItemID.CloudinaBottle);
                chest.item[nextSlot++] = new Item(ItemID.LifeCrystal, stack: 3);
                chest.item[nextSlot++] = new Item(ItemID.SlimeCrown, stack: 10);
            }
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

        public OceanIsland(int x, int y) : base(x, y)
        {
        }

        public void PlaceTiles()
        {
            PlaceTile(-6, -1, TileID.Sand);
            PlaceTile(-5, -1, TileID.Sand);
            PlaceTile(-4, -1, TileID.Sand);
            PlaceTile(-3, -1, TileID.Sand);
            PlaceTile(-2, -1, TileID.Sand);
            PlaceTile( 3, -1, TileID.Sand);
            PlaceTile( 4, -1, TileID.Sand);
            PlaceTile( 5, -1, TileID.Sand);
            PlaceTile( 6, -1, TileID.Sand);
            PlaceTile( 7, -1, TileID.Sand);
            PlaceTile( 8, -1, TileID.Sand);

            PlaceTile(-6, 0, TileID.Sand);
            PlaceTile(-5, 0, TileID.Sand);
            PlaceTile(-4, 0, TileID.Sand);
            PlaceTile(-3, 0, TileID.Sand);
            PlaceTile(-2, 0, TileID.Sand);
            PlaceTile(-1, 0, TileID.Sand);
            PlaceTile( 0, 0, TileID.Sand);
            PlaceTile( 1, 0, TileID.Sand);
            PlaceTile( 2, 0, TileID.Sand);
            PlaceTile( 3, 0, TileID.Sand);
            PlaceTile( 4, 0, TileID.Sand);
            PlaceTile( 5, 0, TileID.Sand);
            PlaceTile( 6, 0, TileID.Sand);
            PlaceTile( 7, 0, TileID.Sand);
            PlaceTile( 8, 0, TileID.HardenedSand);

            PlaceTile(-6, 1, TileID.HardenedSand);
            PlaceTile(-5, 1, TileID.HardenedSand);
            PlaceTile(-4, 1, TileID.Sand);
            PlaceTile(-3, 1, TileID.Sand);
            PlaceTile(-2, 1, TileID.Sand);
            PlaceTile(-1, 1, TileID.Sand);
            PlaceTile( 0, 1, TileID.Sand);
            PlaceTile( 1, 1, TileID.Sand);
            PlaceTile( 2, 1, TileID.Sand);
            PlaceTile( 3, 1, TileID.Sand);
            PlaceTile( 4, 1, TileID.Sand);
            PlaceTile( 5, 1, TileID.Sand);
            PlaceTile( 6, 1, TileID.HardenedSand);
            PlaceTile( 7, 1, TileID.HardenedSand);
            PlaceTile( 8, 1, TileID.HardenedSand);

            PlaceTile(-5, 2, TileID.HardenedSand);
            PlaceTile(-4, 2, TileID.HardenedSand);
            PlaceTile(-3, 2, TileID.HardenedSand);
            PlaceTile(-2, 2, TileID.HardenedSand);
            PlaceTile(-1, 2, TileID.Sand);
            PlaceTile( 0, 2, TileID.Sand);
            PlaceTile( 1, 2, TileID.Sand);
            PlaceTile( 2, 2, TileID.Sand);
            PlaceTile( 3, 2, TileID.Sand);
            PlaceTile( 4, 2, TileID.HardenedSand);
            PlaceTile( 5, 2, TileID.HardenedSand);
            PlaceTile( 6, 2, TileID.HardenedSand);

            PlaceTile(-2, 3, TileID.HardenedSand);
            PlaceTile(-1, 3, TileID.HardenedSand);
            PlaceTile( 0, 3, TileID.HardenedSand);
            PlaceTile( 1, 3, TileID.HardenedSand);
            PlaceTile( 2, 3, TileID.HardenedSand);
            PlaceTile( 3, 3, TileID.HardenedSand);
            PlaceTile( 4, 3, TileID.HardenedSand);

            PlaceTile(-1, -1, TileID.Statues, style: 5); // Goblin Statue
            PlaceTile(-5, -2, TileID.Torches, style: 17); // Coral Torch
            PlaceTile(7, -2, TileID.Torches, style: 17);

            var chest = PlaceChest(1, -1, type: TileID.Containers, style: 17); // Water Chest
            if (chest != null)
            {
                int nextSlot = 0;
                chest.item[nextSlot++] = new Item(ItemID.WaterWalkingBoots);
                chest.item[nextSlot++] = new Item(ItemID.GoldenFishingRod);
                chest.item[nextSlot++] = new Item(ItemID.JourneymanBait, stack: 20);
            }
        }
    }

    sealed class CavernIsland : FloatingIsland
    {
        //           w w
        //         w w w w w
        //       w w s s w w w w
        // -     l w s s c c w l
        // 0     l w s s c c w l w
        // +   w l w x x x x x l x 
        //   g x x x x       x x x
        //   x x x               x
        //             - 0 +

        public CavernIsland(int x, int y) : base(x, y)
        {
        }

        public void PlaceTiles()
        {
            PlaceTile(-2, 1, TileID.Stone);
            PlaceTile(-1, 1, TileID.Stone);
            PlaceTile( 0, 1, TileID.Stone);
            PlaceTile( 1, 1, TileID.Stone);
            PlaceTile( 2, 1, TileID.Stone);

            PlaceTile(-5, 2, TileID.Stone);
            PlaceTile(-4, 2, TileID.Stone);
            PlaceTile(-3, 2, TileID.Stone);
            PlaceTile(-2, 2, TileID.Stone);
            PlaceTile( 2, 2, TileID.Stone);
            PlaceTile( 3, 2, TileID.Stone);
            PlaceTile( 4, 2, TileID.Stone);

            PlaceTile(-6, 3, TileID.Stone);
            PlaceTile(-5, 3, TileID.Stone);
            PlaceTile(-4, 3, TileID.Stone);
            PlaceTile( 4, 3, TileID.Stone);

            PlaceWall(-2, -4, WallID.SpiderUnsafe);
            PlaceWall(-1, -4, WallID.SpiderUnsafe);

            PlaceWall(-3, -3, WallID.SpiderUnsafe);
            PlaceWall(-2, -3, WallID.SpiderUnsafe);
            PlaceWall(-1, -3, WallID.SpiderUnsafe);
            PlaceWall( 0, -3, WallID.SpiderUnsafe);
            PlaceWall( 1, -3, WallID.SpiderUnsafe);

            PlaceWall(-4, -2, WallID.SpiderUnsafe);
            PlaceWall(-3, -2, WallID.SpiderUnsafe);
            PlaceWall(-2, -2, WallID.SpiderUnsafe);
            PlaceWall(-1, -2, WallID.SpiderUnsafe);
            PlaceWall( 0, -2, WallID.SpiderUnsafe);
            PlaceWall( 1, -2, WallID.SpiderUnsafe);
            PlaceWall( 2, -2, WallID.SpiderUnsafe);
            PlaceWall( 3, -2, WallID.SpiderUnsafe);

            PlaceWall(-4, -1, WallID.SpiderUnsafe);
            PlaceWall(-3, -1, WallID.SpiderUnsafe);
            PlaceWall(-2, -1, WallID.SpiderUnsafe);
            PlaceWall(-1, -1, WallID.SpiderUnsafe);
            PlaceWall( 0, -1, WallID.SpiderUnsafe);
            PlaceWall( 1, -1, WallID.SpiderUnsafe);
            PlaceWall( 2, -1, WallID.SpiderUnsafe);
            PlaceWall( 3, -1, WallID.SpiderUnsafe);

            PlaceWall(-4, 0, WallID.SpiderUnsafe);
            PlaceWall(-3, 0, WallID.SpiderUnsafe);
            PlaceWall(-2, 0, WallID.SpiderUnsafe);
            PlaceWall(-1, 0, WallID.SpiderUnsafe);
            PlaceWall( 0, 0, WallID.SpiderUnsafe);
            PlaceWall( 1, 0, WallID.SpiderUnsafe);
            PlaceWall( 2, 0, WallID.SpiderUnsafe);
            PlaceWall( 3, 0, WallID.SpiderUnsafe);
            PlaceWall( 4, 0, WallID.SpiderUnsafe);

            PlaceWall(-5, 1, WallID.SpiderUnsafe);
            PlaceWall(-4, 1, WallID.SpiderUnsafe);
            PlaceWall(-3, 1, WallID.SpiderUnsafe);
            PlaceWall(-2, 1, WallID.SpiderUnsafe);
            PlaceWall(-1, 1, WallID.SpiderUnsafe);
            PlaceWall( 0, 1, WallID.SpiderUnsafe);
            PlaceWall( 1, 1, WallID.SpiderUnsafe);
            PlaceWall( 2, 1, WallID.SpiderUnsafe);
            PlaceWall( 3, 1, WallID.SpiderUnsafe);
            PlaceWall( 4, 1, WallID.SpiderUnsafe);

            PlaceTile(-6, 2, TileID.MushroomGrass);

            PlaceTile(-2, 0, TileID.Statues, style: 37); // Heart Statue
            PlaceTile(-4, 1, TileID.Lamps, style: 32); // Spider Lamp
            PlaceTile(3, 1, TileID.Lamps, style: 32);

            var chest = PlaceChest(0, 0, type: TileID.Containers2, style: 2); // Spider Chest
            if (chest != null)
            {
                int nextSlot = 0;
                chest.item[nextSlot++] = new Item(ItemID.BandofRegeneration);
                chest.item[nextSlot++] = new Item(ItemID.MagicMirror);
                chest.item[nextSlot++] = new Item(ItemID.LifeCrystal, stack: 3);
                chest.item[nextSlot++] = new Item(ItemID.SuspiciousLookingEye, stack: 10);
            }
        }
    }

    sealed class GraniteIsland : FloatingIsland
    {
        //       l             l
        //       l   s s       l
        // - x x l   s s c c   l x x
        // 0 x x x x s s c c x x x x
        // +     x x x x x x x x
        //           x x x x
        //           - 0 +

        public GraniteIsland(int x, int y) : base(x, y)
        {
        }

        public void PlaceTiles()
        {
            PlaceTile(-5, -1, TileID.Granite);
            PlaceTile(-4, -1, TileID.Granite);
            PlaceTile( 5, -1, TileID.Granite);
            PlaceTile( 6, -1, TileID.Granite);

            PlaceTile(-5, 0, TileID.Granite);
            PlaceTile(-4, 0, TileID.Granite);
            PlaceTile(-3, 0, TileID.Granite);
            PlaceTile(-2, 0, TileID.Granite);
            PlaceTile( 3, 0, TileID.Granite);
            PlaceTile( 4, 0, TileID.Granite);
            PlaceTile( 5, 0, TileID.Granite);
            PlaceTile( 6, 0, TileID.Granite);

            PlaceTile(-3, 1, TileID.Granite);
            PlaceTile(-2, 1, TileID.Granite);
            PlaceTile(-1, 1, TileID.Granite);
            PlaceTile( 0, 1, TileID.Granite);
            PlaceTile( 1, 1, TileID.Granite);
            PlaceTile( 2, 1, TileID.Granite);
            PlaceTile( 3, 1, TileID.Granite);
            PlaceTile( 4, 1, TileID.Granite);

            PlaceTile(-1, 2, TileID.Granite);
            PlaceTile( 0, 2, TileID.Granite);
            PlaceTile( 1, 2, TileID.Granite);
            PlaceTile( 2, 2, TileID.Granite);

            PlaceTile(-1, 0, TileID.Statues, style: 73); // Granite Golem Statue
            PlaceTile(-3, -1, TileID.Lamps, style: 29); // Granite Lamp
            PlaceTile(4, -1, TileID.Lamps, style: 29);

            var chest = PlaceChest(1, 0, type: TileID.Containers, style: 50); // Granite Chest
            if (chest != null)
            {
                int nextSlot = 0;
                chest.item[nextSlot++] = new Item(ItemID.FlareGun);
                chest.item[nextSlot++] = new Item(ItemID.Flare, stack: Main.rand.Next(25, 50));
                chest.item[nextSlot++] = new Item(ItemID.LifeCrystal, stack: 3);
                chest.item[nextSlot++] = new Item(ItemID.SnowGlobe, stack: 10);
            }
        }
    }

    sealed class MarbleIsland : FloatingIsland
    {
        //       l             l
        //       l   s s       l
        // - x x l   s s c c   l x x
        // 0 x x x x s s c c x x x x
        // +     x x x x x x x x
        //           x x x x
        //           - 0 +

        public MarbleIsland(int x, int y) : base(x, y)
        {
        }

        public void PlaceTiles()
        {
            PlaceTile(-5, -1, TileID.Marble);
            PlaceTile(-4, -1, TileID.Marble);
            PlaceTile( 5, -1, TileID.Marble);
            PlaceTile( 6, -1, TileID.Marble);

            PlaceTile(-5, 0, TileID.Marble);
            PlaceTile(-4, 0, TileID.Marble);
            PlaceTile(-3, 0, TileID.Marble);
            PlaceTile(-2, 0, TileID.Marble);
            PlaceTile( 3, 0, TileID.Marble);
            PlaceTile( 4, 0, TileID.Marble);
            PlaceTile( 5, 0, TileID.Marble);
            PlaceTile( 6, 0, TileID.Marble);

            PlaceTile(-3, 1, TileID.Marble);
            PlaceTile(-2, 1, TileID.Marble);
            PlaceTile(-1, 1, TileID.Marble);
            PlaceTile( 0, 1, TileID.Marble);
            PlaceTile( 1, 1, TileID.Marble);
            PlaceTile( 2, 1, TileID.Marble);
            PlaceTile( 3, 1, TileID.Marble);
            PlaceTile( 4, 1, TileID.Marble);

            PlaceTile(-1, 2, TileID.Marble);
            PlaceTile( 0, 2, TileID.Marble);
            PlaceTile( 1, 2, TileID.Marble);
            PlaceTile( 2, 2, TileID.Marble);

            PlaceTile(-1, 0, TileID.Statues, style: 72); // Hoplite Statue
            PlaceTile(-3, -1, TileID.Lamps, style: 30); // Marble Lamp
            PlaceTile(4, -1, TileID.Lamps, style: 30);

            var chest = PlaceChest(1, 0, type: TileID.Containers, style: 51); // Marble Chest
            if (chest != null)
            {
                int nextSlot = 0;
                chest.item[nextSlot++] = new Item(ItemID.ShoeSpikes);
                chest.item[nextSlot++] = new Item(ItemID.Mace);
                chest.item[nextSlot++] = new Item(ItemID.LifeCrystal, stack: 3);
                chest.item[nextSlot++] = new Item(ItemID.BloodMoonStarter, stack: 10);
            }
        }
    }

    sealed class SkyIsland : FloatingIsland
    {
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

        public SkyIsland(int x, int y) : base(x, y)
        {
        }
        
        public void PlaceTiles()
        {
            PlaceTile(-7, -1, TileID.Sunplate);
            PlaceTile(-6, -1, TileID.Sunplate);
            PlaceTile(-5, -1, TileID.Sunplate);
            PlaceTile( 3, -1, TileID.Sunplate);
            PlaceTile( 4, -1, TileID.Sunplate);
            PlaceTile( 5, -1, TileID.Sunplate);

            PlaceTile(-16, 0, TileID.Cloud);
            PlaceTile(-15, 0, TileID.Cloud);
            PlaceTile(-14, 0, TileID.Cloud);
            PlaceTile(-11, 0, TileID.Cloud);
            PlaceTile(-10, 0, TileID.Cloud);
            PlaceTile( -9, 0, TileID.Cloud);
            PlaceTile( -8, 0, TileID.Sunplate);
            PlaceTile( -7, 0, TileID.Sunplate);
            PlaceTile( -6, 0, TileID.Sunplate);
            PlaceTile( -5, 0, TileID.Sunplate);
            PlaceTile( -4, 0, TileID.Sunplate);
            PlaceTile( -3, 0, TileID.Sunplate);
            PlaceTile( -2, 0, TileID.Sunplate);
            PlaceTile( -1, 0, TileID.Sunplate);
            PlaceTile(  0, 0, TileID.Sunplate);
            PlaceTile(  1, 0, TileID.Sunplate);
            PlaceTile(  2, 0, TileID.Sunplate);
            PlaceTile(  3, 0, TileID.Sunplate);
            PlaceTile(  4, 0, TileID.Sunplate);
            PlaceTile(  5, 0, TileID.Sunplate);
            PlaceTile(  6, 0, TileID.Sunplate);
            PlaceTile(  7, 0, TileID.Cloud);
            PlaceTile(  8, 0, TileID.Cloud);
            PlaceTile(  9, 0, TileID.Cloud);
            PlaceTile( 10, 0, TileID.Cloud);
            PlaceTile( 11, 0, TileID.Cloud);
            PlaceTile( 12, 0, TileID.Cloud);

            PlaceTile(-15, 1, TileID.Cloud);
            PlaceTile(-14, 1, TileID.Cloud);
            PlaceTile(-13, 1, TileID.Cloud);
            PlaceTile(-12, 1, TileID.Cloud);
            PlaceTile(-11, 1, TileID.Cloud);
            PlaceTile(-10, 1, TileID.Cloud);
            PlaceTile( -9, 1, TileID.Cloud);
            PlaceTile( -8, 1, TileID.Cloud);
            PlaceTile( -7, 1, TileID.Cloud);
            PlaceTile( -6, 1, TileID.Sunplate);
            PlaceTile( -5, 1, TileID.Sunplate);
            PlaceTile( -4, 1, TileID.Sunplate);
            PlaceTile( -3, 1, TileID.Sunplate);
            PlaceTile( -2, 1, TileID.Sunplate);
            PlaceTile( -1, 1, TileID.Sunplate);
            PlaceTile(  0, 1, TileID.Sunplate);
            PlaceTile(  1, 1, TileID.Sunplate);
            PlaceTile(  2, 1, TileID.Sunplate);
            PlaceTile(  3, 1, TileID.Sunplate);
            PlaceTile(  4, 1, TileID.Sunplate);
            PlaceTile(  5, 1, TileID.Cloud);
            PlaceTile(  6, 1, TileID.Cloud);
            PlaceTile(  7, 1, TileID.Cloud);
            PlaceTile(  8, 1, TileID.Cloud);
            PlaceTile(  9, 1, TileID.Cloud);
            PlaceTile( 10, 1, TileID.Cloud);
            PlaceTile( 11, 1, TileID.Cloud);
            PlaceTile( 12, 1, TileID.Cloud);
            PlaceTile( 13, 1, TileID.Cloud);
            PlaceTile( 14, 1, TileID.Cloud);

            PlaceTile(-13, 2, TileID.Cloud);
            PlaceTile(-12, 2, TileID.Cloud);
            PlaceTile(-11, 2, TileID.Cloud);
            PlaceTile(-10, 2, TileID.Cloud);
            PlaceTile( -9, 2, TileID.Cloud);
            PlaceTile( -8, 2, TileID.Cloud);
            PlaceTile( -7, 2, TileID.Cloud);
            PlaceTile( -6, 2, TileID.Cloud);
            PlaceTile( -5, 2, TileID.Cloud);
            PlaceTile( -4, 2, TileID.Cloud);
            PlaceTile( -3, 2, TileID.Cloud);
            PlaceTile( -2, 2, TileID.Cloud);
            PlaceTile( -1, 2, TileID.Sunplate);
            PlaceTile(  0, 2, TileID.Sunplate);
            PlaceTile(  1, 2, TileID.Cloud);
            PlaceTile(  2, 2, TileID.Cloud);
            PlaceTile(  3, 2, TileID.Cloud);
            PlaceTile(  4, 2, TileID.Cloud);
            PlaceTile(  5, 2, TileID.Cloud);
            PlaceTile(  6, 2, TileID.Cloud);
            PlaceTile(  7, 2, TileID.Cloud);
            PlaceTile(  8, 2, TileID.Cloud);
            PlaceTile(  9, 2, TileID.Cloud);
            PlaceTile( 10, 2, TileID.Cloud);
            PlaceTile( 11, 2, TileID.Cloud);
            PlaceTile( 12, 2, TileID.Cloud);
            PlaceTile( 13, 2, TileID.Cloud);
            PlaceTile( 14, 2, TileID.Cloud);
            PlaceTile( 15, 2, TileID.Cloud);

            PlaceTile(-12, 3, TileID.Cloud);
            PlaceTile(-11, 3, TileID.Cloud);
            PlaceTile(-10, 3, TileID.Cloud);
            PlaceTile( -9, 3, TileID.Cloud);
            PlaceTile( -8, 3, TileID.Cloud);
            PlaceTile( -7, 3, TileID.Cloud);
            PlaceTile( -6, 3, TileID.Cloud);
            PlaceTile( -5, 3, TileID.Cloud);
            PlaceTile( -4, 3, TileID.Cloud);
            PlaceTile( -3, 3, TileID.Cloud);
            PlaceTile( -2, 3, TileID.Cloud);
            PlaceTile( -1, 3, TileID.Cloud);
            PlaceTile(  0, 3, TileID.Cloud);
            PlaceTile(  1, 3, TileID.Cloud);
            PlaceTile(  2, 3, TileID.Cloud);
            PlaceTile(  3, 3, TileID.Cloud);
            PlaceTile(  4, 3, TileID.Cloud);
            PlaceTile(  5, 3, TileID.Cloud);
            PlaceTile(  6, 3, TileID.Cloud);
            PlaceTile(  7, 3, TileID.Cloud);
            PlaceTile(  8, 3, TileID.Cloud);
            PlaceTile(  9, 3, TileID.Cloud);
            PlaceTile( 10, 3, TileID.Cloud);

            PlaceTile( -8, 4, TileID.Cloud);
            PlaceTile( -7, 4, TileID.Cloud);
            PlaceTile( -6, 4, TileID.Cloud);
            PlaceTile( -5, 4, TileID.Cloud);
            PlaceTile( -4, 4, TileID.Cloud);
            PlaceTile( -3, 4, TileID.Cloud);
            PlaceTile( -2, 4, TileID.Cloud);
            PlaceTile( -1, 4, TileID.Cloud);
            PlaceTile(  0, 4, TileID.Cloud);
            PlaceTile(  1, 4, TileID.Cloud);
            PlaceTile(  2, 4, TileID.Cloud);
            PlaceTile(  3, 4, TileID.Cloud);
            PlaceTile(  4, 4, TileID.Cloud);
            PlaceTile(  5, 4, TileID.Cloud);
            PlaceTile(  6, 4, TileID.Cloud);
            PlaceTile(  7, 4, TileID.Cloud);

            PlaceTile( -3, 5, TileID.Cloud);
            PlaceTile( -2, 5, TileID.Cloud);
            PlaceTile( -1, 5, TileID.Cloud);
            PlaceTile(  0, 5, TileID.Cloud);
            PlaceTile(  1, 5, TileID.Cloud);
            PlaceTile(  2, 5, TileID.Cloud);
            PlaceTile(  3, 5, TileID.Cloud);
            PlaceTile(  4, 5, TileID.Cloud);
            PlaceTile(  5, 5, TileID.Cloud);

            PlaceTile(-2, -1, TileID.Statues, style: 70); // Harpy Statue
            PlaceTile(1, -1, TileID.SkyMill);

            PlaceTile(-6, -2, TileID.Lamps, style: 9); // Skyware Lamp
            PlaceTile(4, -2, TileID.Lamps, style: 9);

            var chest = PlaceChest(-4, -1, type: TileID.Containers, style: 13); // Skyware Chest
            if (chest != null)
            {
                int nextSlot = 0;
                chest.item[nextSlot++] = new Item(ItemID.CreativeWings);
                chest.item[nextSlot++] = new Item(ItemID.ShinyRedBalloon);
                chest.item[nextSlot++] = new Item(ItemID.Starfury);
            }
        }
    }
}
