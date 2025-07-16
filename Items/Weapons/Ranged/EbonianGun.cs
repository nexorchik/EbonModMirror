using EbonianMod.Projectiles.Bases;
using EbonianMod.Projectiles.Friendly.Corruption;

namespace EbonianMod.Items.Weapons.Ranged;

public class EbonianGun : ModItem
{
    public override void SetDefaults()
    {
        Item.knockBack = 10f;
        Item.width = 72;
        Item.height = 24;
        Item.crit = 15;
        Item.damage = 44;
        Item.DamageType = DamageClass.Ranged;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.rare = ItemRarityID.LightRed;
        Item.useAnimation = 30;
        Item.shoot = ProjectileType<EbonianGunP>();
        Item.value = Item.buyPrice(0, 10, 0, 0);
        Item.autoReuse = false;
        Item.noUseGraphic = true;
        Item.channel = true;
        Item.noMelee = true;
        Item.useAmmo = AmmoID.Bullet;
    }
    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        type = ProjectileType<EbonianGunP>();
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        velocity = Vector2.Zero;
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        return false;
    }
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemID.ShadowScale, 15).AddIngredient(ItemID.RottenChunk, 20).AddIngredient(ItemID.Musket).AddTile(TileID.Anvils).Register();
    }
}
public class EbonianGunP : HeldProjectileGun
{
    public override string Texture => "EbonianMod/Items/Weapons/Ranged/EbonianGun";

    public override bool? CanDamage() => false;


    public override void SetDefaults()
    {
        base.SetDefaults();
        ItemType = ItemType<EbonianGun>();
        RotationSpeed = 0.25f;
        Projectile.Size = new(72, 24);
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = Helper.FromAToB(Main.player[Projectile.owner].Center, Main.MouseWorld).ToRotation();
    }

    float HoldOffset;
    public override void AI()
    {
        base.AI();

        Player player = Main.player[Projectile.owner];

        HoldOffset = Lerp(HoldOffset, 26, 0.2f);

        player.heldProj = Projectile.whoAmI;

        Projectile.ai[0]++;
        if (Projectile.ai[0] == 30)
        {
            Projectile.UseAmmo(AmmoID.Bullet);
            AnimationRotation = -0.2f * player.direction;
            SoundEngine.PlaySound(SoundID.Item11.WithPitchOffset(Main.rand.NextFloat(-1f, -0.5f)), player.Center);
            SoundEngine.PlaySound(SoundID.Item17.WithPitchOffset(Main.rand.NextFloat(-0.5f, -0.2f)), player.Center);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + Projectile.rotation.ToRotationVector2() * 47 - new Vector2(0, 16 * player.direction).RotatedBy(Projectile.rotation), Projectile.rotation.ToRotationVector2() * 10, ProjectileType<CorruptionHitscan>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            HoldOffset = 11;
            Projectile.ai[0] = 0;
        }

        if (!player.channel)
            Projectile.Kill();
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Player player = Main.player[Projectile.owner];
        Texture2D tex = TextureAssets.Projectile[Type].Value;
        Main.EntitySpriteDraw(tex, Projectile.Center + player.GFX() - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(Projectile.width / 2 - HoldOffset, Projectile.height / 2 + 4 * player.direction), Projectile.scale, player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        return false;
    }
}
