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
        Item.useAnimation = 17;
        Item.crit = 15;
        Item.useTime = 17;
        Item.DamageType = DamageClass.Melee;
        Item.damage = 19;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.value = Item.buyPrice(0, 8, 0, 0);
        Item.shoot = ProjectileType<ExolsawP>();
        Item.rare = ItemRarityID.Orange;
        Item.shootSpeed = 10;
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
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D tex = TextureAssets.Projectile[Type].Value;
        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, 42 * Projectile.frame, 42, 42), lightColor, Projectile.rotation, new Vector2(21), Projectile.scale, SpriteEffects.None);
        return false;
    }
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(21, 21), DustID.Torch, Vector2.Zero, Scale: Main.rand.NextFloat(0.9f, 2f)).noGravity = true;

        Projectile.rotation += MathHelper.ToRadians(25);
        Projectile.timeLeft = 2;
        Projectile.ai[0]++;
        if (Projectile.ai[0] < 30)
            Projectile.velocity *= 1.02f;
        if (Projectile.ai[0] > 30 && Projectile.ai[0] < 50)
            Projectile.velocity *= 0.86f;
        if (Projectile.ai[0] > 50)
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, Helper.FromAToB(Projectile.Center, player.Center) * 30f, 0.1f);

        if (Projectile.ai[0] > 50 && Projectile.Center.Distance(player.Center) < 50)
            Projectile.Kill();
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
            Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(41, 41), DustID.Torch, Projectile.velocity * Main.rand.NextFloat(), Scale: Main.rand.NextFloat(0.9f, 2f)).noGravity = true;
        Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
    }
}
