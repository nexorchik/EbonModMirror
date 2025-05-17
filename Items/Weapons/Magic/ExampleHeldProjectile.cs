namespace EbonianMod.Items.Weapons;

public class ExampleHeldProjectile : ModItem
{
    public override string Texture => "EbonianMod/Items/Weapons/Magic/ProjectileGraphics";
    public override bool IsLoadingEnabled(Mod mod) => false;

    public override void SetDefaults()
    {
        Item.width = 48;
        Item.height = 66;
        Item.crit = 10;
        Item.damage = 10; //With zero damage doesn't work
        Item.DamageType = DamageClass.Ranged;
        Item.shoot = ProjectileType<ProjectileGraphics>();
        Item.value = Item.buyPrice(0, 10, 0, 0);
        Item.autoReuse = false;
        Item.noUseGraphic = true;
        Item.channel = true;
        Item.noMelee = true;
        Item.useStyle = 5;
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        velocity = Vector2.Zero;
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        return false;
    }
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemID.TheUndertaker).AddIngredient(ItemID.TissueSample, 20).AddIngredient(ItemID.Ichor, 15).AddTile(TileID.Anvils).Register();
    }
}
public class ProjectileGraphics : ModProjectile
{
    public override bool IsLoadingEnabled(Mod mod) => false;
    public override string Texture => "EbonianMod/Items/Weapons/Magic/ProjectileGraphics";

    public override bool? CanDamage() => false;

    public override void SetDefaults()
    {
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.Size = new(63, 25);
        Projectile.DamageType = DamageClass.Ranged;
        Projectile.penetrate = -1;
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.rotation = Helper.FromAToB(Main.player[Projectile.owner].Center, Main.MouseWorld).ToRotation();
    }

    public override void AI()
    {
        Player player = Main.player[Projectile.owner];

        player.itemTime = 2;
        player.itemAnimation = 2;
        Projectile.timeLeft = 10;

        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - PiOver2);
        Projectile.Center = player.MountedCenter;

        Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Helper.FromAToB(player.Center, Main.MouseWorld).ToRotation(), 0.25f);

        player.direction = player.Center.X < Main.MouseWorld.X ? 1 : -1;

        if (!player.active || player.HeldItem.type != ItemType<ExampleHeldProjectile>() || player.dead || player.CCed || player.noItems || player.channel == false)
        {
            Projectile.Kill();
            return;
        }
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Player player = Main.player[Projectile.owner];
        Texture2D tex = TextureAssets.Projectile[Type].Value;
        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2 + 4 * player.direction), Projectile.scale, player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        return false;
    }
}