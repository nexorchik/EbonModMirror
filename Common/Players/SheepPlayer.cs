using EbonianMod.Content.Projectiles.ArchmageX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.Common.Players;
public class SheepPlayer : ModPlayer
{
    public bool sheep;
    public override void ResetEffects() => sheep = false;
    public override void HideDrawLayers(PlayerDrawSet drawInfo)
    {
        if (sheep)
            foreach (PlayerDrawLayer l in PlayerDrawLayerLoader.Layers)
                l.Hide();
    }
    public override void PostUpdateRunSpeeds()
    {
        if (sheep)
        {
            Player.wingTimeMax = -1;
            Player.wingTime = -1;
            Player.wingsLogic = -1;
            Player.wings = -1;
            Player.mount.Dismount(Player);
            Player.gravity = Player.defaultGravity;
            Player.maxRunSpeed = 4.2f;
            Player.accRunSpeed = 4.2f;
            Player.jumpSpeed = 6.1f;
            Player.jumpHeight = 26;
            Player.dashType = 0;
            Player.channel = false;
            Player.blockExtraJumps = true;
        }
    }
    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (sheep)
        {
            modifiers.DisableSound();
            SoundEngine.PlaySound(SoundID.NPCHit1, Player.Center);
        }
    }
    public override bool CanStartExtraJump(ExtraJump jump)
    {
        if (sheep)
            return false;
        return base.CanStartExtraJump(jump);
    }
    public override bool CanUseItem(Item item)
    {
        if (sheep)
            return false;
        return base.CanUseItem(item);
    }
    public override void PostUpdate() => PreUpdate();
    public override void PreUpdate()
    {
        if (sheep)
        {
            Player.height = Player.width;
            Player.position.Y += Player.width + 2;
            foreach (Projectile proj in Main.ActiveProjectiles)
            {
                if (proj.active && proj.type == ProjectileType<SheepeningPlayerProjectile>() && proj.owner == Player.whoAmI)
                {
                    proj.Center = Player.Bottom + new Vector2(0, -14);
                    Vector2 velLocal = Player.velocity;
                    if (Player.velocity.Y < 0)
                        Collision.StepUp(ref proj.position, ref velLocal, proj.width, proj.height, ref proj.stepSpeed, ref proj.gfxOffY);
                    else
                        Collision.StepDown(ref proj.position, ref velLocal, proj.width, proj.height, ref proj.stepSpeed, ref proj.gfxOffY);

                }
            }
        }
    }
}
