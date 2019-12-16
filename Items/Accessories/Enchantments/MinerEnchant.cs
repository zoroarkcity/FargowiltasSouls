using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class MinerEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'The planet trembles with each swing of your pick'
50% increased mining speed
Shows the location of enemies, traps, and treasures
Light is emitted from the player
Summons a pet Magic Lantern";


        public MinerEnchant() : base("Miner Enchantment", TOOLTIP, 20, 20,
            TileID.DemonAltar, Item.sellPrice(silver: 40), ItemRarityID.Green, new Color(95, 117, 151))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.AddTranslation(GameCulture.Chinese, "矿工魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'你每挥一下镐子, 行星都会震动'
增加50%采掘速度
显示敌人, 陷阱和宝藏
照亮周围
召唤一个魔法灯笼");
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().MinerEffect(hideVisual, .5f);
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.MiningHelmet);
            recipe.AddIngredient(ItemID.MiningShirt);
            recipe.AddIngredient(ItemID.MiningPants);
            recipe.AddIngredient(ItemID.CopperPickaxe);

            recipe.AddIngredient(ItemID.MagicLantern);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(thorium.ItemType("EnforcedThoriumPax"));
            recipe.AddIngredient(thorium.ItemType("aSandstonePickaxe"));
            recipe.AddRecipeGroup(Fargowiltas.RecipeGroups.ANY_GOLD_PICKAXE);

            recipe.AddIngredient(ItemID.CnadyCanePickaxe); //gj 
            recipe.AddIngredient(ItemID.MoltenPickaxe);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.CnadyCanePickaxe);
            recipe.AddIngredient(ItemID.GoldPickaxe);
            recipe.AddIngredient(ItemID.MoltenPickaxe);
        }
    }
}
