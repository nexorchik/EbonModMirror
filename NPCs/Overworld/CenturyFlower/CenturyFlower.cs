using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using EbonianMod.Items.Weapons.Magic;

namespace EbonianMod.NPCs.Overworld.CenturyFlower
{
    // I need to rewrite this at some point because this is some mentally ill code </3 sorry whoever made this
    public class CenturyFlower : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 10;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                new FlavorTextBestiaryInfoElement(NPC.BestiaryKey())
            });
        }
        public override void SetDefaults()
        {
            NPC.width = 30;
            NPC.height = 70;
            NPC.damage = 12;
            NPC.lifeMax = 50;
            NPC.defense = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath32;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.aiStyle = 3;
            AIType = NPCID.GoblinScout;
            NPC.netAlways = true;
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * bossAdjustment * balance);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Color clr = Color.White; // full white
            Vector2 drawPos = NPC.Center - screenPos;
            Texture2D texture = Assets.NPCs.Overworld.CenturyFlower.CenturyFlower_Glow.Value;
            Texture2D origTexture = TextureAssets.Npc[NPC.type].Value;
            Rectangle frame = new Rectangle(0, NPC.frame.Y, NPC.width, NPC.height);
            Vector2 orig = frame.Size() / 2f - new Vector2(0, 3);
            Main.spriteBatch.Draw(origTexture, drawPos, frame, drawColor, NPC.rotation, orig, NPC.scale, NPC.direction < 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            Main.spriteBatch.Draw(texture, drawPos, frame, clr, NPC.rotation, orig, NPC.scale, NPC.direction < 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
        private void SetState(int newState)
        {
            NPC.ai[0] = newState;
            timer = 0;
        }
        public override void FindFrame(int frameHeight)
        {
            if (!NPC.IsABestiaryIconDummy)
            {
                if (NPC.ai[0] == 1)
                    OpenPetalsAnimation();
                NPC.frame.Y = GetFrame() * frameHeight;
            }
            else
            {
                NPC.frameCounter++;
                if (NPC.frameCounter % 5 == 0)
                {
                    if (NPC.frame.Y < NPC.height * 4)
                        NPC.frame.Y += NPC.height;
                    else
                        NPC.frame.Y = 0;
                }
            }
        }
        float timer;
        const float strideSpeed = 1f, jumpHeight = 7f;
        public override bool PreAI()
        {
            timer++;
            if (Main.rand.NextFloat() <= .05f && timer > 250 && NPC.ai[0] == 0)
            {
                RealFrame = ScaleFrame(5);
                SetState(1);
            }

            switch (NPC.ai[0])
            {
                case 1:
                    OpenPetals();
                    break;
                default:
                    ManageMovement();
                    ManageMovementAnimation();
                    break;
            }
            return false;
        }
        const float animationSpdOffset = 4f;
        int GetFrame() => (int)(RealFrame / strideSpeed / animationSpdOffset);
        static int ScaleFrame(int frame) => (int)(animationSpdOffset * frame * strideSpeed); // returns value necessary for real frame to set animation frame to target frame frame
        float RealFrame;
        float Jump;
        void ManageMovement()
        {
            NPC.TargetClosest(false);
            Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY, 1, false, 0);
            var player = Main.player[NPC.target];
            // var sqrDistance = player.DistanceSQ(NPC.position);
            if (NPC.collideX && timer > 2 && Jump <= 0)
            {
                NPC.velocity.Y = -jumpHeight;
                Jump = 1;
            }
            if (Main.tile[NPC.Hitbox.Center.X / 16, NPC.Hitbox.Bottom / 16].HasTile)
            {
                if (Main.tile[NPC.Hitbox.Center.X / 16, NPC.Hitbox.Bottom / 16].LeftSlope || Main.tile[NPC.Hitbox.Center.X / 16, NPC.Hitbox.Bottom / 16].BottomSlope || Main.tile[NPC.Hitbox.Center.X / 16, NPC.Hitbox.Bottom / 16].RightSlope)
                {
                    NPC.velocity.Y = -jumpHeight;
                    Jump = 1;
                }
                else Jump = 0;
            }

            NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, strideSpeed * NPC.direction, 0.1f);
            var horizontalDistance = Math.Abs(NPC.Center.X - player.Center.X);
            if (horizontalDistance >= 35.78f)
            {
                NPC.FaceTarget();
            }
            NPC.spriteDirection = NPC.direction;
        }

        void ManageMovementAnimation()
        {
            if (++NPC.frameCounter % 2 == 0)
                RealFrame++;
            if (NPC.velocity.Y != 0 || NPC.oldVelocity == Vector2.Zero || GetFrame() > 4 || GetFrame() < 0)
            {
                RealFrame = 3;
            }
        }

        public void OpenPetals()
        {
            NPC.knockBackResist = 0.5f;
            if (timer == 1)
                NPC.velocity.X = 0;
            else if (timer < 50 && timer % 20 == 0)
                Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center - new Vector2(1, 16), Main.rand.NextVector2Unit() * Main.rand.NextFloat(1, 2) * Lerp(0, 1, timer / 50f), ModContent.ProjectileType<CenturyFlowerSpore.CenturyFlowerSpore>(), 0, 0);
            else if (timer > 75 && timer % 5 == 0)
            {
                if (timer >= 150)
                {
                    RealFrame = ScaleFrame(4);
                    SetState(0);
                }
                else
                {
                    Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center - new Vector2(1, 19), Main.rand.NextVector2Unit() * Main.rand.NextFloat(1, 2), ModContent.ProjectileType<CenturyFlowerSpore.CenturyFlowerSpore>(), 0, 0);
                }
            }
            if (NPC.velocity.Y == 0)
                NPC.velocity.X *= 0.9f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            /*for (int i = 0; i < 4; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GrassBlades, 2 * hit.HitDirection, -1.5f);
            }*/
            if (NPC.life <= 0)
            {
                if (Main.netMode == NetmodeID.Server)
                    return;

                for (int i = 0; i < 5; i++)
                {
                    Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, Vector2.UnitY, 910, 0.7f + Main.rand.NextFloat() * 0.6f);
                }
                Helper.SpawnGore(NPC, "EbonianMod/Century", 4, 1, Vector2.One * hit.HitDirection);
                Helper.SpawnGore(NPC, "EbonianMod/Century", 2, 2, Vector2.One * hit.HitDirection);
                Helper.SpawnGore(NPC, "EbonianMod/Century", 2, 3, Vector2.One * hit.HitDirection);
                Helper.SpawnGore(NPC, "EbonianMod/Century", 2, 4, Vector2.One * hit.HitDirection);
                Helper.SpawnDust(NPC.position, NPC.Size, DustID.Grass, new Vector2(2 * hit.HitDirection, -1.5f), 10);
                /*for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass, 2 * hit.HitDirection, -1.5f);
				}*/
            }
        }
        public void OpenPetalsAnimation()
        {
            if (timer <= 75 && GetFrame() < 9)
                RealFrame++;
            else if (Helper.InRange(timer, 145, 5) && GetFrame() > 5)
                RealFrame--;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZonePurity)
                return SpawnCondition.OverworldDay.Chance * 0.2f;
            return 0;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CenturySprayer>(), 50));
        }
    }
}