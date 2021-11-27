using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CrabsSkyblockChallenge.NPCs
{
    public class DirtyBlueSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dirty Blue Slime");
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.BlueSlime];
        }

        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 18;
            NPC.damage = 7;
            NPC.defense = 2;
            NPC.lifeMax = 25;
            NPC.value = 25f;
            NPC.alpha = 60;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = NPCAIStyleID.Slime;
            NPC.buffImmune[BuffID.Poisoned] = true;

            AIType = NPCID.BlueSlime;
            AnimationType = NPCID.BlueSlime;
            Banner = Item.NPCtoBanner(NPCID.BlueSlime);
            BannerItem = Item.BannerToItem(Banner);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float baseChance = spawnInfo.spawnTileType == TileID.Dirt ? 0.5f : 0.3f;
            return baseChance * SpawnCondition.OverworldDaySlime.Chance;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, minimumDropped: 1, maximumDropped: Main.expertMode ? 3 : 2));
            npcLoot.Add(ItemDropRule.Common(ItemID.SlimeStaff, chanceDenominator: Main.expertMode || Main.masterMode ? 7000 : 10000));
            npcLoot.Add(ItemDropRule.Common(ItemID.DirtBlock, minimumDropped: 5, maximumDropped: 10));
            npcLoot.Add(ItemDropRule.Common(ItemID.GrassSeeds, chanceDenominator: 200));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement("This type of slime is soiled and will drop some Dirt Blocks"),
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, Main.rand.Next(2) == 0 ? DustID.Dirt : DustID.t_Slime);

                dust.velocity.X += Main.rand.NextFloat(-0.05f, 0.05f);
                dust.velocity.Y += Main.rand.NextFloat(-0.05f, 0.05f);

                dust.scale *= 1f + Main.rand.NextFloat(-0.03f, 0.03f);
            }
        }
    }
}
