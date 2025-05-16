namespace EbonianMod.Common.Utilities;

public static class MPUtils
{
    public static bool NotMPClient => Main.netMode != NetmodeID.MultiplayerClient;
    /// <summary>
    /// Avoids duplicate projectiles in multiplayer.
    /// <br/> <strong>Returns <c>null</c> on multiplayer clients</strong> 
    /// </summary>
    public static Projectile NewProjectile(IEntitySource source, Vector2 position, Vector2 velocity,
        int type, int damage, float knockback, int owner = -1, float ai0 = 0, float ai1 = 0, float ai2 = 0)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient) return null;

        return Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, owner, ai0, ai1, ai2);
    }
    public static void NewNPC(Vector2 position, int type, bool noDupes = false, float ai0 = 0, float ai1 = 0, float ai2 = 0, float ai3 = 0)
    {
        if (!NotMPClient)
        {
            ModPacket packet = EbonianNetCode.Write(noDupes ? MessageType.SpawnBoss : MessageType.SpawnNPC);
            packet.WriteVector2(position);
            packet.Write(type);
            packet.Write(ai0);
            packet.Write(ai1);
            packet.Write(ai2);
            packet.Write(ai3);
            packet.Send();
        }
        else if (!Main.dedServ)
        {
            NPC.NewNPCDirect(NPC.GetSource_NaturalSpawn(), position, type, 0, ai0, ai1, ai2, ai3);
        }
    }
    public static void SyncProjectile(int index) => NetMessage.SendData(MessageID.SyncProjectile, number: index);
    public static void SyncProjectile(this Projectile projectile) => NetMessage.SendData(MessageID.SyncProjectile, number: projectile.whoAmI);
    public static void SyncNPC(int index) => NetMessage.SendData(MessageID.SyncNPC, number: index);
    public static void SyncNPC(this NPC npc) => NetMessage.SendData(MessageID.SyncNPC, number: npc.whoAmI);
}
