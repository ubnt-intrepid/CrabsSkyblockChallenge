using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CrabsSkyblockChallenge
{
    public class SkyblockPlayer : ModPlayer
    {
        const string ReceiveStarterBagName = "ReceiveStarterBag";
        bool ReceiveStarterBag = false;

        public override void LoadData(TagCompound tag)
        {
            ReceiveStarterBag = tag.ContainsKey(ReceiveStarterBagName) && tag.GetBool(ReceiveStarterBagName);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Set(ReceiveStarterBagName, ReceiveStarterBag);
        }

        public override void OnEnterWorld(Player player)
        {
            if (!ReceiveStarterBag && (Main.expertMode || Main.masterMode))
            {
                ReceiveStarterBag = true;
                player.QuickSpawnItem(ModContent.ItemType<Items.StarterBag>());
            }
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (npc.type == ModContent.NPCType<NPCs.DirtSlime>())
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
