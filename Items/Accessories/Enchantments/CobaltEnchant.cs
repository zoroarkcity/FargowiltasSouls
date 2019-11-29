using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.Geode;
using ThoriumMod.Items.MagicItems;
using ThoriumMod.Items.RangedItems;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class CobaltEnchant : EnchantmentItem
    {
        public const string TOOLTIP =
            @"'I can't believe it's not Palladium' 
25% chance for your projectiles to explode into shards
This can only happen every 2 seconds";


        public CobaltEnchant() : base("Cobalt Enchantment", TOOLTIP, 20, 20,
            TileID.CrystalBall, Item.sellPrice(gold: 2), ItemRarityID.LightRed, new Color(61, 164, 196))
        {
            
        }


        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.AddTranslation(GameCulture.Chinese, "钴蓝魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'真不敢相信这不是钯金'
25%概率使你的抛射物爆炸成碎片
仅限每2秒一次");
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().CobaltEnchant = true;
        }


        protected override void AddRecipeBase(ModRecipe recipe)
        {
            recipe.AddRecipeGroup("FargowiltasSouls:AnyCobaltHead");
            recipe.AddIngredient(ItemID.CobaltBreastplate);
            recipe.AddIngredient(ItemID.CobaltLeggings);

            recipe.AddIngredient(ItemID.Chik);
            recipe.AddIngredient(ItemID.CrystalDart, 300);
            recipe.AddIngredient(ItemID.CrystalStorm);
            recipe.AddIngredient(ItemID.CrystalVileShard);
        }

        protected override void AddThoriumRecipe(ModRecipe recipe, Mod thorium)
        {
            recipe.AddIngredient(ModContent.ItemType<CobaltPopper>());
            recipe.AddIngredient(ModContent.ItemType<CobaltStaff>());
            recipe.AddIngredient(ModContent.ItemType<CrystalPhaser>());
        }
    }
}
