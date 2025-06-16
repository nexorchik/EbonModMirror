using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Items.Weapons.Magic;
public class BotanistHead : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.staff[Type] = true;
    }
    public override void SetDefaults()
    {
        Item.damage = 20;
        Item.width = 40;
        Item.height = 40;
        Item.mana = 5;
        Item.DamageType = DamageClass.Magic;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.knockBack = 10;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 20, 0, 0);
        Item.autoReuse = true;
    }
}
