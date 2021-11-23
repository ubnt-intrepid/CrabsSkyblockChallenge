using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CrabsSkyblockChallenge
{
    public class SkyblockPlayer : ModPlayer
    {
        const string RecieveStarterBagName = "RecieveStarterBag";
        public bool RecieveStarterBag = false;

        const string RecieveFromMerchantName = "RecieveFromMerchant";
        public bool RecieveFromMerchant = false;

        const string RecieveFromDryadName = "RecieveFromDryad";
        public bool RecieveFromDryad = false;

        public override void LoadData(TagCompound tag)
        {
            RecieveStarterBag = tag.ContainsKey(RecieveStarterBagName) && tag.GetBool(RecieveStarterBagName);
            RecieveFromMerchant = tag.ContainsKey(RecieveFromMerchantName) && tag.GetBool(RecieveFromMerchantName);
            RecieveFromDryad = tag.ContainsKey(RecieveFromDryadName) && tag.GetBool(RecieveFromDryadName);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Set(RecieveStarterBagName, RecieveStarterBag);
            tag.Set(RecieveFromMerchantName, RecieveFromMerchant);
            tag.Set(RecieveFromDryadName, RecieveFromDryad);
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
