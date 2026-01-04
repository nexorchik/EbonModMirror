using EbonianMod.Content.Projectiles.ArchmageX;
using EbonianMod.Content.Projectiles.Friendly.Generic;
using System.IO;

namespace EbonianMod.Core;

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
        }
    }
}
public enum MessageType : byte
{
    SpawnNPC,
    SpawnBoss
}
