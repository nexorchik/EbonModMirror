using Terraria.GameContent.Creative;

namespace EbonianMod.Content.Items.Consumables.Food;

public class Potato : ModItem
{
    public override string Texture => Helper.AssetPath + "Items/Consumables/Food/Potato";
    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;

        Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

        ItemID.Sets.DrinkParticleColors[Type] = new Color[2] {
            new Color(149, 97, 37),
            new Color(116, 61, 7),
        };
        ItemID.Sets.IsFood[Type] = true;
    }
    public override void SetDefaults()
    {
        Item.DefaultToFood(14, 20, BuffID.WellFed, 60 * 60);
        Item.useTime = 17;
        Item.useAnimation = 17;
        Item.maxStack = Item.CommonMaxStack;
        Item.useStyle = ItemUseStyleID.EatFood;
        Item.value = 2800;
        Item.rare = ItemRarityID.Blue;
        Item.consumable = true;
        Item.UseSound = SoundID.Item2;

        Item.ammo = Item.type;
    }
}
