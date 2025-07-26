using EbonianMod.Items.Consumables.Food;
using EbonianMod.Projectiles.Bases;
using EbonianMod.Projectiles.Friendly.Generic;
using System;

namespace EbonianMod.Items.Weapons.Ranged;

public class SpudCannon : ModItem
{
    public override void SetDefaults()
    {
        Item.Size = new Vector2(58, 24);
        Item.DamageType = DamageClass.Ranged;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.rare = ItemRarityID.Blue;
        Item.knockBack = 5;
        Item.damage = 13;
        Item.useTime = 40;
        Item.useAnimation = 40;
        Item.value = Item.buyPrice(0, 1, 0, 0);
        Item.shoot = ProjectileType<PotatoP>();
        Item.useAmmo = ItemType<Potato>();
        Item.autoReuse = false;
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.channel = true;
    }
    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        type = ProjectileType<SpudCannonP>();
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        velocity = Vector2.Zero;
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        return false;
    }
    public override bool CanConsumeAmmo(Item ammo, Player player) => false;
    public class SpudCannonP : HeldProjectileGun
    {
        public override bool? CanDamage() => false;
        public override string Texture => "EbonianMod/Items/Weapons/Ranged/SpudCannon";

        Vector2 Scale = new Vector2(0, 1);
        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Helper.FromAToB(player.Center, Main.MouseWorld).ToRotation() + player.direction * Pi * 2;
            Projectile.ai[2] = 1;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            ItemType = ItemType<SpudCannon>();
            RotationSpeed = 0.2f;
            Projectile.Size = new Vector2(58, 24);
        }
        float Charge;
        public override void AI()
        {
            base.AI();

            Player player = Main.player[Projectile.owner];

            player.heldProj = Projectile.whoAmI;

            Scale = Vector2.Lerp(Scale, Vector2.One, 0.21f);

            if (player.channel || Projectile.ai[0] < 14)
            {
                if (Projectile.ai[0] < 70)
                    Projectile.ai[0]++;
                if (Projectile.ai[0] == 69)
                {
                    SoundEngine.PlaySound(SoundID.MaxMana.WithPitchOffset(-0.3f), Projectile.Center);
                    Projectile.ai[1] = 1f;
                }
                Charge = Projectile.ai[0] / 35;
                if (Projectile.timeLeft % 2 == 0)
                    SoundEngine.PlaySound(SoundID.Item98.WithPitchOffset(Main.rand.NextFloat(Charge - 4, Charge - 3)) with { Volume = Charge / 10 }, player.Center);
                Projectile.Center = player.MountedCenter;
            }
            else
            {
                Projectile.Center = player.MountedCenter;
                if (Projectile.ai[0] != 105)
                {
                    Projectile.UseAmmo(ItemType<Potato>());

                    SoundEngine.PlaySound(SoundID.Item42.WithPitchOffset(Main.rand.NextFloat(Charge - 3, Charge - 2.6f)) with { Volume = Clamp(Charge - 0.3f, 0.2f, 3) }, player.Center);
                    SoundEngine.PlaySound(SoundID.Item89.WithPitchOffset(Main.rand.NextFloat(Charge - 2.1f, Charge - 1.8f)) with { Volume = Charge - 0.3f }, player.Center);

                    Vector2 SpawnPosition = Projectile.Center + new Vector2(52, 5 * player.direction).RotatedBy(Projectile.rotation);

                    for (int i = 0; i < Clamp(10 * Charge, 8, 100); i++)
                        Dust.NewDustPerfect(SpawnPosition, DustID.Smoke, (Projectile.rotation + Main.rand.NextFloat(-Pi / (Charge * 6), Pi / (Charge * 6))).ToRotationVector2() * Main.rand.NextFloat(0, 8) * Charge, Scale: 1.5f).noGravity = true;

                    Projectile CurrentProjectile = MPUtils.NewProjectile(Projectile.InheritSource(Projectile), SpawnPosition, Projectile.rotation.ToRotationVector2() * Charge * 8, ProjectileType<PotatoP>(), (int)(Projectile.damage * MathF.Sqrt(Charge * 4)), Projectile.knockBack * Charge, Projectile.owner);
                    if (Charge == 2)
                    {
                        SoundEngine.PlaySound(SoundID.Item40.WithPitchOffset(Main.rand.NextFloat(0.5f, 1)) with { Volume = 0.8f }, player.Center);
                        for (int i = 0; i < Clamp(8 * Charge, 8, 100); i++)
                            Dust.NewDustPerfect(SpawnPosition, DustID.Torch, (Projectile.rotation + Main.rand.NextFloat(-Pi / (Charge * 6), Pi / (Charge * 6))).ToRotationVector2() * Main.rand.NextFloat(0, 5) * Charge, Scale: Main.rand.NextFloat(0.4f, 3)).noGravity = true;
                        if (CurrentProjectile is not null)
                            CurrentProjectile.CritChance = 100;
                    }

                    Scale = new Vector2(1 - Charge / 5, 1 + Charge * 0.4f);
                    Projectile.ai[0] = 105;
                }
                if (Scale.Length() < 1.416f)
                    Projectile.Kill();
            }
            if (Projectile.ai[1] > 0)
            {
                Projectile.ai[1] -= 0.04f;
                Projectile.ai[2] += 0.01f;
            }
        }

        float ColorModifier; 
        Vector2 Shake = Vector2.Zero;
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Rectangle frameRect = new Rectangle(0, 0, Projectile.width, Projectile.height);

            if (Projectile.ai[0] == 105)
            {
                ColorModifier = Lerp(ColorModifier, 1, 0.03f);
                Shake = Vector2.Zero;
            }
            else
            {
                ColorModifier = 1 - Charge / 7;
                Shake = new Vector2(Main.rand.NextFloat(-Charge, Charge), Main.rand.NextFloat(-Charge, Charge));
            }
            Main.EntitySpriteDraw(Helper.GetTexture("Items/Weapons/Ranged/SpudCannon").Value, Projectile.Center - Main.screenPosition + player.GFX() + Shake, frameRect, new Color(1, ColorModifier, ColorModifier, 1), Projectile.rotation, new Vector2(Projectile.width / 2 - 25, Projectile.height / 2 - 2 * player.direction), Scale, player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
            Main.EntitySpriteDraw(Helper.GetTexture("Items/Weapons/Ranged/SpudCannonFlash").Value, Projectile.Center + player.GFX() + new Vector2(25, 0).RotatedBy(Projectile.rotation) - Main.screenPosition, frameRect, lightColor * Projectile.ai[1], Projectile.rotation, new Vector2(Projectile.width / 2, Projectile.height / 2), new Vector2(Projectile.ai[2], Projectile.ai[2] * 1.2f) * Scale, player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
            return false;
        }
    }
}