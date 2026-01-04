using EbonianMod.Content.Items.Accessories;
using EbonianMod.Content.Projectiles;
using EbonianMod.Content.Projectiles.ArchmageX;
using EbonianMod.Content.Projectiles.Friendly.Generic;
using Terraria.Chat;

namespace EbonianMod.Common.Players;
public class AccessoryPlayer : ModPlayer
{
    public bool brainAcc, heartAcc, hotShield, rei, reiV, xTent, starBit, goldenTip;
    public int reiBoostCool, reiBoostT, xTentCool, goldenTipI;
    public override void ResetEffects()
    {
        reiBoostCool--;
        xTentCool--;
        if (reiBoostCool > 0)
            reiBoostT--;
        if (!goldenTip)
            goldenTipI = 0;
        rei = false;
        reiV = false;
        hotShield = false;
        brainAcc = false;
        xTent = false;
        heartAcc = false;
        starBit = false;
        goldenTip = false;
    }
    public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (item.DamageType == DamageClass.Magic && xTent && xTentCool <= 0 && Main.myPlayer == Player.whoAmI)
        {
            Projectile p = Projectile.NewProjectileDirect(null, Player.Center, Helper.FromAToB(Player.Center, Main.MouseWorld) * 8, ProjectileType<XAmethystFriendly>(), 50, 0, Player.whoAmI);
            xTentCool = 60;
        }
        return base.Shoot(item, source, position, velocity, type, damage, knockback);
    }

    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (starBit && !target.friendly && hit.Crit && target.lifeMax > 10 && target.type != NPCID.TargetDummy)
        {
            int random = Main.rand.Next(1, 3);
            Vector2 spawnpos = new Vector2(0, Player.position.Y + 900);
            Vector2 pos = Vector2.Zero;
            if (random == 1)
                pos = new Vector2(Player.position.X + 1000, spawnpos.Y - Main.rand.Next(1, 1800));
            else
                pos = new Vector2(Player.position.X - 1000, spawnpos.Y - Main.rand.Next(1, 1800));
            Vector2 direction = Helper.FromAToB(pos, target.Center + target.velocity);

            Projectile.NewProjectile(Player.GetSource_FromThis(), pos, direction * 25, ModContent.ProjectileType<StarBitBlue>(), Player.HeldItem.damage * 2, 4f, Main.myPlayer);
        }
    }

    public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (starBit && !target.friendly && hit.Crit && target.lifeMax > 10 && target.type != NPCID.TargetDummy)
        {
            int random = Main.rand.Next(1, 3);
            Vector2 spawnpos = new Vector2(0, Player.position.Y + 900);
            Vector2 pos = Vector2.Zero;
            if (random == 1)
                pos = new Vector2(Player.position.X + 1000, spawnpos.Y - Main.rand.Next(1, 1800));
            else
                pos = new Vector2(Player.position.X - 1000, spawnpos.Y - Main.rand.Next(1, 1800));
            Vector2 direction = Helper.FromAToB(pos, target.Center + target.velocity);

            Projectile.NewProjectile(Player.GetSource_FromThis(), pos, direction * 25, ModContent.ProjectileType<StarBitBlue>(), Player.HeldItem.damage * 2, 4f, Main.myPlayer);
        }
    }
    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (reiBoostCool > 20)
        {
            modifiers.Cancel();
            Player.AddImmuneTime(ImmunityCooldownID.General, 40);
            Player.AddImmuneTime(ImmunityCooldownID.Bosses, 40);
            Player.AddImmuneTime(ImmunityCooldownID.DD2OgreKnockback, 40);
        }
        if (NPC.AnyNPCs(NPCType<TinyBrain>()))
        {
            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.active && npc.type == NPCType<TinyBrain>())
                {
                    npc.life = 0;
                    npc.checkDead();
                    break;
                }
            }
        }
    }
    public override void PostUpdateRunSpeeds()
    {
        if (rei)
        {
            Player.maxRunSpeed += .5f;
            Player.accRunSpeed += .5f;
        }
        if (hotShield)
            Player.CancelAllBootRunVisualEffects();
    }
    public override void PostUpdateMiscEffects()
    {
        if (hotShield)
        {
            Player.CancelAllBootRunVisualEffects();
            Player.accRunSpeed *= 0.7f;
            Player.moveSpeed *= 0.7f;
        }
    }
    public override void UpdateLifeRegen()
    {
        if (brainAcc)
            Player.lifeRegen += 5;
    }
    public override void PostUpdate()
    {
        if (hotShield)
        {
            Player.CancelAllBootRunVisualEffects();
        }
    }
}
