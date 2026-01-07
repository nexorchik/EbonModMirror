using System;
using EbonianMod.Common.Globals;
using EbonianMod.Common.Misc;
using Terraria.GameContent.Bestiary;

namespace EbonianMod.Content.NPCs.BloodMoon;

public class Cryptid : CommonNPC
{
	public override string Texture => Helper.AssetPath + "NPCs/BloodMoon/" + Name;

	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[Type] = 9;
	}
	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		if (Main.invasionType > 0) return 0;
		return Main.hardMode && spawnInfo.Player.ZoneCrimson && spawnInfo.Player.ZoneOverworldHeight ? 0.07f : 0;
	}
	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
			new FlavorTextBestiaryInfoElement("Mods.EbonianMod.Misc.Types.BigBloodMoon"),
			new FlavorTextBestiaryInfoElement(NPC.BestiaryKey()),
		});
	}

	public override void SetDefaults()
	{
		NPC.Size = new Vector2(56, 86);
		NPC.aiStyle = -1;
		NPC.damage = 30;
		NPC.defense = 4;
		NPC.lifeMax = 750;
		NPC.value = Item.buyPrice(0, 0, 15, 0);
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath1;
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Texture2D tex = TextureAssets.Npc[Type].Value;
		Texture2D glow = Assets.NPCs.BloodMoon.Cryptid_Glow.Value;
		
		spriteBatch.Draw(tex, NPC.Center + gfxOff + new Vector2(0, 2) - screenPos, NPC.frame, NPC.HunterPotionColor(drawColor), NPC.rotation, NPC.Size * 0.5f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		spriteBatch.Draw(glow, NPC.Center + gfxOff + new Vector2(0, 2) - screenPos, NPC.frame, Color.White, NPC.rotation, NPC.Size * 0.5f, NPC.scale, NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		return false;
	}

	enum States
	{
		Walk
	}

	public override void AI()
	{
		Lighting.AddLight(NPC.Center, Color.Gold.ToVector3()*0.2f);
		NPC.TargetClosest(false);
		
		Player player = Main.player[NPC.target];
		float distanceToPlayer = player.Distance(NPC.Center);
		switch ((States)AIState)
		{
			case States.Walk:
			{
				NPC.FaceTarget();
				NPC.damage = 0;

				if (MathF.Abs(player.Center.X - NPC.Center.X) > 40)
					NPC.GetGlobalNPC<FighterGlobalAI>().FighterAI(NPC, 5, 2.5f);
				else
					NPC.velocity.X *= 0;

				if (distanceToPlayer < 400)
					AITimer++;
				if (distanceToPlayer < 70)
					AITimer += 2;
				if (distanceToPlayer < 40)
					AITimer += 2;
			}
				break;
		}
	}

	public override void FindFrame(int frameHeight)
	{
		if (MathF.Abs(NPC.velocity.Y) > 0.05f)
		{
			NPC.frame.Y = 2 * frameHeight;
			return; 
		}

		if (++NPC.frameCounter % 5 != 0)
			return;

		if (MathF.Abs(NPC.velocity.X) > 0.05f)
		{
			if (NPC.frame.Y < frameHeight)
				NPC.frame.Y = frameHeight;	
			
			if ((NPC.frame.Y += frameHeight) > 8 * frameHeight)
				NPC.frame.Y = frameHeight;
		}
		else
			NPC.frame.Y = 0;
	}
}