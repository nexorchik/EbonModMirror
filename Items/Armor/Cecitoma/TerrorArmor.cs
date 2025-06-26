using EbonianMod.Common.Players;
using EbonianMod.Items.BossTreasure;
using EbonianMod.Items.Materials;

namespace EbonianMod.Items.Armor.Cecitoma;
[AutoloadEquip(EquipType.Head)]
public class Terrorhead : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;
        Item.value = 10000;
        Item.defense = 8;
        Item.rare = ItemRarityID.LightRed;
    }
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemType<CecitiorMaterial>(), 12).AddTile(TileID.MythrilAnvil).Register();
    }
    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return body.type == ItemType<Terrorbody>() && legs.type == ItemType<Terrorlegs>();
    }
    public const float DmgIncrease = 0.06f;
    public const int ManaIncrease = 100;
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DmgIncrease * 100, ManaIncrease);
    public override void UpdateEquip(Player player)
    {
        player.magicDamage += DmgIncrease;
        player.statManaMax2 += ManaIncrease;
    }
    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = this.GetLocalization("SetBonus").Value;
        player.GetModPlayer<SetBonusPlayer>().terrortoma = true;
    }
}
[AutoloadEquip(EquipType.Body)]
public class Terrorbody : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;
        Item.value = 10000;
        Item.defense = 16;
        Item.rare = ItemRarityID.LightRed;
    }
    public const float DmgIncrease = 0.07f, CritIncrease = 0.025f;
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DmgIncrease * 100, CritIncrease * 100);
    public override void UpdateEquip(Player player)
    {
        player.magicDamage += DmgIncrease;
        player.magicCrit += CritIncrease;
    }
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemType<CecitiorMaterial>(), 16).AddTile(TileID.MythrilAnvil).Register();
    }
}
[AutoloadEquip(EquipType.Legs)]
public class Terrorlegs : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;
        Item.value = 10000;
        Item.defense = 14;
        Item.rare = ItemRarityID.LightRed;
    }
    public const float DmgIncrease = 0.03f;
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DmgIncrease * 100);
    public override void UpdateEquip(Player player)
    {
        player.magicDamage += DmgIncrease;
    }
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemType<CecitiorMaterial>(), 12).AddTile(TileID.MythrilAnvil).Register();
    }
}
