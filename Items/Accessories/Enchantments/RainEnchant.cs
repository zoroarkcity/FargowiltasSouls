using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class RainEnchant : ModItem
    {        
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rain Enchantment");
            Tooltip.SetDefault(
@"'Come again some other day'
Immunity to Wet
A miniature storm may appear when an enemy dies");
            DisplayName.AddTranslation(GameCulture.Chinese, "云雨魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'改天再来'
敌人死亡时可能会出现微型风暴");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 7;
            item.value = 100000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().RainEnchant = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.RainHat);
            recipe.AddIngredient(ItemID.RainCoat);
            recipe.AddIngredient(ItemID.UmbrellaHat);
            recipe.AddIngredient(ItemID.Umbrella);
            recipe.AddIngredient(ItemID.NimbusRod);
            recipe.AddIngredient(ItemID.WaterGun);
            recipe.AddIngredient(ItemID.RainCloud);

            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
