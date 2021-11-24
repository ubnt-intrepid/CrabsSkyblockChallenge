using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CrabsSkyblockChallenge
{
    public class SkyblockPlayer : ModPlayer
    {
        const string RecieveStarterItemsName = "RecieveStarterItems";
        public bool RecieveStarterItems = false;

        public override void LoadData(TagCompound tag)
        {
            RecieveStarterItems = tag.ContainsKey(RecieveStarterItemsName) && tag.GetBool(RecieveStarterItemsName);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Set(RecieveStarterItemsName, RecieveStarterItems);
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
