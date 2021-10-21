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

            tasks.RemoveRange(1, tasks.Count - 1);
            tasks.Add(new OBCWorldGenPass());
        }

        public static int NextBlock()
        {
            return Main.rand.Next(420) switch
            {
                // w=50
                (>= 0) and (< 50) => ItemID.DirtBlock,
                (>= 50) and (< 100) => ItemID.StoneBlock,
                (>= 100) and (< 150) => ItemID.SandBlock,
                (>= 150) and (< 200) => ItemID.SnowBlock,
                (>= 200) and (< 250) => ItemID.IceBlock,

                // w=30
                (>= 250) and (< 280) => ItemID.ClayBlock,
                (>= 280) and (< 310) => ItemID.HardenedSand,
                (>= 310) and (< 340) => ItemID.SiltBlock,
                (>= 340) and (< 370) => ItemID.SlushBlock,
                (>= 370) and (< 400) => ItemID.DesertFossil,

                // w=10
                (>= 400) and (< 410) => ItemID.Cloud,
                (>= 410) and (< 420) => ItemID.Cobweb,

                _ => 0,
            };
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
            MakeDungeonIsland();
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

        static void MakeDungeonIsland()
        {
            var dungeonDirection = Main.rand.NextBool() ? 1 : -1;

            Main.dungeonX = (int)(Main.maxTilesX * (0.5 + dungeonDirection * 0.3));
            Main.dungeonY = Main.spawnTileY;

            WorldGen.PlaceTile(Main.dungeonX, Main.dungeonY, TileID.GreenDungeonBrick);

            int oldManID = NPC.NewNPC(Main.dungeonX * 16, Main.dungeonY * 16, NPCID.OldMan);
            Main.npc[oldManID].homeless = false;
            Main.npc[oldManID].homeTileX = Main.dungeonX;
            Main.npc[oldManID].homeTileY = Main.dungeonY;
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
            public bool CanDrop(DropAttemptInfo info) => info.player.GetBestPickaxe().pick >= 65 && info.player.ZoneUnderworldHeight;
            public bool CanShowItemDropInUI() => true;
            public string GetConditionDescription() => "Drops in Underworld when player has enough pickaxe power";
        }

        class InSolarEclipse : IItemDropRuleCondition
        {
            public bool CanDrop(DropAttemptInfo info) => Main.eclipse;
            public bool CanShowItemDropInUI() => true;
            public string GetConditionDescription() => "Drops during Solar Eclipse";
        }

        class DefeatPlantera : IItemDropRuleCondition
        {
            public bool CanDrop(DropAttemptInfo info) => NPC.downedPlantBoss && info.player.ZoneJungle && info.player.ZoneRockLayerHeight;
            public bool CanShowItemDropInUI() => true;
            public string GetConditionDescription() => "Drops in post-Plantera Underground Jungle";
        }


        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (Array.IndexOf(HornetNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Hive, chanceDenominator: 10, minimumDropped: 1, maximumDropped: 3));
            }

            if (!NPCID.Sets.CountsAsCritter[npc.type])
            {
                npcLoot.Add(ItemDropRule.ByCondition(new InZoneUnderworld(), ItemID.AshBlock, chanceDenominator: 10, minimumDropped: 3, maximumDropped: 5));
                npcLoot.Add(ItemDropRule.ByCondition(new HellstonePickable(), ItemID.Hellstone, chanceDenominator: 10, minimumDropped: 1, maximumDropped: 3));
                npcLoot.Add(ItemDropRule.ByCondition(new DefeatPlantera(), ItemID.LihzahrdPowerCell, chanceDenominator: 100));
                npcLoot.Add(ItemDropRule.ByCondition(new InSolarEclipse(), ItemID.LunarTabletFragment, chanceDenominator: 100));
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            var player = Main.LocalPlayer;

            if (type == NPCID.Merchant)
            {
                shop.item[nextSlot].SetDefaults(ItemID.Extractinator);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 2);
                nextSlot++;

                if (player.ZoneJungle)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.HiveWand);
                    nextSlot++;
                }
            }
            else if (type == NPCID.WitchDoctor)
            {
                if (NPC.downedPlantBoss && player.ZoneJungle)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.LihzahrdAltar);
                    nextSlot++;
                }
            }
            else if (type == NPCID.SkeletonMerchant)
            {
                shop.item[nextSlot].SetDefaults(ItemID.LifeCrystal);
                nextSlot++;
            }
        }
    }
}
