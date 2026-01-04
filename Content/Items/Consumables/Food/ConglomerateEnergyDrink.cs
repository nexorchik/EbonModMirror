using EbonianMod.Common.Players;
using EbonianMod.Content.Dusts;
using EbonianMod.Core.Systems.Boss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;

namespace EbonianMod.Content.Items.Consumables.Food;

public class ConglomerateEnergyDrink : ModItem
{
    public override string Texture => Helper.AssetPath + "Items/Consumables/Food/ConglomerateEnergyDrink";
    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;

        Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

        ItemID.Sets.DrinkParticleColors[Type] = new Color[2] {
            new Color(149, 97, 37),
            new Color(116, 61, 7 ),
        };
        ItemID.Sets.IsFood[Type] = true;
    }
    public override void SetDefaults()
    {
        Item.DefaultToFood(22, 36, BuffType<ConglomerateEnergyBuff>(), 120 * 60);
        Item.useTime = 17;
        Item.useAnimation = 17;
        Item.maxStack = Item.CommonMaxStack;
        Item.useStyle = ItemUseStyleID.DrinkLiquid;
        Item.value = 2800;
        Item.rare = ItemRarityID.Lime;
        Item.consumable = true;
        Item.UseSound = SoundID.Item3;
    }
    public override void HoldItem(Player player)
    {
        if (!player.ItemTimeIsZero)
            Dust.NewDustPerfect(player.itemLocation + Main.rand.NextVector2Circular(5, 5), DustType<SparkleDust>(), new Vector2(Main.rand.NextFloatDirection(), Main.rand.NextFloat(-0.1f, -7)), 0, Color.Lerp(Color.Maroon, Color.LawnGreen, Main.rand.NextFloat()), Main.rand.NextFloat(0.025f, 0.1f));
    }
    public override bool? UseItem(Player player)
    {
        return true;
    }
}
public class ConglomerateEnergyBuff : ModBuff
{
    public override string Texture => Helper.AssetPath + "Items/Consumables/Food/ConglomerateEnergyBuff";
    public override void Update(Player player, ref int buffIndex)
    {
        if (player.GetModPlayer<FlashPlayer>().flashTime <= 0)
            player.GetModPlayer<FlashPlayer>().FlashScreen(player.Center, player.buffTime[buffIndex], 1, 1);
        player.GetModPlayer<FlashPlayer>().flashPosition = Vector2.Lerp(player.GetModPlayer<FlashPlayer>().flashPosition, player.Center, 0.2f);
        player.maxRunSpeed *= 1.5f;
        ConglomerateSystem.conglomerateSkyFlash = Lerp(4, 8, (MathF.Sin(Main.GlobalTimeWrappedHourly) + 1) * 0.5f) * SmoothStep(0, 1, player.buffTime[buffIndex] / 120 * 60f);
    }
}
