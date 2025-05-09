namespace EbonianMod.Items.Accessories;

public class XTentacleAcc : ModItem
{
    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.rare = 4;
        Item.expert = true;
        Item.value = Item.buyPrice(0, 15, 0, 0);
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.statManaMax += 60;
        player.GetModPlayer<EbonianPlayer>().xTent = true;
    }
}
