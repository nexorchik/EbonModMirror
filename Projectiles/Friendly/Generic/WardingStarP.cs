
using System;
using System.Collections.Generic;

namespace EbonianMod.Projectiles.Friendly.Generic
{
    // TODO: OBTAINABILITYYYYYYYYYYYYYYYY
    public class WardingStarP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(26, 28);
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 30;
            Projectile.netUpdate = true;
            Projectile.netImportant = true;
            Projectile.ownerHitCheck = true;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(Projectile.ai[1] * 0.075f);
            Projectile.ai[1]++;
            Projectile.ai[2] = MathHelper.Lerp(Projectile.ai[2], 0, 0.1f);
            Player player = Main.player[Projectile.owner];
            if (player.whoAmI == Main.myPlayer)
            {
                if ((int)Projectile.ai[1] == 60)
                {
                    SoundStyle style = SoundID.Item82;
                    style.Volume = 0.5f;
                    SoundEngine.PlaySound(style, Projectile.Center);
                    Projectile.ai[2] = 1;
                    for (int i = -1; i < 2; i++)
                    {
                        if (i == 0) continue;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Helper.FromAToB(player.Center, Main.MouseWorld) * 7, ModContent.ProjectileType<WardingStarP2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, i * 0.5f);
                    }
                    Color newColor7 = Color.MediumPurple;
                    for (int num613 = 0; num613 < 3; num613++)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 58, Helper.FromAToB(player.Center, Main.MouseWorld).X * 0.1f, Helper.FromAToB(player.Center, Main.MouseWorld).Y * 0.1f, 150, default(Color), 0.8f);
                    }
                    for (float num614 = 0f; num614 < 0.5f; num614 += 0.125f)
                    {
                        Dust.NewDustPerfect(Projectile.Center, 278, Vector2.UnitY.RotatedBy(num614 * Main.rand.NextFloat(2) * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
                    }
                    for (float num615 = 0f; num615 < 0.5f; num615 += 0.25f)
                    {
                        Dust.NewDustPerfect(Projectile.Center, 278, Vector2.UnitY.RotatedBy(num615 * Main.rand.NextFloat(2) * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
                    }

                    player.CheckMana(player.HeldItem.mana, true, true);
                    player.manaRegenDelay = (int)player.maxRegenDelay;

                    Projectile.ai[1] = 0;
                    Projectile.netUpdate = true;
                }
                Projectile.timeLeft++;
                player.direction = Main.MouseWorld.X >= player.Center.X ? 1 : -1;
                player.itemTime = 2;
                player.itemAnimation = 2;
                Projectile.ModProjectile.DrawHeldProjInFrontOfHeldItemAndArms = true;
                player.heldProj = Projectile.whoAmI;
                Projectile.Center = player.Center + Helper.FromAToB(player.Center, Main.MouseWorld) * 20;
                if (Projectile.position != Projectile.oldPosition)
                    Projectile.netUpdate = true;
                if (!player.channel || player.statMana <= 0 || !player.CheckMana(1)) Projectile.Kill();
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.Draw(Assets.ExtraSprites.Projectiles.WardingStarP_Extra.Value, Projectile.Center - Main.screenPosition, null, Color.White, -Projectile.rotation, Assets.ExtraSprites.Projectiles.WardingStarP_Extra.Value.Size() / 2, 1, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(Assets.ExtraSprites.Projectiles.WardingStarP_Extra.Value, Projectile.Center - Main.screenPosition, null, Color.White with { A = 0 } * Projectile.ai[2], -Projectile.rotation, Assets.ExtraSprites.Projectiles.WardingStarP_Extra.Value.Size() / 2, 1, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Type].Value, Projectile.Center - Main.screenPosition, null, Color.White with { A = 0 } * Projectile.ai[2], Projectile.rotation, TextureAssets.Projectile[Type].Value.Size() / 2, 1, SpriteEffects.None, 0);
        }
    }
    public class WardingStarP2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
            Projectile.hide = true;
            Projectile.timeLeft = 100;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D baseTex = TextureAssets.Projectile[Type].Value;
            Texture2D baseTex2 = Assets.ExtraSprites.Projectiles.WardingStarP2_Alt.Value;
            Texture2D tex = Projectile.ai[0] < 0 ? baseTex : baseTex2;
            var fadeMult = 1f / ProjectileID.Sets.TrailCacheLength[Projectile.type];
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float mult = (1f - fadeMult * i);
                Main.spriteBatch.Draw(tex, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.White with { A = 0 } * mult, Projectile.rotation, Projectile.Size / 2, Projectile.scale * mult, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Projectile.Size / 2, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        Vector2 initCenter, initVel;
        bool drawBehind;
        public override void AI()
        {
            if (Projectile.timeLeft == 99)
            {
                initCenter = Projectile.Center;
                initVel = Projectile.velocity;
            }
            Projectile.rotation += MathHelper.ToRadians(4);
            if (initCenter != Vector2.Zero)
                Projectile.SineMovement(initCenter, initVel, 0.2f - (Projectile.timeLeft * 0.0015f), Projectile.timeLeft * 0.65f);

            float wave = (float)Math.Sin(Projectile.ai[1] * 0.1f);
            drawBehind = wave > 0.5f;
            Projectile.hide = drawBehind;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            Color newColor7 = Color.MediumPurple;
            for (int num613 = 0; num613 < 7; num613++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 58, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default(Color), 0.8f);
            }
            for (float num614 = 0f; num614 < 1f; num614 += 0.125f)
            {
                Dust.NewDustPerfect(Projectile.Center, 278, Vector2.UnitY.RotatedBy(num614 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (4f + Main.rand.NextFloat() * 4f), 150, newColor7).noGravity = true;
            }
            for (float num615 = 0f; num615 < 1f; num615 += 0.25f)
            {
                Dust.NewDustPerfect(Projectile.Center, 278, Vector2.UnitY.RotatedBy(num615 * ((float)Math.PI * 2f) + Main.rand.NextFloat() * 0.5f) * (2f + Main.rand.NextFloat() * 3f), 150, Color.Gold).noGravity = true;
            }
        }
    }
}
