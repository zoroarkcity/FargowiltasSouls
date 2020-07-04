using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class FrostEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Enchantment");
            
            string tooltip =
@"Icicles will start to appear around you
When there are three, attacking will launch them towards the cursor
Your attacks inflict Frostburn
Summons a pet Penguin and Snowman
'Let's coat the world in a deep freeze'";

            string tooltip_ch =
@"'让我们给世界披上一层厚厚的冰衣'
周围将出现冰柱
当冰柱达到三个时,攻击会将它们向光标位置发射
攻击造成寒焰效果
召唤一个宠物企鹅和小雪人";

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "霜冻魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(122, 189, 185);
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
            item.value = 150000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().FrostEffect(50, hideVisual); //add grinch, kill fish
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FrostHelmet);
            recipe.AddIngredient(ItemID.FrostBreastplate);
            recipe.AddIngredient(ItemID.FrostLeggings);
            //snow enchant
            recipe.AddIngredient(ItemID.Frostbrand);
            recipe.AddIngredient(ItemID.IceBow);
            //frost staff
            //coolwhip
            recipe.AddIngredient(ItemID.ToySled);
            //grinch pet

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
