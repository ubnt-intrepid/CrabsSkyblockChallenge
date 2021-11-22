﻿using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrabsSkyblockChallenge.NPCs
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
            if (npc.type == NPCID.SandSlime)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.SandBlock, minimumDropped: 3, maximumDropped: 5));
            }

            if (Array.IndexOf(AntlionNPCs, npc.type) != -1)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Sandstone, minimumDropped: 5, maximumDropped: 10));
                npcLoot.Add(ItemDropRule.Common(ItemID.DesertFossil, minimumDropped: 3, maximumDropped: 5));
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

        public override void GetChat(NPC npc, ref string chat)
        {
            if (npc.type == NPCID.Guide)
            {
                var me = Main.CurrentPlayer.GetModPlayer<SkyblockPlayer>();
                if (!me.ReceiveStarterBag && (Main.expertMode || Main.masterMode))
                {
                    chat = "Are you expert Terrarian? This is a gift from Santa Claus.";
                    me.ReceiveStarterBag = true;
                    me.Player.QuickSpawnItem(ModContent.ItemType<Items.StarterBag>());
                }
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Merchant)
            {
                shop.item[nextSlot].SetDefaults(ItemID.LifeCrystal);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 2);
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ItemID.Extractinator);
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ItemID.SiltBlock);
                shop.item[nextSlot].shopCustomPrice = Item.buyPrice(copper: 50);
                nextSlot++;
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
