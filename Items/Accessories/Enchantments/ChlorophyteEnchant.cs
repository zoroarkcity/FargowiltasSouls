using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class ChlorophyteEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorophyte Enchantment");

            string tooltip =
@"'The jungle's essence crystallizes around you'
Summons a ring of leaf crystals to shoot at nearby enemies
Taking damage will release a lingering spore explosion
All herb collection is doubled
Effects of Flower Boots
Summons a pet Seedling";

            string tooltip_ch =
@"'丛林的精华凝结在你周围'
召唤一圈叶绿水晶射击附近的敌人
受到伤害时会释放出有毒的孢子爆炸
所有草药收获翻倍
拥有植物纤维绳索指南的效果
召唤一颗宠物幼苗";

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "叶绿魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(36, 137, 0);
                }
            }
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 7;
            item.value = 150000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            FargoPlayer modPlayer = player.GetModPlayer<FargoPlayer>();
            //crystal and pet
            modPlayer.ChloroEffect(hideVisual, 100);
            //herb double and bulb effect with thorium
            modPlayer.ChloroEnchant = true;
            modPlayer.FlowerBoots();
            modPlayer.JungleEffect();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("FargowiltasSouls:AnyChloroHead");
            recipe.AddIngredient(ItemID.ChlorophytePlateMail);
            recipe.AddIngredient(ItemID.ChlorophyteGreaves);
            recipe.AddIngredient(null, "JungleEnchant");
            
            if(Fargowiltas.Instance.ThoriumLoaded)
            {
                recipe.AddIngredient(ItemID.FlowerBoots);
                recipe.AddIngredient(ItemID.StaffofRegrowth);
                recipe.AddIngredient(ItemID.ChlorophyteSaber);
                recipe.AddIngredient(ItemID.LeafBlower);
                recipe.AddIngredient(thorium.ItemType("BudBomb"), 300);
            }
            else
            {
                recipe.AddIngredient(ItemID.FlowerBoots);
                recipe.AddIngredient(ItemID.StaffofRegrowth);
            }
            
            recipe.AddIngredient(ItemID.Seedling);
            
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
