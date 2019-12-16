using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.Butter;
using ThoriumMod.Items.MeleeItems;
using ThoriumMod.Items.NPCItems;
using ThoriumMod.Items.Steel;
using ThoriumMod.Tiles;

namespace FargowiltasSouls.ModCompatibilities
{
    public sealed class ThoriumCompatibility : ModCompatibility
    {
        public ThoriumCompatibility(Mod callerMod) : base(callerMod, nameof(ThoriumMod))
        {
        }


        protected override void AddRecipes()
        {
            /*int 
                //foldedMetal = ModContent.ItemType<FoldedMetal>(),
                arcaneArmorFabricator = ModContent.TileType<ArcaneArmorFabricator>();


            /*ModRecipe recipe = new ModRecipe(CallerMod);

            recipe.AddIngredient(foldedMetal);
            recipe.AddTile(arcaneArmorFabricator);
            
            recipe.SetResult(ModContent.ItemType<SteelArrow>());
            recipe.AddRecipe();



            recipe = new ModRecipe(CallerMod);

            recipe.AddIngredient(foldedMetal);
            recipe.AddTile(arcaneArmorFabricator);

            recipe.SetResult(ModContent.ItemType<SteelAxe>());
            recipe.AddRecipe();



            recipe = new ModRecipe(CallerMod);

            recipe.AddIngredient(foldedMetal);
            recipe.AddTile(arcaneArmorFabricator);

            recipe.SetResult(ModContent.ItemType<SteelBattleAxe>(), 10);
            recipe.AddRecipe();



            recipe = new ModRecipe(CallerMod);

            recipe.AddIngredient(foldedMetal);
            recipe.AddTile(arcaneArmorFabricator);

            recipe.SetResult(ModContent.ItemType<SteelBlade>());
            recipe.AddRecipe();



            recipe = new ModRecipe(CallerMod);

            recipe.AddIngredient(foldedMetal);
            recipe.AddTile(arcaneArmorFabricator);

            recipe.SetResult(ModContent.ItemType<SteelBow>());
            recipe.AddRecipe();



            recipe = new ModRecipe(CallerMod);

            recipe.AddIngredient(foldedMetal);
            recipe.AddTile(arcaneArmorFabricator);

            recipe.SetResult(ModContent.ItemType<SteelChestplate>());
            recipe.AddRecipe();



            recipe = new ModRecipe(CallerMod);

            recipe.AddIngredient(foldedMetal);
            recipe.AddTile(arcaneArmorFabricator);

            recipe.SetResult(ModContent.ItemType<SteelGreaves>());
            recipe.AddRecipe();



            recipe = new ModRecipe(CallerMod);

            recipe.AddIngredient(foldedMetal);
            recipe.AddTile(arcaneArmorFabricator);

            recipe.SetResult(ModContent.ItemType<SteelHelmet>());
            recipe.AddRecipe();



            recipe = new ModRecipe(CallerMod);

            recipe.AddIngredient(foldedMetal);
            recipe.AddTile(arcaneArmorFabricator);

            recipe.SetResult(ModContent.ItemType<SteelMallet>());
            recipe.AddRecipe();



            recipe = new ModRecipe(CallerMod);

            recipe.AddIngredient(foldedMetal);
            recipe.AddTile(arcaneArmorFabricator);

            recipe.SetResult(ModContent.ItemType<SteelPickaxe>());
            recipe.AddRecipe();*/
        }

        protected override void AddRecipeGroups()
        {
            Fargowiltas.RecipeGroups.RegisterGroups(new Dictionary<string, RecipeGroup>()
            {
                { Fargowiltas.RecipeGroups.ANY_THORIUM_YOYO, 
                    new RecipeGroup(() => Lang.misc[37] + " Combination Yoyo", ModContent.ItemType<Nocturnal>(), ModContent.ItemType<Sanguine>()) },


                { Fargowiltas.RecipeGroups.ANY_THORIUM_JESTER_HEADPIECE, 
                    new RecipeGroup(() => Lang.misc[37] + " Jester Mask", ModContent.ItemType<JestersMask>(), ModContent.ItemType<JestersMask2>()) },

                { Fargowiltas.RecipeGroups.ANY_THORIUM_JESTER_BODY, 
                    new RecipeGroup(() => Lang.misc[37] + " Jester Shirt", ModContent.ItemType<JestersShirt>(), ModContent.ItemType<JestersShirt2>())  },

                { Fargowiltas.RecipeGroups.ANY_THORIUM_JESTER_LEGGINGS, 
                    new RecipeGroup(() => Lang.misc[37] + " Jester Leggings", ModContent.ItemType<JestersLeggings>(), ModContent.ItemType<JestersLeggings2>()) },


                { Fargowiltas.RecipeGroups.ANY_THORIUM_TAMBOURINE, 
                    new RecipeGroup(() => Lang.misc[37] + " Evil Wood Tambourine", ModContent.ItemType<EbonWoodTambourine>(), ModContent.ItemType<ShadeWoodTambourine>()) },


                { Fargowiltas.RecipeGroups.ANY_THORIUM_FAN_LETTER, 
                    new RecipeGroup(() => Lang.misc[37] + " Fan Letter", ModContent.ItemType<FanLetter>(), ModContent.ItemType<FanLetter2>()) },

                { Fargowiltas.RecipeGroups.ANY_THORIUM_DUNGEON_BUTTERFLY, 
                    new RecipeGroup(() => Lang.misc[37] + " Dungeon Butterfly", 
                        ModContent.ItemType<BlueDungeonButterfly>(), ModContent.ItemType<GreenDungeonButterfly>(), ModContent.ItemType<PinkDungeonButterfly>()) }
            });
        }
    }
}