namespace EbonianMod.Content.Items.Vanity;

public abstract class BossMask : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 24;
        Item.value = Item.sellPrice(gold: 1);
        Item.rare = ItemRarityID.Blue;
        Item.vanity = true;
    }
}
[AutoloadEquip(EquipType.Head)]
public class XMask : BossMask { 
    public override string Texture => Helper.AssetPath + "Items/Vanity/XMask";
}
[AutoloadEquip(EquipType.Head)]
public class GarbageMask : BossMask {
    public override string Texture => Helper.AssetPath + "Items/Vanity/GarbageMask";
}
[AutoloadEquip(EquipType.Head)]
public class TTomaMask : BossMask { 
    public override string Texture => Helper.AssetPath + "Items/Vanity/TTomaMask";
}
[AutoloadEquip(EquipType.Head)]
public class CeciMask : BossMask { 
    public override string Texture => Helper.AssetPath + "Items/Vanity/CeciMask";
}
