using System;
using System.Diagnostics;
using Terraria;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        NPC.lifeMax = 2000;
        NPC.noTileCollide = true;
        NPC.noGravity = true;
    }

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 8;
    }
    public class Leg
    {
        public Vector2 RawPosition, Position, TargetPosition, JointPosition;
        public float MaxDistance, VerticalOffset;
        public bool DefaultState, IsMoving;
    }
    Leg[] Legs = new Leg[6];
    bool IsGrounded, LandingTrigger = true, IsInBlocks, HasWalls;
    Vector2 MovementDirection;
    public override void OnSpawn(IEntitySource source)
    {
        for (int i = 0, j = 1; i < 6; i++, j *= -1)
        {
            Legs[i] = new Leg();
            Legs[i].TargetPosition = NPC.Center + new Vector2((1 - (i + 2) / 20) * 20 * j, 0);
            Legs[i].RawPosition = Legs[i].TargetPosition;
        }
    }

    public override void AI()
    {
        NPC.TargetClosest(true);
        Player player = Main.player[NPC.target];

        int direction = player.Center.X > NPC.Center.X ? 1 : -1;
        MovementDirection = new Vector2(NPC.velocity.X > 0 ? 1 : -1, NPC.velocity.Y > 0 ? 1 : -1);
        Helper.RaycastData bodyCast = Helper.Raycast(NPC.Center, Vector2.UnitY, 110, true, false);
        Helper.RaycastData frontCast = Helper.Raycast(NPC.Center, new Vector2(direction, 0), 76, true, IsInBlocks, true);
        Helper.RaycastData backCast =  Helper.Raycast(NPC.Center, new Vector2(-direction, 0), 76, true, IsInBlocks, true);
        HasWalls = frontCast.Success && backCast.Success;
        IsGrounded = bodyCast.Success || HasWalls;
        IsInBlocks = bodyCast.RayLength < 12;

        if (IsGrounded)
        {
            if(LandingTrigger)
            {
                NPC.velocity.Y = Min(NPC.velocity.Y, 18);
                LandingTrigger = false;
            }

            Vector2 bodyToPlayer = player.Center - NPC.Center;
            if (Helper.Raycast(NPC.Center, bodyToPlayer, bodyToPlayer.Length(), true).Success || HasWalls)
            {
                NPC.velocity.Y = Lerp(NPC.velocity.Y, Distance(player.Center.Y, NPC.Center.Y + 100) > 32 ? (player.Center.Y > NPC.Center.Y + 100 ? 4f : -4f) : 0, 0.03f);
            }
            else
            {
                NPC.Center = Vector2.Lerp(NPC.Center, new Vector2(NPC.Center.X, bodyCast.Point.Y - 100), 0.23f);
                if (NPC.velocity.Y > 0.5f) NPC.velocity.Y = Lerp(NPC.velocity.Y, 0, 0.1f);
                else NPC.velocity.Y = 0;
            }
            float xToPlayer = Distance(NPC.Center.X, player.Center.X);
            if (xToPlayer > 160 || MathF.Abs(NPC.velocity.X) > 4) NPC.velocity.X += direction * xToPlayer / (NPC.velocity.X * direction > 0 ? 7000 : 900);
            else NPC.velocity.X = Lerp(NPC.velocity.X, direction * 2, 0.03f);
        }
        else
        {
            LandingTrigger = true;
            NPC.velocity.Y += NPC.ai[0];
            if (NPC.ai[0] < 0.5f) NPC.ai[0] += 0.1f;
        }

        //------------------------------//
        //MovementDirection = 0;
        //if (Main.mouseRight)
        //{
        //    MovementDirection = Main.MouseWorld.X > NPC.Center.X ? 1 : -1;
        //    NPC.velocity = Vector2.UnitX * MovementDirection * 3;
        //}
        //else NPC.velocity = Vector2.Zero;
        //------------------------------//

        for (int i = 0, j = 1; i < 6; i++, j *= -1)
        {
            float distanceToTargetPosition = Vector2.Distance(Legs[i].TargetPosition, NPC.Center);
            if (Legs[i].IsMoving)
            {
                float distance = Vector2.Distance(Legs[i].RawPosition, Legs[i].TargetPosition);
                Legs[i].RawPosition = Vector2.Lerp(Legs[i].RawPosition, Legs[i].TargetPosition, Clamp(NPC.velocity.Length() * (1 - (2 - i / 2) / 10f) / 15, 0.1f, 1));
                if (Legs[i].MaxDistance != 0) Legs[i].VerticalOffset = (-MathF.Pow(distance * 2 - Legs[i].MaxDistance, 2) / (Legs[i].MaxDistance * Legs[i].MaxDistance) + 1) * 30;
                if (distance < 0.2f || distanceToTargetPosition > 126)
                {
                    Legs[i].IsMoving = false;
                }
                Legs[i].Position = new Vector2(Legs[i].RawPosition.X, Legs[i].RawPosition.Y - Legs[i].VerticalOffset);
            }
            else
            {
                float multiplier = 1 - (i + 2) / 8f;
                if (IsGrounded)
                {
                    if(distanceToTargetPosition > 126)
                    {
                        Helper.RaycastData chosenCast = j == 1 ? frontCast : backCast;
                        Helper.RaycastData otherCast = j == 1 ? backCast : frontCast;

                        if (IsInBlocks)
                        {
                            Legs[i].TargetPosition = Helper.Raycast(NPC.Center + new Vector2(0, j * (chosenCast.RayLength - 8) * multiplier).RotatedBy(MovementDirection.ToRotation()), Vector2.UnitY, 0, true, IsInBlocks, true).Point;
                        }
                        else
                        {
                            Legs[i].TargetPosition = bodyCast.Point;
                            float startValue = chosenCast.RayLength * (HasWalls ? 1 : multiplier);
                            for (float offset = startValue; offset > 0; offset -= 16)
                            {
                                Helper.RaycastData verticalcast = Helper.Raycast(new Vector2(NPC.Center.X + offset * j, NPC.Center.Y), Vector2.UnitY, 110, true, IsInBlocks);
                                if (verticalcast.Success)
                                {
                                    Legs[i].TargetPosition = verticalcast.Point;
                                    break;
                                }   
                            }
                            Main.NewText("Leg " + i + " failed to find suitable surface");
                        }
                        Legs[i].MaxDistance = Vector2.Distance(Legs[i].RawPosition, Legs[i].TargetPosition);
                        Legs[i].IsMoving = true;
                    }
                }
            }
        }
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (!NPC.IsABestiaryIconDummy)
        {
            for (int i = 0, j = 1; i < 6; i++, j *= -1)
            {
                Legs[i].JointPosition = NPC.Center + new Vector2(70, 0).RotatedBy((Legs[i].Position - NPC.Center).ToRotation() - j * MathF.Acos(Min((Legs[i].Position - NPC.Center).Length(), 126) / 126));
                SpriteEffects flip = j == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
                spriteBatch.Draw(Assets.NPCs.BloodMoon.MoonbornLegSegment1.Value, NPC.Center - screenPos + NPC.GFX(), null, NPC.HunterPotionColor(drawColor), (Legs[i].JointPosition - NPC.Center).ToRotation(), new Vector2(0, 8), NPC.scale, flip, 0);
                spriteBatch.Draw(Assets.NPCs.BloodMoon.MoonbornLegSegment2.Value, Legs[i].JointPosition - screenPos + NPC.GFX(), null, NPC.HunterPotionColor(drawColor), (Legs[i].Position - Legs[i].JointPosition).ToRotation(), new Vector2(7, 10), NPC.scale, flip, 0);
                Utils.DrawLine(spriteBatch, NPC.Center, Helper.Raycast(NPC.Center, new Vector2(MovementDirection.X, 0), 76, true, IsInBlocks, true).Point, Color.Red);
                Utils.DrawLine(spriteBatch, NPC.Center, Helper.Raycast(NPC.Center, new Vector2(-MovementDirection.X, 0), 76, true, IsInBlocks, true).Point, Color.Red);
            }
        }
        return true;
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