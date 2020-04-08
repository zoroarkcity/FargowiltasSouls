using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Localization;
using Fargowiltas.Items.Tiles;

namespace FargowiltasSouls.Items.Weapons.SwarmDrops
{
    public class DestroyerGun2 : ModItem
    {
        public override string Texture => "FargowiltasSouls/Items/Weapons/BossDrops/DestroyerGun";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Destroyer Gun EX");
            Tooltip.SetDefault("'The reward for slaughtering many...'");
            DisplayName.AddTranslation(GameCulture.Chinese, "毁灭者之枪 EX");
            Tooltip.AddTranslation(GameCulture.Chinese, "'屠戮众多的奖励...'");
        }

        public override void SetDefaults()
        {
            item.damage = 240;
            item.mana = 30;
            item.summon = true;
            item.width = 24;
            item.height = 24;
            item.useAnimation = 70;
            item.useTime = 70;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 1.5f;
            item.UseSound = new LegacySoundStyle(4, 13);
            item.value = Item.sellPrice(0, 25);
            item.rare = 11;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("DestroyerHead2");
            item.shootSpeed = 18f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DestroyerGun");
            recipe.AddIngredient(null, "MutantScale", 10);
            recipe.AddIngredient(ModLoader.GetMod("Fargowiltas").ItemType("EnergizerDestroy"));
            recipe.AddTile(ModLoader.GetMod("Fargowiltas").TileType("CrucibleCosmosSheet"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}