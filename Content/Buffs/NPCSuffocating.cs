using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EbonianMod.Content.Buffs
{
    public class NPCSuffocating : ModBuff
    {
        public override string Texture => "Terraria/Images/Buff_68";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = false;
        }
        public override void Update(NPC NPC, ref int buffIndex)
        {
            if (NPC.lifeRegen > 0)
                NPC.lifeRegen = 0;

            NPC.lifeRegen -= 75;
        }
    }
}