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
    public bool rolleg, ToxicGland;
    public bool doomMinion, xMinion, cClawMinion, titteringMinion, sudamaMinion;
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
    }
    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (Player.HasBuff<Sheepened>())
        {
            modifiers.DisableSound();
            SoundEngine.PlaySound(SoundID.NPCHit1, Player.Center);
        }
    }
    public override void PreUpdateMovement()
    {
        if (fleshformators > 0)
        {
            Player.controlUseItem = false;
            Player.controlUseTile = false;
            Player.controlThrow = false;
            Player.gravDir = 1f;
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
    }
    public override void PostUpdateMiscEffects()
    {
        consistentTimer++;
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
    }
    public override void PostUpdate()
    {
        if (NPC.AnyNPCs(NPCType<ArchmageX>()) || Player.ownedProjectileCounts[ProjectileType<ArchmageXSpawnAnim>()] > 0)
            Player.noBuilding = true;
    }
}
