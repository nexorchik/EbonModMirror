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
    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        if (Projectile.velocity.Length() > 0f)
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.Torch, (Projectile.rotation - PiOver2).ToRotationVector2() * Main.rand.NextFloat(0.7f, 2f), Scale: (float)Projectile.timeLeft / 70).noGravity = true;
            Dust.NewDustPerfect(Projectile.Center, DustID.Torch, (Projectile.rotation + PiOver2).ToRotationVector2() * Main.rand.NextFloat(0.7f, 2f), Scale: (float)Projectile.timeLeft / 70).noGravity = true;
        }
        foreach (Projectile projectile in Main.ActiveProjectiles)
        {
            if (projectile.type == ProjectileType<CoreProjectile>() && Projectile.timeLeft < 345)
            {
                if (Projectile.Distance(projectile.Center) < 45)
                {
                    if (Main.myPlayer == projectile.owner)
                    {
                        projectile.Kill();
                        Projectile currentProjectile = Projectile.NewProjectileDirect(NPC.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ProjectileType<FlameExplosionWSprite>(), Projectile.damage * 5, 0);
                        currentProjectile.scale *= 1.6f;
                        currentProjectile.CritChance = 100;
                        currentProjectile.friendly = true;
                        currentProjectile.hostile = false;
                        currentProjectile.SyncProjectile();
                    }
                    if (Projectile.velocity.Length() > 0)
                    {
                        for (int i = 0; i < 35; i++)
                        {
                            Dust.NewDustPerfect(Projectile.Center, DustID.Torch, (Projectile.velocity + Main.rand.NextVector2Circular(5, 5)) * Main.rand.NextFloat(0.2f, 1.2f), Scale: Main.rand.NextFloat(1.8f, 2.7f));
                        }
                        Projectile.velocity *= 0;
                        Projectile.netUpdate = true;
                    }
                }
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
            Helper.DrawTexturedPrimitives(vertices.ToArray(), PrimitiveType.TriangleStrip, Images.Extras.Textures.LaserFaintTransparent.Value, false);
        }
        Main.spriteBatch.ApplySaved(sbParams);
        return false;
    }


    public override bool? CanDamage() => Projectile.ai[2] == 0;
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        for (int i = 0; i < 35; i++)
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.Torch, (Projectile.velocity + Main.rand.NextVector2Circular(5, 5)) * Main.rand.NextFloat(0.2f, 1.2f), Scale: Main.rand.NextFloat(1.8f, 2.7f));
        }
        return true;
    }
}