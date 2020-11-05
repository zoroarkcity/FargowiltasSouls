using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using FargowiltasSouls.NPCs.Critters;
using FargowiltasSouls.Projectiles;
using Microsoft.Xna.Framework;
using FargowiltasSouls.Items.Misc;

namespace FargowiltasSouls.Items.Weapons.Misc
{
    public class TophatSquirrelWeapon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Top Hat Squirrel");
            Tooltip.SetDefault("'Who knew this squirrel had phenomenal cosmic power?'");
            DisplayName.AddTranslation(GameCulture.Chinese, "高顶礼帽松鼠");
            Tooltip.AddTranslation(GameCulture.Chinese, "'谁能知道,这只松鼠竟然有着非凡的宇宙力量呢?'");
        }

        public override void SetDefaults()
        {
            item.damage = 50;

            item.width = 20;
            item.height = 20;
            item.rare = 8;
            item.useAnimation = 30;
            item.useTime = 30;

            item.magic = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.knockBack = 3f;

            item.autoReuse = true;

            item.shoot = ModContent.ProjectileType<TopHatSquirrelProj>();
            item.shootSpeed = 8f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            
            recipe.AddIngredient(ModContent.ItemType<TopHatSquirrelCaught>(), 20);
            recipe.AddIngredient(ItemID.ChlorophyteBar);

            recipe.AddTile(TileID.CrystalBall);

            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}