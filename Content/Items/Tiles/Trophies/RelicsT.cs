using EbonianMod.Content.Tiles.Trophies;

namespace EbonianMod.Content.Items.Tiles.Trophies;

public abstract class RelicsT : ModItem
{
    public virtual int PlacedType => TileType<XRelicT>();
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(PlacedType, 0);

        Item.width = 30;
        Item.height = 40;
        Item.rare = ItemRarityID.Master;
        Item.master = true;
        Item.value = Item.buyPrice(0, 5);
    }
}
public class CecitiorRelic : RelicsT
{
    public override int PlacedType => TileType<CecitiorRelicT>();
}
public class XRelic : RelicsT
{
    public override int PlacedType => TileType<XRelicT>();
}
public class TerrortomaRelic : RelicsT
{
    public override int PlacedType => TileType<TerrortomaRelicT>();
}
public class GarbageRelic : RelicsT
{
    public override int PlacedType => TileType<GarbageRelicT>();
}
