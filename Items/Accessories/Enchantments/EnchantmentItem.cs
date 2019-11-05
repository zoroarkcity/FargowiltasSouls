using System;
using FargowiltasSouls.ModCompatibilities;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public abstract class EnchantmentItem : FargowiltasSoulsItem
    {
        protected EnchantmentItem(string displayName, string tooltip, int width, int height, int craftedAt, int value, int rarity) : 
            base(displayName, tooltip, width, height, value: value, rarity: rarity)
        {
            CraftedAt = craftedAt;
        }


        public override void SetDefaults()
        {
            item.accessory = true;

            ItemID.Sets.ItemNoGravity[item.type] = true;

            base.SetDefaults();
        }


        public sealed override void AddRecipes()
        {
            base.AddRecipes();

            ModRecipe recipe = new ModRecipe(mod);

            AddRecipeBase(recipe);

            TryAddModRecipe(Fargowiltas.Instance.ThoriumCompatibility, AddThoriumRecipe, recipe);
            TryAddModRecipe(Fargowiltas.Instance.CalamityCompatibility, AddCalamityRecipe, recipe);
            TryAddModRecipe(Fargowiltas.Instance.SoACompatibility, AddShadowsOfAbaddonRecipe, recipe);

            if (!RecipeAffectedByMods)
                FinishRecipeVanilla(recipe);

            recipe.SetResult(this);
            recipe.AddTile(CraftedAt);
            recipe.AddRecipe();
        }


        protected abstract void AddRecipeBase(ModRecipe recipe);
        protected virtual void FinishRecipeVanilla(ModRecipe recipe) { }

        protected virtual void AddThoriumRecipe(ModRecipe recipe, Mod thorium) { }
        protected virtual void AddCalamityRecipe(ModRecipe recipe, Mod calamity) { }
        protected virtual void AddShadowsOfAbaddonRecipe(ModRecipe recipe, Mod soa) { }


        private void TryAddModRecipe(ModCompatibility compatibility, Action<ModRecipe, Mod> recipeMethod, ModRecipe recipe)
        {
            if (compatibility == null)
                return;

            recipeMethod(recipe, compatibility.ModInstance);
            RecipeAffectedByMods = true;
        }
        

        public bool RecipeAffectedByMods { get; private set; }
        public int CraftedAt { get; }
    }
}