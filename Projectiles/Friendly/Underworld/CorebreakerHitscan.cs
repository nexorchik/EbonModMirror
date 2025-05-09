using EbonianMod.Projectiles.VFXProjectiles;
using System.Collections.Generic;
using System;

namespace EbonianMod.Projectiles.Friendly.Underworld;

public class CorebreakerHitscan : ModProjectile
{
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 500;
        ProjectileID.Sets.TrailingMode[Type] = 2;
    }
    public override string Texture => Helper.Empty;
    public override void SetDefaults()
    {
        Projectile.height = 20;
        Projectile.width = 20;
        Projectile.tileCollide = true;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;
        Projectile.extraUpdates = 60;
        Projectile.timeLeft = 350;
    }
    bool EmitParticles=true;
    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        if(EmitParticles == true)
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.Torch, (Projectile.rotation - PiOver2).ToRotationVector2() * Main.rand.NextFloat(0.7f, 2f), Scale: (float)Projectile.timeLeft / 70).noGravity = true;
            Dust.NewDustPerfect(Projectile.Center, DustID.Torch, (Projectile.rotation + PiOver2).ToRotationVector2() * Main.rand.NextFloat(0.7f, 2f), Scale: (float)Projectile.timeLeft / 70).noGravity = true;
        }
        foreach (Projectile projectile in Main.ActiveProjectiles)
        {
            if (Projectile.Distance(projectile.Center) < 25 && projectile.type == ProjectileType<CorebreakerP>())
            {
                projectile.Kill();
                Projectile CurrentProjectile = Projectile.NewProjectileDirect(NPC.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<FlameExplosionWSprite>(), Projectile.damage * 2, 0);
                CurrentProjectile.scale *= 1.7f;
                CurrentProjectile.damage = (int)(CurrentProjectile.damage * 2f);
                CurrentProjectile.CritChance = 100;
                CurrentProjectile.friendly = true;
                CurrentProjectile.hostile = false;
                CurrentProjectile.SyncProjectile();
                Freeze();
            }
        }
    }
    public override bool PreDraw(ref Color lightColor)
    {
        List<VertexPositionColorTexture> vertices = new();
        for (int i = 0; i < Projectile.oldPos.Length; i++)
        {
            if (Projectile.oldPos[i] == Vector2.Zero)
                continue;
            Vector2 basePos = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;
            float mult = MathF.Pow(1f - 1f / Projectile.oldPos.Length * i, 2);
            float mult2 = SmoothStep(1, 0, MathF.Pow(mult, 3));
            Color col = Color.White;
            vertices.Add(Helper.AsVertex(basePos + new Vector2(20 + 1000 * mult * mult2, 0).RotatedBy(PiOver2 + Projectile.velocity.ToRotation()), col, new Vector2(0, 0)));
            vertices.Add(Helper.AsVertex(basePos + new Vector2(20 + 1000 * mult * mult2, 0).RotatedBy(-PiOver2 + Projectile.velocity.ToRotation()), col, new Vector2(1, 1)));
        }

        SpritebatchParameters sbParams = Main.spriteBatch.Snapshot();
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        if (vertices.Count > 2)
        {
            Helper.DrawTexturedPrimitives(vertices.ToArray(), PrimitiveType.TriangleStrip, ExtraTextures.laser3_transparent.Value, false);
        }
        Main.spriteBatch.ApplySaved(sbParams);
        return false;
    }

    void Freeze()
    {
        for (int i = 0; i < 35; i++)
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.Torch, (Projectile.velocity + Main.rand.NextVector2Circular(5, 5)) * Main.rand.NextFloat(0.2f, 1.2f), Scale: Main.rand.NextFloat(1.8f, 2.7f));
        }
        Projectile.tileCollide = false;
        Projectile.Center += Projectile.velocity;
        Projectile.velocity = Vector2.Zero;
        Projectile.ai[2] = 1;
        EmitParticles = false;
    }

    public override bool? CanDamage() => Projectile.ai[2] == 0;
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Freeze();
        return false;
    }
}