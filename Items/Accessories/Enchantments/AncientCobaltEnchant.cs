using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class AncientCobaltEnchant : ModItem
    {
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Cobalt Enchantment");
            Tooltip.SetDefault(
@"'The jungle of old empowers you'
Your projectiles will occasionally split into stingers that deal damage and poison enemies

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
            player.GetModPlayer<FargoPlayer>().AncientCobaltEnchant = true;
        }

        public override void AddRecipes()
        {
            /*ModRecipe recipe = new ModRecipe(mod);
            
            armor 1
            armor 2
            armor 3

            Ancient Necro Helmet
Rally, Blowpipe, 30 Poisoned Flasks



            goes into cobalt?



            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();*/
        }
    }
}
