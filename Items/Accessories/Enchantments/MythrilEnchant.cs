using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.MagicItems;
using ThoriumMod.Items.NPCItems;
using ThoriumMod.Items.RangedItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class MythrilEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'You feel the knowledge of your weapons seep into your mind'
20% increased weapon use speed";


        public MythrilEnchant() : base("Mythril Enchantment", TOOLTIP, 20, 20,
            TileID.CrystalBall, Item.sellPrice(gold: 2), ItemRarityID.Pink, new Color(157, 210, 144))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.AddTranslation(GameCulture.Chinese, "秘银魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"你感觉你对武器的知识渗透到脑海中'
增加25%武器使用速度");
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (SoulConfig.Instance.GetValue(SoulConfig.Instance.MythrilSpeed) && !player.GetModPlayer<FargoPlayer>().TerrariaSoul)
                player.GetModPlayer<FargoPlayer>().AttackSpeed *= 1.2f;
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddRecipeGroup("FargowiltasSouls:AnyMythrilHead");

            recipe.AddIngredient(ItemID.MythrilChainmail);
            recipe.AddIngredient(ItemID.MythrilGreaves);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<MythrilStaff>());
            recipe.AddIngredient(ModContent.ItemType<BulletStorm>());
            recipe.AddIngredient(ModContent.ItemType<Trigun>());

            recipe.AddIngredient(ItemID.Gatligator);
            recipe.AddIngredient(ItemID.Megashark);
            recipe.AddIngredient(ItemID.LaserRifle);
            recipe.AddIngredient(ItemID.ClockworkAssaultRifle);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.LaserRifle);
            recipe.AddIngredient(ItemID.ClockworkAssaultRifle);
            recipe.AddIngredient(ItemID.Gatligator);
            recipe.AddIngredient(ItemID.OnyxBlaster);
        }
    }
}
