using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrabsSkyblockChallenge
{
    public class SkyblockGlobalItem : GlobalItem
    {
        static readonly int[] crateTypes = new int[]
        {
            ItemID.WoodenCrate,
            ItemID.WoodenCrateHard,
            ItemID.IronCrate,
            ItemID.IronCrateHard,
            ItemID.GoldenCrate,
            ItemID.GoldenCrateHard,
            ItemID.JungleFishingCrate,
            ItemID.JungleFishingCrateHard,
            ItemID.FloatingIslandFishingCrate,
            ItemID.FloatingIslandFishingCrateHard,
            ItemID.CorruptFishingCrate,
            ItemID.CorruptFishingCrateHard,
            ItemID.CrimsonFishingCrate,
            ItemID.CrimsonFishingCrateHard,
            ItemID.HallowedFishingCrate,
            ItemID.HallowedFishingCrateHard,
            ItemID.DungeonFishingCrate,
            ItemID.DungeonFishingCrateHard,
            ItemID.FrozenCrate,
            ItemID.FrozenCrateHard,
            ItemID.OasisCrate,
            ItemID.OasisCrateHard,
            ItemID.LavaCrate,
            ItemID.LavaCrateHard,
            ItemID.OceanCrate,
            ItemID.OceanCrateHard,
        };

        public override void CaughtFishStack(int type, ref int stack)
        {
            if (Array.IndexOf(crateTypes, type) != -1)
            {
                var player = Main.CurrentPlayer.GetModPlayer<SkyblockPlayer>();

                if (player.CrateFishingCounts.ContainsKey((short)type))
                {
                    player.CrateFishingCounts[(short)type] += stack;
                }
                else
                {
                    player.CrateFishingCounts[(short)type] = stack;
                }
            }
        }
    }
}
