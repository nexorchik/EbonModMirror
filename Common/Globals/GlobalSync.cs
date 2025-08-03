using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader.IO;

namespace EbonianMod.Common.Globals;
public class GlobalSync : GlobalProjectile
{
    public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter writer)
    {
        if (projectile.ModProjectile?.Mod.Name == "EbonianMod")
        {
            writer.Write((byte)projectile.extraUpdates);
            writer.Write((short)projectile.aiStyle);

            for (int i = 0; i < projectile.localAI.Length; i++)
                writer.Write(projectile.localAI[i]);

            writer.Write(projectile.hostile);
            writer.Write(projectile.friendly);
            writer.Write(projectile.tileCollide);
        }
    }
    public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader reader)
    {
        if (projectile.ModProjectile?.Mod.Name == "EbonianMod")
        {
            projectile.extraUpdates = reader.ReadByte();
            projectile.aiStyle = reader.ReadInt16();

            for (int i = 0; i < projectile.localAI.Length; i++)
                projectile.localAI[i] = reader.ReadSingle();

            projectile.hostile = reader.ReadBoolean();
            projectile.friendly = reader.ReadBoolean();
            projectile.tileCollide = reader.ReadBoolean();
        }
    }
}

public class GlobalSyncNPC : GlobalNPC
{
    public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter writer)
    {
        if (npc.ModNPC?.Mod.Name == "EbonianMod")
        {
            for (int i = 0; i < npc.localAI.Length; i++)
                writer.Write(npc.localAI[i]);

            writer.Write(npc.noGravity);
            writer.Write(npc.noTileCollide);
            writer.Write(npc.dontTakeDamage);
            writer.Write(npc.immortal);
        }
    }
    public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader reader)
    {
        if (npc.ModNPC?.Mod.Name == "EbonianMod")
        {
            for (int i = 0; i < npc.localAI.Length; i++)
                npc.localAI[i] = reader.ReadSingle();

            npc.noGravity = reader.ReadBoolean();
            npc.noTileCollide = reader.ReadBoolean();
            npc.dontTakeDamage = reader.ReadBoolean();
            npc.immortal = reader.ReadBoolean();
        }
    }
}