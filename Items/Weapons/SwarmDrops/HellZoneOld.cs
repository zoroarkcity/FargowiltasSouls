using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Fargowiltas.Items.Tiles;

namespace FargowiltasSouls.Items.Weapons.SwarmDrops
{
    public class HellZoneOld : ModItem
    {
        public int skullTimer;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hell Zone (Old)");
            Tooltip.SetDefault("Uses gel for ammo\n66% chance to not consume ammo\n'The reward for slaughtering many...'");
            DisplayName.AddTranslation(GameCulture.Chinese, "地狱领域");
            Tooltip.AddTranslation(GameCulture.Chinese, "'屠戮众多的奖励...'");
        }

        public override void SetDefaults()
        {
            item.damage = 205; //
            item.knockBack = 0.5f;
            item.shootSpeed = 12f; //

            item.useStyle = 5;
            item.autoReuse = true;
            item.useAnimation = 30; //
            item.useTime = 6; //
            item.width = 54;
            item.height = 14;
            item.shoot = mod.ProjectileType("HellFlame");
            item.useAmmo = AmmoID.Gel;
            item.UseSound = SoundID.Item34; //

            item.noMelee = true;
            item.value = Item.sellPrice(0, 15); //
            item.rare = 11; //
            item.ranged = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            if (--skullTimer < 0)
            {
                skullTimer = 10;
                //float ai = Main.rand.NextFloat((float)Math.PI * 2);
                Projectile.NewProjectile(position, 1.5f * new Vector2(speedX, speedY), mod.ProjectileType("HellSkull"), damage / 2, knockBack, player.whoAmI, -1);
            }
            return false;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.Next(3) == 0;
        }

        //make them hold it different
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-30, -5);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BoneZone");
            recipe.AddIngredient(null, "MutantScale", 10);
            recipe.AddIngredient(ModLoader.GetMod("Fargowiltas").ItemType("EnergizerSkele"));
            recipe.AddTile(ModLoader.GetMod("Fargowiltas").TileType("CrucibleCosmosSheet"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}