using EbonianMod.Projectiles.Bases;
using EbonianMod.Projectiles.Friendly.Crimson;
using System.IO;

namespace EbonianMod.Items.Weapons.Magic;

public class CrimCannon : ModItem
{
    public override void SetDefaults()
    {
        Item.DamageType = DamageClass.Magic;
        Item.damage = 11;
        Item.useTime = 70;
        Item.mana = 1;
        Item.useAnimation = 25;
        Item.shoot = ProjectileType<CrimCannonGraphics>();
        Item.shootSpeed = 1f;
        Item.rare = ItemRarityID.Green;
        Item.useStyle = 5;
        Item.value = Item.buyPrice(0, 5, 0, 0);
        Item.autoReuse = false;
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.channel = true;
        Item.mana = 25;
    }
    public override void AddRecipes()
    {
        CreateRecipe().AddIngredient(ItemID.CrimtaneBar, 20).AddIngredient(ItemID.Vertebrae, 20).AddTile(TileID.Anvils).Register();
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        velocity.Normalize();
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        return false;
    }
}

public class CrimCannonGraphics : HeldProjectileGun
{
    public override string Texture => "EbonianMod/Items/Weapons/Magic/CrimCannonReload";

    public override void OnSpawn(IEntitySource source)
    {
        Player player = Main.player[Projectile.owner];
        player.CheckMana(-player.HeldItem.mana, true);
        Projectile.rotation = Helper.FromAToB(player.Center, Main.MouseWorld).ToRotation();
        Projectile.frame = 5;
        Projectile.ai[0] = -20;
        Projectile.netUpdate = true; // TEST
    }

    public override bool? CanDamage() => false;

    public override void SetDefaults()
    {
        base.SetDefaults();
        ItemType = ItemType<CrimCannon>();
        RotationSpeed = 0.08f;
        Projectile.Size = new Vector2(56, 38);
    }
    Vector2 Scale = new Vector2(0, 1);
    public override void AI()
    {
        base.AI();

        Player player = Main.player[Projectile.owner];

        player.heldProj = Projectile.whoAmI;

        if (!Main.player[Projectile.owner].channel || !player.CheckMana(player.HeldItem.mana))
            Projectile.Kill();

        Scale = Vector2.Lerp(Scale, new Vector2(1, 1), 0.14f);


        if (Projectile.ai[0]++ > 14)
        {
            Projectile.frameCounter--;
            if (Projectile.frameCounter <= 0)
            {
                Projectile.frameCounter += 6;
                Projectile.frame++;
                if (Projectile.frame > 5)
                {
                    Projectile.frame = 0;
                    AnimationRotation = -0.2f * player.direction;
                    Scale = new Vector2(0.65f, 1.6f);
                    SoundEngine.PlaySound(SoundID.NPCHit9.WithPitchOffset(Main.rand.NextFloat(-1f, -0.5f)), player.Center);
                    player.CheckMana(player.HeldItem.mana, true, true);
                    Vector2 SpawnPosition = Projectile.Center + Projectile.rotation.ToRotationVector2() * 22;
                    if (Main.myPlayer == player.whoAmI)
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), SpawnPosition, Projectile.rotation.ToRotationVector2() * 12, ProjectileType<CrimCannonP>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    for (int i = 0; i < 3; i++)
                    {
                        Dust.NewDustPerfect(SpawnPosition, DustID.Blood, (Projectile.rotation + Main.rand.NextFloat(PiOver2, PiOver4)).ToRotationVector2() * Main.rand.NextFloat(2, 6), Scale: 1.5f).noGravity = true;
                        Dust.NewDustPerfect(SpawnPosition, DustID.Blood, (Projectile.rotation + Main.rand.NextFloat(-PiOver2, -PiOver4)).ToRotationVector2() * Main.rand.NextFloat(2, 6), Scale: 1.5f).noGravity = true;
                    }
                    Projectile.frameCounter += 15;
                }
            }
        }
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Rectangle frameRect = new Rectangle(0, Projectile.frame * Projectile.height, Projectile.width, Projectile.height);
        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frameRect, lightColor, Projectile.rotation, new Vector2(Projectile.width / 2 - 25, Projectile.height / 2), Scale, Main.player[Projectile.owner].direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        return false;
    }
}