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
    }
}
