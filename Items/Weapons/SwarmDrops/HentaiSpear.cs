using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Fargowiltas.Items.Tiles;

namespace FargowiltasSouls.Items.Weapons.SwarmDrops
{
    public class HentaiSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Penetrator");
            Tooltip.SetDefault("'The reward for slaughtering many...'");
            DisplayName.AddTranslation(GameCulture.Chinese, "洞察者");
            Tooltip.AddTranslation(GameCulture.Chinese, "'屠戮众多的奖励...'");
        }

        public override void SetDefaults()
        {
            item.damage = 1700;
            item.useStyle = 5;
            item.useAnimation = 16;
            item.useTime = 16;
            item.shootSpeed = 6f;
            item.knockBack = 7f;
            item.width = 32;
            item.height = 32;
            item.scale = 1f;
            item.rare = 11;
            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType("HentaiSpear");
            item.value = Item.sellPrice(0, 70);
            item.noMelee = true; // Important because the spear is acutally a projectile instead of an item. This prevents the melee hitbox of this item.
            item.noUseGraphic = true; // Important, it's kind of wired if people see two spears at one time. This prevents the melee animation of this item.
            item.melee = true;
            item.autoReuse = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.shoot = mod.ProjectileType("HentaiSpearThrown");
                item.shootSpeed = 25f;
                item.useAnimation = 100;
                item.useTime = 100;
                item.thrown = true;
                item.melee = false;
            }
            else
            {
                item.shoot = mod.ProjectileType("HentaiSpear");
                item.shootSpeed = 6f;
                item.useAnimation = 16;
                item.useTime = 16;
                item.thrown = false;
                item.melee = true;
            }
            return true;

            /*if (player.altFunctionUse == 2) //right click
            {
                item.useAnimation = 32;
                item.useTime = 32;
            }
            else
            {
                item.useAnimation = 16;
                item.useTime = 16;
            }
            return player.ownedProjectileCounts[item.shoot] < 1; // This is to ensure the spear doesn't bug out when using autoReuse = true*/
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = new Color(0, 255, Main.DiscoB);
                }
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2) //right click
            {
                damage /= 4;
                return true;
            }

            if (player.ownedProjectileCounts[item.shoot] < 1 && player.ownedProjectileCounts[mod.ProjectileType("Dash")] < 1)
            {
                Projectile.NewProjectile(position.X, position.Y, speedX * 3f, speedY * 3f, mod.ProjectileType("Dash"), damage, knockBack, player.whoAmI);
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, item.shoot, damage, knockBack, item.owner, 0f, 1f);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ModLoader.GetMod("Fargowiltas").ItemType("EnergizerMoon"));
            recipe.AddIngredient(mod.ItemType("Sadism"), 15);

            recipe.AddTile(ModContent.TileType<CrucibleCosmosSheet>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}