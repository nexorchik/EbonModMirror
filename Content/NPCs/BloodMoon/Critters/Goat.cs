using System;
using System.IO;
using EbonianMod.Content.Items.Critters;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI;

namespace EbonianMod.Content.NPCs.BloodMoon.Critters;

public class Goat : ModNPC
{
	public override string Texture => Helper.AssetPath + "NPCs/BloodMoon/Critters/" + Name;
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[Type] = 8;
		NPCID.Sets.CountsAsCritter[Type] = true;
		NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
		NPCID.Sets.TownCritter[Type] = true;
		Main.npcCatchable[Type] = true;
		NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers()
		{
			Direction = 1
		});
	}
	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
			new FlavorTextBestiaryInfoElement("Mods.EbonianMod.Misc.Types.Animal"),
			new FlavorTextBestiaryInfoElement("Mods.EbonianMod.NPCs.Goat.Bestiary"),
		});
	}

	public override void SetDefaults()
	{
		NPC.CloneDefaults(NPCID.Bunny);
		NPC.Size = new Vector2(42, 38);
		NPC.HitSound = SoundID.NPCHit1;
		NPC.DeathSound = SoundID.NPCDeath1;
		NPC.width = 38;
		NPC.height = 28;
		NPC.catchItem = ItemType<SheepItem>();
	}
	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		return spawnInfo.Player.ZoneForest && Main.bloodMoon ? 0.1f : 0;
	}
	public override bool CheckDead()
	{
		if (Main.dedServ)
			return base.CheckDead();
        
		Gore.NewGore(NPC.GetSource_Death(), NPC.position, Main.rand.NextVector2Circular(1, 1), Find<ModGore>("EbonianMod/SheepGore2").Type, NPC.scale);

		for (int i = 0; i < 4; i++)
			Gore.NewGore(NPC.GetSource_Death(), NPC.position, Main.rand.NextVector2Circular(1, 1), Find<ModGore>("EbonianMod/SheepGore3").Type, NPC.scale);

		for (int i = 0; i < 50; i++)
			Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, Main.rand.NextFloatDirection(), Main.rand.NextFloatDirection());
        
		return base.CheckDead();
	}

	public override void SendExtraAI(BinaryWriter writer)
	{
		writer.Write(screamTimer);
		writer.Write(evilTimer);
		writer.Write(screaming);
	}

	public override void ReceiveExtraAI(BinaryReader reader)
	{
		screamTimer = reader.ReadInt32();
		evilTimer = reader.ReadInt32();
		screaming = reader.ReadBoolean();
	}

	private bool screaming;
	private int screamTimer, evilTimer;

	public override void EmoteBubblePosition(ref Vector2 position, ref SpriteEffects spriteEffects)
	{
		position.X += NPC.direction*18;
		position.Y += 2;
	}

	public override void AI()
	{
		if (--evilTimer <= 0 && NPC.life < NPC.lifeMax)
		{
			EmoteBubble.NewBubble(EmoteID.EmotionAnger, new WorldUIAnchor(NPC), 50);
			evilTimer = 700;
		}

		if (evilTimer > 0)
		{
			NPC.TargetClosest();
			foreach (Player player in Main.ActivePlayers)
			{
				if (player.Distance(NPC.Center) < 20 && NPC.velocity.Length() > 2f)
					player.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), 0, NPC.direction, false, false, false, 0);
			}
			if (evilTimer % 120 == 0)
				NPC.velocity = NPC.DirectionTo(Main.player[NPC.target].Center) * 5 + new Vector2(0, -5);
		}
		
		NPC.spriteDirection = -NPC.direction;
		if (Main.rand.NextBool(2100) && evilTimer< -100 && MPUtils.NotMPClient && screamTimer <= -40) 
			foreach (Player player in Main.ActivePlayers)
				if (NPC.Center.Distance(player.Center) < 600)
				{
					SoundEngine.PlaySound(Sounds.goat.WithVolumeScale(0.35f), NPC.Center);
					screaming = true;
					screamTimer = 0;
					NPC.netUpdate = true;
				}

		if (screaming)
		{
			NPC.velocity.X *= 0f;
			screamTimer++;
			if (screamTimer > 60)
			{
				screaming = false;
				NPC.netUpdate = true;
			}
		}
		else if (screamTimer > 0)
			screamTimer = 0;
		else screamTimer--;
		
		if (Main.netMode == 0)
			Collision.StepDown(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
	}
	public override void FindFrame(int frameHeight)
	{
		NPC.frameCounter++;
		if (!NPC.velocity.Y.InRange(0, 0.2f))
		{
			NPC.frame.Y = frameHeight * 4;
			if (evilTimer > 0 && MathF.Sign(NPC.velocity.X) == NPC.direction)
				NPC.frame.Y = frameHeight * 7;
		}
		else
		{
			if (NPC.velocity.X.InRange(0, 0.05f))
			{
				NPC.frame.Y = 0;
				if (screaming)
					NPC.frame.Y = (NPC.frameCounter % 10 < 5 ? 5 : 6) * frameHeight;
			}
			else
			{
				if (NPC.frameCounter % 5 == 0)
				{
					if (NPC.frame.Y < frameHeight * 3)
						NPC.frame.Y += frameHeight;
					else
						NPC.frame.Y = 0;
				}
			}
		}
	}
}