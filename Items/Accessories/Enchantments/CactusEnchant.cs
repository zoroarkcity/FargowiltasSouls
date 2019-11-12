using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.Consumable;
using ThoriumMod.Items.NPCItems;
using ThoriumMod.Items.ThrownItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class CactusEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'It's the quenchiest!' 
25% of contact damage is reflected
Enemies may explode into needles on death";
        

        public CactusEnchant() : base("Cactus Enchantment", TOOLTIP, 20, 20, 
            TileID.DemonAltar, Item.sellPrice(silver: 40), ItemRarityID.Green, new Color(121, 158, 29))
        {
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.AddTranslation(GameCulture.Chinese, "仙人掌魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'太解渴了!'
反射25%接触伤害
敌人在死亡时可能会爆出刺");
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().CactusEffect();
            player.thorns = .25f;
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.CactusHelmet);
            recipe.AddIngredient(ItemID.CactusBreastplate);
            recipe.AddIngredient(ItemID.CactusLeggings);
            recipe.AddIngredient(ItemID.CactusSword);
            recipe.AddIngredient(ItemID.Sandgun);

            recipe.AddIngredient(ItemID.SecretoftheSands);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ItemID.ThornsPotion, 5);

            recipe.AddIngredient(ModContent.ItemType<CactusNeedle>(), 300);
            recipe.AddIngredient(ModContent.ItemType<CactusFruit>(), 5);
            recipe.AddIngredient(ModContent.ItemType<PricklyJam>(), 5);
        }

        protected override void FinishRecipeVanilla(ModRecipe recipe)
        {
            recipe.AddIngredient(ItemID.PinkPricklyPear);
        }
    }
}
