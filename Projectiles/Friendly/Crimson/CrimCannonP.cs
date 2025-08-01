using System.IO;

namespace EbonianMod.Projectiles.Friendly.Crimson;

public class CrimCannonP : ModProjectile
{
    Vector2 PositionOffset;
    Vector2 Scale = new Vector2(1, 1);
    int TargetIndex = -1;
    public override void SetDefaults()
    {
        Projectile.width = 46;
        Projectile.height = 34;
        Projectile.tileCollide = true;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 290;
        Projectile.frame = 2;
        Projectile.localNPCHitCooldown = 23;
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.ai[1] = 0;
        Projectile.spriteDirection = Main.player[Projectile.owner].direction;
        Projectile.netUpdate = true; // TEST
    }

    public override void OnKill(int timeLeft)
    {
        if (Main.dedServ)
            return;
        SoundEngine.PlaySound(EbonianSounds.chomp1.WithPitchOffset(Main.rand.NextFloat(-0.4f, 0.2f)), Projectile.Center);
        for (int i = 0; i < 4; i++)
        {
            Gore.NewGore(null, Projectile.Center, Main.rand.NextVector2Circular(5, 5), Find<ModGore>("EbonianMod/GoryJaw" + i).Type, 1);
        }
        for (int i = 0; i < 20; i++)
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.Blood, Main.rand.NextFloat(-Pi, Pi).ToRotationVector2() * Main.rand.NextFloat(2, 5), Scale: 1.5f);
        }
    }
    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(TargetIndex);
    }
    public override void ReceiveExtraAI(BinaryReader reader)
    {
        TargetIndex = reader.Read();
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Projectile.ai[2] == 0)
        {
            Projectile.tileCollide = false;
            PositionOffset = Projectile.Center - target.Center;
            TargetIndex = target.whoAmI;
            Projectile.ai[2] = 1;
            Projectile.netUpdate = true; // TEST
        }
    }
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        if (Projectile.ai[2] == 1)
        {
            NPC Target = Main.npc[TargetIndex];
            if (!Target.active)
                return;
            if (Target.life <= 0)
            {
                Projectile.ai[2] = 0;
                Projectile.velocity *= 0;
                Projectile.tileCollide = true;
                Projectile.netUpdate = true;
            }
            Projectile.Center = Target.Center + PositionOffset;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame > 2)
                {
                    Projectile.frame = 0;
                }
            }
        }
        else
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.ai[1] > -0.5f)
                Projectile.ai[1] -= 0.01f;
            Projectile.velocity = new Vector2(Projectile.velocity.X, Projectile.velocity.Y - Projectile.ai[1]);
        }
        Scale = Vector2.Lerp(Scale, new Vector2(Main.rand.NextFloat(2f - Projectile.ai[0] / 120, Projectile.ai[0] / 120), Main.rand.NextFloat(2f - Projectile.ai[0] / 120, Projectile.ai[0] / 120)), Projectile.ai[0] / 800);
        Projectile.ai[0]++;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Rectangle frameRect = new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height);
        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frameRect, lightColor, Projectile.rotation, new Vector2(Projectile.Size.X / 2, Projectile.Size.Y / 2), Scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        return false;
    }
}
