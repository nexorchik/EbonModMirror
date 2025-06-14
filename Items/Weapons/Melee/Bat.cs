
using EbonianMod.Items.Materials;
using EbonianMod.Projectiles.Bases;
using EbonianMod.Projectiles.Friendly.Generic;
using EbonianMod.Projectiles.Friendly.Underworld;
using EbonianMod.Projectiles.VFXProjectiles;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace EbonianMod.Items.Weapons.Melee;

public class Bat : ModItem
{
    public override void SetDefaults()
    {
        Item.knockBack = 10f;
        Item.width = 48;
        Item.height = 66;
        Item.crit = 15;
        Item.damage = 18;
        Item.useAnimation = 20;
        Item.useTime = 20;
        Item.noUseGraphic = true;
        Item.autoReuse = false;
        Item.noMelee = true;
        Item.value = Item.buyPrice(0, 3, 0, 0);
        Item.channel = true;
        Item.DamageType = DamageClass.Melee;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.rare = ItemRarityID.Green;
        Item.shoot = ProjectileType<BatGraphics>();
    }

    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemID.WoodenBoomerang, 2).AddIngredient(ItemType<CecitiorMaterial>(), 20).AddTile(TileID.MythrilAnvil).Register();
    }

    public override bool AltFunctionUse(Player player)
    {
        return true;
    }

    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse == 2)
        {
            Item.shoot = ProjectileType<Ball>();
        }
        else
        {
            Item.shoot = ProjectileType<BatGraphics>();
        }
        return Item.shoot == ProjectileType<BatGraphics>()
            || (Item.shoot == ProjectileType<Ball>()
            && player.ownedProjectileCounts[ProjectileType<Ball>()] < 4);
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (player.altFunctionUse == 2)
        {
            if (MathF.Abs(player.velocity.Y) < 0.03f)
                velocity = new Vector2(player.velocity.X, -5 + player.velocity.Y);
            else
                velocity += new Vector2(player.velocity.X, -1.5f + player.velocity.Y);
        }
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 1);
        return false;
    }
}
public class BatGraphics : HeldSword
{
    public override string Texture => Helper.Empty;

    public override bool? CanDamage() => Projectile.ai[1] < 0 && Projectile.timeLeft > 10;
    public override void SetExtraDefaults()
    {
        Projectile.width = 10;
        Projectile.height = 62;
    }
    public override void OnSpawn(IEntitySource source)
    {
        Projectile.scale = 0.3f;
        Player player = Main.player[Projectile.owner];
        Projectile.spriteDirection = Main.MouseWorld.X > player.Center.X ? 1 : -1;
        Projectile.rotation = Helper.FromAToB(player.Center, Main.MouseWorld).ToRotation() + Pi / 2;
        Projectile.ai[0] = Projectile.rotation - Pi * 0.9f * player.direction;
        Projectile.localAI[0] = Projectile.rotation - Pi;
        Projectile.ai[2] = 36;
    }
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];

        Projectile.scale = Lerp(Projectile.scale, 1, 0.45f);

        Projectile.velocity = new Vector2(0);

        player.heldProj = Projectile.whoAmI;
        Projectile.ai[1]++;
        if (Projectile.ai[1] > 0)
        {
            if (Main.mouseRightRelease && Main.myPlayer == player.whoAmI)
                Projectile.localAI[1]--;

            if (Main.mouseRight && Main.myPlayer == player.whoAmI && Projectile.localAI[1] < 0)
            {
                Vector2 v = new Vector2(player.velocity.X, -5 + player.velocity.Y);
                Projectile.NewProjectile(Projectile.InheritSource(Projectile), player.MountedCenter + v, v, ProjectileType<Ball>(), Projectile.damage, Projectile.knockBack, player.whoAmI);
                Projectile.localAI[1] = 20;
                Projectile.netUpdate = true;
            }
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Projectile.ai[0], 0.2f);
            if (!player.channel && Projectile.ai[1] > 12)
            {
                Projectile.ai[1] = -100;
            }
            Projectile.timeLeft = 30;
        }
        Vector2 StartRotationDirection = (Projectile.localAI[0] + Pi / 2).ToRotationVector2();
        if (Projectile.ai[1] < 0)
        {
            if (Projectile.timeLeft == 29)
            {
                SoundEngine.PlaySound(SoundID.Item1.WithPitchOffset(Main.rand.NextFloat(-0.82f, -0.6f)), Projectile.Center);
            }
            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (Vector2.Distance(player.Center + StartRotationDirection * 30, projectile.Center) < 55 && projectile.type == ProjectileType<Ball>() && projectile.ai[1] == 0 && Projectile.ai[2] > 27)
                {
                    SoundEngine.PlaySound(SoundID.Item126.WithPitchOffset(Main.rand.NextFloat(-0.9f, -0.7f)), Projectile.Center);
                    SoundEngine.PlaySound(SoundID.Item10.WithPitchOffset(Main.rand.NextFloat(1f, 2f)), Projectile.Center);
                    float BaseVelocity = projectile.velocity.Length();
                    float VelocityPower = MathF.Pow(BaseVelocity, 2) / 9;
                    projectile.velocity = StartRotationDirection * Clamp(VelocityPower, 14, 34);
                    projectile.timeLeft = 360;
                    projectile.Opacity = 1;
                    if (Projectile.localAI[0] > -3.3f && Projectile.localAI[0] < -2.9f)
                        projectile.velocity.X = player.velocity.X;
                    projectile.ai[1] = 10;
                    projectile.SyncProjectile();
                }
            }
            Projectile.rotation += player.direction * ToRadians(Projectile.ai[2]);
            if (Projectile.timeLeft == 23)
            {
                Projectile.ai[2] /= 2.5f;
            }
            if (Projectile.timeLeft < 23)
            {
                Projectile.Opacity *= 0.72f;
                Projectile.ai[2] *= 0.8f;
            }
        }

        if (Projectile.Opacity > 0.2f)
        {
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + Pi);
            player.itemAnimation = 2;
        }

        Projectile.Center = player.MountedCenter;



        player.itemTime = 2;

        if (player.HeldItem.type != ItemType<Bat>() && !player.active || player.dead || player.CCed || player.noItems)
        {
            Projectile.Kill();
        }
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Player player = Main.player[Projectile.owner];
        Texture2D tex = Helper.GetTexture("EbonianMod/Items/Weapons/Melee/Bat").Value;
        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, lightColor * Projectile.Opacity, Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2 + 41), Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
        return false;
    }
}