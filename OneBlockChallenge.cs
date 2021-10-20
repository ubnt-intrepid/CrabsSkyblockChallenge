using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

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

    public class OBCWorld : ModSystem
    {
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

    public class OBCGlobalItem : GlobalItem
    {
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
                    resultType = Main.rand.Next(6) switch
                    {
                        0 => ItemID.CopperOre,
                        1 => ItemID.TinOre,
                        2 => ItemID.IronOre,
                        3 => ItemID.LeadOre,
                        4 => ItemID.CobaltOre,
                        _ => ItemID.PalladiumOre,
                    };
                    break;

                case ItemID.SilverOre:
                case ItemID.TungstenOre:
                    resultType = Main.rand.Next(4) switch
                    {
                        0 => ItemID.SilverOre,
                        1 => ItemID.TungstenOre,
                        2 => ItemID.MythrilOre,
                        _ => ItemID.OrichalcumOre,
                    };
                    break;

                case ItemID.GoldOre:
                case ItemID.PlatinumOre:
                    resultType = Main.rand.Next(4) switch
                    {
                        0 => ItemID.GoldOre,
                        1 => ItemID.PlatinumOre,
                        2 => ItemID.AdamantiteOre,
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

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (Array.IndexOf(HornetNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Hive, chanceDenominator: 10, minimumDropped: 3, maximumDropped: 5));
            }
        }

        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            globalLoot.Add(new HellStoneDropRule());
            globalLoot.Add(ItemDropRule.ByCondition(new LihzahrdCellDropCondition(), ItemID.LihzahrdPowerCell, chanceDenominator: 100));

            // TODOs:
            // * Solar Tablet Fragment from Solar Eclipse enemies
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

    class HellStoneDropRule : IItemDropRule
    {
        public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

        public bool CanDrop(DropAttemptInfo info) => NPC.downedBoss2 && info.player.ZoneUnderworldHeight;

        public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
        {
            if (info.player.RollLuck(/* denom */3) < /* num */1)
            {
                int item;
                int amount;
                if (info.rng.Next(2) == 0)
                {
                    item = ItemID.AshBlock;
                    amount = info.rng.Next(5, 10);
                }
                else
                {
                    item = ItemID.Hellstone;
                    amount = info.rng.Next(3, 5);
                }
                CommonCode.DropItemFromNPC(info.npc, item, amount);

                return new ItemDropAttemptResult
                {
                    State = ItemDropAttemptResultState.Success
                };
            }
            else
            {
                return new ItemDropAttemptResult
                {
                    State = ItemDropAttemptResultState.FailedRandomRoll
                };
            }
        }

        public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
        {
            float num = 1f / 3f;
            float baseDropRate = num * ratesInfo.parentDroprateChance;
            drops.Add(new DropRateInfo(ItemID.AshBlock, 5, 10, 0.5f * baseDropRate, ratesInfo.conditions));
            drops.Add(new DropRateInfo(ItemID.Hellstone, 3, 5, 0.5f * baseDropRate, ratesInfo.conditions));

            Chains.ReportDroprates(ChainedRules, num, drops, ratesInfo);
        }
    }

    class LihzahrdCellDropCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info) => NPC.downedPlantBoss && info.player.ZoneJungle && info.player.ZoneRockLayerHeight;

        public bool CanShowItemDropInUI() => true;

        public string GetConditionDescription() => "Drops in post-Plantera Underground Jungle";
    }
}
