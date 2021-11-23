using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CrabsSkyblockChallenge
{
    public class SkyblockPlayer : ModPlayer
    {
        const string ReceiveStarterBagName = "ReceiveStarterBag";
        public bool RecieveStarterBag = false;

        const string RecieveExtractinatorName = "RecieveExtractinator";
        public bool RecieveExtractinator = false;

        public override void LoadData(TagCompound tag)
        {
            RecieveStarterBag = tag.ContainsKey(ReceiveStarterBagName) && tag.GetBool(ReceiveStarterBagName);
            RecieveExtractinator = tag.ContainsKey(RecieveExtractinatorName) && tag.GetBool(RecieveExtractinatorName);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Set(ReceiveStarterBagName, RecieveStarterBag);
            tag.Set(RecieveExtractinatorName, RecieveExtractinator);
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (npc.type == ModContent.NPCType<NPCs.DirtyBlueSlime>())
            {
                // Check only the default accessory slot, and cannot be used with utility Mods like Antisocial.
                for (int i = 3; i < 10; i++)
                {
                    if (Player.IsAValidEquipmentSlotForIteration(i) && Player.armor[i].type == ItemID.RoyalGel)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
