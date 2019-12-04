using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class ApprenticeEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Apprentice Enchantment");
            Tooltip.SetDefault(
@"''
While attacking, Flameburst shots manifest themselves from your shadows
Flameburst field of view and range are dramatically increased");
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
            player.GetModPlayer<FargoPlayer>().ApprenticeEnchant = true;
        }

        public override void AddRecipes()
        {
            //ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(ItemID.);
            //recipe.AddIngredient(ItemID.);
            //recipe.AddIngredient(ItemID.);
            //recipe.AddIngredient(ItemID.ApprenticeScarf);
            //recipe.AddIngredient(ItemID.FlameStaff2);
            //recipe.AddIngredient(ItemID.TomeofInfiniteWisdom);


            /*
             * Clinger Staff
Golden Shower

Demon Fire Blast-Wand (with Thorium)
Wither Staff (with Thorium)
Kinetic Knife (with Thorium)
Rainbow Rod

             */


            //recipe.AddTile(TileID.CrystalBall);
            //recipe.SetResult(this);
            //recipe.AddRecipe();
        }
    }
}
