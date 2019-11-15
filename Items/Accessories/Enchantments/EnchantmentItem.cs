using System;
using System.Collections.Generic;
using FargowiltasSouls.ModCompatibilities;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public abstract class EnchantmentItem : FargowiltasSoulsItem
    {
        protected EnchantmentItem(string displayName, string tooltip, int width, int height, int craftedAt, int value, int rarity, Color? nameColor) : 
            base(displayName, tooltip, width, height, value: value, rarity: rarity)
        {
            CraftedAt = craftedAt;

            NameColor = nameColor;
        }


        public override void SetDefaults()
        {
            item.accessory = true;

            ItemID.Sets.ItemNoGravity[item.type] = true;

            base.SetDefaults();
        }


        public override void ModifyTooltips(List<TooltipLine> list)
        {
            if (!NameColor.HasValue)
                return;

            foreach (TooltipLine tooltipLine in list)
                if (tooltipLine.mod == nameof(Terraria) && tooltipLine.Name == TooltipLines.ITEM_NAME)
                    tooltipLine.overrideColor = NameColor;
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
            try
            {
                if (compatibility == null)
                    return;

                recipeMethod(recipe, compatibility.ModInstance);
                RecipeAffectedByMods = true;
            }
            catch (Exception)
            {
                mod.Logger.Error($"Error while adding recipe: method {recipeMethod.Method.Name} failed completing.");
            }
        }
        

        public bool RecipeAffectedByMods { get; private set; }
        public int CraftedAt { get; }

        public Color? NameColor { get; }
    }
}