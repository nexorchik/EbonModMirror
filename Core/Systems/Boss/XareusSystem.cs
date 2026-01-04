using EbonianMod.Content.NPCs.ArchmageX;
using EbonianMod.Content.Tiles;
using Terraria.ModLoader.IO;

namespace EbonianMod.Core.Systems.Boss;

public class XareusSystem : ModSystem
{ public static bool heardXareusIntroMonologue;
    public static int xareusFightCooldown;
    public int timesDiedToXareus;
    public bool downedXareus, gotTheStaff, downedMartianXareus;
    public Point staffTilePosition;
    public override void Load()
    {
        heardXareusIntroMonologue = false;
        xareusFightCooldown = 0;
    }
    
    public override void PostUpdateEverything()
    {
        xareusFightCooldown--;

    if (!NPC.AnyNPCs(NPCType<ArchmageStaffNPC>()) && (TimeSystem.ConstantTimer % 600 == 0 || TimeSystem.ConstantTimer == 1))
        {
            if (staffTilePosition == Point.Zero || Main.tile[staffTilePosition.X, staffTilePosition.Y].HasTile
                || Main.tile[staffTilePosition.X, staffTilePosition.Y].TileType != (ushort)TileType<ArchmageStaffTile>())
            {
                for (int i = Main.maxTilesX / 2 - 740; i < Main.maxTilesX / 2 + 740; i++)
                    for (int j = 85; j < Main.maxTilesY / 2; j++)
                    {
                        if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType == (ushort)TileType<ArchmageStaffTile>())
                        {
                            staffTilePosition = new Point(i, j);
                            break;
                        }
                    }
            }
        }
        if (!NPC.AnyNPCs(NPCType<ArchmageStaffNPC>()))
        {
            if (staffTilePosition != Point.Zero && Main.tile[staffTilePosition.X, staffTilePosition.Y].HasTile && Main.tile[staffTilePosition.X, staffTilePosition.Y].TileType == (ushort)TileType<ArchmageStaffTile>())
            {
                MPUtils.NewNPC(new Vector2(staffTilePosition.X * 16 + 20, staffTilePosition.Y * 16 + 40), NPCType<ArchmageStaffNPC>(), ai3: 1);
            }
        }
    }
    public override void SaveWorldData(TagCompound tag)
    {
        tag.Set("XarusDown", downedXareus);
        tag.Set("XarusDownForReal", gotTheStaff);
        tag.Set("XarusDownForRealReal", downedMartianXareus);
        tag.Set("XareusDieTimes", timesDiedToXareus);
    }
    public override void LoadWorldData(TagCompound tag)
    {
        downedXareus = tag.GetBool("XarusDown");
        gotTheStaff = tag.GetBool("XarusDownForReal");
        downedMartianXareus = tag.GetBool("XarusDownForRealReal");
        timesDiedToXareus = tag.GetInt("XareusDieTimes");
    }
    public override void PostWorldGen()
    {
        downedXareus = false;
        gotTheStaff = false;
        downedMartianXareus = false;
        timesDiedToXareus = 0;
    }
    public override void OnWorldLoad()
    {
        xareusFightCooldown = 0;
    }
}