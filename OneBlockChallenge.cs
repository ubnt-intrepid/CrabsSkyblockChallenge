using System;
using System.Collections.Generic;
using System.Linq;
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
            Main.spawnTileY = (int)Main.worldSurface - 100;

            MakeSpawnIsland();

            // Dungeon and Jungle Temple are the only early structures in the world
            // (except the initial spawn point.)
            var dungeonDirection = Main.rand.NextBool() ? 1 : -1;

            var dungeonX = (int)(Main.maxTilesX * (0.5 + dungeonDirection * 0.3));
            var dungeonY = (int)((Main.spawnTileY + Main.rockLayer) / 2.0) + Main.rand.Next(-200, 200);
            WorldGen.MakeDungeon(dungeonX, dungeonY);

            var templeX = (int)(Main.maxTilesX * (0.5 - dungeonDirection * 0.3));
            var templeY = Main.rand.Next((int)Main.rockLayer, Main.maxTilesY - 500);
            WorldGen.makeTemple(templeX, templeY);
            WorldGen.templePart2();
        }

        static void MakeSpawnIsland()
        {
            WorldGen.PlaceTile(Main.spawnTileX, Main.spawnTileY, ModContent.TileType<Tiles.InfiniteBlock>());

            int guideID = NPC.NewNPC(Main.spawnTileX * 16, Main.spawnTileY * 16, NPCID.Guide);
            Main.npc[guideID].homeless = true;
            Main.npc[guideID].homeTileX = Main.spawnTileX;
            Main.npc[guideID].homeTileY = Main.spawnTileY;
            Main.npc[guideID].direction = 1;
        }
    }

    public class OBCPlayer : ModPlayer
    {
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            if (mediumCoreDeath)
            {
                return Enumerable.Empty<Item>();
            }

            return new[]
            {
                new Item(ItemID.Acorn, stack: 5),
                new Item(ItemID.GrassSeeds, stack: 1),
                new Item(ItemID.JungleGrassSeeds, stack: 1),
                new Item(ItemID.MushroomGrassSeeds, stack: 1),
                new Item(ItemID.WaterBucket, stack: 1),
                new Item(ItemID.Torch, stack: 1),
            };
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
                item.useTime = 10;
                item.autoReuse = true;
                item.consumable = true;
                item.createTile = TileID.Hive;
            }
        }

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

        class InZoneUnderworld : IItemDropRuleCondition
        {
            public bool CanDrop(DropAttemptInfo info) => info.player.ZoneUnderworldHeight;
            public bool CanShowItemDropInUI() => true;
            public string GetConditionDescription() => "Drops in Underworld";
        }

        class HellstonePickable : IItemDropRuleCondition
        {
            public bool CanDrop(DropAttemptInfo info) => info.player.ZoneUnderworldHeight && info.player.GetBestPickaxe().pick >= 65;
            public bool CanShowItemDropInUI() => true;
            public string GetConditionDescription() => "Drops in Underworld when player has enough pickaxe power";
        }

        class UndergroundChestLoot : IItemDropRule
        {
            public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

            public UndergroundChestLoot()
            {
                ChainedRules = new();
            }

            public bool CanDrop(DropAttemptInfo info) => info.player.ZoneRockLayerHeight && !info.player.ZoneDungeon && !info.player.ZoneLihzhardTemple;

            public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
            {
                if (info.player.RollLuck(50) != 0)
                {
                    return new ItemDropAttemptResult
                    {
                        State = ItemDropAttemptResultState.FailedRandomRoll,
                    };
                }

                var type = info.rng.Next(6) switch
                {
                    0 => ItemID.BandofRegeneration,
                    1 => ItemID.MagicMirror,
                    2 => ItemID.CloudinaBottle,
                    3 => ItemID.HermesBoots,
                    4 => ItemID.ShoeSpikes,
                    5 => ItemID.FlareGun,
                    _ => throw new Exception("unreachable code"),
                };

                CommonCode.DropItemFromNPC(info.npc, type, stack: 1);
                if (type == ItemID.FlareGun)
                {
                    CommonCode.DropItemFromNPC(info.npc, ItemID.Flare, stack: info.rng.Next(25, 50));
                }

                return new ItemDropAttemptResult
                {
                    State = ItemDropAttemptResultState.Success,
                };
            }

            public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
            {
                float baseDropRate = 0.02f * ratesInfo.parentDroprateChance;

                drops.Add(new DropRateInfo(ItemID.BandofRegeneration, 1, 1, baseDropRate / 6f, ratesInfo.conditions));
                drops.Add(new DropRateInfo(ItemID.MagicMirror, 1, 1, baseDropRate / 6f, ratesInfo.conditions));
                drops.Add(new DropRateInfo(ItemID.CloudinaBottle, 1, 1, baseDropRate / 6f, ratesInfo.conditions));
                drops.Add(new DropRateInfo(ItemID.HermesBoots, 1, 1, baseDropRate / 6f, ratesInfo.conditions));
                drops.Add(new DropRateInfo(ItemID.ShoeSpikes, 1, 1, baseDropRate / 6f, ratesInfo.conditions));
                drops.Add(new DropRateInfo(ItemID.FlareGun, 1, 1, baseDropRate / 6f, ratesInfo.conditions));
                drops.Add(new DropRateInfo(ItemID.Flare, 25, 50, baseDropRate / 6f, ratesInfo.conditions));

                Chains.ReportDroprates(ChainedRules, 0.02f, drops, ratesInfo);
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (Array.IndexOf(HornetNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Hive, chanceDenominator: 10, minimumDropped: 1, maximumDropped: 3));
            }

            if (npc.type == NPCID.Harpy)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Cloud, chanceDenominator: 10, minimumDropped: 3, maximumDropped: 5));
            }

            if (!NPCID.Sets.CountsAsCritter[npc.type])
            {
                npcLoot.Add(ItemDropRule.ByCondition(new InZoneUnderworld(), ItemID.AshBlock, chanceDenominator: 10, minimumDropped: 3, maximumDropped: 5));
                npcLoot.Add(ItemDropRule.ByCondition(new HellstonePickable(), ItemID.Hellstone, chanceDenominator: 10, minimumDropped: 1, maximumDropped: 3));

                npcLoot.Add(new UndergroundChestLoot());
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Merchant)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Extractinator);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 2);
                nextSlot++;
            }
        }
    }
}
