using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonianMod.NPCs.Overworld.Botanist;
public class Botanist : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 3;
    }
    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (spawnInfo.Player.ZoneOverworldHeight && spawnInfo.Player.ZoneForest)
            return .07f;
        else
            return 0;
    }
    public override void SetDefaults()
    {
        NPC.width = 40;
        NPC.height = 22;
        NPC.damage = 10;
        NPC.defense = 3;
        NPC.lifeMax = 60;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath2;
        NPC.value = 30;
        NPC.knockBackResist = 0.1f;
        NPC.aiStyle = -1;
        NPC.noGravity = false;
        NPC.noTileCollide = false;
        NPC.hide = true;
    }
    private const int Idle = 0;
    private const int Walk = 1;
    private const int Attack = 2;
    public float AIState
    {
        get => NPC.ai[0];
        set => NPC.ai[0] = value;
    }
    public float AITimer
    {
        get => NPC.ai[1];
        set => NPC.ai[1] = value;
    }
    public float AITimer2
    {
        get => NPC.ai[2];
        set => NPC.ai[2] = value;
    }
    Vector2 headOffset;
    float headRotation;
    public override void DrawBehind(int index)
    {
        Main.instance.DrawCacheNPCsMoonMoon.Add(index);
    }
    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D neck = ExtraSpriteTextures.BotanistNeck.Value;
        Texture2D head = ExtraSpriteTextures.BotanistHead.Value;

        Vector2 baseC = NPC.Center + new Vector2(0, 6);
        Vector2 center = baseC;
        Vector2 distVector = headOffset;
        float distance = distVector.Length();
        int attempts = 0;
        while (distance > (neck.Height / 3) + 5 && !float.IsNaN(distance) && ++attempts < 400)
        {
            distVector.Normalize();
            distVector *= (neck.Height / 3) * 0.65f;
            center += distVector;
            distVector = baseC + headOffset - center;
            distance = distVector.Length();
            int frameY = 6 * (attempts % 3);
            spriteBatch.Draw(neck, center - Main.screenPosition, new Rectangle(0, frameY, 10, 6), NPC.HunterPotionColor(Lighting.GetColor((center).ToTileCoordinates())), distVector.ToRotation() + PiOver2, neck.Size() / 2, NPC.scale, SpriteEffects.None, 0);
        }

        spriteBatch.Draw(TextureAssets.Npc[Type].Value, NPC.Center + new Vector2(0, 4 + (NPC.frame.Y == 24 ? -1 : 0)) - screenPos, NPC.frame, NPC.HunterPotionColor(drawColor), NPC.rotation, NPC.Size / 2, NPC.scale, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

        spriteBatch.Draw(head, center - screenPos + new Vector2(NPC.direction, (NPC.frame.Y == 24 ? -1 : 0)).RotatedBy(headRotation), null, NPC.HunterPotionColor(Lighting.GetColor((NPC.Center + headOffset).ToTileCoordinates())), headRotation, head.Size() / 2, NPC.scale, NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        return false;
    }
    public override void FindFrame(int frameHeight)
    {
        if (MathF.Abs(NPC.velocity.X) > .1f)
        {
            if (++NPC.frameCounter % 5 == 0)
            {
                if (NPC.frame.Y < frameHeight * 2)
                    NPC.frame.Y += frameHeight;
                else
                    NPC.frame.Y = 0;
            }
        }
        else
            NPC.frame.Y = 0;
    }
    public override void AI()
    {
        Player player = Main.player[NPC.target];
        headOffset = NPC.Center.FromAToB(Main.MouseWorld, false);
        headRotation = headOffset.ToRotation() + PiOver2;
        if (AIState == Idle)
        {
            headOffset = new Vector2(0, -8);
            NPC.TargetClosest(true);
            if (NPC.HasValidTarget)
            {
                if (NPC.Distance(player.Center) < 100 || NPC.life < NPC.lifeMax)
                {
                    AIState = Walk;
                    AITimer = 0;
                }
            }
        }
        else if (AIState == Walk)
        {
            headRotation = Utils.AngleLerp(headRotation, 0, 0.1f);
            headOffset = Vector2.Lerp(headOffset, new Vector2(0, -8), 0.1f);
            NPC.damage = 0;
            AITimer2++;
            AITimer = MathHelper.Clamp(AITimer, 0, 500);
            if (AITimer2 % 7 == 0)
                NPC.direction = NPC.Center.X < player.Center.X ? 1 : -1;

            NPC.spriteDirection = NPC.direction;

            if (player.Center.Distance(NPC.Center) > 100 && (player.Center.Y - NPC.Center.Y < 100 || player.Center.Y - NPC.Center.Y > -100))
                NPC.velocity.X = Lerp(NPC.velocity.X, NPC.direction * 1.7f, 0.1f);
            if (player.Center.Distance(NPC.Center) < 100)
            {
                NPC.netUpdate = true;
                AITimer += 4;
                NPC.velocity.X *= 0.9f;
            }
            if (MathF.Abs(player.Center.Y - NPC.Center.Y) > 100)
            {
                AITimer--;
            }

            Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY, 1, false, 0);

            if (NPC.collideY && NPC.collideX)
            {
                NPC.velocity.Y = -3;
            }

            if (AITimer >= 100)
            {
                NPC.netUpdate = true;
                NPC.velocity.X = 0;
                //AIState = Attack;
                AITimer = 0;
            }
        }
    }
}
