using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace FargowiltasSouls.Items.Accessories.Enchantments
{
    public class SquireEnchant : ModItem
    {
        public override string Texture => "FargowiltasSouls/Items/Placeholder";
        private readonly Mod thorium = ModLoader.GetMod("ThoriumMod");

        public override bool Autoload(ref string name)
        {
            return false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Squire Enchantment");
            Tooltip.SetDefault(
@"'Squire, will you hurry?'
Attacks will slowly remove enemy knockback immunity
This does not affect bosses
Ballista pierces more targets and panics when you take damage");
            DisplayName.AddTranslation(GameCulture.Chinese, "精金魔石");
            Tooltip.AddTranslation(GameCulture.Chinese, 
@"'谁需要瞄准?'
第8个抛射物将会分裂成3个
分裂出的抛射物同样可以分裂");
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
            player.GetModPlayer<FargoPlayer>().SquireEnchant = true;
        }

        public override void AddRecipes()
        {
            /*ModRecipe recipe = new ModRecipe(mod);
            
            armor 1
            armor 2
            armor 3
            brand of the inferno
            squire shield
            ballista tier 2

            Breaker Blade
Chlorophyte Saber
Chlorophyte Claymore

Doom Fire Axe (with Thorium)
Dragon's Tooth (with Thorium)
Rapier (with Thorium)
Warp Slicer (with Thorium)
Scalper (with Thorium)


            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();*/
        }
    }
}
