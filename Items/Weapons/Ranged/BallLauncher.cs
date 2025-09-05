using EbonianMod.Items.Materials;
using EbonianMod.Projectiles.Bases;
using EbonianMod.Projectiles.Friendly.Corruption;
using EbonianMod.Projectiles.Friendly.Crimson;

namespace EbonianMod.Items.Weapons.Ranged;

public class BallLauncher : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 80;
        Item.useTime = 70;
        Item.value = Item.buyPrice(0, 40, 0, 0);
        Item.useAnimation = 30;
        Item.DamageType = DamageClass.Ranged;
        Item.useStyle = 5;
        Item.knockBack = 10;
        Item.value = 1000;
        Item.rare = ItemRarityID.Red;
        Item.shootSpeed = 14;
        Item.autoReuse = false;
        Item.noUseGraphic = true;
        Item.channel = true;
        Item.noMelee = true;
        Item.useAmmo = AmmoID.Rocket;
    }


    public override bool AltFunctionUse(Player player)
    {
        return true;
    }

    public override bool CanUseItem(Player player)
    {
        Item.shoot = player.altFunctionUse == 2 ? ProjectileType<BallLauncherCharge>() : ProjectileType<BallLauncherPrimary>();
        return base.CanUseItem(player);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        velocity.Normalize();
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        return false;
    }

    public override bool? CanAutoReuseItem(Player player) => false;

    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemID.GrenadeLauncher).AddIngredient(ItemType<CecitiorMaterial>(), 10).AddIngredient(ItemType<TerrortomaMaterial>(), 10).AddTile(TileID.MythrilAnvil).Register();
    }
    public override bool CanConsumeAmmo(Item ammo, Player player) => false;
}


public class BallLauncherPrimary : HeldProjectileGun
{

    Vector2 Scale = new Vector2(0, 0);

    public override void SetDefaults()
    {
        base.SetDefaults();
        ItemType = ItemType<BallLauncher>();
        RotationSpeed = 0.2f;
        PositionOffset = new Vector2(0, -10);
        Projectile.Size = new Vector2(56, 48);
    }
    public override bool? CanDamage() => false;
    public override void OnSpawn(IEntitySource source)
    {
        Player player = Main.player[Projectile.owner];
        Projectile.rotation = Helper.FromAToB(player.Center, Main.MouseWorld).ToRotation() + player.direction * PiOver2;
    }

    public override void AI()
    {
        base.AI();

        Player player = Main.player[Projectile.owner];

        player.heldProj = Projectile.whoAmI;

        Projectile.ai[0]++;

        if (Projectile.ai[0] > 15)
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                if (Projectile.frame == 0)
                {
                    Scale = new Vector2(0.65f, 1.6f);
                    if (player.whoAmI == Main.myPlayer)
                    {
                        AnimationRotation = -0.2f * player.direction;
                        Vector2 SpawnPosition = Projectile.Center + new Vector2(14, -15 * player.direction).RotatedBy(Projectile.rotation);
                        if (player.whoAmI == Main.myPlayer)
                            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), SpawnPosition, Projectile.rotation.ToRotationVector2() * 16, ProjectileType<CrimsonBall>(), (int)(Projectile.damage * 0.8f), Projectile.knockBack, Projectile.owner);
                        for (int i = 0; i < 14; i++)
                            Dust.NewDustPerfect(SpawnPosition, DustID.Blood, (Projectile.rotation + Main.rand.NextFloat(-PiOver4, PiOver4)).ToRotationVector2() * Main.rand.NextFloat(2, 8), Scale: 1.5f).noGravity = true;
                    }
                    SoundEngine.PlaySound(SoundID.NPCDeath13.WithPitchOffset(Main.rand.NextFloat(0, 0.3f)), player.Center);
                    Projectile.UseAmmo(AmmoID.Rocket);
                }
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 3)
                    Projectile.frame = 0;
            }
        }
        Scale = Vector2.Lerp(Scale, new Vector2(1, 1), 0.14f);

        if (!Main.player[Projectile.owner].channel)
            Projectile.Kill();

    }
    public override bool PreDraw(ref Color lightColor)
    {
        Main.EntitySpriteDraw(Helper.GetTexture("Items/Weapons/Ranged/BallLauncherPrimary").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), lightColor, Projectile.rotation, new Vector2(Projectile.Size.X / 2 - 15, Projectile.Size.Y / 2), Scale, Main.player[Projectile.owner].direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        return false;
    }
}

