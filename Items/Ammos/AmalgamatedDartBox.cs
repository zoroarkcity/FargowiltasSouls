using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Fargowiltas.Items.Tiles;

namespace FargowiltasSouls.Items.Ammos
{
    public class AmalgamatedDartBox : ModItem
    {
        Mod fargos = ModLoader.GetMod("Fargowiltas");

        public override bool Autoload(ref string name)
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Amalgamated Dart Box");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.damage = 60;
            item.ranged = true;
            item.width = 26;
            item.height = 26;
            item.knockBack = 3.5f;
            item.rare = 10;
            item.shoot = mod.ProjectileType("AmalgamatedDart");
            item.shootSpeed = 3f;
            item.ammo = AmmoID.Dart;
            //item.UseSound = SoundID.Item63;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(fargos, "PoisonDartBox");
            recipe.AddIngredient(fargos, "CursedDartBox");
            recipe.AddIngredient(fargos, "IchorDartBox");
            recipe.AddIngredient(fargos, "CrystalDartBox");

            recipe.AddIngredient(mod.ItemType("Sadism"), 15);
            recipe.AddTile(ModLoader.GetMod("Fargowiltas").TileType("CrucibleCosmosSheet"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}