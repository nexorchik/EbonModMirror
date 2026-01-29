using EbonianMod.Content.Projectiles.Enemy.BloodMoon;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;
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
        public Vector2 Position, TargetPosition;
        public Vector2 WorldPosition, WorldJointPosition;
        public float Speed, Distance, MaxDistance;
        public bool DefaultState, IsMoving;
    }
    bool Airborne;
    public Leg[] Legs = new Leg[6];
    public override void OnSpawn(IEntitySource source)
    {
        for (int i = 0; i < 6; i++)
        {
            Legs[i] = new Leg();
            Legs[i].MaxDistance = 100;
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

        for (int i = 0, j = 1; i < 2; i++, j *= -1)
        {
            float lengthToPosition = Legs[i].Position.Length();
            float lengthToTargetPosition = Legs[i].TargetPosition.Length();
            Helper.RaycastData horizontalCast = Helper.Raycast(NPC.Center, new Vector2(j, 0), 60, CRUTCH: true);
            Helper.RaycastData verticalCast = Helper.Raycast(horizontalCast.Point - new Vector2(j * 16, 0), Vector2.UnitY, 126, CRUTCH: true);
            if(false) Legs[i].TargetPosition = (verticalCast.Success || (!verticalCast.Success && !horizontalCast.Success) ? verticalCast.Point : horizontalCast.Point);
            Legs[i].Position = Vector2.Lerp(Legs[i].Position, Legs[i].TargetPosition - NPC.Center, 0.3f);

            Legs[i].WorldPosition = NPC.Center + Legs[i].Position;
            Legs[i].WorldJointPosition = NPC.Center + new Vector2(70, 0).RotatedBy(Legs[i].Position.ToRotation() - j * MathF.Acos(Min(lengthToPosition, 126) / 126));
        }
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (!NPC.IsABestiaryIconDummy)
        {
            for (int i = 0, j = 1; i < 2; i++, j *= -1)
            {
                SpriteEffects flip = j == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
                spriteBatch.Draw(Assets.NPCs.BloodMoon.MoonbornLegSegment1.Value, NPC.Center - screenPos + NPC.GFX(), null, NPC.HunterPotionColor(drawColor), (Legs[i].WorldJointPosition - NPC.Center).ToRotation(), new Vector2(0, 8), NPC.scale, flip, 0);
                spriteBatch.Draw(Assets.NPCs.BloodMoon.MoonbornLegSegment2.Value, Legs[i].WorldJointPosition - screenPos + NPC.GFX(), null, NPC.HunterPotionColor(drawColor), (Legs[i].WorldPosition - Legs[i].WorldJointPosition).ToRotation(), new Vector2(7, 10), NPC.scale, flip, 0);
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