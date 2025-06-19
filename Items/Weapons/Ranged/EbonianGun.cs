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
        Item.damage = 33;
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
public class EbonianGunP : ModProjectile
{
    public override string Texture => "EbonianMod/Items/Weapons/Ranged/EbonianGun";

    public override bool? CanDamage() => false;


    public override void SetDefaults()
    {
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.Size = new(72, 24);
        Projectile.ignoreWater = true;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.penetrate = -1;
        HoldOffset = 7;
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = Helper.FromAToB(Main.player[Projectile.owner].Center, Main.MouseWorld).ToRotation();
    }

    float RotationOffset, RotationSpeed = 0.12f, HoldOffset;
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];

        player.itemTime = 2;
        player.itemAnimation = 2;
        player.heldProj = Projectile.whoAmI;
        Projectile.timeLeft = 10;
        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - PiOver2);
        Projectile.Center = player.MountedCenter;

        Projectile.ai[0]++;
        if (Projectile.ai[0] == 30)
        {
            RotationOffset = 0.22f;
            for (int j = 0; j < 58; j++)
            {
                if (player.inventory[j].ammo == AmmoID.Bullet && player.inventory[j].stack > 0)
                {
                    if (player.inventory[j].maxStack > 1)
                        player.inventory[j].stack--;
                    break;
                }
            }
            SoundEngine.PlaySound(SoundID.Item11.WithPitchOffset(Main.rand.NextFloat(-1f, -0.5f)), player.Center);
            SoundEngine.PlaySound(SoundID.Item17.WithPitchOffset(Main.rand.NextFloat(-0.5f, -0.2f)), player.Center);
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Projectile.Center + Projectile.rotation.ToRotationVector2() * 45 + (Projectile.rotation + 90 * -player.direction).ToRotationVector2() * 12, Projectile.rotation.ToRotationVector2() * 10, ProjectileType<CorruptionHitscan>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
        if (Projectile.ai[0] > 30)
        {
            RotationOffset -= RotationSpeed;
            RotationSpeed += 0.12f;
            HoldOffset = Lerp(HoldOffset, 16, 0.4f);
            if (RotationOffset <= 0)
            {
                RotationSpeed = 0.12f;
                RotationOffset = 0;
                Projectile.ai[0] = 0;
            }
        }
        else
            HoldOffset = Lerp(HoldOffset, 26, 0.2f);

        Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Helper.FromAToB(player.Center, Main.MouseWorld).ToRotation(), 0.25f) - RotationOffset * player.direction;



        if (!player.active || player.HeldItem.type != ItemType<EbonianGun>() || player.dead || player.CCed || player.noItems || player.channel == false)
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
        Main.EntitySpriteDraw(tex, Projectile.Center + player.GFX() - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(Projectile.width / 2 - HoldOffset, Projectile.height / 2 + 4 * player.direction), Projectile.scale, player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        return false;
    }
}