public class BallLauncherCharge : HeldProjectileGun
{

    Vector2 Scale = new Vector2(0, 0);
    public override bool? CanDamage() => false;

    public override void SetDefaults()
    {
        base.SetDefaults();
        ItemType = ItemType<BallLauncher>();
        RotationSpeed = 0.13f;
        Projectile.Size = new Vector2(58, 50);
    }

    public override void OnSpawn(IEntitySource source)
    {
        Player player = Main.player[Projectile.owner];
        Projectile.rotation = Helper.FromAToB(player.Center, Main.MouseWorld).ToRotation() + player.direction * PiOver2;
        Projectile.ai[2] = 1;
    }
    public override void AI()
    {
        base.AI();

        Player player = Main.player[Projectile.owner];

        player.heldProj = Projectile.whoAmI;

        if (Projectile.frame < 6)
        {
            if (Projectile.ai[0] < 120)
            {
                Projectile.ai[0]++;
                Projectile.frameCounter++;
                if (Projectile.frameCounter > 20)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                }
            }
            if (Projectile.ai[0] == 119)
            {
                SoundEngine.PlaySound(SoundID.MaxMana.WithPitchOffset(-0.3f), Projectile.Center);
                Projectile.ai[1] = 1;
            }
            float Charge = Projectile.ai[0];
            float ScaleNoise = Charge / 82;
            Scale = Vector2.Lerp(Scale, new Vector2(Main.rand.NextFloat(2f - ScaleNoise, ScaleNoise), Main.rand.NextFloat(2f - ScaleNoise, ScaleNoise)), ScaleNoise / 10);
            if (!Main.mouseRight && player.whoAmI == Main.myPlayer)
            {
                Projectile.frame = 6;
                AnimationRotation = -Charge / 240f * player.direction;
                Vector2 SpawnPosition = Projectile.Center + new Vector2(14, -15 * player.direction).RotatedBy(Projectile.rotation);
                for (int u = 0; u < 14; u++)
                    Dust.NewDustPerfect(SpawnPosition, DustID.Blood, (Projectile.rotation + Main.rand.NextFloat(-PiOver4, PiOver4)).ToRotationVector2() * Main.rand.NextFloat(2, 8), Scale: 1.5f).noGravity = true;
                for (int i = 0; i < Charge / 15; i++)
                {
                    Scale = new Vector2(0.55f, 1.7f);
                    SoundEngine.PlaySound(SoundID.NPCDeath13, player.Center);
                    if (player.whoAmI == Main.myPlayer)
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), SpawnPosition, Main.rand.NextFloat(Projectile.rotation - (PiOver2 * 20 / Charge), Projectile.rotation + (PiOver2 * 20 / Charge)).ToRotationVector2() * Main.rand.NextFloat(Charge / 5, Charge / 8), ProjectileType<CorruptionBalls>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
                    Projectile.UseAmmo(AmmoID.Rocket);
                }
            }
        }
        else
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 8)
                    Projectile.Kill();
            }
        }
        Scale = Vector2.Lerp(Scale, new Vector2(1, 1), 0.17f);
        if (Projectile.ai[1] > 0)
        {
            Projectile.ai[1] -= 0.04f;
            Projectile.ai[2] += 0.01f;
        }
    }
    public override bool PreDraw(ref Color lightColor)
    {
        int Direction = Main.player[Projectile.owner].direction;
        Main.EntitySpriteDraw(Helper.GetTexture("Items/Weapons/Ranged/BallLauncherCharge").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height), lightColor, Projectile.rotation, new Vector2(Projectile.Size.X / 2 - 17, Projectile.Size.Y / 2 + 9 * Direction), Scale, Main.player[Projectile.owner].direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        Main.EntitySpriteDraw(Helper.GetTexture("Items/Weapons/Ranged/BallLauncherFlash").Value, Projectile.Center + new Vector2(17, -9 * Direction).RotatedBy(Projectile.rotation) - Main.screenPosition, new Rectangle(0, 0, Projectile.width, Projectile.height), Color.White * Projectile.ai[1], Projectile.rotation, new Vector2(Projectile.Size.X / 2, Projectile.Size.Y / 2), new Vector2(Projectile.ai[2], Projectile.ai[2] * 1.1f) * Scale, Main.player[Projectile.owner].direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        return false;
    }
}