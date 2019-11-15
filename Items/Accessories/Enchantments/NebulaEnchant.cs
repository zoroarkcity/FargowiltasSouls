using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.Tracker;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class NebulaEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'The pillars of creation have shined upon you'
Hurting enemies has a chance to spawn buff boosters";


        public NebulaEnchant() : base("Nebula Enchantment", TOOLTIP, 20, 20,
            TileID.LunarCraftingStation, Item.sellPrice(gold: 8), ItemRarityID.Red, new Color(254, 126, 229))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();


            DisplayName.AddTranslation(GameCulture.Chinese, "星云魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'创造之柱照耀着你'
杀死敌人有概率产生增益效果");
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().NebulaEffect();
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.NebulaHelmet);
            recipe.AddIngredient(ItemID.NebulaBreastplate);
            recipe.AddIngredient(ItemID.NebulaLeggings);

            recipe.AddIngredient(ItemID.NebulaArcanum);
            recipe.AddIngredient(ItemID.NebulaBlaze);
            recipe.AddIngredient(ItemID.LunarFlareBook);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ItemID.WingsNebula);

            recipe.AddIngredient(ModContent.ItemType<BlackStaff>());
            recipe.AddIngredient(ModContent.ItemType<CatsEye>());
            recipe.AddIngredient(ModContent.ItemType<NebulaReflection>());
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.ShadowbeamStaff);
        }
    }
}
