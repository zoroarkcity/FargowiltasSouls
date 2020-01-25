using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Weapons.SwarmDrops
{
    public class FleshCannon : ModItem
    {
        public override string Texture => "FargowiltasSouls/Items/Placeholder";

        public int counter;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Regurgitator");
            Tooltip.SetDefault("'The enslaved face of a defeated foe...'");
        }

        public override void SetDefaults()
        {
            item.damage = 310;
            item.magic = true;
            item.mana = 6;
            item.width = 24;
            item.height = 24;
            item.useTime = 8;
            item.useAnimation = 8;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 2f;
            item.UseSound = SoundID.Item12;
            item.value = Item.sellPrice(0, 10);
            item.rare = 11;
            item.autoReuse = true;
            item.shoot = ProjectileID.PurpleLaser;
            item.shootSpeed = 10f;
        }   

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 speed = new Vector2(speedX, speedY);

            const int factor = 7;

            if (counter == 0) //burp hungy
            {
                int p = Projectile.NewProjectile(position, speed * 2f, mod.ProjectileType("Hungry"), damage, knockBack, player.whoAmI);
                if (p != Main.maxProjectiles)
                {
                    Main.projectile[p].minion = false;
                    Main.projectile[p].magic = true;
                }
                Main.PlaySound(new LegacySoundStyle(4, 13), position);
            }

            float rotation = MathHelper.ToRadians(10) * (float)Math.Sin((counter + 0.25) * Math.PI / factor);
            Projectile.NewProjectile(position, speed.RotatedBy(rotation), type, damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(position, speed.RotatedBy(-rotation), type, damage, knockBack, player.whoAmI);

            if (++counter >= factor) //reset
            {
                counter = 0;
            }

            return false;
        }

        public override void AddRecipes()
        {
            if (Fargowiltas.Instance.FargowiltasLoaded)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "FleshHand");
                recipe.AddIngredient(ModLoader.GetMod("Fargowiltas").ItemType("EnergizerWall"));
                recipe.AddIngredient(ItemID.LunarBar, 10);
                recipe.AddIngredient(null, "LunarCrystal", 5);

                recipe.AddTile(mod, "CrucibleCosmosSheet");
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}