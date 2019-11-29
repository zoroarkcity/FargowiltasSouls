using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using Terraria.Localization;
using System.Collections.Generic;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.Donate;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class JungleEnchant : EnchantmentItem
    {
        public JungleEnchant() : base("Jungle Enchantment", "", 20, 20,
            TileID.DemonAltar, Item.sellPrice(gold: 1), ItemRarityID.Orange, new Color(113, 151, 31))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            string tooltip =
@"'The wrath of the jungle dwells within'
Taking damage will release a lingering spore explosion
Spore damage scales with magic damage
All herb collection is doubled
";
            string tooltip_ch =
@"'丛林之怒深藏其中'
受到伤害会释放出有毒的孢子爆炸
所有草药收获翻倍";

            if(Fargowiltas.Instance.ThoriumLoaded)
            {
                tooltip += "Effects of Guide to Plant Fiber Cordage";
                tooltip_ch += "拥有植物纤维绳索指南的效果";
            }

            Tooltip.SetDefault(tooltip);

            DisplayName.AddTranslation(GameCulture.Chinese, "丛林魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, tooltip_ch);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().JungleEffect();

            /*if (player.jump)
            {

            }*/
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.JungleHat);
            recipe.AddIngredient(ItemID.JungleShirt);
            recipe.AddIngredient(ItemID.JunglePants);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<MantisCane>());
            recipe.AddIngredient(ModContent.ItemType<RivetingTadpole>());

            recipe.AddIngredient(ItemID.Buggy);
            recipe.AddIngredient(ItemID.JungleRose);
            recipe.AddIngredient(ItemID.ThornChakram);
            recipe.AddIngredient(ItemID.Boomstick);
            recipe.AddIngredient(ItemID.PoisonedKnife, 300);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.CordageGuide);
            recipe.AddIngredient(ItemID.JungleRose);
            recipe.AddIngredient(ItemID.ThornChakram);
            recipe.AddIngredient(ItemID.Buggy);
        }
    }
}
