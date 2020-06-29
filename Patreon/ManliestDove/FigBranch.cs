using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Patreon.ManliestDove
{
    public class FigBranch : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fig Branch");
            Tooltip.SetDefault("Summons a Dove companion");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = ModContent.ProjectileType<DoveProj>();
            item.buffType = ModContent.BuffType<DoveBuff>();
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("FargowiltasSouls:AnyBird");
            recipe.AddIngredient(ItemID.Wood, 50);
            recipe.AddIngredient(ItemID.BorealWood, 50);
            recipe.AddIngredient(ItemID.RichMahogany, 50);
            recipe.AddIngredient(ItemID.PalmWood, 50);
            recipe.AddIngredient(ItemID.Ebonwood, 50);
            recipe.AddIngredient(ItemID.Shadewood, 50);

            recipe.AddTile(TileID.LivingLoom);

            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}