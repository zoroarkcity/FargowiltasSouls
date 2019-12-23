using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class DarkArtistEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Artist Enchantment");

            string tooltip =
@"'The shadows hold more than they seem'
Summons a Flameburst minion that will fire after charging up
While attacking, Flameburst shots manifest themselves from your shadows
Greatly enhances Flameburst effectiveness
Summons a pet Flickerwick";

            Tooltip.SetDefault(tooltip); 
            DisplayName.AddTranslation(GameCulture.Chinese, "暗黑艺术家魔石");
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(155, 92, 176);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 8;
            item.value = 250000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            modPlayer.DarkArtistEffect(hideVisual);
            modPlayer.ApprenticeEffect();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ApprenticeAltHead);
            recipe.AddIngredient(ItemID.ApprenticeAltShirt);
            recipe.AddIngredient(ItemID.ApprenticeAltPants);
            recipe.AddIngredient(null, "ApprenticeEnchant");

            if (Fargowiltas.Instance.ThoriumLoaded)
            {      
                
                recipe.AddIngredient(ItemID.DD2FlameburstTowerT3Popper);
                recipe.AddIngredient(thorium.ItemType("DarkMageStaff"));
                recipe.AddIngredient(thorium.ItemType("WitherStaff"));
                recipe.AddIngredient(ItemID.ShadowFlameHexDoll);
                recipe.AddIngredient(ItemID.InfernoFork);
                recipe.AddIngredient(thorium.ItemType("ShadowFlareBow"));
            }
            else
            {
                recipe.AddIngredient(ItemID.DD2FlameburstTowerT3Popper);
                recipe.AddIngredient(ItemID.ShadowFlameHexDoll);
                recipe.AddIngredient(ItemID.InfernoFork);
            }

            //betsy wrath
            
            recipe.AddIngredient(ItemID.DD2PetGhost);
            
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
