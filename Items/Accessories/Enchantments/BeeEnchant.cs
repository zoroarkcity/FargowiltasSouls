using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.BardItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class BeeEnchant : EnchantmentItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");
        public int timer;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bee Enchantment");

            string tooltip = 
@"'According to all known laws of aviation, there is no way a bee should be able to fly'
50% chance for any friendly bee to become a Mega Bee
Mega Bees ignore most enemy defense, immune frames, and last twice as long
";
            string tooltip_ch = 
@"'根据目前所知的所有航空原理, 蜜蜂应该根本不可能会飞'
50%概率使友善的蜜蜂成为巨型蜜蜂
巨型蜜蜂忽略大多数敌人的防御, 无敌帧, 并持续双倍的时间
";

            tooltip += "Summons a pet Baby Hornet";
            tooltip_ch += "召唤一只小黄蜂";

            Tooltip.SetDefault(tooltip);
            DisplayName.AddTranslation(GameCulture.Chinese, "蜜蜂魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color(254, 246, 37);
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
            item.value = 50000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().BeeEffect(hideVisual);
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.BeeHeadgear);
            recipe.AddIngredient(ItemID.BeeBreastplate);
            recipe.AddIngredient(ItemID.BeeGreaves);
            recipe.AddIngredient(ItemID.HiveBackpack);

            recipe.AddIngredient(ItemID.BeeGun);
            recipe.AddIngredient(ItemID.WaspGun);

            recipe.AddIngredient(ItemID.Nectar);
            recipe.AddTile(TileID.CrystalBall);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<HoneyRecorder>());

            recipe.AddIngredient(ItemID.NettleBurst);
            recipe.AddIngredient(ItemID.VenusMagnum);
        }
    }
}
