using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CrabsSkyblockChallenge
{
    public class SkyblockPlayer : ModPlayer
    {
        const string ReceiveStarterItemsName = "ReceiveStarterItems";
        public bool ReceiveStarterItems = false;

        public override void LoadData(TagCompound tag)
        {
            ReceiveStarterItems = tag.ContainsKey(ReceiveStarterItemsName);
        }

        public override void SaveData(TagCompound tag)
        {
            if (ReceiveStarterItems)
            {
                tag[ReceiveStarterItemsName] = true;
            }
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
