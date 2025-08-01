using EbonianMod.Projectiles.ArchmageX;
using EbonianMod.Projectiles.Friendly.Generic;
using System.IO;

namespace EbonianMod.Common;

public static class Packets
{
    public static ModPacket Write(MessageType type)
    {
        ModPacket packet = EbonianMod.Instance.GetPacket();
        packet.Write((byte)type);
        return packet;
    }
    public static void HandlePackets(BinaryReader reader)
    {
        MessageType msg = (MessageType)reader.ReadByte();
        switch (msg)
        {
            case MessageType.SpawnNPC:
                {
                    Vector2 position = reader.ReadVector2();
                    int type = reader.ReadInt32();
                    float ai0 = reader.ReadSingle();
                    float ai1 = reader.ReadSingle();
                    float ai2 = reader.ReadSingle();
                    float ai3 = reader.ReadSingle();
                    NPC.NewNPCDirect(NPC.GetSource_NaturalSpawn(), position, type, 0, ai0, ai1, ai2, ai3).netUpdate = true;

                }
                break;
            case MessageType.SpawnBoss:
                {
                    Vector2 position = reader.ReadVector2();
                    int type = reader.ReadInt32();
                    float ai0 = reader.ReadSingle();
                    float ai1 = reader.ReadSingle();
                    float ai2 = reader.ReadSingle();
                    float ai3 = reader.ReadSingle();
                    if (!NPC.AnyNPCs(type))
                        NPC.NewNPCDirect(NPC.GetSource_NaturalSpawn(), position, type, 0, ai0, ai1, ai2, ai3).netUpdate = true;
                }
                break;
            case MessageType.OnHitAccessory:
                {
                    OnHitAccessory(reader);
                }
                break;
        }
    }
    public static void OnHitAccessory(BinaryReader reader)
    {
        int type = reader.ReadInt32();
        switch (type) // 0: Xareus Tentacle, 1: Starbit
        {
            case 0:
                {
                    int index = reader.ReadInt32();
                    Vector2 mousePos = reader.ReadVector2();
                    Player player = Main.player[index];
                    Projectile p = Projectile.NewProjectileDirect(null, player.Center, Helper.FromAToB(player.Center, mousePos) * 8, ProjectileType<XAmethyst>(), 50, 0, player.whoAmI);
                    p.DamageType = DamageClass.Magic;
                    p.friendly = true;
                    p.hostile = false;
                    p.SyncProjectile();
                }
                break;

            case 1:
                {
                    int index = reader.ReadInt32();
                    Vector2 pos = reader.ReadVector2();
                    Vector2 pos2 = reader.ReadVector2();
                    Player player = Main.player[index];

                    Vector2 direction = Helper.FromAToB(pos, pos2);
                    Projectile.NewProjectile(null, pos, direction * 25, ModContent.ProjectileType<StarBitBlue>(), player.HeldItem.damage * 2, 4f, player.whoAmI);
                }
                break;
        }
    }
}
public enum MessageType
{
    SpawnNPC,
    SpawnBoss,
    OnHitAccessory
}
