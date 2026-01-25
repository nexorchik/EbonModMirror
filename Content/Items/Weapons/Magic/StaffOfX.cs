using EbonianMod.Common.UI.Dialogue;
using EbonianMod.Content.Projectiles.ArchmageX;
using EbonianMod.Content.Projectiles.Bases;
using EbonianMod.Content.Projectiles.VFXProjectiles;
using System;
using Terraria.Graphics.CameraModifiers;

namespace EbonianMod.Content.Items.Weapons.Magic;

public class StaffOfX : ModItem
{
    public override string Texture => Helper.AssetPath + "Items/Weapons/Magic/StaffOfX";
    public override void SetDefaults()
    {
        Item.knockBack = 10f;
        Item.width = Item.height = 80;
        Item.crit = 10;
        Item.damage = 350;
        Item.useAnimation = 40;
        Item.useTime = 40;
        Item.value = Item.buyPrice(3, 0, 0, 0);
        Item.noUseGraphic = true;
        Item.autoReuse = false;
        Item.noMelee = true;
        Item.channel = true;
        Item.DamageType = DamageClass.Magic;
        Item.UseSound = SoundID.Item1;
        Item.useStyle = ItemUseStyleID.HiddenAnimation;
        Item.rare = ItemRarityID.Yellow;
        Item.shootSpeed = 1f;
        Item.shoot = ProjectileType<StaffOfXP>();
    }
    public override bool? CanAutoReuseItem(Player player)
    {
        return false;
    }
    public override Color? GetAlpha(Color lightColor) => Color.White;
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(source, position, Vector2.UnitX * player.direction, type, damage, knockback, player.whoAmI, 0, -player.direction, 1);
        return false;
    }
}
public class StaffOfXP : HeldSword
{
    public override string Texture => Helper.AssetPath + "Items/Weapons/Magic/StaffOfX";
    public override void SetExtraDefaults()
    {
        Projectile.width = 162;
        Projectile.height = 162;
        swingTime = 90;
        Projectile.ignoreWater = true;
        useHeld = false;
        minSwingTime = 72;
        holdOffset = 90;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.tileCollide = false;
    }
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.DontCancelChannelOnKill[Type] = true;
        ProjectileID.Sets.TrailCacheLength[Type] = 130;
        ProjectileID.Sets.TrailingMode[Type] = 2;
    }
    public override float Ease(float x)
    {
        return x == 0
        ? 0
        : x == 1
        ? 1
        : x < 0.5 ? MathF.Pow(2, 20 * x - 10) / 2
        : (2 - MathF.Pow(2, -20 * x + 10)) / 2;
    }
    public override Color? GetAlpha(Color lightColor) => Color.White;
    float visualOff;
    public override bool PreDraw(ref Color lightColor)
    {
        if (Projectile.timeLeft > swingTime - 1) return false;
        visualOff -= 0.05f;
        if (visualOff <= 0)
            visualOff = 1;
        visualOff = MathHelper.Clamp(visualOff, float.Epsilon, 1 - float.Epsilon);
        Player player = Main.player[Projectile.owner];


        Texture2D bloom = Helper.GetTexture(Texture + "_Bloom").Value;
        Main.EntitySpriteDraw(bloom, Projectile.Center + visualOffset - Main.screenPosition, null, Color.Indigo with { A = 0 } * alphaOff, Projectile.rotation + (Projectile.ai[1] == -1 ? 0 : MathHelper.PiOver2 * 3), bloom.Size() / 2, Projectile.scale, Projectile.ai[1] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);

        Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
        Vector2 orig = texture.Size() / 2;
        Main.EntitySpriteDraw(texture, Projectile.Center + visualOffset - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White * alphaOff, Projectile.rotation + (Projectile.ai[1] == -1 ? 0 : MathHelper.PiOver2 * 3), orig, Projectile.scale, Projectile.ai[1] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);

        Main.EntitySpriteDraw(bloom, Projectile.Center + visualOffset - Main.screenPosition, null, Color.White with { A = 0 } * bloomAlpha * alphaOff, Projectile.rotation + (Projectile.ai[1] == -1 ? 0 : MathHelper.PiOver2 * 3), bloom.Size() / 2, Projectile.scale, Projectile.ai[1] == -1 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);



        return false;
    }
    float bloomAlpha = 1, alphaOff = 1;
    public override void OnKill(int timeLeft)
    {
        Player player = Main.player[Projectile.owner];
        player.ChangeDir(Main.MouseWorld.X < player.Center.X ? -1 : 1);
        if (player.active && player.channel && !player.dead && !player.CCed && !player.noItems && player.whoAmI == Main.myPlayer)
        {
            Projectile p = Projectile.NewProjectileDirect(null, Projectile.Center, Vector2.UnitX * player.direction, Projectile.type, Projectile.damage, Projectile.knockBack, player.whoAmI, 0, -player.direction, 1);
            p.Center = Projectile.Center;
            p.rotation = Projectile.rotation;
            p.timeLeft = swingTime + 10;
            p.SyncProjectile();
        }
    }
    float lerpProg = 1, swingProgress, rotation;
    public override void ExtraAI()
    {
        Player player = Main.player[Projectile.owner];
        if (Projectile.timeLeft < swingTime - 5)
            bloomAlpha = Lerp(bloomAlpha, 0, 0.1f);
        if (bloomAlpha.InRange(0, 0.05f)) bloomAlpha = 0;
        if (lerpProg != 1 && lerpProg != -1)
            lerpProg = MathHelper.SmoothStep(lerpProg, 1, 0.1f);
        if (player.whoAmI == Main.myPlayer)
            if (Projectile.timeLeft == swingTime - 1)
            {
                for (int i = 0; i < 15; i++)
                    Projectile.NewProjectile(null, Projectile.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(10, 20), ProjectileType<XAnimeSlash>(), 0, 0, -1, 0, Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(0.1f, 0.3f));

                Projectile.NewProjectile(null, Projectile.Center, Vector2.Zero, ProjectileType<XExplosion>(), 0, 0);
            }
        if (swingProgress > 0.25f && swingProgress < 0.85f)
            if (Projectile.ai[0] == 0 && Helper.Raycast(Projectile.Center, Vector2.UnitY, 100).RayLength < 30)
            {
                Projectile.ai[0] = 1;
                bloomAlpha = 1;
                Helper.AddCameraModifier(new PunchCameraModifier(Projectile.Center, Main.rand.NextVector2Unit(), 10, 6, 30, 1000));
                Projectile.timeLeft = 15;
                SoundEngine.PlaySound(SoundID.Item70, Projectile.Center);
                SoundEngine.PlaySound(Sounds.xSpirit, Projectile.Center);
                WeightedRandom<string> chat = new();
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Yell1").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Yell2").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Yell3").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Yell4").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Yell5").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Yell6").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Yell7").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Yell8").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Yell9").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Yell10").Value);
                DialogueSystem.NewDialogueBox(40, Projectile.Center - new Vector2(0, 40), chat, Color.White, -1, 0.6f, Color.Magenta * 0.6f, 8f, true, DialogueAnimationIDs.BopDown | DialogueAnimationIDs.ColorWhite, SoundID.DD2_CrystalCartImpact.WithPitchOffset(0.9f), 2);

                if (player.whoAmI == Main.myPlayer)
                    Projectile.NewProjectile(null, Helper.Raycast(Projectile.Center, Vector2.UnitY, 80).Point, Vector2.Zero, ProjectileType<XImpact>(), 0, 0);

                lerpProg = -1;
            }

        if (Projectile.timeLeft == 45)
        {
            float targetDist = 400;
            bool target = false;

            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.active)
                    if (npc.CanBeChasedBy(this, false))
                    {
                        float distance = Vector2.Distance(npc.Center, Projectile.Center);
                        if ((distance < targetDist || !target))// && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                        {
                            targetDist = distance;
                            target = true;
                        }
                    }
            }
            if (!target || !Main.rand.NextBool(3))
                return;
            bool failed = false;
            for (int i = Projectile.Center.ToTileCoordinates().X - 10; i < Projectile.Center.ToTileCoordinates().X + 10; i++)
                for (int j = Projectile.Center.ToTileCoordinates().Y - 10; j < Projectile.Center.ToTileCoordinates().Y + 10; j++)
                    if (Main.tile[i, j].HasTile)
                        failed = true;

            if (!failed)
            {
                WeightedRandom<string> chat = new();
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Happy1").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Happy2").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Happy3").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Happy4").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Happy5").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Happy6").Value);
                chat.Add(Language.GetText("Mods.EbonianMod.Dialogue.ArchstaffDialogue.Weapon.Happy7").Value);
                DialogueSystem.NewDialogueBox(40, Projectile.Center - new Vector2(0, 40), chat, Color.White, -1, 0.6f, Color.Magenta * 0.6f, 8f, true, DialogueAnimationIDs.BopDown | DialogueAnimationIDs.ColorWhite, SoundID.DD2_CrystalCartImpact.WithPitchOffset(0.9f), 2);
            }
        }
        if (Projectile.timeLeft <= 15)
        {
            alphaOff = Lerp(0, 1, (float)Projectile.timeLeft / 15);
        }
        int direction = (int)Projectile.ai[1];
        if (lerpProg >= 0)
            swingProgress = MathHelper.Lerp(swingProgress, Ease(Utils.GetLerpValue(0f, swingTime, Projectile.timeLeft)), lerpProg);
        float defRot = Projectile.velocity.ToRotation();
        float start = defRot - (MathHelper.PiOver2 + MathHelper.PiOver4);
        float end = defRot + (MathHelper.PiOver2 + MathHelper.PiOver4);
        if (lerpProg >= 0)
            rotation = MathHelper.Lerp(rotation, direction == 1 ? start + MathHelper.Pi * 3 / 2 * swingProgress : end - MathHelper.Pi * 3 / 2 * swingProgress, lerpProg);
        Vector2 position = player.GetFrontHandPosition(stretch, rotation - MathHelper.PiOver2) +
            rotation.ToRotationVector2() * holdOffset * ScaleFunction(swingProgress);
        if (Projectile.timeLeft > swingTime)
        {
            if (player.gravDir != -1)
                player.SetCompositeArmFront(true, stretch, rotation - MathHelper.PiOver2);
            return;
        }
        Projectile.Center = position;
        Projectile.rotation = (position - player.Center).ToRotation() + MathHelper.PiOver4;
        if (player.gravDir != -1)
            player.SetCompositeArmFront(true, stretch, rotation - MathHelper.PiOver2);

        if (player.whoAmI == Main.myPlayer)
            if (lerpProg > -1 && swingProgress > 0.15f && swingProgress < 0.85f && Projectile.timeLeft % 2 == 0)
            {
                Projectile.NewProjectile(null, Projectile.Center + rotation.ToRotationVector2() * Projectile.height * 0.5f, Helper.FromAToB(Projectile.Center + rotation.ToRotationVector2() * Projectile.height, Main.MouseWorld).RotatedByRandom(PiOver4 * MathF.Sin(MathHelper.Pi * swingProgress)) * 15, ProjectileType<XBoltFriendly2>(), Projectile.damage, 0, player.whoAmI);
            }
    }
    public override bool? CanDamage()
    {
        return Projectile.ai[0] < 1 && swingProgress > 0.35f && swingProgress < 0.65f;
    }
}
