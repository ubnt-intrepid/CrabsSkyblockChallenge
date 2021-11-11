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
            AddCraftingStationRecipes();

            AddSurfaceChestLootRecipes();
            AddCavernChestLootRecipies();
            AddLivingWoodRecipes();
            AddUndergroundDesertRecipes();
            AddUndergroundJungleRecipes();
            AddMushroomRecipes();
            AddUnderworldRecipes();

            CreateRecipe(ItemID.SharpeningStation)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 5)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 4)
                .AddIngredient(ItemID.IronskinPotion, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.AmmoBox)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 5)
                .AddIngredient(ItemID.AmmoReservationPotion, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.DartTrap)
                .AddIngredient(ItemID.StoneBlock)
                .AddIngredient(ItemID.PoisonDart, stack: 100)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.GeyserTrap)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 2)
                .AddIngredient(ItemID.LavaBucket)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.WebCoveredChest)
                .AddIngredient(ItemID.Chest)
                .AddIngredient(ItemID.Cobweb, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        void AddCraftingStationRecipes()
        {
            CreateRecipe(ItemID.LivingLoom)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 20)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe(ItemID.HoneyDispenser)
                .AddIngredient(ItemID.HoneyBlock, stack: 10)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 5)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 3)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.SkyMill)
                .AddIngredient(ItemID.StoneBlock, stack: 20)
                .AddIngredient(ItemID.Cloud, stack: 10)
                .AddIngredient(ItemID.FallenStar)
                .AddTile(TileID.Anvils)
                .Register();

            // imported from Calamity Mod
            CreateRecipe(ItemID.IceMachine)
                .AddIngredient(ItemID.SnowBlock, stack: 25)
                .AddIngredient(ItemID.IceBlock, stack: 15)
                .AddRecipeGroup(RecipeGroupID.IronBar, stack: 3)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.Hellforge)
                .AddIngredient(ItemID.Furnace)
                .AddIngredient(ItemID.Hellstone, stack: 30)
                .AddIngredient(ItemID.Obsidian, stack: 30)
                .AddTile(TileID.Anvils)
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
                .AddIngredient(ItemID.Torch, stack: 5)
                .AddIngredient(ItemID.ManaCrystal)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe(ItemID.PortableStool)
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

        void AddLivingWoodRecipes()
        {
            CreateRecipe(ItemID.LivingWoodWand)
                .AddIngredient(ItemID.Wood, stack: 8)
                .AddTile(TileID.LivingLoom)
                .Register();

            CreateRecipe(ItemID.LeafWand)
                .AddIngredient(ItemID.Wood, stack: 5)
                .AddIngredient(ItemID.GrassSeeds, stack: 10)
                .AddTile(TileID.LivingLoom)
                .Register();

            CreateRecipe(ItemID.BabyBirdStaff)
                .AddRecipeGroup(RecipeGroupID.Wood, stack: 5)
                .AddIngredient(ItemID.ManaCrystal)
                .AddTile(TileID.LivingLoom)
                .Register();

            CreateRecipe(ItemID.SunflowerMinecart)
                .AddIngredient(ItemID.Minecart)
                .AddIngredient(ItemID.Sunflower)
                .AddTile(TileID.LivingLoom)
                .Register();

            CreateRecipe(ItemID.LadybugMinecart)
                .AddIngredient(ItemID.Minecart)
                .AddIngredient(ItemID.LadyBug)
                .AddTile(TileID.LivingLoom)
                .Register();
        }

        void AddUndergroundJungleRecipes()
        {
            CreateRecipe(ItemID.LivingMahoganyLeafWand)
                .AddIngredient(ItemID.RichMahogany, stack: 5)
                .AddIngredient(ItemID.JungleGrassSeeds, stack: 10)
                .AddTile(TileID.LivingLoom)
                .Register();

            CreateRecipe(ItemID.LivingMahoganyWand)
                .AddIngredient(ItemID.RichMahogany, stack: 5)
                .AddTile(TileID.LivingLoom)
                .Register();

            CreateRecipe(ItemID.BeeMinecart)
                .AddIngredient(ItemID.Minecart)
                .AddIngredient(ItemID.Hive, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();
        }

        void AddUndergroundDesertRecipes()
        {
            CreateRecipe(ItemID.HardenedSand)
                .AddIngredient(ItemID.SandBlock, stack: 2)
                .AddTile(TileID.Furnaces)
                .Register();

            CreateRecipe(ItemID.DesertMinecart)
                .AddIngredient(ItemID.Minecart)
                .AddIngredient(ItemID.Sandstone, stack: 10)
                .AddIngredient(ItemID.Amber)
                .AddTile(TileID.Anvils)
                .Register();
        }

        void AddMushroomRecipes()
        {
            CreateRecipe(ItemID.ShroomMinecart)
                .AddIngredient(ItemID.Minecart)
                .AddIngredient(ItemID.GlowingMushroom, stack: 10)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe(ItemID.MushroomHat)
                .AddIngredient(ItemID.Silk, stack: 15)
                .AddIngredient(ItemID.MushroomGrassSeeds, stack: 3)
                .AddTile(TileID.Loom)
                .Register();

            CreateRecipe(ItemID.MushroomVest)
                .AddIngredient(ItemID.Silk, stack: 20)
                .AddIngredient(ItemID.MushroomGrassSeeds, stack: 3)
                .AddTile(TileID.Loom)
                .Register();

            CreateRecipe(ItemID.MushroomPants)
                .AddIngredient(ItemID.Silk, stack: 20)
                .AddIngredient(ItemID.MushroomGrassSeeds, stack: 3)
                .AddTile(TileID.Loom)
                .Register();
        }

        void AddUnderworldRecipes()
        {
            CreateRecipe(ItemID.HellMinecart)
                .AddIngredient(ItemID.Minecart)
                .AddIngredient(ItemID.HellstoneBar, stack: 5)
                .AddIngredient(ItemID.LavaCharm)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

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
            PlaceChestIsland(Main.spawnTileX - dungeonDirection * 100, Main.spawnTileY);

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

        static void PlaceSpawnIsland(int x, int y)
        {
            WorldGen.PlaceTile(x, y, ModContent.TileType<Tiles.InfiniteBlock>());

            int guideIndex = NPC.NewNPC(x * 16, y * 16, NPCID.Guide);
            Main.npc[guideIndex].homeless = true;
            Main.npc[guideIndex].homeTileX = x;
            Main.npc[guideIndex].homeTileY = y;
            Main.npc[guideIndex].direction = 1;
        }

        static readonly Tuple<int, int>[] ChestIslandOffsets = new[] {
            new Tuple<int, int>(-3, 0),
            new Tuple<int, int>(-2, 0),
            new Tuple<int, int>(-1, 0),
            new Tuple<int, int>( 0, 0),
            new Tuple<int, int>( 1, 0),
            new Tuple<int, int>( 1, 1),
            new Tuple<int, int>( 2, 1),
            new Tuple<int, int>( 3, 1),
            new Tuple<int, int>( 3, 0),
        };

        static void PlaceChestIsland(int x, int y)
        {
            foreach (var offset in ChestIslandOffsets)
            {
                WorldGen.PlaceTile(x + offset.Item1, y + offset.Item2, TileID.Stone);
            }

            WorldGen.PlaceTile(x - 2, y - 1, TileID.Extractinator);
            WorldGen.PlaceTile(x + 3, y - 1, TileID.Torches, style: TorchID.Torch);

            int chestIndex = WorldGen.PlaceChest(x, y - 1);
            if (chestIndex != -1)
            {
                var chest = Main.chest[chestIndex];
                int nextSlot = 0;

                chest.item[nextSlot++] = new Item(ItemID.JungleGrassSeeds);
                chest.item[nextSlot++] = new Item(ItemID.MushroomGrassSeeds);
                chest.item[nextSlot++] = new Item(ItemID.Acorn, stack: 5);
                chest.item[nextSlot++] = new Item(ItemID.Cobweb, stack: 10);
                chest.item[nextSlot++] = new Item(ItemID.DirtBlock, stack: 25);
                chest.item[nextSlot++] = new Item(ItemID.SandBlock, stack: 5);
                chest.item[nextSlot++] = new Item(ItemID.Marble, stack: 20);
                chest.item[nextSlot++] = new Item(ItemID.Granite, stack: 20);
            }

            WorldGen.PlaceLiquid(x + 2, y, LiquidID.Water, amount: 180);
        }
    }

    public class OBCGlobalItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Hive)
            {
                item.useStyle = ItemUseStyleID.Swing;
                item.useTurn = true;
                item.useAnimation = 15;
                item.useTime = 15;
                item.autoReuse = true;
                item.consumable = true;
                item.createTile = TileID.Hive;
            }
        }

        public override void ExtractinatorUse(int extractType, ref int resultType, ref int resultStack)
        {
            if (!Main.hardMode)
            {
                return;
            }

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

        static readonly int[] MarbleNPCs = new int[]
        {
            NPCID.GreekSkeleton,
            NPCID.Medusa,
        };

        static readonly int[] IcyNPCs = new int[]
        {
            NPCID.IceSlime,
            NPCID.IceBat,
            NPCID.SpikedIceSlime,

            // hardmode
            NPCID.IceElemental,
            NPCID.IcyMerman,
        };

        static readonly int[] SpiderCaveNPCs = new int[]
        {
            NPCID.WallCreeper,
            NPCID.BlackRecluse,
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

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (Array.IndexOf(AntlionNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.DesertFossil, minimumDropped: 3, maximumDropped: 5));
            }

            if (Array.IndexOf(HornetNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Hive, minimumDropped: 5, maximumDropped: 10));
            }

            if (Array.IndexOf(MarbleNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Marble, minimumDropped: 5, maximumDropped: 10));
            }

            if (Array.IndexOf(IcyNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.IceBlock, minimumDropped: 3, maximumDropped: 5));
            }

            if (Array.IndexOf(SpiderCaveNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.WebSlinger, chanceDenominator: 100));
            }

            if (npc.type == NPCID.Harpy)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Cloud, minimumDropped: 5, maximumDropped: 10));
            }

            if (Array.IndexOf(UnderworldDropNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.AshBlock, minimumDropped: 5, maximumDropped: 10));
                npcLoot.Add(ItemDropRule.ByCondition(new PickaxePowerCondition(65), ItemID.Hellstone, minimumDropped: 3, maximumDropped: 5));
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Merchant)
            {
                shop.item[nextSlot].SetDefaults(ItemID.LifeCrystal);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 2);
                nextSlot++;
            }
        }
    }
}
