using EbonianMod.Content.Tiles.Ambient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace EbonianMod.Core.Systems.Worldgen.AmbientWorldgen;
public class RustyAxeGen : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Pots"));
        if (ShiniesIndex != -1)
        {
            tasks.Insert(ShiniesIndex + 1, new PassLegacy("Throwing axes into a pond", GenAxe));
        }
    }
    readonly List<ushort> AllowedBottomTiles = [TileID.Dirt, TileID.Grass, TileID.Stone, TileID.Sand, TileID.ClayBlock, TileID.Silt];
    public void GenAxe(GenerationProgress progress, GameConfiguration _)
    {
        int amount = 0;
        int attempts = 0;
        while (++attempts < 500 && amount < 30 + WorldGen.GetWorldSize() * 10)
        {
            int y = (int)Main.worldSurface / 16 + 20;
            int x = 100;
            for (int i = x; i < Main.maxTilesX - 100; i++)
            {
                for (int j = y; j < Main.UnderworldLayer / 2; j++)
                {
                    bool suitable = false;
                    bool canSpawn = Main.tile[i, j].CheckingLiquid && Main.tile[i, j].LiquidType == LiquidID.Water && Main.tile[i, j].LiquidAmount > 0;
                    if (canSpawn)
                    {
                        for (int l = i - 1; l < i + 5; l++)
                        {
                            for (int m = j; m < j + 4; m++)
                            {
                                if (Main.tile[l, m].LiquidType == LiquidID.Water && Main.tile[l, m].LiquidAmount > 0)
                                    suitable = true;
                            }
                            if (l >= i && l < i + 4)
                            {
                                if (!Main.tile[l, j + 5].HasTile)
                                    suitable = false;
                                ushort type = Main.tile[l, j + 5].TileType;
                                if (Main.tile[l, j + 5].HasTile && !AllowedBottomTiles.Contains(type))
                                    suitable = false;
                            }
                        }
                    }
                    if (suitable)
                    {
                        WorldGen.PlaceTile(i, j, TileType<RustyAxeT>(), true, true);
                        if (Main.tile[i, j].TileType == TileType<RustyAxeT>())
                        {
                            amount++;
                            i += 25;
                            continue;
                        }
                    }
                }
            }
        }
    }
}

