using EbonianMod.Content.NPCs.ArchmageX;

namespace EbonianMod.Common.Players;

public class ArenaPlayer : ModPlayer
{
    public bool InXArena => NPC.AnyNPCs(NPCType<ArchmageX>()) ||
                           Player.ownedProjectileCounts[ProjectileType<ArchmageXSpawnAnim>()] > 0;
    void RestrictBuilding(bool condition) {
        if (condition)
            Player.noBuilding = true;
    }
    
    public override void ResetEffects() => RestrictBuilding(InXArena);

    public override void PostUpdateRunSpeeds() => RestrictBuilding(InXArena);
    
    public override void PostUpdateBuffs() => RestrictBuilding(InXArena);
    
    public override void PostUpdate() => RestrictBuilding(InXArena);
}