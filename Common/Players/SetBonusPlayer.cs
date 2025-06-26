using EbonianMod.Items.Weapons.Magic;
using EbonianMod.Projectiles.Friendly.Corruption;
using EbonianMod.Projectiles.Friendly.Crimson;
using EbonianMod.Projectiles.VFXProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Common.Players;
public class SetBonusPlayer : ModPlayer
{
    public bool cecitior, terrortoma;
    public int terrortomaCharge;
    public override void ResetEffects()
    {
        cecitior = false;
        if (!terrortoma)
            terrortomaCharge = 0;
        terrortoma = false;
    }
    public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (cecitior && hit.Damage >= target.life && target.life <= 0 && !target.CountsAsACritter && target.lifeMax > 5 && Main.rand.NextBool(4))
            MPUtils.NewProjectile(null, target.Center, Vector2.Zero, ProjectileType<IchorArmorExplosion>(), 30, 0, Player.whoAmI, ai2: (proj.type == ProjectileID.GoldenShowerFriendly ? (proj.ai[2] == 52 ? 2 : 0) : 0));
    }
    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (cecitior && hit.Damage >= target.life && target.life <= 0 && !target.CountsAsACritter && target.lifeMax > 5 && Main.rand.NextBool(4))
            MPUtils.NewProjectile(null, target.Center, Vector2.Zero, ProjectileType<IchorArmorExplosion>(), 30, 0, Player.whoAmI);
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (terrortoma)
        {
            terrortomaCharge += MathHelper.Clamp(hit.Damage, 0, 100);
            Main.NewText(terrortomaCharge);
            if (terrortomaCharge > 1500)
            {
                SoundEngine.PlaySound(SoundID.Item20, Player.Center);
                for (int i = 0; i < 10; i++)
                {
                    MPUtils.NewProjectile(null, Player.Center, Player.FromAToB(target.Center).RotatedByRandom(i * 0.1f) * Main.rand.NextFloat(1, 5), ProjectileType<TerrorArmorRay>(), 50, 0);
                }
                MPUtils.NewProjectile(null, Player.Center - Player.FromAToB(target.Center) * 40, Player.FromAToB(target.Center), ProjectileType<ConglomerateScream>(), 0, 0, ai0: 0.3f, ai2: 1);
                terrortomaCharge = -1000;
            }
        }
    }
}
