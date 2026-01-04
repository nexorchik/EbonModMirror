using EbonianMod.Items.Materials;
using EbonianMod.Items.Misc;
using EbonianMod.Items.Misc.Critters;
using Humanizer;
using System.IO;
using Terraria.GameContent.Bestiary;

namespace EbonianMod.NPCs.Overworld.Critters;

public class Ram : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 6;
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
            new FlavorTextBestiaryInfoElement("Mods.EbonianMod.NPCs.Ram.Bestiary"),
        });
    }
    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ItemType<Wool>(), 4, 1, 3));
    }
    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Bunny);
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.width = 38;
        NPC.height = 28;
        NPC.catchItem = ItemType<RamItem>();
        NPC.Size = new Vector2(38, 28);
    }
    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(sheared);
        writer.Write(dyeId);
    }
    public override void ReceiveExtraAI(BinaryReader reader)
    {
        sheared = reader.ReadBoolean();
        dyeId = reader.ReadInt32();
    }
    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return spawnInfo.Player.ZoneForest ? 0.1f : 0;
    }

    public override bool CheckDead()
    {
        if (Main.dedServ)
            return base.CheckDead();
        
        if (!sheared)
        {
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, Main.rand.NextVector2Circular(1, 1), Find<ModGore>("EbonianMod/RamGore0").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, Main.rand.NextVector2Circular(1, 1), Find<ModGore>("EbonianMod/RamGore1").Type, NPC.scale);
        }
        Gore.NewGore(NPC.GetSource_Death(), NPC.position, Main.rand.NextVector2Circular(1, 1), Find<ModGore>("EbonianMod/RamGore2").Type, NPC.scale);

        for (int i = 0; i < 4; i++)
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, Main.rand.NextVector2Circular(1, 1), Find<ModGore>("EbonianMod/RamGore3").Type, NPC.scale);

        for (int i = 0; i < 50; i++)
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, Main.rand.NextFloatDirection(), Main.rand.NextFloatDirection());
        
        return base.CheckDead();
    }
    
    int dyeId = -1, lastClicked = 0;
    bool sheared;
    public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        if ((item.type == ItemType<Shears>() || item.type == ItemID.StylistKilLaKillScissorsIWish))
        {
            modifiers.FinalDamage *= 0;
        }
    }
    public override bool? CanBeHitByItem(Player player, Item item)
    {
        if (sheared && (item.type == ItemType<Shears>() || item.type == ItemID.StylistKilLaKillScissorsIWish))
            return false;
        return base.CanBeHitByItem(player, item);
    }
    bool spawn;
    public override void AI()
    {
        NPC.spriteDirection = NPC.direction;
        if (!spawn)
        {
            if (NPC.ai[2] > 0)
            {
                sheared = true;
                NPC.ai[2] = 0;
            }
            else if (Main.netMode < 1)
            {
                WeightedRandom<int> dye = new();
                dye.Add(ItemID.PinkDye, 0.01f);
                dye.Add(ItemID.NegativeDye, 0.001f);
                dye.Add(ItemID.BlackDye);
                dye.Add(ItemID.BlueDye, 0.1f);
                dye.Add(ItemID.BrownDye);
                dye.Add(ItemID.YellowDye, 0.3f);
                dye.Add(ItemID.BrightSilverDye);
                dye.Add(ItemID.GreenDye, 0.2f);
                dye.Add(ItemID.ShadowDye);
                dye.Add(ItemID.BrightBrownDye);
                dye.Add(ItemID.BrownAndBlackDye);
                dye.Add(ItemID.BrownAndSilverDye);
                dye.Add(ItemID.SkyBlueDye, 0.5f);
                dye.Add(ItemID.OrangeandSilverDye);
                dye.Add(ItemID.ReflectiveGoldDye, 0.025f);
                dye.Add(-1, 8);
                dyeId = dye;

                string name = Main.LocalPlayer.name;
                name.ApplyCase(LetterCasing.LowerCase);
                if (name == "jeb" || name == "jeb_")
                    dyeId = ItemID.LivingRainbowDye;
            }

            if (Main.rand.NextBool(4) && NPC.Center.Distance(Main.LocalPlayer.Center) < 600)
                SoundEngine.PlaySound(EbonianSounds.sheep, NPC.Center);
            NPC.netUpdate = true;
            spawn = true;
        }
        lastClicked--;
        if (Main.rand.NextBool(2000) && NPC.Center.Distance(Main.LocalPlayer.Center) < 600)
            SoundEngine.PlaySound(EbonianSounds.sheep.WithVolumeScale(0.35f), NPC.Center);
        if (Main.netMode < 1 && Main.LocalPlayer.Center.Distance(NPC.Center) < 175 && new Rectangle((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 5, 5).Intersects(NPC.getRect()) && Main.mouseRight && lastClicked < 0)
        {
            if (Main.LocalPlayer.HeldItem.dye > 0 && dyeId != Main.LocalPlayer.HeldItem.type && !sheared)
            {
                dyeId = Main.LocalPlayer.HeldItem.type;
                SoundEngine.PlaySound(SoundID.Item176, NPC.Center);
                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Smoke);
                    Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.ShimmerSpark);
                }
                NPC.netUpdate = true;
            }
        }
        foreach (Player player in Main.ActivePlayers)
        {
            if ((player.HeldItem.type == ItemType<Shears>() || player.HeldItem.type == ItemID.StylistKilLaKillScissorsIWish) && player.Center.Distance(NPC.Center) < 50 && player.itemAnimation > 0)
            {
                if (!sheared)
                {
                    sheared = true;
                    Item.NewItem(null, NPC.getRect(), ItemType<Wool>(), Main.rand.Next(2, 5));
                    SoundEngine.PlaySound(EbonianSounds.shears, NPC.Center);
                    SoundEngine.PlaySound(EbonianSounds.sheep, NPC.Center);
                    for (int i = 0; i < 30; i++)
                    {
                        Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Smoke);
                        Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Silk);
                    }
                    NPC.netUpdate = true;
                }
            }
        }
        if (sheared && NPC.catchItem != ItemType<RamItemNaked>())
        {
            NPC.catchItem = ItemType<RamItemNaked>();
        }
        if (Main.netMode == 0)
            Collision.StepDown(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
    }
    public override void PostAI()
    {
        if (Main.LocalPlayer.Center.Distance(NPC.Center) < 175 && new Rectangle((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y, 5, 5).Intersects(NPC.getRect()) && Main.mouseRight && lastClicked < 0)
            lastClicked = 30;
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (NPC.IsABestiaryIconDummy || !spawn) return true;
        Texture2D tex = TextureAssets.Npc[Type].Value;
        if (sheared)
            tex = Assets.ExtraSprites.Overworld.Ram_Naked.Value;

        string name = Main.LocalPlayer.name;
        name.ApplyCase(LetterCasing.LowerCase);
        spriteBatch.Draw(tex, NPC.Center + new Vector2(0, NPC.gfxOffY + 2) - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, NPC.Size / 2, NPC.scale, (NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally) | (name == "dinnerbone" || name == "grumm" ? SpriteEffects.FlipVertically : SpriteEffects.None), 0);

        return false;
    }
    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (NPC.IsABestiaryIconDummy || sheared || !spawn || Main.netMode > 0) return;
        Texture2D tex = Assets.ExtraSprites.Overworld.Ram_Wool.Value;
        if (dyeId > 0 && !sheared)
        {
            string name = Main.LocalPlayer.name;
            name.ApplyCase(LetterCasing.LowerCase);
            DrawData data = new(tex, NPC.Center + new Vector2(0, NPC.gfxOffY + 2) - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, NPC.Size / 2, NPC.scale, (NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally) | (name == "dinnerbone" || name == "grumm" ? SpriteEffects.FlipVertically : SpriteEffects.None));
            MiscDrawingMethods.DrawWithDye(spriteBatch, data, dyeId, NPC);
        }
    }
    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter++;
        if (!NPC.velocity.Y.InRange(0, 0.2f))
        {
            NPC.frame.Y = frameHeight * 4;
        }
        else
        {
            if (NPC.velocity.X.InRange(0, 0.05f))
            {
                NPC.frame.Y = 0;
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
