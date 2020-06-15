using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Weapons.FinalUpgrades
{
    public class SparklingLove : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sparkling Love");
            Tooltip.SetDefault("'Let's keep how you got this a secret'");
        }

        public override void SetDefaults()
        {
            item.damage = 1700;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useAnimation = 27;
            item.useTime = 27;
            item.shootSpeed = 16f;
            item.knockBack = 14f;
            item.width = 32;
            item.height = 32;
            item.scale = 2f;
            item.rare = 11;
            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<Projectiles.BossWeapons.SparklingLove>();
            item.value = Item.sellPrice(0, 70);
            item.noMelee = true; //no melee hitbox
            item.noUseGraphic = true; //dont draw item
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
                item.shoot = ModContent.ProjectileType<Projectiles.BossWeapons.SparklingDevi>();
                item.useStyle = ItemUseStyleID.SwingThrow;
                item.summon = true;
                item.melee = false;
                item.noUseGraphic = false;
                item.noMelee = false;
                item.useAnimation = 61;
                item.useTime = 61;
            }
            else
            {
                item.shoot = ModContent.ProjectileType<Projectiles.BossWeapons.SparklingLove>();
                item.useStyle = ItemUseStyleID.SwingThrow;
                item.summon = false;
                item.melee = true;
                item.noUseGraphic = true;
                item.noMelee = true;
                item.useAnimation = 27;
                item.useTime = 27;
            }
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = new Color(255, 0, Main.DiscoB);
                }
            }
        }

        /*public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            flip = !flip;
            Vector2 speed = new Vector2(speedX, speedY);

            if (player.altFunctionUse == 2) //right click
            {
                speed = speed.RotatedBy(Math.PI / 2 * (flip ? 1 : -1));
                Projectile.NewProjectile(position, speed, type, damage, knockBack, item.owner, (float)Math.PI / 120 * (flip ? -1 : 1));
            }
            else
            {
                const int max = 5;
                for (int i = 0; i < max; i++)
                {
                    Projectile.NewProjectile(position, speed.RotatedBy(2 * Math.PI / max * i), type,
                        damage, knockBack, item.owner, 0, (Main.MouseWorld - position).Length() * (flip ? 1 : -1));
                }
            }
            return false;
        }*/

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ModLoader.GetMod("Fargowiltas").ItemType("EnergizerMoon"));
            recipe.AddIngredient(mod.ItemType("Sadism"), 15);

            recipe.AddTile(ModLoader.GetMod("Fargowiltas").TileType("CrucibleCosmosSheet"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/
    }
}