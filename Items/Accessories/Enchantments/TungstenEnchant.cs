using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class TungstenEnchant : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tungsten Enchantment");

            string tooltip =
@"150% increased sword size
Every half second a projectile will be doubled in size
Enlarged swords and projectiles deal 15% more damage on crits
'Bigger is always better'";
            string tooltip_ch =
@"'大就是好'
增加150%剑的尺寸
增加100%抛射物尺寸
减少10%移动速度和近战速度
抛射物仍然具有同样的砖块碰撞箱";

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "钨金魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(176, 210, 178);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 1;
            item.value = 40000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().TungstenEnchant = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TungstenHelmet);
            recipe.AddIngredient(ItemID.TungstenChainmail);
            recipe.AddIngredient(ItemID.TungstenGreaves);
            //tungsten sword
            //ruler
            recipe.AddIngredient(ItemID.CandyCaneSword);
            recipe.AddIngredient(ItemID.GreenPhaseblade);
            recipe.AddIngredient(ItemID.EmeraldStaff);
            recipe.AddIngredient(ItemID.Snail);
            //recipe.AddIngredient(ItemID.Sluggy);

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
