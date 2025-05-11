using EbonianMod.Projectiles.Enemy.Overworld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Drawing;

namespace EbonianMod.Items.Weapons.Magic;
public class KodamaStaff : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.staff[Item.type] = true;
    }

    public override void SetDefaults()
    {
        Item.damage = 19;
        Item.width = 40;
        Item.height = 40;
        Item.mana = 5;
        Item.DamageType = DamageClass.Magic;
        Item.useTime = 10;
        Item.useAnimation = 50;
        Item.reuseDelay = 30;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.knockBack = 10;
        Item.value = 1000;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(0, 30, 0, 0);
        Item.autoReuse = true;
        Item.shoot = ProjectileType<KodamaF>();
        Item.shootSpeed = 5;
    }
    public override void HoldItem(Player player)
    {
        if (!player.ItemTimeIsZero)
            Lighting.AddLight(player.itemLocation, 197f / 255f, 226f / 255f, 105f / 255f);
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Vector2 pointPoisition = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: true);
        float num2 = Main.mouseX + Main.screenPosition.X - pointPoisition.X;
        float num3 = Main.mouseY + Main.screenPosition.Y - pointPoisition.Y;
        Vector2 vector5 = new Vector2(num2, num3);
        vector5.X = Main.mouseX + Main.screenPosition.X - pointPoisition.X;
        vector5.Y = Main.mouseY + Main.screenPosition.Y - pointPoisition.Y - 1000f;
        player.itemRotation = (float)Math.Atan2(vector5.Y * player.direction, vector5.X * player.direction);
        NetMessage.SendData(13, -1, -1, null, player.whoAmI);
        NetMessage.SendData(41, -1, -1, null, player.whoAmI);

        Vector2 vel = new Vector2(velocity.X * 0.5f * Main.rand.NextFloat(1, 2), -Item.shootSpeed).RotatedByRandom(PiOver4);
        Projectile.NewProjectile(source, position + vel * 10, vel, type, damage, knockback, player.whoAmI);
        return false;
    }
}
