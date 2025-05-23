using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EbonianMod.Projectiles.Friendly.Generic
{
    public class GoldTippedArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.extraUpdates = 1;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.CanBeChasedBy(this))
            {
                target.value += Item.buyPrice(0, 0, target.boss ? 0 : Main.rand.Next(new int[] { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 5, 5, 5, 10, 10, 25 }), Main.rand.Next(50, 100));
                target.GetGlobalNPC<FighterGlobalAI>().GoldMarked = true;
                target.moneyPing(target.Center);
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.width, DustID.GoldCoin, Projectile.velocity.X * 0.3f, Projectile.velocity.Y * 0.3f);
        }
        public override void AI()
        {
            Dust.NewDustPerfect(Projectile.Center, DustID.GoldCoin, Projectile.velocity.RotatedByRandom(0.2f) * Main.rand.NextFloat(0.05f, 0.2f));
        }
    }
}
