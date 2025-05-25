using EbonianMod.Projectiles.Friendly.Crimson;

namespace EbonianMod.Items.Weapons.Ranged;

public class IchorFlintlock : ModItem
{
    public override void SetDefaults()
    {
        Item.knockBack = 10f;
        Item.width = 42;
        Item.height = 24;
        Item.crit = 6;
        Item.damage = 63;
        Item.DamageType = DamageClass.Ranged;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.rare = ItemRarityID.LightRed;
        Item.useAnimation = 70;
        Item.shoot = ProjectileType<IchorFlintlockP>();
        Item.value = Item.buyPrice(0, 10, 0, 0);
        Item.autoReuse = false;
        Item.noUseGraphic = true;
        Item.channel = true;
        Item.noMelee = true;
        Item.useAmmo = AmmoID.Bullet;
    }
    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        type = ProjectileType<IchorFlintlockP>();
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
}
public class IchorFlintlockP : ModProjectile
{
    public override string Texture => "EbonianMod/Items/Weapons/Ranged/IchorFlintlock";

    public override bool? CanDamage() => false;


    public override void SetDefaults()
    {
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.Size = new(42, 24);
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = Helper.FromAToB(Main.player[Projectile.owner].Center, Main.MouseWorld).ToRotation();
    }

    float RotationOffset, RotationSpeed = 0.2f, HoldOffset;

    public override void AI()
    {
        Player player = Main.player[Projectile.owner];

        player.itemTime = 2;
        player.itemAnimation = 2;
        Projectile.timeLeft = 10;
        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - PiOver2);
        Projectile.Center = player.MountedCenter;

        Projectile.ai[0]++;
        if (Projectile.ai[0] == 125)
        {
            RotationOffset = 0.5f;
            for (int j = 0; j < 58; j++)
            {
                if (player.inventory[j].ammo == AmmoID.Bullet && player.inventory[j].stack > 0)
                {
                    if (player.inventory[j].maxStack > 1)
                        player.inventory[j].stack--;
                    break;
                }
            }
            SoundEngine.PlaySound(SoundID.Item38.WithPitchOffset(Main.rand.NextFloat(1f, 3f)), player.Center);
            Vector2 SpawnPosition = Projectile.Center + new Vector2(Projectile.rotation.ToRotationVector2().X, Projectile.rotation.ToRotationVector2().Y) * 42 + (Projectile.rotation + 90 * -player.direction).ToRotationVector2() * 12;
            Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), SpawnPosition, Projectile.rotation.ToRotationVector2() * 20, ProjectileType<ToothProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            for (int i = 0; i < 7; i++)
            {
                Dust.NewDustPerfect(SpawnPosition, DustID.Blood, (Projectile.rotation + Main.rand.NextFloat(-PiOver4, PiOver4)).ToRotationVector2() * Main.rand.NextFloat(2, 8), Scale: 1.5f).noGravity = true;
            }
        }
        if (Projectile.ai[0] > 125)
        {
            RotationOffset -= RotationSpeed;
            RotationSpeed += 0.07f;
            HoldOffset = Lerp(HoldOffset, 12, 0.4f);
            if (RotationOffset <= 0)
            {
                RotationSpeed = 0.2f;
                RotationOffset = 0;
                Projectile.ai[0] = 0;
            }
        }
        else
            HoldOffset = Lerp(HoldOffset, 27, 0.2f);

        Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Helper.FromAToB(player.Center, Main.MouseWorld).ToRotation(), 0.25f) - RotationOffset * player.direction;



        if (!player.active || player.HeldItem.type != ItemType<IchorFlintlock>() || player.dead || player.CCed || player.noItems || player.channel == false)
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