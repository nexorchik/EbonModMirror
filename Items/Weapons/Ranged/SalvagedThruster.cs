using EbonianMod.Projectiles.Bases;

namespace EbonianMod.Items.Weapons.Ranged;

public class SalvagedThruster : ModItem
{
    public override void SetDefaults()
    {
        Item.DamageType = DamageClass.Ranged;
        Item.damage = 4;
        Item.shoot = ProjectileType<SalvagedThrusterP>();
        Item.shootSpeed = 1f;
        Item.rare = ItemRarityID.Green;
        Item.useStyle = 5;
        Item.value = Item.buyPrice(0, 1, 0, 0);
        Item.autoReuse = false;
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.channel = true;
        Item.useAmmo = AmmoID.Gel;
    }

    public override bool CanConsumeAmmo(Item ammo, Player player) => false;
    public override bool? CanAutoReuseItem(Player player) => false;
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        velocity.Normalize();
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        return false;
    }
}
public class SalvagedThrusterP : HeldProjectileGun
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        ItemType = ItemType<SalvagedThruster>();
        RotationSpeed = 0.025f;
        Projectile.Size = new Vector2(44, 30);
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 4;
        Projectile.frame = 0;
        Projectile.ArmorPenetration = 100;
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = Helper.FromAToB(Main.player[Projectile.owner].Center, Main.MouseWorld).ToRotation();
    }
    Vector2 Scale;
    float RayLength = 0, Charge;
    bool CanAttack, IsReady;
    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        float a = 0f;
        return CanAttack && IsReady ? Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * RayLength, 50, ref a) : false;
    }
    public override void AI()
    {
        base.AI();

        Player player = Main.player[Projectile.owner];

        IsReady = Scale.Length() > 1.4f;

        if (Projectile.timeLeft % 10 == 0)
        {
            CanAttack = Projectile.UseAmmo(AmmoID.Gel);
            if (!CanAttack)
            {
                Dust.NewDustPerfect(Projectile.Center + Projectile.rotation.ToRotationVector2() * 47, DustID.Smoke, Projectile.rotation.ToRotationVector2().RotatedByRandom(PiOver4 / 3) * Main.rand.NextFloat(0.1f, 3));
                Dust.NewDustPerfect(Projectile.Center + Projectile.rotation.ToRotationVector2() * 47, DustID.Smoke, Projectile.rotation.ToRotationVector2().RotatedByRandom(PiOver4 / 3) * Main.rand.NextFloat(0.1f, 3));
                Projectile.frame = 0;
                return;
            }
            SoundEngine.PlaySound(SoundID.Item34 with { Volume = Projectile.ai[0] / 250 }, Projectile.Center);
        }

        if (CanAttack && IsReady)
        {
            Projectile.ai[0] = Lerp(Projectile.ai[0], 225, 0.05f);
            Charge = Projectile.ai[0];
            RayLength = Helper.TRay.CastLength(Projectile.Center, Projectile.rotation.ToRotationVector2(), Charge);
            Dust.NewDustPerfect(Projectile.Center + Projectile.rotation.ToRotationVector2() * 47, DustID.Smoke, (Projectile.rotation).ToRotationVector2().RotatedByRandom(Pi / 4) * Main.rand.NextFloat(0.1f, 2), Scale: Main.rand.NextFloat(1.1f, 2));
            for (int i = 0; i < Charge / 12; i++)
                for (int u = 0; u < i / 4 + 1; u++)
                    Dust.NewDustPerfect(player.MountedCenter + Projectile.rotation.ToRotationVector2() * (47 + ((RayLength - 47) / 20) * i), DustID.Torch, (Main.rand.NextFloat(0, Pi * 2)).ToRotationVector2() * Main.rand.NextFloat(1.5f, 2) * Clamp(i * Charge / 1500, 0.5f, 5), Scale: Main.rand.NextFloat(1.9f, 2.5f)).noGravity = true;

            Projectile.frame = 1;
            float ScaleMultiplier = Charge / 225;
        }
        else
            Scale = Vector2.Lerp(Scale, Vector2.One, 0.2f);

        if (!player.channel)
            Projectile.Kill();
    }
    public override bool? CanDamage() => CanAttack;
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.OnFire, 210);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Player player = Main.player[Projectile.owner];
        Main.EntitySpriteDraw(TextureAssets.Projectile[Type].Value, Projectile.Center + player.GFX() + Main.rand.NextVector2Circular(Charge * 0.01f, Charge * 0.01f) - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), lightColor, Projectile.rotation, new Vector2(Projectile.width / 2 - 20, Projectile.height / 2 - 4 * player.direction), Scale, player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        return false;
    }
}

