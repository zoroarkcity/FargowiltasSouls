using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class EbonwoodEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'Untapped potential'
You have an aura of Shadowflame
While in the Corruption, the radius is doubled";


        public EbonwoodEnchant() : base("Ebonwood Enchantment", TOOLTIP, 20, 20,
            TileID.DemonAltar, Item.sellPrice(gold: 1), ItemRarityID.Green, new Color(100, 90, 141))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.AddTranslation(GameCulture.Chinese, "乌木魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'未开发的潜力'
环绕一个暗影烈焰光环
在腐地时, 半径加倍
");
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().EbonEffect();
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.EbonwoodHelmet);
            recipe.AddIngredient(ItemID.EbonwoodBreastplate);
            recipe.AddIngredient(ItemID.EbonwoodGreaves);
            recipe.AddIngredient(ItemID.EbonwoodSword);
            recipe.AddIngredient(ItemID.Ebonkoi);
            recipe.AddIngredient(ItemID.VileMushroom);
            recipe.AddIngredient(ItemID.LightlessChasms);
        }
    }
}
