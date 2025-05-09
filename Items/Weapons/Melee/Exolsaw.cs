using EbonianMod.Dusts;
using System.Collections.Generic;
using Terraria;
using static System.Net.Mime.MediaTypeNames;

namespace EbonianMod.Items.Weapons.Melee;

public class Exolsaw : ModItem
{
    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noUseGraphic = true;
        Item.consumable = false;
        Item.Size = new(20);
        Item.useAnimation = 100;
        Item.crit = 15;
        Item.useTime = 100;
        Item.DamageType = DamageClass.Melee;
        Item.damage = 19;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.value = Item.buyPrice(0, 8, 0, 0);
        Item.shoot = ProjectileType<ExolsawP>();
        Item.rare = ItemRarityID.Orange;
        Item.shootSpeed = 30;
    }
}
public class ExolsawP : ModProjectile
{
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 2;
    }
    public override string Texture => "EbonianMod/Items/Weapons/Melee/ExolsawP";
    public override void SetDefaults()
    {
        Projectile.width = 42;
        Projectile.height = 42;
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.localNPCHitCooldown = 5;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.timeLeft = 500;

    }
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(21, 21), DustID.Torch, Vector2.Zero, Scale: Main.rand.NextFloat(0.9f, 2f)).noGravity = true;
        if (Projectile.ai[0] <= 0)
        {
            Projectile.timeLeft += 2;
            Projectile.rotation += MathHelper.ToRadians(15);
            Projectile.frame = 0;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.velocity.SafeNormalize(Vector2.UnitY) * 14f, 0.13f);
        }
        else
        {
            Projectile.ai[0] -= 0.01f;
            Projectile.rotation += MathHelper.ToRadians(40);
            Projectile.frame = 1;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.velocity.SafeNormalize(Vector2.UnitY), 0.2f);
        }
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hitinfo, int damage)
    {
        Vector2 Position = Projectile.Center + new Vector2(22, 0).RotatedBy(Projectile.velocity.ToRotation());
        for (int i = 0; i < 4; i++)
        {
            Dust.NewDustPerfect(Position, DustID.Torch, Main.rand.NextFloat(-Pi, Pi).ToRotationVector2() * Main.rand.NextFloat(2, 5), Scale: Main.rand.NextFloat(0.5f, 1f));
        }
        Projectile.ai[0] = 0.08f;
    }
    public override void OnKill(int timeLeft)
    {
        Player player = Main.player[Projectile.owner];
        Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
    }
}
