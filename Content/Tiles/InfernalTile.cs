namespace EbonianMod.Content.Tiles;

public class InfernalTile : ModTile
{
    public override string Texture => "Terraria/Images/Tiles_" + TileID.Ash;
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileMergeDirt[Type] = true;
        Main.tileLighted[Type] = true;

        TileID.Sets.BlockMergesWithMergeAllBlock[Type] = false;
        DustType = DustID.Ash;
        //ItemDrop = ItemType<Items.Tiles.EbonHiveI>();

        AddMapEntry(Color.Gray);
    }
    public override bool CanExplode(int i, int j)
    {
        return false;
    }
    public override bool CanKillTile(int i, int j, ref bool blockDamaged)
    {
        return false;
    }
    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        num = fail ? 1 : 3;
    }
}

