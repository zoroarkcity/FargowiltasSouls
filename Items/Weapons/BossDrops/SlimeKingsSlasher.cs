using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Fargowiltas.Projectiles;
using FargowiltasSouls.Projectiles.BossWeapons;

namespace FargowiltasSouls.Items.Weapons.BossDrops
{
    public class SlimeKingsSlasher : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime King's Slasher");
            Tooltip.SetDefault("'Torn from the insides of a defeated foe..'");
            DisplayName.AddTranslation(GameCulture.Chinese, "史莱姆王的屠戮者");
            Tooltip.AddTranslation(GameCulture.Chinese, "'撕裂敌人内部而得来的..'");
        }

        public override void SetDefaults()
        {
            item.damage = 18;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 23;
            item.useAnimation = 23;
            item.useStyle = 1;
            item.knockBack = 6;
            item.value = 10000;
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<SlimeSpikeFriendly>();
            item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockback)
        {
            int p = Projectile.NewProjectile(player.Center, new Vector2(speedX, speedY), type, damage, knockback, player.whoAmI);

            FargoGlobalProjectile.SplitProj(Main.projectile[p], Main.rand.Next(3, 6));

            return false;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Slimed, 120);
        }
    }
}