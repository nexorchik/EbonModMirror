using EbonianMod.NPCs.Overworld.Critters;

namespace EbonianMod.Items.Misc.Critters;

public class SheepItem : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Bunny);
        Item.makeNPC = NPCType<Sheep>();
        Item.value = Item.buyPrice(0, 0, 10, 0);
    }
    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
            MPUtils.NewNPC(Main.MouseWorld, NPCType<Sheep>());
        return true;
    }
}
public class SheepItemNaked : ModItem
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
            MPUtils.NewNPC(Main.MouseWorld, NPCType<Sheep>(), ai2: 1);
        return true;
    }
}
