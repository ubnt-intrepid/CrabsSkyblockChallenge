using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace OneBlockChallenge
{
    public class OneBlockChallenge : Mod
    {
        public override void AddRecipes()
        {
            AddSurfaceChestLootRecipes();
            AddCavernChestLootRecipies();

            CreateRecipe(ItemID.Hellforge)
                .AddIngredient(ItemID.Furnace)
                .AddIngredient(ItemID.Hellstone, stack: 30)
                .AddIngredient(ItemID.Obsidian, stack: 30)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.SunplateBlock, amount: 25)
                .AddIngredient(ItemID.StoneBlock, stack: 25)
                .AddIngredient(ItemID.FallenStar, stack: 1)
                .AddTile(TileID.Furnaces)
                .AddCondition(Recipe.Condition.InSkyHeight)
                .Register();

            CreateRecipe(ItemID.Sandstone)
                .AddIngredient(ItemID.HardenedSand, stack: 2)
                .AddTile(TileID.Furnaces)
                .Register();
        }

        void AddSurfaceChestLootRecipes()
        {
            CreateRecipe(ItemID.Spear)
                .AddRecipeGroup(RecipeGroupID.IronBar)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.Blowpipe)
                .AddIngredient(ItemID.BambooBlock, stack: 5)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe(ItemID.WoodenBoomerang)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 5)
                .AddRecipeGroup(RecipeGroupID.IronBar)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.WandofSparking)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 4)
                .AddIngredient(ItemID.ManaCrystal)
                .AddTile(TileID.WorkBenches)
                .Register();

            // Step Stool            
            // FIXME: remove magic number
            CreateRecipe(4341)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }

        void AddCavernChestLootRecipies()
        {
            CreateRecipe(ItemID.MagicMirror)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 10)
                .AddIngredient(ItemID.Glass, stack: 5)
                .AddIngredient(ItemID.RecallPotion, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.Mace)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 20)
                .AddIngredient(ItemID.Chain, stack: 5)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.FlareGun)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 10)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 2)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.Flare, amount: 25)
                .AddRecipeGroup(RecipeGroupID.IronBar)
                .AddIngredient(ItemID.Torch, stack: 25)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.HermesBoots)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 5)
                .AddIngredient(ItemID.Silk, stack: 10)
                .AddIngredient(ItemID.SwiftnessPotion, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.CloudinaBottle)
                .AddIngredient(ItemID.Bottle)
                .AddIngredient(ItemID.Cloud, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.BandofRegeneration)
                .AddIngredient(ItemID.Chain, stack: 5)
                .AddIngredient(ItemID.LifeCrystal)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.ShoeSpikes)
                .AddIngredient(ItemID.ClimbingClaws)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    public class OBCWorld : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            var isOBCSeed = string.Equals(WorldGen.currentWorldSeed, "one block challenge", StringComparison.OrdinalIgnoreCase);
            if (!isOBCSeed)
            {
                return;
            }

            Mod.Logger.Info("Switch to OBC world generation");

            var resetTask = tasks.Find(pass => pass.Name.Contains("Reset"));

            tasks.Clear();
            tasks.Add(resetTask);
            tasks.Add(new OBCWorldGenPass());
        }
    }

    class OBCWorldGenPass : GenPass
    {
        public OBCWorldGenPass() : base("OBC World Generation", 0f) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Generate One Block Challenge World";

            WorldGen.clearWorld();

            Main.worldSurface = Main.maxTilesY * 0.3;
            Main.rockLayer = Main.maxTilesY * 0.5;

            Main.spawnTileX = (int)(Main.maxTilesX * 0.5);
            Main.spawnTileY = (int)Main.worldSurface;

            WorldGen.PlaceTile(Main.spawnTileX, Main.spawnTileY, ModContent.TileType<Tiles.InfiniteBlock>());

            int guideID = NPC.NewNPC(Main.spawnTileX * 16, Main.spawnTileY * 16, NPCID.Guide);
            Main.npc[guideID].homeless = true;
            Main.npc[guideID].homeTileX = Main.spawnTileX;
            Main.npc[guideID].homeTileY = Main.spawnTileY;
            Main.npc[guideID].direction = 1;

            // Dungeon and Jungle Temple are the only early structures in the world
            // (except the initial spawn point.)
            var dungeonDirection = Main.rand.NextBool() ? 1 : -1;

            var dungeonX = (int)(Main.maxTilesX * (0.5 + dungeonDirection * 0.3));
            var dungeonY = (int)((Main.spawnTileY + Main.rockLayer) / 2.0) + Main.rand.Next(-200, 200);
            WorldGen.MakeDungeon(dungeonX, dungeonY);

            var templeX = (int)(Main.maxTilesX * (0.5 - dungeonDirection * 0.3));
            var templeY = Main.rand.Next((int)Main.rockLayer, Main.UnderworldLayer - 200);
            WorldGen.makeTemple(templeX, templeY);
            WorldGen.templePart2();

            var chestIslandX = Main.spawnTileX + (dungeonDirection == 1 ? 100 : -101);
            var chestIslandY = Main.spawnTileY;
            WorldGen.PlaceTile(chestIslandX, chestIslandY, TileID.Stone);
            WorldGen.PlaceTile(chestIslandX + 1, chestIslandY, TileID.Stone);
            int chestIndex = WorldGen.PlaceChest(chestIslandX, chestIslandY - 1);
            if (chestIndex != -1)
            {
                var chest = Main.chest[chestIndex];
                int nextSlot = 0;
                chest.item[nextSlot++] = new Item(ItemID.Torch, stack: 10);
                chest.item[nextSlot++] = new Item(ItemID.GrassSeeds);
                chest.item[nextSlot++] = new Item(ItemID.JungleGrassSeeds);
                chest.item[nextSlot++] = new Item(ItemID.MushroomGrassSeeds);
                chest.item[nextSlot++] = new Item(ItemID.Acorn, stack: 5);
                chest.item[nextSlot++] = new Item(ItemID.WaterBucket);
                chest.item[nextSlot++] = new Item(ItemID.SandBlock, stack: 5);
                chest.item[nextSlot++] = new Item(ItemID.Cobweb, stack: 10);
                chest.item[nextSlot++] = new Item(ItemID.Extractinator);
                chest.item[nextSlot++] = new Item(ItemID.HiveWand);
            }
        }
    }

    public class OBCGlobalItem : GlobalItem
    {
        public override void ExtractinatorUse(int extractType, ref int resultType, ref int resultStack)
        {
            switch (resultType)
            {
                case ItemID.CopperOre:
                case ItemID.TinOre:
                case ItemID.IronOre:
                case ItemID.LeadOre:
                case ItemID.SilverOre:
                case ItemID.TungstenOre:
                case ItemID.GoldOre:
                case ItemID.PlatinumOre:
                    if (Main.hardMode)
                    {
                        var maxValue = Main.LocalPlayer.GetBestPickaxe().pick switch
                        {
                            < 100                => 8,  // pre-hardmode ores
                            (>= 100) and (< 110) => 10, // Cobalt, Palladium
                            (>= 110) and (< 150) => 12, // Mythril, Orichalcum
                            _                    => 14, // Adamantite, Titanium
                        };
                        resultType = Main.rand.Next(maxValue) switch
                        {
                            0 => ItemID.CopperOre,
                            1 => ItemID.TinOre,
                            2 => ItemID.IronOre,
                            3 => ItemID.LeadOre,
                            4 => ItemID.SilverOre,
                            5 => ItemID.TungstenOre,
                            6 => ItemID.GoldOre,
                            7 => ItemID.PlatinumOre,
                            8 => ItemID.CobaltOre,
                            9 => ItemID.PalladiumOre,
                            10 => ItemID.MythrilOre,
                            11 => ItemID.OrichalcumOre,
                            12 => ItemID.AdamantiteOre,
                            _ => ItemID.TitaniumOre,
                        };
                    }
                    break;

                default:
                    break;
            }
        }
    }

    public class OBCGlobalNPC : GlobalNPC
    {
        static readonly int[] AntlionNPCs = new int[]
        {
            NPCID.Antlion,
            NPCID.WalkingAntlion,
            NPCID.FlyingAntlion,
            NPCID.GiantFlyingAntlion,
            NPCID.GiantWalkingAntlion,
        };

        static readonly int[] HornetNPCs = new int[]
        {
            NPCID.Hornet,
            NPCID.LittleStinger,
            NPCID.BigStinger,
            NPCID.HornetFatty,
            NPCID.LittleHornetFatty,
            NPCID.BigHornetFatty,
            NPCID.HornetHoney,
            NPCID.LittleHornetHoney,
            NPCID.BigHornetHoney,
            NPCID.HornetLeafy,
            NPCID.LittleHornetLeafy,
            NPCID.BigHornetLeafy,
            NPCID.HornetSpikey,
            NPCID.LittleHornetSpikey,
            NPCID.BigHornetSpikey,
            NPCID.HornetStingy,
            NPCID.LittleHornetStingy,
            NPCID.BigHornetStingy,

            // Moss hornets
            NPCID.TinyMossHornet,
            NPCID.LittleMossHornet,
            NPCID.MossHornet,
            NPCID.BigMossHornet,
            NPCID.GiantMossHornet,
        };

        static readonly int[] UnderworldDropNPCs = new int[]
        {
            NPCID.Hellbat,
            NPCID.LavaSlime,
            NPCID.Lavabat,
        };

        class PickaxePowerCondition : IItemDropRuleCondition
        {
            readonly int minPick;

            public PickaxePowerCondition(int minPick)
            {
                this.minPick = minPick;
            }

            public bool CanDrop(DropAttemptInfo info) => info.player.GetBestPickaxe().pick >= minPick;
            public bool CanShowItemDropInUI() => true;
            public string GetConditionDescription() => $"Drops when player's pickaxe power is greater or equal to {minPick}%";
        }

        class UndergroundDesertCondition : IItemDropRuleCondition
        {
            public bool CanDrop(DropAttemptInfo info) => info.player.ZoneUndergroundDesert && !info.player.ZoneBeach;
            public bool CanShowItemDropInUI() => true;
            public string GetConditionDescription() => "Drops in Underground Desert";
        }


        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (Array.IndexOf(AntlionNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new UndergroundDesertCondition(), ItemID.DesertFossil, chanceDenominator: 2, minimumDropped: 3, maximumDropped: 5));
            }

            if (Array.IndexOf(HornetNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Hive, chanceDenominator: 10, minimumDropped: 1, maximumDropped: 3));
            }

            if (npc.type == NPCID.Harpy)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Cloud, chanceDenominator: 10, minimumDropped: 3, maximumDropped: 5));
            }

            if (Array.IndexOf(UnderworldDropNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.AshBlock, chanceDenominator: 2, minimumDropped: 5, maximumDropped: 10));
                npcLoot.Add(ItemDropRule.ByCondition(new PickaxePowerCondition(65), ItemID.Hellstone, chanceDenominator: 2, minimumDropped: 3, maximumDropped: 5));
            }
        }
    }

    public class OBCGlobalTile : GlobalTile
    {
        public override bool Drop(int i, int j, int type)
        {
            if (type == TileID.Plants && Main.rand.Next(100) == 0)
            {
                var num = Item.NewItem(new Vector2(i, j).ToWorldCoordinates(), ItemID.GrassSeeds);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, num, 1f);
                }
            }

            return true;
        }

        public override void RandomUpdate(int i, int j, int type)
        {
            if (type == TileID.Grass
                && Framing.GetTileSafely(i, j).IsActive
                && Framing.GetTileSafely(i + 1, j).IsActive
                && !Framing.GetTileSafely(i, j - 1).IsActive
                && !Framing.GetTileSafely(i, j - 2).IsActive
                && Main.rand.Next(5000) == 0)
            {
                WorldGen.PlaceTile(i, j - 1, ModContent.TileType<Tiles.HeartFruit>(), true, false, -1, 0);
            }
        }
    }
}
