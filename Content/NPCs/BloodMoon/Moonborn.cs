using EbonianMod.Content.Projectiles.Enemy.BloodMoon;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;
using Terraria.GameContent.Bestiary;

public class Moonborn : ModNPC
{
    public override string Texture => Helper.AssetPath + "NPCs/BloodMoon/Moonborn";

    public class Leg
    {
        public Vector2 TargetPosition, JointPosition;
        public bool defaultState;
    }
    public Leg[] Legs = new Leg[6];
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

    public override void OnSpawn(IEntitySource source)
    {
        for (int i = 0; i < 6; i++)
        {
            Legs[i] = new Leg();
        }
    }

    public override void AI()
    {
        Player player = Main.player[NPC.target];
        NPC.TargetClosest(true);

        Vector2 basePosition = Helper.TileRaycast.Cast(NPC.Center, Vector2.UnitY, 140);

        float distance = MathF.Abs(basePosition.X - player.Center.X);
        float movementDirection = player.Center.X > NPC.Center.X ? 1 : -1;
        NPC.Center = Vector2.Lerp(NPC.Center, new Vector2(basePosition.X, basePosition.Y - 80), Max(NPC.velocity.X / 80, 0.06f));
        if (distance > 150 || MathF.Abs(NPC.velocity.X) > 4) NPC.velocity.X += movementDirection * distance / (NPC.velocity.X * movementDirection > 0 ? 7000 : 900);
        else NPC.velocity.X = Lerp(NPC.velocity.X, movementDirection * 2, 0.03f);

        for (int i = 0, j = 1; i < 2; i++, j *= -1)
        {
            Legs[i].TargetPosition = Helper.TileRaycast.Cast(NPC.Center + new Vector2(60 * j, 0), Vector2.UnitY, 126);
            Legs[i].JointPosition = NPC.Center + new Vector2(70, 0).RotatedBy((Legs[i].TargetPosition - NPC.Center).ToRotation() - j * MathF.Acos(Min((Legs[i].TargetPosition - NPC.Center).Length(), 126) / 126));
        }
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        if (!NPC.IsABestiaryIconDummy)
        {
            for (int i = 0, j = 1; i < 2; i++, j *= -1)
            {
                spriteBatch.Draw(Helper.GetTexture(Helper.AssetPath + "NPCs/BloodMoon/MoonbornLegSegment1").Value, NPC.Center - screenPos, null, NPC.HunterPotionColor(drawColor), (Legs[i].JointPosition - NPC.Center).ToRotation(), new Vector2(0, 8), NPC.scale, j == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
                spriteBatch.Draw(Helper.GetTexture(Helper.AssetPath + "NPCs/BloodMoon/MoonbornLegSegment2").Value, Legs[i].JointPosition - screenPos, null, NPC.HunterPotionColor(drawColor), (Legs[i].TargetPosition - Legs[i].JointPosition).ToRotation(), new Vector2(7, 10), NPC.scale, j == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
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