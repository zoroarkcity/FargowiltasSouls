using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class SnowEnchant : ModItem
    {        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Snow Enchantment");
            Tooltip.SetDefault(
@"You have a small area around you that Frostburns enemies and slows projectiles
'It's Burning Cold Outside'");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
            item.rare = 1;
            item.value = 50000;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FargoPlayer>().SnowEffect(); //add fish pet
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.EskimoHood);
            recipe.AddIngredient(ItemID.EskimoCoat);
            recipe.AddIngredient(ItemID.EskimoPants);
            //hand warmer
            //fruitcake chakram
            //ice boomerang
            //frost daggerfish
            recipe.AddIngredient(ItemID.FrostMinnow);
            recipe.AddIngredient(ItemID.AtlanticCod);
            //fish pet

            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
