using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace CrabsSkyblockChallenge.Items
{
    public class SkyblockGlobalItem : GlobalItem
    {
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "crate")
            {
                switch (arg)
                {
                    case ItemID.WoodenCrate:
                    case ItemID.WoodenCrateHard:
                        AppendSurfaceChestLoot(player);
                        break;

                    case ItemID.Aglet:
                    case ItemID.IronCrate:
                    case ItemID.IronCrateHard:
                    case ItemID.GoldenCrate:
                    case ItemID.GoldenCrateHard:
                        AppendUndergroundChestLoot(player);
                        break;

                    default:
                        break;
                }
            }
        }

        static void AppendSurfaceChestLoot(Player player)
        {
            if (Main.rand.Next(5) == 0)
            {
                var item = Main.rand.Next(6) switch
                {
                    0 => ItemID.Spear,
                    1 => ItemID.Blowpipe,
                    2 => ItemID.WoodenBoomerang,
                    3 => ItemID.WandofSparking,
                    4 => ItemID.PortableStool,
                    _ => ItemID.BabyBirdStaff,
                };
                player.QuickSpawnItem(item);
            }
        }

        static void AppendUndergroundChestLoot(Player player)
        {
            if (Main.rand.Next(5) == 0)
            {
                var item = Main.rand.Next(7) switch
                {
                    0 => ItemID.MagicMirror,
                    1 => ItemID.Mace,
                    2 => ItemID.FlareGun,
                    3 => ItemID.HermesBoots,
                    4 => ItemID.CloudinaBottle,
                    5 => ItemID.BandofRegeneration,
                    _ => ItemID.ShoeSpikes,
                };
                player.QuickSpawnItem(item);
                if (item == ItemID.FlareGun)
                {
                    player.QuickSpawnItem(ItemID.Flare, Main.rand.Next(25, 51));
                }
            }
        }
    }
}
