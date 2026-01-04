using EbonianMod.Projectiles.Bases;
using System;
using System.Collections.Generic;

namespace EbonianMod.Items.Weapons.Melee;

public class SanguineSlasher : ModItem
{
    public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(25); // heal
    public override void SetDefaults()
    {
        Item.knockBack = 10f;
        Item.width = 48;
        Item.height = 66;
        Item.crit = 15;
        Item.damage = 18;
        Item.useAnimation = 32;
        Item.useTime = 32;
        Item.noUseGraphic = true;
        Item.autoReuse = false;
        Item.noMelee = true;
        Item.value = Item.buyPrice(0, 3, 0, 0);
        Item.channel = true;
        //Item.reuseDelay = 45;
        Item.DamageType = DamageClass.Melee;
        //Item.UseSound = SoundID.Item1;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.rare = ItemRarityID.Green;
        Item.shootSpeed = 1f;
        Item.shoot = ProjectileType<SanguineSlasherP>();
    }
    int dir = 1;
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        dir = -dir;
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 1, dir);
        return false;
    }
}
public class SanguineSlasherP : HeldSword
{
    public override string Texture => Helper.Empty;
    public override void SetExtraDefaults()
    {
        Projectile.width = 102;
        Projectile.height = 94;
        swingTime = 30;
        holdOffset = 30;
    }
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];

        if (Projectile.ai[0] % 3 == 0)
        {
            Projectile.scale = 1.4f;
            Projectile.ai[1] = 0;
        }
        player.itemTime = 2;
        player.itemAnimation = 2;
        if (player.HeldItem.type != ItemType<SanguineSlasher>() && !player.active || player.dead || player.CCed || player.noItems)
        {
            Projectile.Kill();
        }
        if (Projectile.ai[1] != 0) // AI1 being -1 or 1 = Slash
        {
            Projectile.spriteDirection = (int)Projectile.ai[1];
            visualOffset = new Vector2(-16, Projectile.ai[1] == -1 ? 4 : 0).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.scale;
            int direction = (int)Projectile.ai[1];
            float swingProgress = Ease(Utils.GetLerpValue(0f, swingTime, Projectile.timeLeft));
            float defRot = Projectile.velocity.ToRotation();
            float start = defRot - Pi * 0.75f;
            float end = defRot + Pi * 0.75f;
            float rotation = (direction == 1 ? start + Pi * 1.5f * swingProgress : end - Pi * 1.5f * swingProgress);
            Vector2 position = player.GetFrontHandPosition(stretch, rotation - PiOver2) + rotation.ToRotationVector2() * holdOffset * ScaleFunction(swingProgress) - (rotation + Pi * 1.02f).ToRotationVector2() * 20;
            Projectile.Center = position;
            Projectile.rotation = (position - player.Center).ToRotation() + PiOver2;
            player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            player.SetCompositeArmFront(true, stretch, rotation - (player.direction == -1 ? -PiOver4 + Pi : PiOver2));
            if (Projectile.timeLeft == swingTime - 1)
            {
                SoundEngine.PlaySound(SoundID.Item1);
            }
        }
        else // AI1 being 0 = Thrust (or well, bite here)
        {
            Projectile.scale = 1.4f;
            Projectile.ai[2] = Lerp(Projectile.ai[2], 0, 0.2f);
            Projectile.spriteDirection = player.direction;
            visualOffset = new Vector2(-18, 14).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.scale;
            if (Projectile.frameCounter < 9)
            {
                float progress = Ease(Utils.GetLerpValue(0f, swingTime, Projectile.timeLeft));
                holdOffset = baseHoldOffset * (progress + 1.5f);
            }
            Vector2 pos = player.RotatedRelativePoint(player.MountedCenter);
            player.ChangeDir(Projectile.velocity.X < 0 ? -1 : 1);
            player.itemRotation = Projectile.velocity.ToRotation() * player.direction;
            pos += Projectile.velocity.ToRotation().ToRotationVector2() * holdOffset;
            if (player.gravDir != -1)
            {
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.velocity.ToRotation() - PiOver2);
                Projectile.rotation = Projectile.velocity.ToRotation() + Pi / 2;
            }
            Projectile.Center = pos;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4 && Projectile.frameCounter < 9)
            {
                if (Projectile.frame == 2)
                {
                    SoundEngine.PlaySound(EbonianSounds.chomp0, player.Center);
                    for (int i = 0; i < 30; i++)
                    {
                        Dust.NewDustPerfect(Projectile.Center - (Projectile.rotation + Pi).ToRotationVector2() * 21 * player.direction + Projectile.velocity.ToRotation().ToRotationVector2() * 25, DustID.Blood, (Projectile.rotation + Main.rand.NextFloat(-Pi / 2, Pi / 2)).ToRotationVector2() * player.direction * Main.rand.NextFloat(2, 4), Scale: Main.rand.NextFloat(1f, 1.7f));
                    }
                }
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 3)
                {
                    Projectile.ai[2] = 1;
                    Projectile.frame = 0;
                    Projectile.frameCounter = 10;
                    Projectile.timeLeft = 15;
                }
            }
        }
        ExtraAI();
    }
    public override void OnKill(int timeLeft)
    {
        Player player = Main.player[Projectile.owner];
        if (player.active && player.channel && !player.dead && !player.CCed && !player.noItems)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                Vector2 direction = Vector2.Normalize(Main.MouseWorld - player.Center);
                Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, direction, Projectile.type, Projectile.damage, Projectile.knockBack, player.whoAmI, Projectile.ai[0] + 1, (Projectile.ai[1] == 0 ? 1 : -Projectile.ai[1]));
                if (Projectile.ai[1] != 0)
                    projectile.rotation = Projectile.rotation;
                projectile.Center = Projectile.Center;
                projectile.ai[0] = Projectile.ai[0] + 1;
                projectile.SyncProjectile();
            }
        }
    }
    bool _hit;
    public override bool? CanDamage()
    {
        if (Projectile.ai[1] == 0 && (Projectile.timeLeft > swingTime - 10  || Projectile.timeLeft < swingTime - 16))
            return false;
        return true;
    }

    public override void OnHit(NPC target, NPC.HitInfo hitinfo, int damage)
    {
        Player player = Main.player[Projectile.owner];
        if (Projectile.ai[0] % 3 == 0 && !_hit)
        {
            player.Heal(25);
            _hit = true;
        }
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Player player = Main.player[Projectile.owner];
        Texture2D tex = Helper.GetTexture("EbonianMod/Items/Weapons/Melee/SanguineSlasherAnimation").Value;
        Rectangle frameRect = new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height);
        Vector2 scale = Vector2.Lerp(Vector2.One * Projectile.scale, new Vector2(0.8f, 1.2f) * Projectile.scale, Projectile.ai[2]);
        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, frameRect, lightColor, Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);

        if (Projectile.ai[1] != 0)
        {
            Texture2D slash = Assets.Extras.Extras2.slash_06.Value;
            float mult = Ease(Utils.GetLerpValue(0f, swingTime, Projectile.timeLeft));
            float alpha = (float)Math.Sin(mult * Math.PI);
            Vector2 pos = player.Center + Projectile.velocity * 40f;
            Main.spriteBatch.Draw(slash, pos + visualOffset - Main.screenPosition, null, Color.Maroon with { A = 0 } * alpha * 0.5f, Projectile.velocity.ToRotation(), slash.Size() / 2, 0.8f, SpriteEffects.None, 0f);
        }
        return false;
    }
    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        behindNPCsAndTiles.Add(index);
    }
}