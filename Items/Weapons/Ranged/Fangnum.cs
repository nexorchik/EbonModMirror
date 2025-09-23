using EbonianMod.Projectiles.Bases;
using EbonianMod.Projectiles.Friendly.Crimson;

namespace EbonianMod.Items.Weapons.Ranged;

public class Fangnum : ModItem
{
    public override void SetDefaults()
    {
        Item.knockBack = 10f;
        Item.width = 42;
        Item.height = 24;
        Item.crit = 10;
        Item.damage = 23;
        Item.DamageType = DamageClass.Ranged;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.rare = ItemRarityID.LightRed;
        Item.useAnimation = 70;
        Item.shoot = ProjectileType<FangnumProjectile>();
        Item.value = Item.buyPrice(0, 10, 0, 0);
        Item.autoReuse = false;
        Item.noUseGraphic = true;
        Item.channel = true;
        Item.noMelee = true;
        Item.useAmmo = AmmoID.Bullet;
    }
    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        type = ProjectileType<FangnumProjectile>();
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        velocity = Vector2.Zero;
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        return false;
    }
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemID.TheUndertaker).AddIngredient(ItemID.TissueSample, 20).AddIngredient(ItemID.Vertebrae, 15).AddTile(TileID.Anvils).Register();
    }
    public override bool CanConsumeAmmo(Item ammo, Player player) => false;
}
public class FangnumProjectile : HeldProjectileGun
{
    float HoldOffset;
    public override string Texture => "EbonianMod/Items/Weapons/Ranged/Fangnum";
    public override bool? CanDamage() => false;
    public override void SetDefaults()
    {
        base.SetDefaults();
        ItemType = ItemType<Fangnum>();
        RotationSpeed = 0.23f;
        AnimationRotationSpeed = 0.3f;
        CursorOffset = new Vector2(0, 11);  
        Projectile.Size = new(42, 24);
    }
    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = (Main.MouseWorld - Projectile.Center).ToRotation();
    }
    public override void AI()
    {
        base.AI();

        Player player = Main.player[Projectile.owner];

        player.heldProj = Projectile.whoAmI;

        Projectile.ai[0]++;
        if (Projectile.ai[0] == 50)
        {
            AnimationRotation = -0.3f * player.direction;
            Projectile.UseAmmo(AmmoID.Bullet);
            SoundEngine.PlaySound(SoundID.Item38.WithPitchOffset(Main.rand.NextFloat(0.5f, 1f)), player.Center);
            Vector2 shotPoint = Projectile.Center + new Vector2(42, -11 * player.direction).RotatedBy(Projectile.rotation);
            if (player.whoAmI == Main.myPlayer)
                Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), shotPoint, Projectile.rotation.ToRotationVector2() * 20, ProjectileType<ToothProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            for (int i = 0; i < 7; i++)
                Dust.NewDustPerfect(shotPoint, DustID.Blood, (Projectile.rotation + Main.rand.NextFloat(-PiOver4, PiOver4)).ToRotationVector2() * Main.rand.NextFloat(2, 8), Scale: 1.5f).noGravity = true;
            Projectile.ai[0] = 0;
            HoldOffset = 6;
        }
        HoldOffset = Lerp(HoldOffset, 27, 0.2f);

        if (!player.channel)
            Projectile.Kill();
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Player player = Main.player[Projectile.owner];
        Main.EntitySpriteDraw(Helper.GetTexture(Texture).Value, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(Projectile.width / 2 - HoldOffset, Projectile.height / 2 + 4 * player.direction), Projectile.scale, player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        return false;
    }
}