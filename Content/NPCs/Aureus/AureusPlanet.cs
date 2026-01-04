using System;
using Terraria.Graphics.CameraModifiers;

namespace EbonianMod.Content.NPCs.Aureus;

public class AureusPlanet : ModProjectile
{
    public override string Texture => Helper.AssetPath + "NPCs/Aureus/"+Name;
    public override void SetDefaults()
    {
        Projectile.Size = new Vector2(200);
        Projectile.aiStyle = -1;
        Projectile.scale = 0;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 200;
    }
    public override Color? GetAlpha(Color lightColor) => Color.White;
    public override void AI()
    {
        if (++Projectile.ai[0] < 60)
        {
            Projectile.scale = Lerp(0, 1, InOutCirc.Invoke(Projectile.ai[0] / 60f));
            Projectile.rotation = Lerp(0, PiOver4, InOutElastic.Invoke(Projectile.ai[0] / 60f));
        }
        if (Projectile.velocity.Length() < 30 && Projectile.ai[0] > 60)
            Projectile.velocity *= 1.05f;
        if (Projectile.ai[0] > 60)
            foreach (Projectile p in Main.ActiveProjectiles)
            {
                if (p.type == Type && p.whoAmI != Projectile.whoAmI && p.ai[2] == Projectile.ai[2])
                    if (p.Distance(Projectile.Center) < 100)
                    {
                        PunchCameraModifier modifier = new PunchCameraModifier(Projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 5f, 100, 1000f, FullName);
                        Main.instance.CameraModifiers.Add(modifier);
                        for (int i = 0; i < 10; i++)
                        {
                            if (i % 2 == 0)
                            {
                                Projectile.NewProjectile(null, Projectile.Center + Main.rand.NextVector2Circular(20, 20), Vector2.Zero, ProjectileType<AExplosion>(), 0, 0);
                                Projectile.NewProjectile(null, p.Center + Main.rand.NextVector2Circular(20, 20), Vector2.Zero, ProjectileType<AExplosion>(), 0, 0);
                            }
                            Vector2 vel = Main.rand.NextVector2Circular(5, 5);
                            Projectile.NewProjectile(null, Projectile.Center, vel.Length() <= 0 ? Main.rand.NextVector2Unit() : vel, ProjectileType<AureusMissile>(), 30, 0);
                            Projectile.NewProjectile(null, p.Center, vel.Length() <= 0 ? Main.rand.NextVector2Unit() : vel, ProjectileType<AureusMissile>(), 30, 0);
                        }
                        SoundEngine.PlaySound(Aureus.splodeSound, Projectile.Center);
                        p.Kill();
                        Projectile.Kill();
                    }
            }
    }
}
