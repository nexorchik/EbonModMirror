using EbonianMod.Content.Buffs;
using EbonianMod.Content.Projectiles.Dev;

namespace EbonianMod.Content.Items.Dev;

public class RollegItem : ModItem
{
    public override string Texture => Helper.Placeholder;
    public override bool IsLoadingEnabled(Mod mod) => false;
    public override void SetDefaults()
    {
        Item.damage = 80;
        Item.DamageType = DamageClass.Summon;
        Item.useTime = Item.useAnimation = 30;
        Item.shoot = ProjectileType<Rolleg>();
        Item.shootSpeed = 0f;
        Item.UseSound = new("EbonianMod/Assets/Sounds/rolleg");
        Item.useStyle = ItemUseStyleID.RaiseLamp;
        Item.autoReuse = false;
        Item.buffType = BuffType<RollegB>();
        Item.rare = 7;
        Item.buffTime = 69; //LMAO HAHA XD
    }
}
