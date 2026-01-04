using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static Terraria.NPC.NPCNameFakeLanguageCategoryPassthrough;

namespace EbonianMod.Projectiles.Friendly.Generic;
public class StarBitBlue : ModProjectile
{
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 6;
        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
    }

    public override void SetDefaults()
    {
        Projectile.width = 18;
        Projectile.height = 18;
        Projectile.friendly = true;
        Projectile.penetrate = 1;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = false;
        Projectile.timeLeft = 400;
        Projectile.netUpdate = true;
        Projectile.penetrate = -1;
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.frame = Main.rand.Next(6);
    }
    public override Color? GetAlpha(Color lightColor)
    {
        return Color.White;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Color baseCol, secondaryCol;
        switch (Projectile.frame)
        {
            case 0:
                baseCol = Color.Cyan;
                secondaryCol = Color.LightBlue;
                break;
            case 1:
                baseCol = Color.OrangeRed;
                secondaryCol = Color.IndianRed;
                break;
            case 2:
                baseCol = Color.Violet;
                secondaryCol = Color.Purple;
                break;
            case 3:
                baseCol = Color.LightGreen;
                secondaryCol = Color.Green;
                break;
            case 4:
                baseCol = Color.LightGoldenrodYellow;
                secondaryCol = Color.LightYellow;
                break;
            case 5:
                baseCol = Color.White;
                secondaryCol = Color.Gray;
                break;
            default:
                baseCol = Color.MediumBlue;
                secondaryCol = Color.LightBlue;
                break;
        }
        Vector2 ori = Projectile.Size / 2;
        Texture2D trail = Images.Extras.Textures.LintyTrail.Value;
        List<VertexPositionColorTexture> vertices = new();
        for (int j = 0; j < Projectile.oldPos.Length; j++)
        {
            if (Projectile.oldPos[j] != Vector2.Zero)
            {
                float mult = (float)j / Projectile.oldPos.Length;
                vertices.Add(Helper.AsVertex(
                    Projectile.oldPos[j] + ori - Main.screenPosition + (Projectile.oldRot[j] + MathHelper.PiOver2).ToRotationVector2() * 10f,
                    baseCol * SmoothStep(1, 0, mult),
                    new Vector2((float)j / Projectile.oldPos.Length - Main.GlobalTimeWrappedHourly, 0)));
                vertices.Add(Helper.AsVertex(
                    Projectile.oldPos[j] + ori - Main.screenPosition + (Projectile.oldRot[j] - MathHelper.PiOver2).ToRotationVector2() * 10f,
                    secondaryCol * SmoothStep(1, 0, mult),
                    new Vector2((float)j / Projectile.oldPos.Length - Main.GlobalTimeWrappedHourly, 1)));
            }
        }
        Main.spriteBatch.Snapshot(out var sb);
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        if (vertices.Count > 2)
            Helper.DrawTexturedPrimitives(vertices.ToArray(), PrimitiveType.TriangleStrip, trail, false);
        Main.spriteBatch.ApplySaved(sb);
        return true;
    }
    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        Projectile.extraUpdates = 2;
    }

    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.MaxMana, Projectile.Center);
        for (int i = 0; i < 60; i++)
        {
            Vector2 speed = Main.rand.NextVector2CircularEdge(0.5f, 0.5f);
            var d = Dust.NewDustPerfect(Projectile.Center, DustID.PurpleTorch, speed * 5, Scale: 1f);
            d.noGravity = true;
        }
    }
}
