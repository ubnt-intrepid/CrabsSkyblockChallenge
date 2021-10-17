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

        public static int NextBlock()
        {
            var maxValue = 0 switch
            {
                _ when defeatPlantera => 600,
                _ when defeatSkeletron => 550,
                _ when defeatBoss2 => 500,
                _ => 450,
            };

            return Main.rand.Next(maxValue) switch
            {
                // w=50
                (>=   0) and (<  50) => ItemID.DirtBlock,
                (>=  50) and (< 100) => ItemID.StoneBlock,
                (>= 100) and (< 150) => ItemID.SandBlock,
                (>= 150) and (< 200) => ItemID.SnowBlock,
                (>= 200) and (< 250) => ItemID.IceBlock,

                // w=30
                (>= 250) and (< 280) => ItemID.ClayBlock,
                (>= 280) and (< 310) => ItemID.HardenedSand,
                (>= 310) and (< 340) => ItemID.Sandstone,
                (>= 340) and (< 370) => ItemID.Cloud,

                // w=20
                (>= 370) and (< 390) => ItemID.SiltBlock,
                (>= 390) and (< 410) => ItemID.SlushBlock,
                (>= 410) and (< 430) => ItemID.DesertFossil,

                // w=15
                (>= 430) and (< 445) => ItemID.Cobweb,

                // w=5
                (>= 445) and (< 450) => ItemID.LifeCrystal,

                // Boss2
                (>= 450) and (< 480) => ItemID.AshBlock,
                (>= 480) and (< 500) => ItemID.Hellstone,

                // Skeletron
                (>= 500) and (< 550) => Main.rand.Next(3) switch {
                    0 => ItemID.BlueBrick,
                    1 => ItemID.GreenBrick,
                    2 => ItemID.PinkBrick,
                    _ => 0,
                },

                // Plantera
                (>= 550) and (< 600) => ItemID.LihzahrdBrick,

                _ => 0,
            };
        }
    }

    public class OBCGlobalItem : GlobalItem
    {
        public override void ExtractinatorUse(int extractType, ref int resultType, ref int resultStack)
        {
            if (OBCWorld.defeatWoF)
            {
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
    }

    public class OBCGlobalNPC : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Merchant)
            {
                if (Main.LocalPlayer.ZoneJungle)
                {
                    shop.item[nextSlot].SetDefaults(ItemID.HiveWand);
                    nextSlot++;

                    shop.item[nextSlot].SetDefaults(ItemID.Hive);
                    nextSlot++;
                }
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
}
