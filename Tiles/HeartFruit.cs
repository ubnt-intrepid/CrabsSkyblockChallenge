using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;

namespace OneBlockChallenge.Tiles
{
    public class HeartFruit : ModTile
    {
        // FIXME: draw original texture
        public override string Texture => $"Terraria/Images/Tiles_{TileID.LifeFruit}"; 

        public override void PostSetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileObsidianKill[Type] = true;
            Main.tileSolid[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new int[2] { 16, 16 };
            TileObjectData.addTile(Type);

            var name = CreateMapEntryName();
            name.SetDefault("Heart Fruit");
            AddMapEntry(new Color(240, 80, 100), name);

            DustType = DustID.GrassBlades;
            SoundType = SoundID.Grass;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int num = Item.NewItem(new Vector2(i, j).ToWorldCoordinates(), Type: ItemID.LifeCrystal, Stack: 1, noBroadcast: false, -1);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, num, 1f);
            }
        }
    }
}
