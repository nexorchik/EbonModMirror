﻿using System;
using System.Collections.Generic;

namespace EbonianMod.Content.Projectiles.Friendly.Corruption;

public class CorruptionHitscan : ModProjectile
{
    public override string Texture => Helper.Empty;
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 200;
        ProjectileID.Sets.TrailingMode[Type] = 2;
    }
    bool EmitParticles = true;
    public override void SetDefaults()
    {
        Projectile.height = 20;
        Projectile.width = 20;
        Projectile.tileCollide = true;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;
        Projectile.extraUpdates = 40;
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
            Color col = Color.DarkSlateBlue;
            vertices.Add(Helper.AsVertex(basePos + new Vector2(20 + 100 * mult * mult2, 0).RotatedBy(PiOver2 + Projectile.velocity.ToRotation()), col, new Vector2(0, 0)));
            vertices.Add(Helper.AsVertex(basePos + new Vector2(20 + 100 * mult * mult2, 0).RotatedBy(-PiOver2 + Projectile.velocity.ToRotation()), col, new Vector2(1, 1)));
        }

        SpritebatchParameters sbParams = Main.spriteBatch.Snapshot();
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        if (vertices.Count > 2)
        {
            Helper.DrawTexturedPrimitives(vertices.ToArray(), PrimitiveType.TriangleStrip, Assets.Extras.laser3_transparent.Value, false);
        }
        Main.spriteBatch.ApplySaved(sbParams);
        return false;
    }
    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        if (Projectile.ai[2] == 0)
            Projectile.timeLeft = 200;
        if(EmitParticles)
            Dust.NewDustPerfect(Projectile.Center, DustID.Demonite, Main.rand.NextVector2Circular(1, 1), Scale: Main.rand.NextFloat(0.5f, 0.8f));
    }
    public override bool? CanDamage() => Projectile.ai[2] == 0;
    void Freeze()
    {
        for (int i = 0; i < 15; i++)
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.Demonite, (Projectile.rotation + Main.rand.NextFloat(-Pi/6, Pi/6)).ToRotationVector2() * Main.rand.NextFloat(2, 4), Scale: 1f);
        }
        EmitParticles = false;
        Projectile.tileCollide = false;
        Projectile.Center += Projectile.velocity;
        Projectile.velocity = Vector2.Zero;
        Projectile.ai[2] = 1;
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => Freeze();
    public override void OnHitPlayer(Player target, Player.HurtInfo info) => Freeze();
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Freeze();
        return false;
    }
}