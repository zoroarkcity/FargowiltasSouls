using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.BasicAccessories;
using ThoriumMod.Items.Magma;
using ThoriumMod.Items.Vanity;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class ObsidianEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'The earth calls'
Grants immunity to fire, fall damage, and 5 seconds of lava immunity
While standing in lava, you gain 20 armor penetration, 15% attack speed, and your attacks ignite enemies";


        public ObsidianEnchant() : base("Obsidian Enchantment", TOOLTIP, 20, 20,
            TileID.DemonAltar, Item.sellPrice(gold: 1), ItemRarityID.Orange, new Color(69, 62, 115))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.AddTranslation(GameCulture.Chinese, "黑曜石魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'大地在呼唤'
免疫火焰,掉落伤害,获得5秒岩浆免疫
在岩浆中时,获得20点护甲穿透,15%攻击速度,攻击会点燃敌人");
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().ObsidianEffect();
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.ObsidianHelm);
            recipe.AddIngredient(ItemID.ObsidianShirt);
            recipe.AddIngredient(ItemID.ObsidianPants);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<aObsidianHelmet>());
            recipe.AddIngredient(ModContent.ItemType<bObsidianChestGuard>());
            recipe.AddIngredient(ModContent.ItemType<cObsidianGreaves>());
            recipe.AddIngredient(ModContent.ItemType<ObsidianScale>());
            recipe.AddIngredient(ModContent.ItemType<MagmaBlade>());

            recipe.AddIngredient(ItemID.ObsidianRose);
            recipe.AddIngredient(ItemID.SharkToothNecklace);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.ObsidianRose);
            recipe.AddIngredient(ItemID.ObsidianHorseshoe);
            recipe.AddIngredient(ItemID.SharkToothNecklace);
            recipe.AddIngredient(ItemID.Fireblossom);
        }
    }
}
