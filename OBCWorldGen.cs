using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace OneBlockChallenge
{
    public class OBCWorldGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            var isOBCSeed = string.Equals(WorldGen.currentWorldSeed, "one block challenge", StringComparison.OrdinalIgnoreCase);
            if (!isOBCSeed)
            {
                return;
            }

            Mod.Logger.Info("Switch to OBC world generation");

            tasks.RemoveRange(1, tasks.Count - 1);

            tasks.Add(new PassLegacy("OBC World Generation", (progress, config) =>
            {
                progress.Message = "Generate One Block Challenge World";

                WorldGen.clearWorld();

                Main.worldSurface = Main.maxTilesY * 0.3;
                Main.rockLayer = Main.maxTilesY * 0.5;

                Main.spawnTileX = (int)(Main.maxTilesX * 0.5);
                Main.spawnTileY = (int)Main.worldSurface - 100;

                var dungeonDirection = Main.rand.NextBool() ? 1 : -1;
                Main.dungeonX = (int)(Main.maxTilesX * (0.5 + dungeonDirection * 0.25));
                Main.dungeonY = Main.spawnTileY;

                WorldGen.PlaceTile(Main.spawnTileX, Main.spawnTileY, ModContent.TileType<Tiles.InfiniteBlock>());
                WorldGen.PlaceTile(Main.dungeonX, Main.dungeonY, TileID.GreenDungeonBrick);

                int guideID = NPC.NewNPC(Main.spawnTileX * 16, Main.spawnTileY * 16, NPCID.Guide);
                Main.npc[guideID].homeless = true;
                Main.npc[guideID].homeTileX = Main.spawnTileX;
                Main.npc[guideID].homeTileY = Main.spawnTileY;
                Main.npc[guideID].direction = 1;

                int oldManID = NPC.NewNPC(Main.dungeonX * 16, Main.dungeonY * 16, NPCID.OldMan);
                Main.npc[oldManID].homeless = false;
                Main.npc[oldManID].homeTileX = Main.dungeonX;
                Main.npc[oldManID].homeTileY = Main.dungeonY;
            }));
        }
    }
}
