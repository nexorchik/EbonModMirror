
using EbonianMod.Projectiles.Friendly.Underworld;

namespace EbonianMod.Items.Weapons.Ranged;

public class Corebreaker : ModItem
{
    public override void SetDefaults()
    {
        Item.knockBack = 10f;
        Item.crit = 25;
        Item.damage = 21;
        Item.DamageType = DamageClass.Ranged;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.rare = ItemRarityID.Yellow;
        Item.shootSpeed = 2f;
        Item.value = Item.buyPrice(0, 17, 0, 0);
        Item.shoot = ProjectileType<CorebreakerGraphics>();
        Item.autoReuse = false;
        Item.noUseGraphic = true;
        Item.channel = true;
        Item.noMelee = true;
    }
    public override bool AltFunctionUse(Player player)
    {
        if (player.altFunctionUse == 2)
        {
            Item.shoot = ProjectileType<CorebreakerGraphics>();
        }
        return base.AltFunctionUse(player);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        velocity = Vector2.Zero;
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        return false;
    }
}
public class CorebreakerGraphics : ModProjectile
{
    public override string Texture => "EbonianMod/Items/Weapons/Ranged/Corebreaker";

    public override bool? CanDamage() => false;

    public override void SetDefaults()
    {
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.width = 62;
        Projectile.height = 38;
        Projectile.ignoreWater = true;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = Helper.FromAToB(Main.player[Projectile.owner].Center, Main.MouseWorld).ToRotation();
        Projectile.ai[1] = 40;
        Projectile.ai[0] = 50;
    }

    float RotationOffset, RotationSpeed, HoldOffset;
    bool AltAttack = true;

    public override void AI()
    {
        Player player = Main.player[Projectile.owner];

        player.itemTime = 2;
        player.itemAnimation = 2;
        Projectile.timeLeft = 10;
        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - PiOver2);
        Projectile.Center = player.Center;

        HoldOffset = Lerp(HoldOffset, 24, 0.13f);
        if (!AltAttack)
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] == 120)
            {
                Vector2 SpawnPosition = Projectile.Center + new Vector2(Projectile.rotation.ToRotationVector2().X, Projectile.rotation.ToRotationVector2().Y) * 45;
                SoundEngine.PlaySound(SoundID.Item40.WithPitchOffset(Main.rand.NextFloat(-4f, -2f)), player.Center);
                SoundEngine.PlaySound(SoundID.Item38.WithPitchOffset(Main.rand.NextFloat(-0.8f, -0.4f)), player.Center);
                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDustPerfect(SpawnPosition, DustID.Torch, (Projectile.rotation + Main.rand.NextFloat(-PiOver4, PiOver4)).ToRotationVector2() * Main.rand.NextFloat(0.3f, 8), Scale: Main.rand.NextFloat(1f, 4f)).noGravity = true;
                }
                MPUtils.NewProjectile(Projectile.InheritSource(Projectile), SpawnPosition, Projectile.rotation.ToRotationVector2() * Vector2.Distance(Main.MouseWorld, Projectile.Center) / 35, ProjectileType<CorebreakerP>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
            if (Projectile.ai[0] > 120)
            {
                RotationOffset += 0.8f;
                RotationSpeed += 0.4f;
                RotationOffset = Lerp(RotationOffset, 0, RotationSpeed);
                HoldOffset = Lerp(HoldOffset, 0, 0.5f);
                if (RotationOffset <= 0.1f)
                {
                    RotationSpeed = 0;
                    RotationOffset = 0;
                    Projectile.ai[0] = 30;
                }
            }
            if (RotationOffset <= 0.1f && Main.mouseRight && Main.myPlayer == Projectile.owner)
            {
                HoldOffset = 0;
                Vector2 SpawnPosition = Projectile.Center + new Vector2(Projectile.rotation.ToRotationVector2().X, Projectile.rotation.ToRotationVector2().Y) * 45;
                SoundEngine.PlaySound(SoundID.Item62.WithPitchOffset(Main.rand.NextFloat(0.5f, 1.2f)), player.Center);
                SoundEngine.PlaySound(SoundID.Item98.WithPitchOffset(Main.rand.NextFloat(-4f, -2f)), player.Center);
                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDustPerfect(SpawnPosition, DustID.Smoke, (Projectile.rotation + Main.rand.NextFloat(PiOver2, PiOver4)).ToRotationVector2() * Main.rand.NextFloat(1, 8), Scale: Main.rand.NextFloat(0.5f, 3f)).noGravity = true;
                    Dust.NewDustPerfect(SpawnPosition, DustID.Smoke, (Projectile.rotation + Main.rand.NextFloat(-PiOver2, -PiOver4)).ToRotationVector2() * Main.rand.NextFloat(1, 8), Scale: Main.rand.NextFloat(0.5f, 3f)).noGravity = true;
                }
                MPUtils.NewProjectile(Projectile.InheritSource(Projectile), SpawnPosition, Projectile.rotation.ToRotationVector2() * 4, ProjectileType<CorebreakerHitscan>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                AltAttack = true;
            }
        }
        else
        {
            Projectile.ai[1]++;
            if (Projectile.ai[1] >= 30)
            {
                AltAttack = false;
                Projectile.ai[1] = 0;
            }
        }

        Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Helper.FromAToB(player.Center, Main.MouseWorld).ToRotation(), 0.25f) - RotationOffset * player.direction;

        if (!player.active || player.HeldItem.type != ItemType<Corebreaker>() || player.dead || player.CCed || player.noItems || !player.channel)
        {
            Projectile.Kill();
            return;
        }
        player.direction = player.Center.X < Main.MouseWorld.X ? 1 : -1;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Player player = Main.player[Projectile.owner];
        Texture2D tex = TextureAssets.Projectile[Type].Value;
        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(Projectile.width / 2 - HoldOffset, Projectile.height / 2 + 4 * player.direction), Projectile.scale, player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        return false;
    }
}
