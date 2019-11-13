using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.ArcaneArmor;
using ThoriumMod.Items.Bronze;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.Steel;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class GladiatorEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'Are you not entertained?'
Spears will rain down on struck enemies 
Summons a pet Minotaur";


        public GladiatorEnchant() : base("Gladiator Enchantment", TOOLTIP, 20, 20,
            TileID.DemonAltar, Item.sellPrice(silver: 80), ItemRarityID.Blue, new Color(156, 146, 78))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.AddTranslation(GameCulture.Chinese, "角斗士魔石");
            Tooltip.AddTranslation(GameCulture.Chinese,
@"'你难道不高兴吗?'
长矛将倾泄在被攻击的敌人身上
召唤一个小牛头人");
        }
        

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().GladiatorEffect(hideVisual);
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.GladiatorHelmet);
            recipe.AddIngredient(ItemID.GladiatorBreastplate);
            recipe.AddIngredient(ItemID.GladiatorLeggings);

            recipe.AddIngredient(ItemID.TartarSauce);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<SteelBattleAxe>(), 300);
            recipe.AddIngredient(ModContent.ItemType<GoblinWarSpear>(), 300);
            recipe.AddIngredient(ModContent.ItemType<BronzeGladius>());
            recipe.AddIngredient(ModContent.ItemType<GorganGazeStaff>());
            recipe.AddIngredient(ModContent.ItemType<RodAsclepius>());

            recipe.AddIngredient(ItemID.Javelin, 300);
        }
    }
}
