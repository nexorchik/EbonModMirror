using EbonianMod.NPCs.Overworld.Critters;

namespace EbonianMod.Items.Critters;

public class RamItem : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Bunny);
        Item.makeNPC = NPCType<Ram>();
        Item.value = Item.buyPrice(0, 0, 10, 0);
    }
}
public class RamItemNaked : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Bunny);
        Item.makeNPC = 0;
        Item.value = Item.buyPrice(0, 0, 10, 0);
    }
    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
            MPUtils.NewNPC(Main.MouseWorld, NPCType<Ram>(), ai2: 1);
        return base.UseItem(player);
    }
}
