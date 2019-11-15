using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.Thorium;
using ThoriumMod.Items.ThrownItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class MoltenEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'They shall know the fury of hell.' 
Nearby enemies are ignited
The closer they are to you the more damage they take
When you die, you violently explode dealing massive damage";


        public MoltenEnchant() : base("Molten Enchantment", TOOLTIP, 20, 20,
            TileID.DemonAltar, Item.sellPrice(gold: 1), ItemRarityID.Orange, new Color(193, 43, 43))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.AddTranslation(GameCulture.Chinese, "熔融魔石");
            Tooltip.AddTranslation(GameCulture.Chinese,
@"他们将感受到地狱的愤怒.'
点燃附近敌人
敌人距离越近, 收到的伤害越多
死亡时剧烈爆炸, 造成大量伤害");
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().MoltenEffect(20);
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.MoltenHelmet);
            recipe.AddIngredient(ItemID.MoltenBreastplate);
            recipe.AddIngredient(ItemID.MoltenGreaves);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<MeleeThorHammer>());
            recipe.AddIngredient(ModContent.ItemType<MeteoriteClusterBomb>(), 300);

            recipe.AddIngredient(ItemID.MoltenHamaxe);
            recipe.AddIngredient(ItemID.Sunfury);
            recipe.AddIngredient(ItemID.DarkLance);
            recipe.AddIngredient(ItemID.PhoenixBlaster);
            recipe.AddIngredient(ItemID.DemonsEye);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.Sunfury);
            recipe.AddIngredient(ItemID.DarkLance);
            recipe.AddIngredient(ItemID.PhoenixBlaster);
            recipe.AddIngredient(ItemID.DemonsEye);
        }
    }
}
