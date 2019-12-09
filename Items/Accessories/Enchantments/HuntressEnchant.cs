using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class HuntressEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Huntress Enchantment");
            Tooltip.SetDefault(
@"'The Hunt is On'
Explosive Traps recharge faster and oil enemies
Set oiled enemies on fire for extra damage

Double tap DOWN / press special key to create a localized rain of arrows at the cursor's position for a few seconds, but has a lengthy cooldown. 
The arrows that rain down are based on the arrows in the player's inventory.

If the player does not have any arrows, it defaults to basic Wooden Arrows pre-Golem and special Huntress Bolts post-Golem. 
Huntress Bolts inflict both Oiled and Betsy's Curse, as well as exploding like Hellfire Arrows.
");
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
            player.GetModPlayer<FargoPlayer>().HuntressEnchant = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.HuntressWig);
            recipe.AddIngredient(ItemID.HuntressJerkin);
            recipe.AddIngredient(ItemID.HuntressPants);
            recipe.AddIngredient(ItemID.HuntressBuckler);
            recipe.AddIngredient(ItemID.DD2ExplosiveTrapT2Popper);
            recipe.AddIngredient(ItemID.DD2PhoenixBow);
            recipe.AddIngredient(ItemID.DaedalusStormbow);


            /*  

          Cinder String (with Thorium)
          Chlorophyte Shotbow*/


            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
