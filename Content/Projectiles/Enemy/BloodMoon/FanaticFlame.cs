using System;
using System.Collections.Generic;

namespace EbonianMod.Content.Projectiles.Enemy.BloodMoon;

public class FanaticFlame : ModProjectile
{
	public override string Texture => Helper.AssetPath + "Projectiles/Enemy/BloodMoon/" + Name;
	public override void SetStaticDefaults()
	{
		Main.projFrames[Type] = 4;
		ProjectileID.Sets.TrailCacheLength[Type] = 20;
		ProjectileID.Sets.TrailingMode[Type] = 2;
	}

	public override void SetDefaults()
	{
		Projectile.Size = new Vector2(32);
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.tileCollide = false;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 50;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D tex = TextureAssets.Projectile[Type].Value;
		Texture2D glow = Assets.Extras.flameEye2.Value;

		List<VertexPositionColorTexture> vertices = new();

		float count = 0; 
		for (int i = 0; i < Projectile.oldPos.Length; i++)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			count++;
		}
		
		for (int i = 0; i < count - 1; i++)
		{
			if (Projectile.oldPos[i+1] == Vector2.Zero)
				continue;
			Vector2 position = Projectile.oldPos[i] + Projectile.Size / 2- Main.screenPosition;
			float rotation = (Projectile.oldPos[i] - Projectile.oldPos[i+1]).ToRotation();
			Color color = Color.Maroon with { A = 0 } * MathF.Pow(1f - i / count, 2) * (1 - Projectile.localAI[1]) * 0.5f;
			for (int j = -1; j < 2; j += 2)
			{
				Vector2 texCoord = new Vector2(-i / (float)(Projectile.oldPos.Length) +Main.GlobalTimeWrappedHourly*2 + Projectile.whoAmI*4, j > 0 ? 1 : 0); 
				vertices.Add(Helper.AsVertex(position + new Vector2(20*Projectile.localAI[0], 0).RotatedBy(rotation + PiOver2*j), color, texCoord));
			}
		}
		
		Main.spriteBatch.End(out var ss);
		Main.spriteBatch.Begin(ss with {samplerState = SamplerState.PointWrap});

		if (vertices.Count > 2)
		{
			Helper.DrawTexturedPrimitives(vertices.ToArray(), PrimitiveType.TriangleStrip,
				Assets.Extras.Tentacle.Value);
			Helper.DrawTexturedPrimitives(vertices.ToArray(), PrimitiveType.TriangleStrip,
				Assets.Extras.FlamesSeamless.Value);
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(ss);
		
		Rectangle frame = tex.Frame(1, 4, 0, Projectile.frame);
		Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, null, Color.Maroon with {A = 0} * (1-Projectile.localAI[1]), Main.rand.NextFloat(Pi), glow.Size()/2, Projectile.scale * Projectile.localAI[0]*0.28f, SpriteEffects.None,0);
		
		Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, frame, Color.White * 0.5f*(1-Projectile.localAI[1]), Projectile.rotation, frame.Size()/2, Projectile.scale * Projectile.localAI[0], SpriteEffects.None,0);
		Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, frame, Color.White with {A = 0} * (1-Projectile.localAI[1]), Projectile.rotation, frame.Size()/2, Projectile.scale * Projectile.localAI[0], SpriteEffects.None,0);
		
		return false;
	}

	public override void AI()
	{
		Lighting.AddLight(Projectile.Center, Color.Maroon.ToVector3());
		
		if (Projectile.timeLeft > 25)
			Projectile.localAI[0] = Lerp(Projectile.localAI[0], 1, 0.1f);
		else
			Projectile.localAI[1] = Lerp(Projectile.localAI[1], 1, 0.1f);
		
		if (Helper.Raycast(Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.UnitY), 20).RayLength < 16f)
			Projectile.velocity *= 0.85f;
		
		if (Projectile.frameCounter++ % 5 == 0)
			if (Projectile.frame++ > 2)
				Projectile.frame = 0;

		if (Projectile.velocity.Length() < 10f)
		{
			Projectile.velocity.X *= 1.005f;
			Projectile.velocity.Y -= 0.1f;
		}
	}
}