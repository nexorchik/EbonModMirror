using EbonianMod.Buffs;
using EbonianMod.Items.Accessories;
using EbonianMod.Items.Consumables.Food;
using EbonianMod.Items.Weapons.Melee;
using EbonianMod.NPCs.ArchmageX;
using EbonianMod.NPCs.Aureus;
using EbonianMod.NPCs.Crimson.Fleshformator;
using EbonianMod.Projectiles;
using EbonianMod.Projectiles.ArchmageX;
using System;
using Terraria;
using Terraria.Graphics.Effects;


namespace EbonianMod;

public class EbonianPlayer : ModPlayer
{
    public int timeSlowProgress, timeSlowMax, fleshformators, consistentTimer;
    public static EbonianPlayer Instance;
    public Vector2 stabDirection;
    public bool rolleg, ToxicGland, sheep;
    public bool doomMinion, xMinion, cClawMinion, titteringMinion, sudamaMinion;
    public override void HideDrawLayers(PlayerDrawSet drawInfo)
    {
        if (sheep)
            foreach (PlayerDrawLayer l in PlayerDrawLayerLoader.Layers)
                l.Hide();
    }
    public override void ResetEffects()
    {

        if (NPC.AnyNPCs(NPCType<ArchmageX>()) || Player.ownedProjectileCounts[ProjectileType<ArchmageXSpawnAnim>()] > 0)
            Player.noBuilding = true;
        rolleg = false;
        doomMinion = false;
        xMinion = false;
        sudamaMinion = false;
        titteringMinion = false;
        cClawMinion = false;
        if (!NPC.AnyNPCs(NPCType<Fleshformator>()))
            fleshformators = 0;
        ToxicGland = false;
        sheep = false;
    }
    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (Player.HasBuff<Sheepened>())
        {
            modifiers.DisableSound();
            SoundEngine.PlaySound(SoundID.NPCHit1, Player.Center);
        }
    }
    public override void PostUpdateRunSpeeds()
    {
        if (NPC.AnyNPCs(NPCType<ArchmageX>()) || Player.ownedProjectileCounts[ProjectileType<ArchmageXSpawnAnim>()] > 0)
            Player.noBuilding = true;
        if (Player.HeldItem.type == ItemType<EbonianScythe>() && !Player.ItemTimeIsZero)
        {
            Player.maxRunSpeed += 2;
            Player.accRunSpeed += 2;
        }
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
    public override void PostUpdateMiscEffects()
    {
        consistentTimer++;
        Player.ManageSpecialBiomeVisuals("EbonianMod:XMartian", NPC.AnyNPCs(NPCType<ArchmageCutsceneMartian>()));
        Player.ManageSpecialBiomeVisuals("EbonianMod:Conglomerate", Player.HasBuff<ConglomerateEnergyBuff>()); // add npc here later when we add conglo
        Player.ManageSpecialBiomeVisuals("EbonianMod:Aureus", NPC.AnyNPCs(NPCType<Aureus>()));
        #region "hell stuff"
        Player.ManageSpecialBiomeVisuals("EbonianMod:HellTint", Player.ZoneUnderworldHeight);
        #endregion
    }

    public override void OnEnterWorld()
    {
        Instance = Player.GetModPlayer<EbonianPlayer>();
    }
    public override void PostUpdateBuffs()
    {

        if (NPC.AnyNPCs(NPCType<ArchmageX>()) || Player.ownedProjectileCounts[ProjectileType<ArchmageXSpawnAnim>()] > 0)
            Player.noBuilding = true;
        if (sheep)
        {
            Player.height = Player.width;
            Player.position.Y += Player.width + 2;
            foreach (Projectile proj in Main.ActiveProjectiles)
            {
                if (proj.active && proj.type == ProjectileType<player_sheep>())
                {
                    proj.Center = Player.Bottom + new Vector2(0, -14);
                    if (Player.velocity.Y < 0)
                        Collision.StepUp(ref proj.position, ref Player.velocity, proj.width, proj.height, ref proj.stepSpeed, ref proj.gfxOffY);
                    else
                        Collision.StepDown(ref proj.position, ref Player.velocity, proj.width, proj.height, ref proj.stepSpeed, ref proj.gfxOffY);

                }
            }
        }
    }
    public override void PostUpdate()
    {
        if (NPC.AnyNPCs(NPCType<ArchmageX>()) || Player.ownedProjectileCounts[ProjectileType<ArchmageXSpawnAnim>()] > 0)
            Player.noBuilding = true;
    }
}
