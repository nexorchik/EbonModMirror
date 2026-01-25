using EbonianMod.Content.Items.Misc;
using EbonianMod.Content.Projectiles.Minions;
using EbonianMod.Content.Projectiles.Terrortoma;

namespace EbonianMod.Content.Projectiles.Friendly.Corruption;

public class TerrorStaffP : ModProjectile
{
    public override string Texture => Helper.AssetPath + "Projectiles/Friendly/Corruption/" + Name;
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 2;
    }
    public override void SetDefaults()
    {
        Projectile.Size = new Vector2(32);
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.aiStyle = 2;
        Projectile.DamageType = DamageClass.Summon;
    }
    /*public override bool PreDraw(ref Color lightColor)
    {
        Texture2D tex = TextureAssets.Projectile[Type].Value;
        DrawData data = new DrawData(tex, Projectile.Center - Main.screenPosition, null, lightColor, 0, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
        RenderingUtils.DrawWithDye(Main.spriteBatch, data, ItemID.GreenFlameDye, Projectile);
        return false;
    }*/
    public override void OnKill(int timeLeft)
    {
        //Helper.DustExplosion(Projectile.Center, Projectile.Size, 0, Color.Green);

        if (Projectile.owner == Main.myPlayer)
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Main.rand.NextVector2Unit() * 10, ProjectileType<EbonFlyMinion>(), Projectile.damage / 4, Projectile.knockBack, Projectile.owner);

    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (Helper.Raycast(Projectile.Center, -Vector2.UnitY, Projectile.height * 1.5f).RayLength < Projectile.height)
        {
            Projectile.velocity.Y = 0;
            return false;
        }
        if ((Helper.Raycast(Projectile.Center, Vector2.UnitX, Projectile.height * 1.5f).RayLength < Projectile.width || Helper.Raycast(Projectile.Center, -Vector2.UnitX, Projectile.height * 1.5f).RayLength < Projectile.width) && Helper.Raycast(Projectile.Center, Vector2.UnitY, Projectile.height * 1.5f).RayLength > Projectile.height)
        {
            Projectile.velocity.X = 0;
            return false;
        }
        Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Helper.Raycast(Projectile.Center, Vector2.UnitY, 200).Point, Vector2.Zero, ProjectileType<TExplosion>(), 0, 0);
        Terraria.Audio.SoundEngine.PlaySound(Sounds.eggplosion, Projectile.Center);
        return true;
    }
    public override void AI()
    {
        Projectile.frameCounter++;
        if (Projectile.frameCounter % 5 == 0)
        {
            if (Projectile.frame == 0) Projectile.frame++;
            else Projectile.frame = 0;
        }
        Projectile.velocity.Y += 0.25f;
    }
}
public class TerrorStaffPEvil : ModProjectile
{
    public override string Texture => Helper.AssetPath+"Projectiles/Friendly/Corruption/TerrorStaffP";
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 2;
    }
    public override void SetDefaults()
    {
        Projectile.Size = new Vector2(32);
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.aiStyle = 2;
        Projectile.timeLeft = 500;
    }
    public override void OnKill(int timeLeft)
    {
        if (Projectile.owner == Main.myPlayer)
            Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Main.rand.NextVector2Unit() * 10, ProjectileType<OstertagiExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (Helper.Raycast(Projectile.Center, -Vector2.UnitY, Projectile.height * 1.5f).RayLength < Projectile.height)
        {
            Projectile.velocity.Y = 0;
            return false;
        }
        if ((Helper.Raycast(Projectile.Center, Vector2.UnitX, Projectile.height * 1.5f).RayLength < Projectile.width || Helper.Raycast(Projectile.Center, -Vector2.UnitX, Projectile.height * 1.5f).RayLength < Projectile.width) && Helper.Raycast(Projectile.Center, Vector2.UnitY, Projectile.height * 1.5f).RayLength > Projectile.height)
        {
            Projectile.velocity.X = 0;
            return false;
        }

        Projectile.NewProjectileDirect(Projectile.InheritSource(Projectile), Helper.Raycast(Projectile.Center, Vector2.UnitY, 200).Point, Vector2.Zero, ProjectileType<TExplosion>(), 0, 0);
        SoundEngine.PlaySound(Sounds.eggplosion, Projectile.Center);
        return true;
    }
    public override Color? GetAlpha(Color lightColor) => Color.White;
    public override void AI()
    {
        Projectile.frameCounter++;
        if (Projectile.frameCounter % 5 == 0)
        {
            if (Projectile.frame == 0) Projectile.frame++;
            else Projectile.frame = 0;
        }
        Projectile.velocity.Y += 0.25f;

        for (int i = 0; i < 3; i++) Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CorruptionThorns, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f).noGravity = true;
    }
}
