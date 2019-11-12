using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class BorealWoodEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'The cooler wood'
Attack will be periodically accompanied by a snowball
While in the Snow Biome, you shoot 5 snowballs instead";


        public BorealWoodEnchant() : base("Boreal Wood Enchantment", TOOLTIP, 20, 20,
            TileID.DemonAltar, Item.sellPrice(gold: 1), ItemRarityID.Green, new Color(139, 116, 100))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.AddTranslation(GameCulture.Chinese, "针叶木魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'冷木'
每5次攻击附带着一个雪球
在冰雪地形时, 发射5个雪球");
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().BorealEnchant = true;
            player.GetModPlayer<FargoPlayer>().AdditionalAttacks = true;
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.BorealWoodHelmet);
            recipe.AddIngredient(ItemID.BorealWoodBreastplate);
            recipe.AddIngredient(ItemID.BorealWoodGreaves);
            recipe.AddIngredient(ItemID.Snowball, 300);
            recipe.AddIngredient(ItemID.Penguin);
            recipe.AddIngredient(ItemID.ColdWatersintheWhiteLand);
            recipe.AddIngredient(ItemID.Shiverthorn);
        }
    }
}
