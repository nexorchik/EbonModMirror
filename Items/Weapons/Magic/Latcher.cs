using EbonianMod.Projectiles.Bases;
using EbonianMod.Projectiles.Friendly.Crimson;

namespace EbonianMod.Items.Weapons.Magic;

public class Latcher : ModItem
{
    public override void SetDefaults()
    {
        Item.DamageType = DamageClass.Magic;
        Item.damage = 80;
        Item.useTime = 1;
        Item.mana = 1;
        Item.useAnimation = 100;
        Item.shoot = ProjectileType<LatcherGraphics>();
        Item.shootSpeed = 1f;
        Item.rare = ItemRarityID.Green;
        Item.useStyle = 5;
        Item.value = Item.buyPrice(0, 5, 0, 0);
        Item.autoReuse = false;
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.channel = true;
        Item.mana = 50;
    }
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemID.Vertebrae, 20).AddIngredient(ItemID.Hook).AddTile(TileID.Anvils).Register();
    }
    public override bool CanUseItem(Player player)
    {
        return player.ownedProjectileCounts[ProjectileType<Projectiles.Friendly.Crimson.LatcherP>()] < 1;
    }
    public override bool? CanAutoReuseItem(Player player) => false;

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        velocity.Normalize();
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        return false;
    }
}
public class LatcherGraphics : HeldProjectileGun
{
    public override string Texture => "EbonianMod/Items/Weapons/Magic/Latcher";

    Vector2 Scale = new Vector2(0, 0);
    public override void OnSpawn(IEntitySource source)
    {
        Player player = Main.player[Projectile.owner];
        Projectile.rotation = Helper.FromAToB(player.Center, Main.MouseWorld).ToRotation() + player.direction * Pi;
    }

    public override bool? CanDamage() => false;

    public override void SetDefaults()
    {
        base.SetDefaults();
        ItemType = ItemType<Latcher>();
        Projectile.Size = new Vector2(60, 38);
    }

    bool CanShoot = true;

    public override void AI()
    {
        base.AI();

        Player player = Main.player[Projectile.owner];

        if (player.ownedProjectileCounts[ProjectileType<LatcherP>()] < 1 && !CanShoot)
            Projectile.Kill();

        Projectile.ai[0]++;
        if (!CanShoot)
            RotationSpeed = 0.02f;
        else
        {
            RotationSpeed = 0.08f;
            if (!player.channel && Projectile.ai[0] > 40)
            {
                CanShoot = false;
                Scale = new Vector2(0.65f, 1.6f);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center + Projectile.rotation.ToRotationVector2() * 30, Projectile.rotation.ToRotationVector2() * 37, ProjectileType<LatcherP>(), 1, Projectile.knockBack, Projectile.owner);
                SoundEngine.PlaySound(SoundID.NPCHit8.WithPitchOffset(Main.rand.NextFloat(-0.4f, 0.4f)), player.Center);
            }
        }
        Scale = Vector2.Lerp(Scale, new Vector2(1, 1), 0.14f);
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = Helper.GetTexture("Items/Weapons/Magic/Latcher").Value;
        Rectangle frameRect = new Rectangle(0, 0, Projectile.width, Projectile.height);
        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frameRect, lightColor, Projectile.rotation, new Vector2(Projectile.width / 2 - 25, Projectile.height / 2), Scale, Main.player[Projectile.owner].direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        return false;
    }
}
