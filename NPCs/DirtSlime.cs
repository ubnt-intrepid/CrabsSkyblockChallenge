using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CrabsSkyblockChallenge.NPCs
{
    public class DirtSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dirt Slime");
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.BlueSlime];
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.BlueSlime);
            AIType = NPCID.BlueSlime;
            AnimationType = NPCID.BlueSlime;

            // TODO: add banner
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDaySlime.Chance * 0.1f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, minimumDropped: 1, maximumDropped: Main.expertMode ? 3 : 2));
            npcLoot.Add(ItemDropRule.Common(ItemID.SlimeStaff, chanceDenominator: Main.expertMode || Main.masterMode ? 7000 : 10000));
            npcLoot.Add(ItemDropRule.Common(ItemID.DirtBlock, minimumDropped: 1, maximumDropped: 5));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
            });
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 10; i++)
            {
                var dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Dirt);

                dust.velocity.X += Main.rand.NextFloat(-0.05f, 0.05f);
                dust.velocity.Y += Main.rand.NextFloat(-0.05f, 0.05f);

                dust.scale *= 1f + Main.rand.NextFloat(-0.03f, 0.03f);
            }
        }
    }
}
