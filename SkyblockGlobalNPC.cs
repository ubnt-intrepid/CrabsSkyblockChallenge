using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrabsSkyblockChallenge
{
    public class SkyblockGlobalNPC : GlobalNPC
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

        static readonly int[] IcyNPCs = new int[]
        {
            NPCID.IceSlime,
            NPCID.IceBat,
            NPCID.SpikedIceSlime,

            // hardmode
            NPCID.IceElemental,
            NPCID.IcyMerman,
        };

        static readonly int[] MarbleNPCs = new int[]
        {
            NPCID.GreekSkeleton,
            NPCID.Medusa,
        };

        static readonly int[] LavaFlavoredNPCs = new int[]
        {
            NPCID.Hellbat,
            NPCID.LavaSlime,
            NPCID.Lavabat,
        };

        static readonly int[] FrostRegionNPCs = new int[]
        {
            NPCID.MisterStabby,
            NPCID.SnowmanGangsta,
            NPCID.SnowBalla,
        };


        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (Array.IndexOf(AntlionNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Sandstone, minimumDropped: 5, maximumDropped: 10));
            }

            if (Array.IndexOf(HornetNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Hive, minimumDropped: 3, maximumDropped: 5));
            }

            if (Array.IndexOf(IcyNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.IceBlock, minimumDropped: 3, maximumDropped: 5));
            }

            if (Array.IndexOf(MarbleNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Marble, minimumDropped: 5, maximumDropped: 10));
            }

            if (Array.IndexOf(LavaFlavoredNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Hellstone, minimumDropped: 3, maximumDropped: 5));
            }

            if (Array.IndexOf(FrostRegionNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.SnowGlobe, chanceDenominator: 100));
            }
        }

        public override void OnKill(NPC npc)
        {
            if (npc.type == NPCID.UmbrellaSlime && Main.netMode != NetmodeID.MultiplayerClient)
            {
                var waterX = (int)(npc.Center.X / 16f);
                var waterY = (int)(npc.Center.Y / 16f);
                if (!WorldGen.SolidTile(waterX, waterY) && Main.tile[waterX, waterY].LiquidAmount == 0)
                {
                    Main.tile[waterX, waterY].LiquidAmount = (byte)Main.rand.Next(50, 150);
                    Main.tile[waterX, waterY].LiquidType = LiquidID.Water;
                    WorldGen.SquareTileFrame(waterX, waterY);
                }
            }
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            var me = Main.CurrentPlayer.GetModPlayer<SkyblockPlayer>();

            if (!me.ReceiveStarterItems && (Main.expertMode || Main.masterMode))
            {
                chat = "Are you expert Terrarian? These are gifts from Santa Claus.";
                me.ReceiveStarterItems = true;

                var player = me.Player;
                player.QuickSpawnItem(ItemID.SilverBroadsword);
                player.QuickSpawnItem(ItemID.SilverPickaxe);
                player.QuickSpawnItem(ItemID.SilverAxe);
                player.QuickSpawnItem(ItemID.SilverHammer);
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            switch (type)
            {
                case NPCID.Merchant:
                    SetupMerchantShop(shop, ref nextSlot);
                    break;

                case NPCID.Dryad:
                    SetupDryadShop(shop, ref nextSlot);
                    break;

                case NPCID.Demolitionist:
                    SetupDemolitionistShop(shop, ref nextSlot);
                    break;

                case NPCID.Wizard:
                    SetupWizardShop(shop, ref nextSlot);
                    break;

                default:
                    break;
            }
        }

        private static void SetupMerchantShop(Chest shop, ref int nextSlot)
        {
            if (NPC.AnyNPCs(NPCID.Angler))
            {
                var priceMultiplier = Main.hardMode ? 10 : 5;

                shop.item[nextSlot].SetDefaults(Main.hardMode ? ItemID.WoodenCrateHard : ItemID.WoodenCrate);
                shop.item[nextSlot].shopCustomPrice = priceMultiplier * Item.buyPrice(silver: 10);
                nextSlot++;

                shop.item[nextSlot].SetDefaults(Main.hardMode ? ItemID.IronCrateHard : ItemID.IronCrate);
                shop.item[nextSlot].shopCustomPrice = priceMultiplier * Item.buyPrice(silver: 50);
                nextSlot++;

                shop.item[nextSlot].SetDefaults(Main.hardMode ? ItemID.GoldenCrateHard : ItemID.GoldenCrate);
                shop.item[nextSlot].shopCustomPrice = priceMultiplier * Item.buyPrice(gold: 2);
                nextSlot++;

                if (Main.CurrentPlayer.ZoneJungle)
                {
                    shop.item[nextSlot].SetDefaults(Main.hardMode ? ItemID.JungleFishingCrateHard : ItemID.JungleFishingCrate);
                    shop.item[nextSlot].shopCustomPrice = priceMultiplier * Item.buyPrice(gold: 1);
                    nextSlot++;
                }

                if (Main.CurrentPlayer.ZoneSkyHeight)
                {
                    shop.item[nextSlot].SetDefaults(Main.hardMode ? ItemID.FloatingIslandFishingCrateHard : ItemID.FloatingIslandFishingCrate);
                    shop.item[nextSlot].shopCustomPrice = priceMultiplier * Item.buyPrice(gold: 1);
                    nextSlot++;
                }

                if (Main.CurrentPlayer.ZoneCorrupt)
                {
                    shop.item[nextSlot].SetDefaults(Main.hardMode ? ItemID.CorruptFishingCrateHard : ItemID.CorruptFishingCrate);
                    shop.item[nextSlot].shopCustomPrice = priceMultiplier * Item.buyPrice(gold: 1);
                    nextSlot++;
                }

                if (Main.CurrentPlayer.ZoneCrimson)
                {
                    shop.item[nextSlot].SetDefaults(Main.hardMode ? ItemID.CrimsonFishingCrateHard : ItemID.CrimsonFishingCrate);
                    shop.item[nextSlot].shopCustomPrice = priceMultiplier * Item.buyPrice(gold: 1);
                    nextSlot++;
                }

                if (Main.CurrentPlayer.ZoneHallow)
                {
                    shop.item[nextSlot].SetDefaults(Main.hardMode ? ItemID.HallowedFishingCrateHard : ItemID.HallowedFishingCrate);
                    shop.item[nextSlot].shopCustomPrice = priceMultiplier * Item.buyPrice(gold: 1);
                    nextSlot++;
                }

                if (Main.CurrentPlayer.ZoneDungeon)
                {
                    shop.item[nextSlot].SetDefaults(Main.hardMode ? ItemID.DungeonFishingCrateHard : ItemID.DungeonFishingCrate);
                    shop.item[nextSlot].shopCustomPrice = priceMultiplier * Item.buyPrice(gold: 1);
                    nextSlot++;
                }

                if (Main.CurrentPlayer.ZoneSnow)
                {
                    shop.item[nextSlot].SetDefaults(Main.hardMode ? ItemID.FrozenCrateHard : ItemID.FrozenCrate);
                    shop.item[nextSlot].shopCustomPrice = priceMultiplier * Item.buyPrice(gold: 1);
                    nextSlot++;
                }

                if (Main.CurrentPlayer.ZoneDesert || Main.CurrentPlayer.ZoneUndergroundDesert)
                {
                    shop.item[nextSlot].SetDefaults(Main.hardMode ? ItemID.OasisCrateHard : ItemID.OasisCrate);
                    shop.item[nextSlot].shopCustomPrice = priceMultiplier * Item.buyPrice(gold: 1);
                    nextSlot++;
                }

                if (Main.CurrentPlayer.ZoneUnderworldHeight)
                {
                    shop.item[nextSlot].SetDefaults(Main.hardMode ? ItemID.LavaCrateHard : ItemID.LavaCrate);
                    shop.item[nextSlot].shopCustomPrice = priceMultiplier * Item.buyPrice(gold: 1);
                    nextSlot++;
                }

                if (Main.CurrentPlayer.ZoneBeach)
                {
                    shop.item[nextSlot].SetDefaults(Main.hardMode ? ItemID.OceanCrateHard : ItemID.OceanCrate);
                    shop.item[nextSlot].shopCustomPrice = priceMultiplier * Item.buyPrice(gold: 1);
                    nextSlot++;
                }
            }

            if (NPC.downedBoss1)
            {
                var siltPrice = Main.hardMode ? Item.buyPrice(silver: 20) : Item.buyPrice(silver: 2);

                if (Main.CurrentPlayer.ZoneSnow)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.SlushBlock);
                    shop.item[nextSlot].shopCustomPrice = siltPrice;
                    nextSlot++;
                }
                else if (Main.CurrentPlayer.ZoneDesert || Main.CurrentPlayer.ZoneUndergroundDesert)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.DesertFossil);
                    shop.item[nextSlot].shopCustomPrice = siltPrice;
                    nextSlot++;
                }
                else
                {
                    shop.item[nextSlot].SetDefaults(ItemID.SiltBlock);
                    shop.item[nextSlot].shopCustomPrice = siltPrice;
                    nextSlot++;
                }
            }

            if (NPC.downedBoss3)
            {
                if (NPC.AnyNPCs(NPCID.Nurse))
                {
                    shop.item[nextSlot].SetDefaults(ItemID.HeartLantern);
                    nextSlot++;

                    if (Main.netMode != NetmodeID.SinglePlayer)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.LifeCrystal);
                        nextSlot++;
                    }
                }
            }

            if (Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(ItemID.HeartStatue);
                nextSlot++;
            }

        }

        private static void SetupDryadShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ItemID.HerbBag);
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(silver: 50);
            nextSlot++;

            shop.item[nextSlot].SetDefaults(ItemID.BottledWater);
            nextSlot++;

            if (Main.CurrentPlayer.ZoneJungle)
            {
                shop.item[nextSlot].SetDefaults(ItemID.JungleGrassSeeds);
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ItemID.BottledHoney);
                nextSlot++;
            }
        }

        private static void SetupDemolitionistShop(Chest shop, ref int nextSlot)
        {
            if (Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(ItemID.DirtBomb);
                nextSlot++;
            }
        }

        private static void SetupWizardShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ItemID.StarStatue);
            nextSlot++;
        }
    }
}
