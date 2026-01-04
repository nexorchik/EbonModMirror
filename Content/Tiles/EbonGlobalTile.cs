using EbonianMod.Content.Items.Consumables.Food;
using System.Collections.Generic;
using System.Linq;

namespace EbonianMod.Content.Tiles;

public class EbonGlobalTile : GlobalTile
{
    public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (type == TileID.Dirt && !fail)
        {
            if (Main.rand.NextBool(70))
                Item.NewItem(Item.GetSource_NaturalSpawn(), new Vector2(i * 16, j * 16), ItemType<Potato>(), Main.rand.Next(1, 10));
        }

        int[] plants = [TileID.BloomingHerbs, TileID.MatureHerbs, TileID.ImmatureHerbs];
        if (plants.Contains(type) && Main.tile[i, j].TileFrameX == 3 * 18)
        {
            if (Main.rand.NextBool(25 * (plants.ToList().IndexOf(type) + 1)))
                Item.NewItem(Item.GetSource_NaturalSpawn(), new Vector2(i * 16, j * 16), ItemType<Potato>(), Main.rand.Next(1, 10));
        }
    }
    public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
    {
        if (type == TileID.GemLocks && Main.tile[i, j + 5].TileType == TileType<XHouseBrick>()) return false;
        return base.CanKillTile(i, j, type, ref blockDamaged);
    }
    public override bool CanExplode(int i, int j, int type)
    {
        if (type == TileID.GemLocks && Main.tile[i, j + 5].TileType == TileType<XHouseBrick>()) return false;
        return base.CanExplode(i, j, type);
    }
}
public class EbonGlobalWall : GlobalWall
{
    public override void KillWall(int i, int j, int type, ref bool fail)
    {
        if (type == TileID.GemLocks && Main.tile[i, j + 5].TileType == TileType<XHouseBrick>())
            fail = true;
        base.KillWall(i, j, type, ref fail);
    }
    public override bool CanExplode(int i, int j, int type)
    {
        if (type == TileID.GemLocks && Main.tile[i, j + 5].TileType == TileType<XHouseBrick>())
            return false;
        return base.CanExplode(i, j, type);
    }
}
