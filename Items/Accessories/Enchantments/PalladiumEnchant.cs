using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class PalladiumEnchant : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Palladium Enchantment");
            Tooltip.SetDefault(
@"Greatly increases life regeneration after striking an enemy
One attack gains 10% life steal every 4 seconds, capped at 8 HP
'You feel your wounds slowly healing' ");
            DisplayName.AddTranslation(GameCulture.Chinese, "钯金魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'你感到你的伤口在慢慢愈合'
攻击敌人后大大增加生命回复
一次攻击获得每秒5%的生命窃取,上限为5点");
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(245, 172, 40);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 5;
            item.value = 100000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().PalladiumEffect();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("FargowiltasSouls:AnyPallaHead");
            recipe.AddIngredient(ItemID.PalladiumBreastplate);
            recipe.AddIngredient(ItemID.PalladiumLeggings);
            recipe.AddIngredient(ItemID.PalladiumSword);
            recipe.AddIngredient(ItemID.SoulDrain);
            //sanguine staff
            recipe.AddIngredient(ItemID.VampireKnives);
            recipe.AddIngredient(ItemID.UndergroundReward);

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
