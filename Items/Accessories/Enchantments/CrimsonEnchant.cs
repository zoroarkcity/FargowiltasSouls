using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.ThrownItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class CrimsonEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'The blood of your enemy is your rebirth'
Greatly increases life regen
Summons a pet Face Monster and Crimson Heart";


        public CrimsonEnchant() : base("Crimson Enchantment", TOOLTIP, 20, 20,
            TileID.DemonAltar, Item.sellPrice(gold: 1), ItemRarityID.Blue, new Color(200, 54, 75))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.AddTranslation(GameCulture.Chinese, "血腥魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'你从敌人的血中重生'
大幅度增加生命回复速度
召唤巨脸怪宝宝和血腥心脏");
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().CrimsonEffect(hideVisual);
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.CrimsonHelmet);
            recipe.AddIngredient(ItemID.CrimsonScalemail);
            recipe.AddIngredient(ItemID.CrimsonGreaves);

            recipe.AddIngredient(ItemID.TheUndertaker);
            recipe.AddIngredient(ItemID.TheMeatball);
            recipe.AddIngredient(ItemID.BoneRattle);
            recipe.AddIngredient(ItemID.CrimsonHeart);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<CrimtaneTomahawk>(), 300);

            recipe.AddIngredient(ItemID.BloodLustCluster);
            recipe.AddIngredient(ItemID.TheRottedFork);
        }
    }
}
