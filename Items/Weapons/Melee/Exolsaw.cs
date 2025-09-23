namespace EbonianMod.Items.Weapons.Melee;

public class Exolsaw : ModItem
{
    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noUseGraphic = true;
        Item.consumable = false;
        Item.Size = new(20);
        Item.useAnimation = 30;
        Item.useTime = 30;
        Item.crit = 12;
        Item.DamageType = DamageClass.Melee;
        Item.damage = 13;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.value = Item.buyPrice(0, 8, 0, 0);
        Item.shoot = ProjectileType<ExolsawProjectile>();
        Item.rare = ItemRarityID.Orange;
    }
}
public class ExolsawProjectile : ModProjectile
{
    public override string Texture => "EbonianMod/Items/Weapons/Melee/ExolsawProjectile";
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 2;
    }
    public override void SetDefaults()
    {
        Projectile.width = 10;
        Projectile.height = 10;
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.penetrate = -1;
        Projectile.tileCollide = true;
        Projectile.localNPCHitCooldown = 5;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.timeLeft = 500;
    }
    public override void OnSpawn(IEntitySource source)
    {
        Projectile.Center = Main.MouseWorld;
    }
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(21, 21), DustID.Torch, Vector2.Zero, Scale: Main.rand.NextFloat(0.9f, 2f)).noGravity = true;
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hitinfo, int damage)
    {
        Vector2 Position = Projectile.Center + new Vector2(22, 0).RotatedBy(Projectile.velocity.ToRotation());
        for (int i = 0; i < 4; i++)
        {
            Dust.NewDustPerfect(Position, DustID.Torch, Main.rand.NextFloat(-Pi, Pi).ToRotationVector2() * Main.rand.NextFloat(2, 5), Scale: Main.rand.NextFloat(0.5f, 1f));
        }
    }
    public override void OnKill(int timeLeft)
    {
        Player player = Main.player[Projectile.owner];
        for (int i = 0; i < 40; i++)
            Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(41, 41), DustID.Torch, Main.rand.NextFloat(0, Pi * 2).ToRotationVector2(), Scale: Main.rand.NextFloat(0.9f, 2f)).noGravity = true;
        Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Main.EntitySpriteDraw(Helper.GetTexture(Texture).Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 42 * Projectile.frame, 42, 42), lightColor, Projectile.rotation, new Vector2(21), Projectile.scale, SpriteEffects.None);
        Main.EntitySpriteDraw(Helper.GetTexture("EbonianMod/Items/Weapons/Melee/ExolsawGlow").Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 42 * Projectile.frame, 42, 42), Color.White, Projectile.rotation, new Vector2(21), Projectile.scale, SpriteEffects.None);
        return false;
    }
}
