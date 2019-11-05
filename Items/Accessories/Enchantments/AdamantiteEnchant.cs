using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.DD;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.MagicItems;
using ThoriumMod.Items.MiniBoss;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class AdamantiteEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'Who needs to aim?'
Every 8th projectile you shoot will split into 3
Any secondary projectiles may also split";


        public AdamantiteEnchant() : base("Adamantite Enchantment", TOOLTIP, 20, 20, 
            Item.sellPrice(gold: 2), TileID.CrystalBall, ItemRarityID.Lime)
        {
        }


        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation(GameCulture.Chinese, "精金魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'谁需要瞄准?'
第8个抛射物将会分裂成3个
分裂出的抛射物同样可以分裂");

            base.SetStaticDefaults();
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine tooltipLine in list)
            {
                if (tooltipLine.mod == nameof(Terraria) && tooltipLine.Name == TooltipLines.ITEM_NAME)
                {
                    tooltipLine.overrideColor = new Color(221, 85, 125);
                }
            }
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().AdamantiteEnchant = true;
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddRecipeGroup(Fargowiltas.RECIPE_GROUP_ANY_ADAMANTITE_HEAD);

            recipe.AddIngredient(ItemID.AdamantiteBreastplate);
            recipe.AddIngredient(ItemID.AdamantiteLeggings);

            recipe.AddIngredient(ItemID.VenomStaff);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.AdamantiteGlaive);
            recipe.AddIngredient(ItemID.TitaniumTrident);
            recipe.AddIngredient(ItemID.CrystalSerpent);
        }


        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<AdamantiteStaff>());
            recipe.AddIngredient(ModContent.ItemType<DynastyWarFan>());
            recipe.AddIngredient(ModContent.ItemType<Scorn>());
            recipe.AddIngredient(ModContent.ItemType<OgreSnotGun>());
            recipe.AddIngredient(ModContent.ItemType<MidasMallet>());

            recipe.AddIngredient(ItemID.CrystalSerpent);
        }
    }
}
