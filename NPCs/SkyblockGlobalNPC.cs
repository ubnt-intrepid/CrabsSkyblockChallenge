﻿using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrabsSkyblockChallenge.NPCs
{
    class EnemyLootRules : GlobalNPC
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
            if (npc.type == NPCID.SandSlime)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.SandBlock, minimumDropped: 3, maximumDropped: 5));
            }

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
                npcLoot.Add(ItemDropRule.Common(ItemID.AshBlock, minimumDropped: 5, maximumDropped: 10));
                npcLoot.Add(ItemDropRule.Common(ItemID.Hellstone, minimumDropped: 3, maximumDropped: 5));
            }

            if (npc.type == NPCID.Harpy)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Cloud, minimumDropped: 3, maximumDropped: 5));
            }

            if (Array.IndexOf(FrostRegionNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.SnowGlobe, chanceDenominator: 100));
            }
        }
    }

    class TownNPCMods : GlobalNPC
    {
        static bool IsStarterBagProvider(NPC npc)
        {
            return (Main.tenthAnniversaryWorld && npc.type == NPCID.Princess)
                || (Main.getGoodWorld && npc.type == NPCID.Demolitionist)
                || (Main.drunkWorld && npc.type == NPCID.PartyGirl)
                || (Main.notTheBeesWorld && npc.type == NPCID.Merchant)
                || npc.type == NPCID.Guide;
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            var me = Main.CurrentPlayer.GetModPlayer<SkyblockPlayer>();

            if (IsStarterBagProvider(npc) && !me.RecieveStarterBag && (Main.expertMode || Main.masterMode))
            {
                chat = "Are you expert Terrarian? This is a gift from Santa Claus.";
                me.RecieveStarterBag = true;
                me.Player.QuickSpawnItem(ModContent.ItemType<Items.StarterBag>());
                return;
            }

            if (npc.type == NPCID.Merchant && !me.RecieveFromMerchant)
            {
                chat = "Now is a time to get ready to confront evil presenses.";
                me.RecieveFromMerchant = true;
                me.Player.QuickSpawnItem(ItemID.Extractinator);
                me.Player.QuickSpawnItem(ItemID.LifeCrystal, stack: 5);
                return;
            }

            if (npc.type == NPCID.Dryad && !me.RecieveFromDryad)
            {
                chat = "Be careful with poisoned Bee needles.";
                me.RecieveFromDryad = true;
                me.Player.QuickSpawnItem(ItemID.HiveWand);
                return;
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Merchant)
            {
                shop.item[nextSlot].SetDefaults(ItemID.SiltBlock);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(silver: 1);
                nextSlot++;

                if (Main.CurrentPlayer.ZoneSnow)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.SlushBlock);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(silver: 1);
                    nextSlot++;
                }

                if (Main.CurrentPlayer.ZoneDesert || Main.CurrentPlayer.ZoneUndergroundDesert)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.DesertFossil);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(silver: 1);
                    nextSlot++;
                }

                if (NPC.downedBoss1 && NPC.AnyNPCs(NPCID.Nurse))
                {
                    shop.item[nextSlot].SetDefaults(ItemID.LifeCrystal);
                    nextSlot++;
                }
            }

            if (type == NPCID.Dryad)
            {
                shop.item[nextSlot].SetDefaults(ItemID.HerbBag);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(silver: 50);
                nextSlot++;

                if (Main.CurrentPlayer.ZoneJungle)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.JungleGrassSeeds);
                    nextSlot++;
                }
            }

            if (type == NPCID.Demolitionist && Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(ItemID.DirtBomb);
                nextSlot++;
            }
        }
    }
}
