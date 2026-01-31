using EbonianMod.Content.Projectiles.Enemy.BloodMoon;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Threading;
using Terraria;
using Terraria.GameContent.Bestiary;

public class Moonborn : ModNPC
{
    public override string Texture => Helper.AssetPath + "NPCs/BloodMoon/Moonborn";

    public override void SetDefaults()
    {
        NPC.Size = new Vector2(60, 80);
        //NPC.HitSound = SoundID.Item49;
        //NPC.DeathSound = SoundID.Item27;
        NPC.damage = 1;
        NPC.defense = 0;
        NPC.lifeMax = 20;
        NPC.noTileCollide = true;
        NPC.noGravity = true;
    }

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 8;
    }
    public class Leg
    {
        public Vector2 RawPosition, Position, TargetPosition;
        public Vector2 WorldTargetPosition, WorldJointPosition;
        public float MaxDistance, VerticalOffset;
        public bool DefaultState, IsMoving;
    }
    bool Airborne;
    public Leg[] Legs = new Leg[6];
    public override void OnSpawn(IEntitySource source)
    {
        for (int i = 0, j = 1; i < 6; i++, j *= -1)
        {
            Legs[i] = new Leg();
        }
    }

    public override void AI()
    {
        NPC.TargetClosest(true);
        Player player = Main.player[NPC.target];

        Helper.RaycastData heightHit = Helper.Raycast(NPC.Center, Vector2.UnitY, 110, true);
        Airborne = heightHit.Success;
        if (Airborne)
        {
            float distanceToPlayer = MathF.Abs(NPC.Center.X - player.Center.X);
            float movementDirection = player.Center.X > NPC.Center.X ? 1 : -1;

            if (heightHit.RayLength > 16)
            {
                NPC.Center = Vector2.Lerp(NPC.Center, new Vector2(NPC.Center.X, heightHit.Point.Y - 100), 0.23f);
            }
            else
            {
                if(player.Center.Y < NPC.Center.Y + 100) NPC.Center -= new Vector2(0, 5);
                if (player.Center.Y > NPC.Center.Y + 100) NPC.Center += new Vector2(0, 5);

                //  NOTE: cannot use "else" here. Inclusion of "player.Center.Y == NPC.Center.Y + 100" case creates jitter  //
            }

            if (NPC.velocity.Y < 0.4f)
            {
                NPC.velocity.Y = 0;
                if (distanceToPlayer > 150 || MathF.Abs(NPC.velocity.X) > 4) NPC.velocity.X += movementDirection * distanceToPlayer / (NPC.velocity.X * movementDirection > 0 ? 7000 : 900);
                else NPC.velocity.X = Lerp(NPC.velocity.X, movementDirection * 2, 0.03f);
            }
            else NPC.velocity.Y = Lerp(NPC.velocity.Y, 0, 0.18f);
        }
        else
        {
            NPC.velocity.Y += NPC.ai[0];
            if (NPC.ai[0] < 0.5f)
                NPC.ai[0] += 0.1f;
        }

        //TEST CODE
        if (Main.mouseRight) NPC.velocity.X = NPC.Center.X < Main.MouseWorld.X ? 5 : -5;
        else NPC.velocity.X = 0;
        //TEST CODE

        NPC.ai[1]++;
        for (int i = 0, j = 1; i < 2; i++, j *= -1)
        {
            float lengthToTargetPosition = Legs[i].TargetPosition.Length();
            Helper.RaycastData horizontalCast = Helper.Raycast(NPC.Center, new Vector2(j, 0), 70, CRUTCH: true);
            Helper.RaycastData verticalCast = Helper.Raycast(horizontalCast.Point - new Vector2(j * 16, 0), Vector2.UnitY, 126, CRUTCH: true);

            if (Legs[i].IsMoving)
            {
                float distance = Vector2.Distance(Legs[i].RawPosition, Legs[i].TargetPosition);
                Legs[i].RawPosition = Vector2.Lerp(Legs[i].RawPosition, Legs[i].TargetPosition, 0.16f);
                if (distance < 1)
                {
                    Legs[i].WorldTargetPosition = Legs[i].TargetPosition + NPC.Center;
                    Legs[i].IsMoving = false;
                }
                else Legs[i].VerticalOffset = (-MathF.Pow(distance * 2 - Legs[i].MaxDistance, 2) / (Legs[i].MaxDistance * Legs[i].MaxDistance) + 1) * Legs[i].MaxDistance * 0.8f;
            }
            else
            {
                Legs[i].RawPosition = Legs[i].WorldTargetPosition - NPC.Center;
                if (NPC.ai[1] == 60)
                {
                    if (verticalCast.Success) Legs[i].TargetPosition = verticalCast.Point;
                    else if (horizontalCast.Success) Legs[i].TargetPosition = horizontalCast.Point;
                    Legs[i].TargetPosition -= NPC.Center;
                    Legs[i].MaxDistance = Vector2.Distance(Legs[i].RawPosition, Legs[i].TargetPosition);
                    Legs[i].IsMoving = true;
                }
            }
            Legs[i].Position = new Vector2(Legs[i].RawPosition.X, Legs[i].RawPosition.Y - Legs[i].VerticalOffset);
            Legs[i].WorldJointPosition = NPC.Center + new Vector2(70, 0).RotatedBy(Legs[i].Position.ToRotation() - j * MathF.Acos(Min(Legs[i].Position.Length(), 126) / 126));
        }
        if (NPC.ai[1] == 60) NPC.ai[1] = 0;
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (!NPC.IsABestiaryIconDummy)
        {
            for (int i = 0, j = 1; i < 2; i++, j *= -1)
            {
                SpriteEffects flip = j == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
                spriteBatch.Draw(Assets.NPCs.BloodMoon.MoonbornLegSegment1.Value, NPC.Center - screenPos + NPC.GFX(), null, NPC.HunterPotionColor(drawColor), (Legs[i].WorldJointPosition - NPC.Center).ToRotation(), new Vector2(0, 8), NPC.scale, flip, 0);
                spriteBatch.Draw(Assets.NPCs.BloodMoon.MoonbornLegSegment2.Value, Legs[i].WorldJointPosition - screenPos + NPC.GFX(), null, NPC.HunterPotionColor(drawColor), (Legs[i].Position + NPC.Center - Legs[i].WorldJointPosition).ToRotation(), new Vector2(7, 10), NPC.scale, flip, 0);
            }
        }
        spriteBatch.Draw(TextureAssets.Npc[Type].Value, NPC.Center - screenPos, NPC.frame, NPC.HunterPotionColor(drawColor), NPC.rotation, NPC.Size / 2, NPC.scale, SpriteEffects.None, 0);
        return false;
    }

    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter++;
        if (NPC.frameCounter > 7)
        {
            NPC.frameCounter = 0;
            NPC.frame.Y += frameHeight;
            if (NPC.frame.Y > frameHeight * 3)
            {
                NPC.frame.Y = 0;
            }
        }
    }
}