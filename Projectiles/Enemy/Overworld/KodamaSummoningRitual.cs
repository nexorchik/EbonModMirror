using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Projectiles.Enemy.Overworld;
public class KodamaSummoningRitual : ModProjectile
{
    public override string Texture => Helper.Empty;
    public override bool ShouldUpdatePosition() => false;
    public override bool? CanDamage() => false;
    public override void SetDefaults()
    {
        Projectile.height = 1;
        Projectile.width = 1;
        Projectile.hostile = true;
        Projectile.friendly = false;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
    }
    public override void AI()
    {
        Projectile.timeLeft = 10;
        Projectile.ai[0]++;

    }
}
