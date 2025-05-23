using EbonianMod.Common.Players;
using Terraria.GameContent.Creative;
namespace EbonianMod.Items.Accessories;

[AutoloadEquip(EquipType.Back)]
public class EbonianHeart : ModItem
{
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(5, 1, 5); // Defense reduction, Minion increase, Summon damage increase
    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.rare = 4;
        Item.defense = -5;
        Item.expert = true;
        Item.value = Item.buyPrice(0, 15, 0, 0);
    }
    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        if (!NPC.AnyNPCs(NPCType<EbonianHeartNPC>()))
        {
            NPC.NewNPC(player.GetSource_Accessory(Item), (int)player.Center.X, (int)player.Center.Y, NPCType<EbonianHeartNPC>(), Target: player.whoAmI);
        }
        player.GetDamage(DamageClass.Summon) += 0.05f;
        player.maxMinions += 1;
        AccessoryPlayer modPlayer = player.GetModPlayer<AccessoryPlayer>();
        modPlayer.heartAcc = true;
    }
}