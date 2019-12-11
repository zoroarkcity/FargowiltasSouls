using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class MonkEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Monk Enchantment");
            Tooltip.SetDefault(
@"'Hours of Meditation have led to this…'

Standing still for ech seconds grants you a single use dash that will launch any enemy, or its really long nimmun n does damage n yes

Lightning Aura can now crit and strikes faster");
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
            player.GetModPlayer<FargoPlayer>().MonkEnchant = true;
        }

        public override void AddRecipes()
        {
            /*ModRecipe recipe = new ModRecipe(mod);
            
            armor 1
            armor 2
            armor 3
            recipe.AddIngredient(ItemID.MonkBelt);
            sleepy octopod
            ghastly glaive
            lightning staff tier 2

            Dao of Pow
Fetid Baghnakhs
Ale Tosser

Schmelze (with Thorium)
Rocket Fist (with Thorium)



            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();*/
        }
    }
}
