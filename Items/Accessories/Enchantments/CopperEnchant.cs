using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class CopperEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        public int timer;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Copper Enchantment");

            string tooltip = 
@"'Behold'
Attacks have a chance to shock enemies with lightning
If an enemy is wet, the chance and damage is increased
Attacks that cause Wet cannot proc the lightning";
            string tooltip_ch =
@"'注视'
攻击有概率用闪电打击敌人
如果敌人处于潮湿状态,增加概率和伤害
造成潮湿的攻击不能触发闪电";

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "铜魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(213, 102, 23);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 3;
            item.value = 100000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().CopperEnchant = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CopperHelmet);
            recipe.AddIngredient(ItemID.CopperChainmail);
            recipe.AddIngredient(ItemID.CopperGreaves);
            
            if(Fargowiltas.Instance.ThoriumLoaded)
            {      
                recipe.AddIngredient(thorium.ItemType("CopperBuckler"));
                recipe.AddIngredient(ItemID.CopperShortsword);
                recipe.AddIngredient(ItemID.AmethystStaff);
                recipe.AddIngredient(ItemID.PurplePhaseblade);
                recipe.AddIngredient(thorium.ItemType("ThunderTalon"));
                recipe.AddIngredient(thorium.ItemType("Zapper"));
                recipe.AddIngredient(ItemID.FirstEncounter);
            }
            else
            {
                recipe.AddIngredient(ItemID.CopperShortsword);
                recipe.AddIngredient(ItemID.AmethystStaff);
                recipe.AddIngredient(ItemID.FirstEncounter);
                //recipe.AddIngredient(ItemID.PurplePhaseblade);
                recipe.AddIngredient(ItemID.Wire, 20);
            }
                       
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
