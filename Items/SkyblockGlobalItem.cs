using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrabsSkyblockChallenge.Items
{
    public class SkyblockGlobalItem : GlobalItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ExtractinatorMode[ItemID.AshBlock] = ItemID.AshBlock;
        }

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
            if (extractType == ItemID.AshBlock && Main.hardMode)
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
                        var maxValue = Main.LocalPlayer.GetBestPickaxe().pick switch
                        {
                            < 100 => 8,  // pre-hardmode ores
                            (>= 100) and (< 110) => 10, // Cobalt, Palladium
                            (>= 110) and (< 150) => 12, // Mythril, Orichalcum
                            _ => 14, // Adamantite, Titanium
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
    }
}
