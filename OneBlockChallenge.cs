using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace OneBlockChallenge
{
    public class OneBlockChallenge : Mod
    {
        public override void AddRecipes()
        {
            CreateRecipe(ItemID.Hive)
                .AddIngredient(ItemID.Wood, stack: 5) // FIXME: available for any wood
                .AddCondition(Recipe.Condition.NearHoney)
                .Register();

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
                new Item(ItemID.HoneyBucket, stack: 1),
                new Item(ItemID.Torch, stack: 1),
            };
        }
    }

    public class OBCWorld : ModSystem
    {
        public static bool defeatBoss2 = false;   // Eater of World / Brain of Cthulhu
        public static bool defeatSkeletron = false;
        public static bool defeatWoF = false;
        public static bool defeatPlantera = false;

        public override void LoadWorldData(TagCompound tags)
        {
            TryGetTag(tags, "defeatBoss2", out defeatBoss2);
            TryGetTag(tags, "defeatSkeletron", out defeatSkeletron);
            TryGetTag(tags, "defeatWoF", out defeatWoF);
            TryGetTag(tags, "defeatPlantera", out defeatPlantera);
        }

        public override void SaveWorldData(TagCompound tags)
        {
            tags.Set("defeatBoss2", defeatBoss2);
            tags.Set("defeatSkeletron", defeatSkeletron);
            tags.Set("defeatWoF", defeatWoF);
            tags.Set("defeatPlantera", defeatPlantera);
        }

        static bool TryGetTag<T>(TagCompound me, string name, out T dest)
        {
            if (me.ContainsKey(name))
            {
                dest = me.Get<T>(name);
                return true;
            }
            else
            {
                dest = default(T);
                return false;
            }
        }
    }

    public class OBCGlobalNPC : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            switch (type)
            {
                case NPCID.Merchant:
                    shop.item[nextSlot].SetDefaults(ItemID.Extractinator);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(gold: 2);
                    nextSlot++;

                    shop.item[nextSlot].SetDefaults(ItemID.LifeCrystal);
                    nextSlot++;

                    shop.item[nextSlot].SetDefaults(ItemID.Cobweb);
                    shop.item[nextSlot].shopCustomPrice = Item.buyPrice(copper: 10);
                    nextSlot++;

                    if (OBCWorld.defeatPlantera)
                    {
                        shop.item[nextSlot].SetDefaults(ItemID.LihzahrdAltar);
                        nextSlot++;
                    }

                    break;

                default:
                    break;
            }
        }

        public override void OnKill(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                case NPCID.BrainofCthulhu:
                    NPC.SetEventFlagCleared(ref OBCWorld.defeatBoss2, -1);
                    break;

                case NPCID.SkeletronHead:
                    NPC.SetEventFlagCleared(ref OBCWorld.defeatSkeletron, -1);
                    break;

                case NPCID.WallofFlesh:
                    NPC.SetEventFlagCleared(ref OBCWorld.defeatWoF, -1);
                    break;

                case NPCID.Plantera:
                    NPC.SetEventFlagCleared(ref OBCWorld.defeatPlantera, -1);
                    break;

                default:
                    break;
            }
        }
    }

    public class OBCGlobalItem : GlobalItem
    {
        static int[] tier1Ores = new int[] { ItemID.CopperOre, ItemID.TinOre, ItemID.IronOre, ItemID.LeadOre, ItemID.CobaltOre, ItemID.PalladiumOre };
        static int[] tier2Ores = new int[] { ItemID.SilverOre, ItemID.TungstenOre, ItemID.MythrilOre, ItemID.OrichalcumOre };
        static int[] tier3Ores = new int[] { ItemID.GoldOre, ItemID.PlatinumOre, ItemID.AdamantiteOre, ItemID.TitaniumOre };

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ExtractinatorMode[ItemID.StoneBlock] = ItemID.StoneBlock;
            ItemID.Sets.ExtractinatorMode[ItemID.HardenedSand] = ItemID.HardenedSand;
            ItemID.Sets.ExtractinatorMode[ItemID.IceBlock] = ItemID.IceBlock;
        }

        public override void ExtractinatorUse(int extractType, ref int resultType, ref int resultStack)
        {
            switch (extractType)
            {
                case ItemID.StoneBlock:
                    StoneExtractinatorUse(ref resultType, ref resultStack);
                    break;

                case ItemID.HardenedSand:
                    SandExtractinatorUse(ref resultType, ref resultStack);
                    break;

                case ItemID.IceBlock:
                    IceExtractinatorUse(ref resultType, ref resultStack);
                    break;

                default:
                    if (OBCWorld.defeatWoF) {
                        HardmodeExtractinatorUse(ref resultType, ref resultStack);
                    }
                    break;
            }
        }

        // FIXME: tweak extractinator chances

        private static void StoneExtractinatorUse(ref int resultType, ref int resultStack)
        {
            var extracted = new int[] { ItemID.SiltBlock, ItemID.Marble, ItemID.Granite };
            resultType = Utils.Choice(extracted);
            resultStack = 1 + Main.rand.Next(3);
        }

        private static void SandExtractinatorUse(ref int resultType, ref int resultStack)
        {
            var extracted = new int[] { ItemID.SandBlock, ItemID.Sandstone, ItemID.DesertFossil };
            resultType = Utils.Choice(extracted);
            resultStack = 1 + Main.rand.Next(3);
        }

        private static void IceExtractinatorUse(ref int resultType, ref int resultStack) {
            var extracted = new int[] { ItemID.SnowBlock, ItemID.SlushBlock };
            resultType = Utils.Choice(extracted);
            resultStack = 1 + Main.rand.Next(3);
        }

        private void HardmodeExtractinatorUse(ref int resultType, ref int resultStack) {
            switch (resultType)
            {
                case ItemID.CopperOre:
                case ItemID.TinOre:
                case ItemID.IronOre:
                case ItemID.LeadOre:
                    resultType = Utils.Choice(tier1Ores);
                    break;

                case ItemID.SilverOre:
                case ItemID.TungstenOre:
                    resultType = Utils.Choice(tier2Ores);
                    break;

                case ItemID.GoldOre:
                case ItemID.PlatinumOre:
                    resultType = Utils.Choice(tier3Ores);
                    break;

                default:
                    break;
            }
        }
    }

    public class Utils {
        public static T Choice<T>(T[] array)
        {
            var index = Main.rand.Next(0, array.Length);
            return array[index];
        }
    }
}
