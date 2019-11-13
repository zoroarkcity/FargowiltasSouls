using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.BasicAccessories;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.HealerItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class HallowEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'Hallowed be your sword and shield'
You gain a shield that can reflect projectiles
Summons an Enchanted Sword familiar that scales with minion damage
Summons a magical fairy";


        public HallowEnchant() : base("Hallowed Enchantment", TOOLTIP, 20, 20,
            TileID.CrystalBall, Item.sellPrice(gold: 3, silver: 60), ItemRarityID.LightPurple, new Color(150, 133, 100))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.AddTranslation(GameCulture.Chinese, "神圣魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'愿人都尊你的剑与盾为圣'
获得一个可以反射抛射物的护盾
召唤一柄附魔剑
召唤魔法妖精");
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().HallowEffect(hideVisual, 80);
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddRecipeGroup("FargowiltasSouls:AnyHallowHead");

            recipe.AddIngredient(ModContent.ItemType<SilverEnchant>());

            recipe.AddIngredient(ItemID.HallowedPlateMail);
            recipe.AddIngredient(ItemID.HallowedGreaves);
            recipe.AddIngredient(ItemID.FairyBell);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<EnchantedShield>());
            recipe.AddIngredient(ModContent.ItemType<SteamgunnerController>());
            recipe.AddIngredient(ModContent.ItemType<HolyStaff>());

            recipe.AddIngredient(ItemID.Excalibur);
            recipe.AddIngredient(ItemID.LightDisc, 5);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.Excalibur);
            recipe.AddIngredient(ItemID.LightDisc, 5);
        }
    }
}
